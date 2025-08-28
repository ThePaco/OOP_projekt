using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WorldCupWPF.Commands;
using WorldCupWPF.Models;

namespace WorldCupWPF.ViewModels;

public class MatchSelectViewModel : BaseViewModel
{
    private readonly IMatchDataRepo matchDataRepo;
    private readonly IUserSettingsRepo userSettingsRepo;
    private readonly State state;
    public State State => state;
    public int MatchId { get; set; }

    private string vsLabelContent = "VS";
    private bool isInitialized;

    private ObservableCollection<Teams> teams = [];
    private ObservableCollection<Teams> opposingTeams = [];
    private ObservableCollection<StartingEleven> homeGoaliePlayers = [];
    private ObservableCollection<StartingEleven> homeDefenderPlayers = [];
    private ObservableCollection<StartingEleven> homeMidfieldPlayers = [];
    private ObservableCollection<StartingEleven> homeForwardPlayers = [];
    private ObservableCollection<StartingEleven> opposingGoaliePlayers = [];
    private ObservableCollection<StartingEleven> opposingDefenderPlayers = [];
    private ObservableCollection<StartingEleven> opposingMidfieldPlayers = [];
    private ObservableCollection<StartingEleven> opposingForwardPlayers = [];

    public MatchSelectViewModel() : this(new LocalMatchDataRepo(), new UserSettingsRepo(), new State())
    {

    }

    public MatchSelectViewModel(IMatchDataRepo matchDataRepo, IUserSettingsRepo userSettingsRepo, State state)
    {
        this.matchDataRepo = matchDataRepo;
        this.userSettingsRepo = userSettingsRepo;
        this.state = state;

        InitializeCommand = new RelayCommand(async () => await InitializeAsync());

        NavigateToSettingsCommand = new RelayCommand(() => NavigateToSettingsRequested?.Invoke(this, EventArgs.Empty));

        NavigateToDetailsCommand = new RelayCommand(() => NavigateToDetailsRequested?.Invoke(this, new NavigateToDetailsEventArgs(SelectedHomeTeam!, SelectedOpposingTeam!)),
                                                    () => SelectedHomeTeam is not null && SelectedOpposingTeam is not null);
    }

    public ObservableCollection<Teams> Teams
    {
        get => teams;
        set
        {
            teams = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Teams> OpposingTeams
    {
        get => opposingTeams;
        set
        {
            opposingTeams = value;
            OnPropertyChanged();
        }
    }

    private Teams? selectedHomeTeam;

    public Teams? SelectedHomeTeam
    {
        get => selectedHomeTeam;
        set
        {
            selectedHomeTeam = value;
            Task.Run(async () => await LoadOpponentsAsync());
        }
    }

    private Teams? selectedOpposingTeam;

    public Teams? SelectedOpposingTeam
    {
        get => selectedOpposingTeam;
        set
        {
            selectedOpposingTeam = value;
            Task.Run(async () =>
            {
                await UpdateMatchInfoAsync();
                await LoadPlayersAsync();
            });
        }
    }

    public string VsLabelContent
    {
        get => vsLabelContent;
        set
        {
            vsLabelContent = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<StartingEleven> HomeGoaliePlayers
    {
        get => homeGoaliePlayers;
        set => homeGoaliePlayers = value;
    }

    public ObservableCollection<StartingEleven> HomeDefenderPlayers
    {
        get => homeDefenderPlayers;
        set => homeDefenderPlayers = value;
    }

    public ObservableCollection<StartingEleven> HomeMidfieldPlayers
    {
        get => homeMidfieldPlayers;
        set => homeMidfieldPlayers = value;
    }

    public ObservableCollection<StartingEleven> HomeForwardPlayers
    {
        get => homeForwardPlayers;
        set => homeForwardPlayers = value;
    }

    public ObservableCollection<StartingEleven> OpposingGoaliePlayers
    {
        get => opposingGoaliePlayers;
        set => opposingGoaliePlayers = value;
    }

    public ObservableCollection<StartingEleven> OpposingDefenderPlayers
    {
        get => opposingDefenderPlayers;
        set => opposingDefenderPlayers = value;
    }

    public ObservableCollection<StartingEleven> OpposingMidfieldPlayers
    {
        get => opposingMidfieldPlayers;
        set => opposingMidfieldPlayers = value;
    }

    public ObservableCollection<StartingEleven> OpposingForwardPlayers
    {
        get => opposingForwardPlayers;
        set => opposingForwardPlayers = value;
    }

    public ICommand NavigateToSettingsCommand { get; }
    public ICommand NavigateToDetailsCommand { get; }
    public ICommand InitializeCommand { get; }

    public event EventHandler? NavigateToSettingsRequested;
    public event EventHandler<NavigateToDetailsEventArgs>? NavigateToDetailsRequested;

    public async Task InitializeAsync()
    {
        if (isInitialized)
            return;

        try
        {
            var hasSettings = await LoadSettingsAsync();
            if (hasSettings)
            {
                await LoadTeamsAsync();
                isInitialized = true;

                SelectedHomeTeam = teams[0];
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load the App!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task<bool> LoadSettingsAsync()
    {
        if (!userSettingsRepo.HasSavedSettings())
        {
            NavigateToSettingsRequested?.Invoke(this, EventArgs.Empty);
            return false;
        }

        try
        {
            var userSettings = await userSettingsRepo.GetUserSettingsAsync();
            if (!ApplySettingsToState(userSettings))
            {
                NavigateToSettingsRequested?.Invoke(this, EventArgs.Empty);
                return false;
            }
            return true;
        }
        catch (Exception)
        {
            NavigateToSettingsRequested?.Invoke(this, EventArgs.Empty);
            return false;
        }
    }

    private bool ApplySettingsToState(UserSettings? userSettings)
    {
        if (userSettings is null)
            return false;

        state.Language = userSettings.Language;
        state.Gender = userSettings.Gender;
        state.Resolution = userSettings.Resolution;
        state.FifaCode = userSettings.FifaCode;
        return true;
    }

    private async Task LoadTeamsAsync()
    {
        try
        {
            var teams = await matchDataRepo.GetTeams(state.Gender);
            teams = teams.OrderBy(t => t.Country).ToArray();
            Teams = new ObservableCollection<Teams>(teams);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load teams: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private async Task LoadOpponentsAsync()
    {
        if (SelectedHomeTeam is null)
        {
            OpposingTeams.Clear();
            return;
        }

        try
        {
            var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
            var opponents = matches
                .Where(m => m.HomeTeam.Code == SelectedHomeTeam.FifaCode || m.AwayTeam.Code == SelectedHomeTeam.FifaCode)
                .Select(m => m.HomeTeam.Code == SelectedHomeTeam.FifaCode ? m.AwayTeam : m.HomeTeam)
                .Where(team => team.Code != SelectedHomeTeam.FifaCode)
                .DistinctBy(team => team.Code)
                .OrderBy(team => team.Country)
                .Select(team => new Teams
                {
                    Country = team.Country,
                    FifaCode = team.Code,
                })
                .ToList();

            OpposingTeams = new ObservableCollection<Teams>(opponents);
        }
        catch (Exception e)
        {
            OpposingTeams.Clear();
            MessageBox.Show($"Failed to load opponents: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task UpdateMatchInfoAsync()
    {
        if (SelectedHomeTeam is null || SelectedOpposingTeam is null)
        {
            VsLabelContent = "VS";
            ClearPlayerPositions();
            return;
        }

        try
        {
            var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
            var match = matches.FirstOrDefault(m =>
                (m.HomeTeam.Code == SelectedHomeTeam.FifaCode && m.AwayTeam.Code == SelectedOpposingTeam.FifaCode) ||
                (m.HomeTeam.Code == SelectedOpposingTeam.FifaCode && m.AwayTeam.Code == SelectedHomeTeam.FifaCode));

            if (match != null)
            {
                VsLabelContent = match.HomeTeam.Code == SelectedHomeTeam.FifaCode
                    ? $"{match.HomeTeam.Goals}:{match.AwayTeam.Goals}"
                    : $"{match.AwayTeam.Goals}:{match.HomeTeam.Goals}";

                MatchId = (int)match.FifaId;
            }
            else
            {
                VsLabelContent = "VS";
                ClearPlayerPositions();
            }
        }
        catch (Exception e)
        {
            VsLabelContent = "VS";
            ClearPlayerPositions();
            MessageBox.Show($"Failed to update match: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadPlayersAsync()
    {
        if (SelectedHomeTeam is null || SelectedOpposingTeam is null)
        {
            ClearPlayerPositions();
            return;
        }

        try
        {
            var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
            var match = matches.FirstOrDefault(m =>
                (m.HomeTeam.Code == SelectedHomeTeam.FifaCode && m.AwayTeam.Code == SelectedOpposingTeam.FifaCode) ||
                (m.HomeTeam.Code == SelectedOpposingTeam.FifaCode && m.AwayTeam.Code == SelectedHomeTeam.FifaCode));

            if (match != null)
            {
                Application.Current.Dispatcher.Invoke(() => LoadPlayersForMatch(match));
                //LoadPlayersForMatch(match);
            }
            else
            {
                ClearPlayerPositions();
            }
        }
        catch (Exception)
        {
            ClearPlayerPositions();
        }
    }

    private void LoadPlayersForMatch(Matches match)
    {
        ClearPlayerPositions();

        try
        {
            var isHomeTeamFirst = match.HomeTeam.Code == SelectedHomeTeam!.FifaCode;
            var homeMatchStats = isHomeTeamFirst ? match.HomeTeamStatistics : match.AwayTeamStatistics;
            var opposingMatchStats = isHomeTeamFirst ? match.AwayTeamStatistics : match.HomeTeamStatistics;

            LoadTeamPlayers(homeMatchStats.StartingEleven, true);
            LoadTeamPlayers(opposingMatchStats.StartingEleven, false);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Failed to load players: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadTeamPlayers(IEnumerable<StartingEleven> players, bool isHomeTeam)
    {
        var playersByPosition = players.GroupBy(p => p.Position).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var positionGroup in playersByPosition)
        {
            var targetCollection = GetPlayerCollectionForPosition(positionGroup.Key, isHomeTeam);
            if (targetCollection != null)
            {
                foreach (var player in positionGroup.Value)
                {
                    targetCollection.Add(player);
                }
            }
        }
    }

    private ObservableCollection<StartingEleven>? GetPlayerCollectionForPosition(Position position, bool isHomeTeam)
    {
        return position switch
        {
            Position.Goalie => isHomeTeam ? HomeGoaliePlayers : OpposingGoaliePlayers,
            Position.Defender => isHomeTeam ? HomeDefenderPlayers : OpposingDefenderPlayers,
            Position.Midfield => isHomeTeam ? HomeMidfieldPlayers : OpposingMidfieldPlayers,
            Position.Forward => isHomeTeam ? HomeForwardPlayers : OpposingForwardPlayers,
            _ => null
        };
    }

    private void ClearPlayerPositions()
    {
        HomeGoaliePlayers.Clear();
        HomeDefenderPlayers.Clear();
        HomeMidfieldPlayers.Clear();
        HomeForwardPlayers.Clear();

        OpposingGoaliePlayers.Clear();
        OpposingDefenderPlayers.Clear();
        OpposingMidfieldPlayers.Clear();
        OpposingForwardPlayers.Clear();
    }
}

public class NavigateToDetailsEventArgs : EventArgs
{
    public Teams HomeTeam { get; }
    public Teams OpposingTeam { get; }

    public NavigateToDetailsEventArgs(Teams homeTeam, Teams opposingTeam)
    {
        HomeTeam = homeTeam;
        OpposingTeam = opposingTeam;
    }
}
