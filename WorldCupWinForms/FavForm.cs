using DAL.Models;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Models.Enums;
using WorldCupWinForms.Model;

namespace WorldCupWinForms;

public partial class FavForm : Form
{
    private readonly State state;
    private readonly LocalMatchDataRepo repository;
    private readonly IUserFavouritesRepo favouritesRepository;
    private readonly IUserSettingsRepo settingsRepository;
    private readonly IImagesRepo imagesRepository;
    private readonly List<StartingEleven> favoritePlayersList = [];
    private readonly List<StartingEleven> selectedPlayers = [];

    public FavForm()
    {
        InitializeComponent();
        state = new State(); //settings track
        repository = new LocalMatchDataRepo();
        favouritesRepository = new UserFavouritesRepo();
        settingsRepository = new UserSettingsRepo();
        imagesRepository = new ImagesRepo();

        KeyPreview = true;
        pnlFavPlayers.AllowDrop = true;
        pnlFavPlayers.DragEnter += PnlFavPlayers_DragEnter;
        pnlFavPlayers.DragDrop += PnlFavPlayers_DragDrop;

        InitializeApplicationAsync();
    }

    private async void InitializeApplicationAsync()
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
                                              Language = DAL.Models.Enums.Language.English,
                                              Gender = DAL.Models.Enums.Gender.Men,
                                              DataSource = DAL.Models.Enums.DataSource.Local,
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

            UpdateStateLabel();

            LoadTeamsAsync();
            LoadFavouritePlayersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error initializing application: {ex.Message}", "Initialization Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private async Task SaveCurrentSettingsAsync()
    {
        try
        {
            var userSettings = new UserSettings
                               {
                                   Language = state.SelectedLanguage,
                                   Gender = state.SelectedGender,
                                   DataSource = state.SelectedSource,
                                   Resolution = Resolution.w1080_h1920 // not needed in forms
                               };

            await settingsRepository.SaveUserSettingsAsync(userSettings);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving user settings: {ex.Message}", "Settings Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ApplySettingsToState(UserSettings userSettings)
    {
        state.SelectedLanguage = userSettings.Language;
        state.SelectedGender = userSettings.Gender;
        state.SelectedSource = userSettings.DataSource;
    }

    private void UpdateStateLabel()
    {
        if (state.SelectedLanguage == Language.Croatian)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hr-HR");
            ApplyResourceToControl(this, new ComponentResourceManager(typeof(FavForm)), new CultureInfo("hr-HR"));
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            ApplyResourceToControl(this, new ComponentResourceManager(typeof(FavForm)), new CultureInfo("en-US"));
        }

        

        //lblState.Text = $@"Current: {state.SelectedLanguage.ToString()} - " +
        //                $@" {state.SelectedGender.ToString()}";
    }

    private void ApplyResourceToControl(Control control, ComponentResourceManager cmp, CultureInfo cultureInfo)
    {
        cmp.ApplyResources(control, control.Name, cultureInfo);

        foreach (Control child in control.Controls)
        {
            ApplyResourceToControl(child, cmp, cultureInfo);
        }
    }

    private async void LoadFavouritePlayersAsync()
    {
        try
        {
            var savedFavourites = await favouritesRepository.GetFavouritePlayersAsync();
            favoritePlayersList.Clear();
            favoritePlayersList.AddRange(savedFavourites);
            RefreshFavoritePlayersPanel();
            RefreshTeamPlayersPanel();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading favourite players: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void SaveFavouritePlayersAsync()
    {
        try
        {
            await favouritesRepository.SaveFavouritePlayersAsync(favoritePlayersList);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving favourite players: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void PopulatePlayersPanel(Panel panel, IEnumerable<StartingEleven> players)
    {
        panel.Controls.Clear();
        panel.AutoScroll = true;

        var yPosition = 10;
        const int playerHeight = 60; // Increased height to accommodate image
        const int margin = 5;
        const int imageSize = 50;

        foreach (var player in players.OrderBy(p => p.ShirtNumber))
        {
            var isPlayerInFavorites = panel == pnlTeamPlayers &&
                                      favoritePlayersList.Any(fp => fp.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                                                    fp.ShirtNumber == player.ShirtNumber);

            // Check if this player is selected for multi-selection
            var isPlayerSelected = selectedPlayers.Any(sp => sp.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                                             sp.ShirtNumber == player.ShirtNumber);

            var displayText = $"#{player.ShirtNumber} {player.Name} ({player.Position})";

            //playername decorations
            if (player.Captain) displayText += " [🧢]";
            if (isPlayerInFavorites) displayText += " ⭐";
            if (isPlayerSelected) displayText = "✓ " + displayText;

            Panel playerPanel = new Panel
                                {
                                    Location = new Point(10, yPosition),
                                    Size = new Size(280, playerHeight),
                                    BackColor = isPlayerSelected ? Color.LightCyan : (panel == pnlFavPlayers ? Color.LightBlue : Color.LightGray),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Cursor = Cursors.Hand,
                                    Tag = player // Store the player object for drag and drop
                                };

            PictureBox playerImage = new PictureBox
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
            catch (Exception ex)
            {
                playerImage.BackColor = Color.LightGray;
            }

            Label playerLabel = new Label
                                {
                                    Text = displayText,
                                    Location = new Point(imageSize + 10, 5),
                                    Size = new Size(215, playerHeight - 10),
                                    Font = new Font("Arial", 9),
                                    ForeColor = isPlayerSelected ? Color.Blue : (isPlayerInFavorites ? Color.OrangeRed : Color.Black),
                                    TextAlign = ContentAlignment.MiddleLeft
                                };

            ContextMenuStrip contextMenu = CreatePlayerContextMenu(player, isPlayerInFavorites, panel == pnlFavPlayers);

            playerPanel.Controls.Add(playerImage);
            playerPanel.Controls.Add(playerLabel);
            playerPanel.ContextMenuStrip = contextMenu;
            playerImage.ContextMenuStrip = contextMenu;
            playerLabel.ContextMenuStrip = contextMenu;

            //drag and drop for team players only (not favs)
            if (panel == pnlTeamPlayers)
            {
                playerPanel.MouseDown += PlayerPanel_MouseDown;
                playerPanel.Click += PlayerPanel_Click; // Handle selection

                playerLabel.MouseDown += PlayerPanel_MouseDown;
                playerLabel.Click += PlayerPanel_Click;

                playerImage.MouseDown += PlayerPanel_MouseDown;
                playerImage.Click += PlayerPanel_Click;
            }
            else if (panel == pnlFavPlayers)
            {
                //favorite players, add right-click context menu or double-click to remove
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
        ContextMenuStrip contextMenu = new ContextMenuStrip();

        if (!isFavoritePanel)
        {
            if (isInFavorites)
            {
                var removeFavoriteItem = new ToolStripMenuItem("Remove from Favorites");
                removeFavoriteItem.Click += (s, e) => RemoveFromFavorites(player);
                contextMenu.Items.Add(removeFavoriteItem);
            }
            else
            {
                var addFavoriteItem = new ToolStripMenuItem("Add to Favorites");
                addFavoriteItem.Click += (s, e) => AddToFavorites(player);
                contextMenu.Items.Add(addFavoriteItem);
            }

            contextMenu.Items.Add(new ToolStripSeparator());
        }

        var addImageItem = new ToolStripMenuItem("Add/Change Image");
        addImageItem.Click += async (s, e) => await AddPlayerImageAsync(player);
        contextMenu.Items.Add(addImageItem);

        var removeImageItem = new ToolStripMenuItem("Remove Image");
        removeImageItem.Click += async (s, e) => await RemovePlayerImageAsync(player);
        contextMenu.Items.Add(removeImageItem);

        if (isFavoritePanel)
        {
            contextMenu.Items.Add(new ToolStripSeparator());
            var removeFavoriteItem = new ToolStripMenuItem("Remove from Favorites");
            removeFavoriteItem.Click += (s, e) => RemoveFromFavorites(player);
            contextMenu.Items.Add(removeFavoriteItem);
        }

        return contextMenu;
    }

    private async Task AddPlayerImageAsync(StartingEleven player)
    {
        try
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = $"Select Image for {player.Name}";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] imageData = await File.ReadAllBytesAsync(openFileDialog.FileName);
                    var extension = Path.GetExtension(openFileDialog.FileName).ToLower();
                    if (extension == ".jpeg")
                        extension = ".jpg";
                    var fileName = $"{player.Name}{extension}";

                    await imagesRepository.UploadImageAsync(imageData, fileName);

                    MessageBox.Show($"Image successfully added for {player.Name}!", "Image Added",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshFavoritePlayersPanel();
                    RefreshTeamPlayersPanel();
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
        try
        {
            var result = MessageBox.Show($"Remove image for {player.Name}?", "Confirm Image Removal",
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

                MessageBox.Show($"Image removed for {player.Name}!", "Image Removed",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshFavoritePlayersPanel();
                RefreshTeamPlayersPanel();
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
            // Check if Control key is held
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                //TODO HandleMultiSelection(player);
            }
            else
            {
                // Clear selection if Control is not held
                selectedPlayers.Clear();
                RefreshTeamPlayersPanel();
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
            // If we have selected players and Control is held, drag all selected players
            if (selectedPlayers.Count > 0 && ModifierKeys.HasFlag(Keys.Control))
            {
                // Ensure the clicked player is in the selection
                if (!selectedPlayers.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                              p.ShirtNumber == player.ShirtNumber))
                {
                    if (selectedPlayers.Count < 3)
                    {
                        selectedPlayers.Add(player);
                        RefreshTeamPlayersPanel();
                    }
                }

                // Start drag operation with selected players
                var sourceControl = sender as Control;
                sourceControl?.DoDragDrop(selectedPlayers.ToList(), DragDropEffects.Copy);
            }
            else
            {
                // Single player drag
                selectedPlayers.Clear();
                RefreshTeamPlayersPanel();
                var sourceControl = sender as Control;
                sourceControl?.DoDragDrop(player, DragDropEffects.Copy);
            }
        }
    }

    private void PnlFavPlayers_DragEnter(object sender, DragEventArgs e)
    {
        // Check if the dragged data is a StartingEleven player or a list of players
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
        // Handle multiple players
        if (e.Data.GetData(typeof(List<StartingEleven>)) is List<StartingEleven> players)
        {
            AddMultipleToFavorites(players);
            // Clear selection after successful drop
            selectedPlayers.Clear();
            RefreshTeamPlayersPanel();
        }
        // Handle single player
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
            // Check if player is already in favorites
            if (favoritePlayersList.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                             p.ShirtNumber == player.ShirtNumber))
            {
                duplicatePlayers.Add(player.Name);
                continue;
            }

            // Check if adding this player would exceed the limit
            if (favoritePlayersList.Count + playersToAdd.Count >= 3)
            {
                break;
            }

            playersToAdd.Add(player);
        }

        // Check if we can add any players
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

        // Add the players
        favoritePlayersList.AddRange(playersToAdd);

        // Show feedback
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

        RefreshFavoritePlayersPanel();
        RefreshTeamPlayersPanel();
        SaveFavouritePlayersAsync();
    }

    private void AddToFavorites(StartingEleven player)
    {
        if (favoritePlayersList.Count >= 3) // Maximum 3 favourite players
        {
            MessageBox.Show("You can only have a maximum of 3 favourite players!", "Maximum Reached",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!favoritePlayersList.Any(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                          p.ShirtNumber == player.ShirtNumber))
        {
            favoritePlayersList.Add(player);
            RefreshFavoritePlayersPanel();
            RefreshTeamPlayersPanel(); // Refresh team panel to update visual indicators
            SaveFavouritePlayersAsync(); // Save to file
        }
        else
        {
            MessageBox.Show($"{player.Name} is already in your favorites!", "Duplicate Player",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void RemoveFromFavorites(StartingEleven player)
    {
        var result = MessageBox.Show($"Remove {player.Name} from favorites?", "Confirm Removal",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            favoritePlayersList.RemoveAll(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase) &&
                                               p.ShirtNumber == player.ShirtNumber);
            RefreshFavoritePlayersPanel();
            RefreshTeamPlayersPanel();
            SaveFavouritePlayersAsync();
        }
    }

    private void RefreshFavoritePlayersPanel()
    {
        if (favoritePlayersList.Any())
        {
            PopulatePlayersPanel(pnlFavPlayers, favoritePlayersList);
        }
        else
        {
            pnlFavPlayers.Controls.Clear();
            Label emptyLabel = new Label
                               {
                                   Text = "Drag (or hold Ctrl) players here to add to favorites\n\n(Double-click favorite players to remove)\n\nMaximum: 3 players",
                                   Location = new Point(10, 10),
                                   Size = new Size(300, 100),
                                   Font = new Font("Arial", 9),
                                   ForeColor = Color.Gray,
                                   TextAlign = ContentAlignment.MiddleCenter
                               };
            pnlFavPlayers.Controls.Add(emptyLabel);
        }
    }

    private void RefreshTeamPlayersPanel()
    {
        if (cmbTeams.SelectedItem is Teams selectedTeam)
        {
            LoadTeamPlayersAsync(selectedTeam.FifaCode);
        }
    }

    private void cmbTeams_SelectedIndexChanged_1(object sender, EventArgs e)
    {
        selectedPlayers.Clear();

        if (cmbTeams.SelectedItem is Teams selectedTeam)
        {
            LoadTeamPlayersAsync(selectedTeam.FifaCode);
        }
    }

    private async void LoadTeamsAsync()
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading teams: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void LoadTeamPlayersAsync(string fifaCode)
    {
        try
        {
            var players = await repository.GetPlayersByTeamAsync(state.SelectedGender, fifaCode);

            if (players?.Any() == true)
            {
                PopulatePlayersPanel(pnlTeamPlayers, players);
            }
            else
            {
                pnlTeamPlayers.Controls.Clear();
                Label noPlayersLabel = new Label
                                       {
                                           Text = "No player data available for this team",
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
        var settingsForm = new SettingsForm(state);
        var result = settingsForm.ShowDialog();

        if (result == DialogResult.OK || result == DialogResult.Cancel)
        {
            UpdateStateLabel();

            await SaveCurrentSettingsAsync();

            LoadTeamsAsync();
        }
    }

    private void btnRankings_Click(object sender, EventArgs e)
    {
        state.SelectedFifaCode = (cmbTeams.SelectedItem as Teams)?.FifaCode ?? string.Empty;
        new RankForm(state).ShowDialog();
    }
}
