using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using WorldCupWinForms.Model;

namespace WorldCupWinForms;

public partial class FavForm : Form
{
    private readonly State state;
    private readonly ResourceManager resourceManager;
    private readonly LocalMatchDataRepo repository;
    private readonly RemoteMatchDataRepo repositoryAPI;
    private readonly IUserFavouritesRepo favouritesRepository;
    private readonly IUserSettingsRepo settingsRepository;
    private readonly IImagesRepo imagesRepository;
    private readonly List<StartingEleven> favoritePlayersList = [];
    private readonly List<StartingEleven> selectedPlayers = [];

    public FavForm()
    {
        InitializeComponent();
        state = new State();
        state.OnSettingsChanged += (sender, e) => UpdateForm();

        resourceManager = new ResourceManager($"{typeof(FavForm).FullName}", typeof(FavForm).Assembly);
        repository = new LocalMatchDataRepo();
        favouritesRepository = new UserFavouritesRepo();
        settingsRepository = new UserSettingsRepo();
        imagesRepository = new ImagesRepo();

        KeyPreview = true;
        pnlFavPlayers.AllowDrop = true;
        pnlFavPlayers.DragEnter += PnlFavPlayers_DragEnter;
        pnlFavPlayers.DragDrop += PnlFavPlayers_DragDrop;
    }

    public async void InitializeApplication()
    {
        try
        {
            if (!settingsRepository.HasSavedSettings())
            {
                var settingsForm = new SettingsForm(state);
                var result = settingsForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    var defaultSettings = new UserSettings
                                          {
                                              Language = Language.English,
                                              Gender = Gender.Men,
                                              DataSource = DataSource.Local,
                                              Resolution = Resolution.w1080_h1920
                                          };
                    ApplySettingsToState(defaultSettings);
                }

                await SaveCurrentSettingsAsync();
            }
            else
            {
                await LoadUserSettingsAsync();
            }

            await LoadTeamsAsync();
            await LoadFavouritePlayersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing application: {ex.Message}", "Initialization Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateForm()
    {
        if (state.SelectedLanguage == Language.Croatian)
        {
            var culture = new CultureInfo("hr-HR");
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            ApplyResourceToControl(this, new ComponentResourceManager(typeof(FavForm)), culture);
        }
        else
        {
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            ApplyResourceToControl(this, new ComponentResourceManager(typeof(FavForm)), culture);
        }
    }

    private async Task LoadUserSettingsAsync()
    {
        try
        {
            var userSettings = await settingsRepository.GetUserSettingsAsync();
            ApplySettingsToState(userSettings);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading user settings: {ex.Message}", "Settings Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ApplySettingsToState(UserSettings userSettings)
    {
        state.SelectedLanguage = userSettings.Language;
        state.SelectedGender = userSettings.Gender;
        state.SelectedSource = userSettings.DataSource;

        UpdateForm();
    }

    private Task SaveCurrentSettingsAsync()
    {
        var userSettings = new UserSettings
                           {
                               Language = state.SelectedLanguage,
                               Gender = state.SelectedGender,
                               DataSource = state.SelectedSource,
                               Resolution = Resolution.w1080_h1920 // not needed in forms
                           };

        return settingsRepository.SaveUserSettingsAsync(userSettings);
    }

    private void ApplyResourceToControl(Control control, ComponentResourceManager cmp, CultureInfo cultureInfo)
    {
        cmp.ApplyResources(control, control.Name, cultureInfo);

        foreach (Control child in control.Controls)
        {
            ApplyResourceToControl(child, cmp, cultureInfo);
        }
    }

    private async Task LoadTeamsAsync()
    {
        cmbTeams.Items.Clear();
        cmbTeams.DisplayMember = "Country";
        cmbTeams.ValueMember = "FifaCode";

        var teams = await repository.GetTeams(state.SelectedGender);
        var sortedTeams = teams.OrderBy(t => t.Country).ToArray();

        cmbTeams.Items.AddRange(sortedTeams.Cast<object>().ToArray());

        if (cmbTeams.Items.Count > 0)
        {
            cmbTeams.SelectedIndex = 0;
        }
    }

    private async Task LoadTeamPlayersAsync(string fifaCode)
    {
        try
        {
            var players = await repository.GetPlayersByTeamAsync(state.SelectedGender, fifaCode);

            if (players?.Any() == true)
            {
                PopulatePlayersPanelAsync(pnlTeamPlayers, players);
            }
            else
            {
                pnlTeamPlayers.Controls.Clear();
                var noPlayersLabel = new Label
                                     {
                                         Text = $"{resourceManager.GetString("No_team_players_data")}",
                                         Location = new Point(10, 10),
                                         Size = new Size(280, 30),
                                         Font = new Font("Arial", 10),
                                         ForeColor = Color.Gray
                                     };
                pnlTeamPlayers.Controls.Add(noPlayersLabel);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading team players: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task RefreshTeamPlayersPanelAsync()
    {
        if (cmbTeams.SelectedItem is Teams selectedTeam)
            await LoadTeamPlayersAsync(selectedTeam.FifaCode);
    }

    private async Task LoadFavouritePlayersAsync()
    {
        var savedFavourites = await favouritesRepository.GetFavouritePlayersAsync();
        favoritePlayersList.Clear();
        favoritePlayersList.AddRange(savedFavourites);
        await RefreshFavoritePlayersPanelAsync();
        await RefreshTeamPlayersPanelAsync();
    }

    private Task SaveFavouritePlayersAsync() => favouritesRepository.SaveFavouritePlayersAsync(favoritePlayersList);

    private async Task PopulatePlayersPanelAsync(Panel panel, IEnumerable<StartingEleven> players)
    {
        panel.Controls.Clear();
        panel.AutoScroll = true;

        var yPosition = 10;
        const int playerHeight = 60;
        const int margin = 5;
        const int imageSize = 50;

        foreach (var player in players.OrderBy(p => p.ShirtNumber))
        {
            var isPlayerInFavorites = panel == pnlTeamPlayers &&
                                      favoritePlayersList.Any(fp => fp.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                    fp.ShirtNumber == player.ShirtNumber);

            var isPlayerSelected = selectedPlayers.Any(sp => sp.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                                             sp.ShirtNumber == player.ShirtNumber);

            var displayText = $"#{player.ShirtNumber} {player.Name} ({player.Position})";

            //playername decorations
            if (player.Captain) displayText += " [🧢]";
            if (isPlayerInFavorites) displayText += " ⭐";
            if (isPlayerSelected) displayText = "✓ " + displayText;

            var playerPanel = new Panel
                              {
                                  Location = new Point(10, yPosition),
                                  Size = new Size(280, playerHeight),
                                  BackColor = isPlayerSelected ? Color.LightCyan : (panel == pnlFavPlayers ? Color.LightBlue : Color.LightGray),
                                  BorderStyle = BorderStyle.FixedSingle,
                                  Cursor = Cursors.Hand,
                                  Tag = player // Store the player object for drag and drop
                              };

            var playerImage = new PictureBox
                              {
                                  Location = new Point(5, 5),
                                  Size = new Size(imageSize, imageSize),
                                  SizeMode = PictureBoxSizeMode.Zoom,
                                  BorderStyle = BorderStyle.FixedSingle
                              };

            try
            {
                var imageData = await imagesRepository.GetImageAsync(player.Name);
                using var ms = new MemoryStream(imageData);
                playerImage.Image = Image.FromStream(ms);
            }
            catch (Exception)
            {
                playerImage.BackColor = Color.LightGray;
                throw;
            }

            var playerLabel = new Label
                              {
                                  Text = displayText,
                                  Location = new Point(imageSize + 10, 5),
                                  Size = new Size(215, playerHeight - 10),
                                  Font = new Font("Arial", 9),
                                  ForeColor = isPlayerSelected ? Color.Blue : (isPlayerInFavorites ? Color.OrangeRed : Color.Black),
                                  TextAlign = ContentAlignment.MiddleLeft
                              };

            var contextMenu = CreatePlayerContextMenu(player, isPlayerInFavorites, panel == pnlFavPlayers);

            playerPanel.Controls.Add(playerImage);
            playerPanel.Controls.Add(playerLabel);
            playerPanel.ContextMenuStrip = contextMenu;
            playerImage.ContextMenuStrip = contextMenu;
            playerLabel.ContextMenuStrip = contextMenu;

            if (panel == pnlTeamPlayers)
            {
                playerPanel.MouseDown += PlayerPanel_MouseDown;
                playerPanel.Click += PlayerPanel_Click;

                playerLabel.MouseDown += PlayerPanel_MouseDown;
                playerLabel.Click += PlayerPanel_Click;

                playerImage.MouseDown += PlayerPanel_MouseDown;
                playerImage.Click += PlayerPanel_Click;
            }
            else if (panel == pnlFavPlayers)
            {
                playerPanel.DoubleClick += (sender, e) => RemoveFromFavorites(player);
                playerLabel.DoubleClick += (sender, e) => RemoveFromFavorites(player);
                playerImage.DoubleClick += (sender, e) => RemoveFromFavorites(player);
            }

            panel.Controls.Add(playerPanel);
            yPosition += playerHeight + margin;
        }
    }

    private ContextMenuStrip CreatePlayerContextMenu(StartingEleven player, bool isInFavorites, bool isFavoritePanel)
    {
        var contextMenu = new ContextMenuStrip();

        //todo localize
        if (!isFavoritePanel)
        {
            if (isInFavorites)
            {
                var removeFavoriteItem = new ToolStripMenuItem(resourceManager.GetString("Remove_from_favs"));
                removeFavoriteItem.Click += (s, e) => RemoveFromFavorites(player);
                contextMenu.Items.Add(removeFavoriteItem);
            }
            else
            {
                var addFavoriteItem = new ToolStripMenuItem(resourceManager.GetString("Add_to_favs"));
                addFavoriteItem.Click += (s, e) => AddToFavorites(player);
                contextMenu.Items.Add(addFavoriteItem);
            }

            contextMenu.Items.Add(new ToolStripSeparator());
        }

        var addImageItem = new ToolStripMenuItem(resourceManager.GetString("Add_change_img"));
        addImageItem.Click += async (s, e) => await AddPlayerImageAsync(player);
        contextMenu.Items.Add(addImageItem);

        var removeImageItem = new ToolStripMenuItem(resourceManager.GetString("Remove_img"));
        removeImageItem.Click += async (s, e) => await RemovePlayerImageAsync(player);
        contextMenu.Items.Add(removeImageItem);

        if (isFavoritePanel)
        {
            contextMenu.Items.Add(new ToolStripSeparator());
            var removeFavoriteItem = new ToolStripMenuItem(resourceManager.GetString("Remove_from_favs"));
            removeFavoriteItem.Click += (s, e) => RemoveFromFavorites(player);
            contextMenu.Items.Add(removeFavoriteItem);
        }

        return contextMenu;
    }

    private async Task AddPlayerImageAsync(StartingEleven player)
    {
        try
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"{resourceManager.GetString("Select_img_for")} {player.Name}";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var imageData = await File.ReadAllBytesAsync(openFileDialog.FileName);
                    var extension = Path.GetExtension(openFileDialog.FileName).ToLower();
                    if (extension == ".jpeg")
                        extension = ".jpg";
                    var fileName = $"{player.Name}{extension}";

                    await imagesRepository.UploadImageAsync(imageData, fileName);

                    MessageBox.Show($"{resourceManager.GetString("Add_change_img")} {player.Name}!",
                                    $"{resourceManager.GetString("Img_added")}",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshFavoritePlayersPanelAsync();
                    RefreshTeamPlayersPanelAsync();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding image for {player.Name}: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task RemovePlayerImageAsync(StartingEleven player)
    {
        //todo localize
        try
        {
            var result = MessageBox.Show($"{resourceManager.GetString("Remove_img_for")} {player.Name}?", 
                                         $"{resourceManager.GetString("Confirm_img_removal")}",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    await imagesRepository.RemoveImageAsync($"{player.Name}.png");
                }
                catch
                {
                    // Ignore if .png doesn't exist
                }

                try
                {
                    await imagesRepository.RemoveImageAsync($"{player.Name}.jpg");
                }
                catch
                {
                    // Ignore if .jpg doesn't exist
                }

                MessageBox.Show($"{resourceManager.GetString("Img_removed_for")} {player.Name}",
                                $"{resourceManager.GetString("Img_removed")}",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshFavoritePlayersPanelAsync();
                RefreshTeamPlayersPanelAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing image for {player.Name}: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PlayerPanel_Click(object sender, EventArgs e)
    {
        StartingEleven player = null;

        if (sender is Panel panel && panel.Tag is StartingEleven)
        {
            player = (StartingEleven)panel.Tag;
        }
        else if (sender is Control control && control.Parent?.Tag is StartingEleven)
        {
            player = (StartingEleven)control.Parent.Tag;
        }

        if (player != null)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                //TODO HandleMultiSelection(player);
            }
            else
            {
                selectedPlayers.Clear();
                RefreshTeamPlayersPanelAsync();
            }
        }
    }

    private void PlayerPanel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
            return;

        if (e.Button != MouseButtons.Left)
            return;

        StartingEleven player = null;

        if (sender is Panel panel && panel.Tag is StartingEleven)
        {
            player = (StartingEleven)panel.Tag;
        }
        else if (sender is Control control && control.Parent?.Tag is StartingEleven)
        {
            player = (StartingEleven)control.Parent.Tag;
        }

        if (player != null)
        {
            if (selectedPlayers.Count > 0 && ModifierKeys.HasFlag(Keys.Control))
            {
                if (!selectedPlayers.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                              p.ShirtNumber == player.ShirtNumber))
                {
                    if (selectedPlayers.Count < 3)
                    {
                        selectedPlayers.Add(player);
                        RefreshTeamPlayersPanelAsync();
                    }
                }

                var sourceControl = sender as Control;
                sourceControl?.DoDragDrop(selectedPlayers.ToList(), DragDropEffects.Copy);
            }
            else
            {
                selectedPlayers.Clear();
                RefreshTeamPlayersPanelAsync();
                var sourceControl = sender as Control;
                sourceControl?.DoDragDrop(player, DragDropEffects.Copy);
            }
        }
    }

    private void PnlFavPlayers_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(StartingEleven)) ||
            e.Data.GetDataPresent(typeof(List<StartingEleven>)))
        {
            e.Effect = DragDropEffects.Copy;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void PnlFavPlayers_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(typeof(List<StartingEleven>)) is List<StartingEleven> players)
        {
            AddMultipleToFavorites(players);
            selectedPlayers.Clear();
            RefreshTeamPlayersPanelAsync();
        }
        else if (e.Data.GetData(typeof(StartingEleven)) is StartingEleven player)
        {
            AddToFavorites(player);
        }
    }

    private void AddMultipleToFavorites(List<StartingEleven> players)
    {
        var playersToAdd = new List<StartingEleven>();
        var duplicatePlayers = new List<string>();

        foreach (var player in players)
        {
            if (favoritePlayersList.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                             p.ShirtNumber == player.ShirtNumber))
            {
                duplicatePlayers.Add(player.Name);
                continue;
            }

            if (favoritePlayersList.Count + playersToAdd.Count >= 3)
            {
                break;
            }

            playersToAdd.Add(player);
        }

        if (favoritePlayersList.Count + playersToAdd.Count > 3)
        {
            var remainingSlots = 3 - favoritePlayersList.Count;
            if (remainingSlots > 0)
            {
                playersToAdd = playersToAdd.Take(remainingSlots).ToList();
                MessageBox.Show($"Only {remainingSlots} slots remaining. Added first {remainingSlots} selected players.",
                                "Favorites Limit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You already have the maximum of 3 favourite players!", "Maximum Reached",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        favoritePlayersList.AddRange(playersToAdd);

        if (playersToAdd.Count > 0)
        {
            var message = $"Added {playersToAdd.Count} player(s) to favorites";
            if (duplicatePlayers.Count > 0)
            {
                message += $"\n\nSkipped duplicates: {string.Join(", ", duplicatePlayers)}";
            }

            MessageBox.Show(message, "Players Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else if (duplicatePlayers.Count > 0)
        {
            MessageBox.Show($"All selected players are already in favorites: {string.Join(", ", duplicatePlayers)}",
                            "Duplicate Players", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        RefreshFavoritePlayersPanelAsync();
        RefreshTeamPlayersPanelAsync();
        SaveFavouritePlayersAsync();
    }

    private void AddToFavorites(StartingEleven player)
    {
        // todo try catch

        if (favoritePlayersList.Count >= 3)
        {
            MessageBox.Show($"{resourceManager.GetString("Max_three_fav_players")}", 
                            $"{resourceManager.GetString("Max_reached")}",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!favoritePlayersList.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                          p.ShirtNumber == player.ShirtNumber))
        {
            favoritePlayersList.Add(player);
            RefreshFavoritePlayersPanelAsync();
            RefreshTeamPlayersPanelAsync();
            SaveFavouritePlayersAsync(); 
        }
        else
        {
            MessageBox.Show($"{player.Name} {resourceManager.GetString("Player_already_in_fav")}", 
                            $"{resourceManager.GetString("Duplicate_player")}",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void RemoveFromFavorites(StartingEleven player)
    {
        //todo localize
        try
        {
            var result = MessageBox.Show($"{resourceManager.GetString("Remove_player_from_favs")} ({player.Name})", 
                                         $"{resourceManager.GetString("Confirm_removal")}",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                favoritePlayersList.RemoveAll(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                                   p.ShirtNumber == player.ShirtNumber);
                await RefreshFavoritePlayersPanelAsync();
                await RefreshTeamPlayersPanelAsync();
                await SaveFavouritePlayersAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error removing player from favorites: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task RefreshFavoritePlayersPanelAsync()
    {
        if (favoritePlayersList.Any())
        {
            await PopulatePlayersPanelAsync(pnlFavPlayers, favoritePlayersList);
        }
        else
        {
            pnlFavPlayers.Controls.Clear();
            var emptyLabel = new Label
                             {
                                 Text = $"{resourceManager.GetString("Fav_panel_info")}",
                                 Location = new Point(10, 10),
                                 Size = new Size(300, 100),
                                 Font = new Font("Arial", 9),
                                 ForeColor = Color.Gray,
                                 TextAlign = ContentAlignment.MiddleCenter
                             };
            pnlFavPlayers.Controls.Add(emptyLabel);
        }
    }

    private async void cmbTeams_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        // todo try catch
        selectedPlayers.Clear();
        await RefreshTeamPlayersPanelAsync();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Enter:
            case Keys.Escape:
                ConfirmAndClose();
                return true;
            default:
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    private async void btnSettings_Click(object sender, EventArgs e)
    {
        // todo try catch
        var settingsForm = new SettingsForm(state);
        var result = settingsForm.ShowDialog();

        if (result == DialogResult.OK || result == DialogResult.Cancel)
        {
            await SaveCurrentSettingsAsync();

            await LoadTeamsAsync();
        }
    }

    private void btnRankings_Click(object sender, EventArgs e)
    {
        state.SelectedFifaCode = (cmbTeams.SelectedItem as Teams)?.FifaCode ?? string.Empty;
        new RankForm(state).ShowDialog();
    }

    protected override async void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            await SaveCurrentSettingsAsync();
            SaveFavouritePlayersAsync();

            var result = MessageBox.Show("Exit app?",
                                         "Confirm Exit",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        base.OnFormClosing(e);
    }

    private async void ConfirmAndClose()
    {
        await SaveCurrentSettingsAsync();
        SaveFavouritePlayersAsync();

        var result = MessageBox.Show("Exit app?",
                                     "Confirm Exit",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            Close();
        }
    }
}
