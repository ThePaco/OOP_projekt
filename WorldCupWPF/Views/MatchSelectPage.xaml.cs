using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using WorldCupWPF.Model;
using WorldCupWPF.UserControls;
using WorldCupWPF.Windows;

namespace WorldCupWPF.Views
{
    /// <summary>
    /// Interaction logic for MatchSelectPage.xaml
    /// </summary>
    public partial class MatchSelectPage : Page, INotifyPropertyChanged
    {
        private readonly IMatchDataRepo matchDataRepo = new LocalMatchDataRepo();
        private readonly IUserSettingsRepo userSettingsRepo = new UserSettingsRepo();
        private readonly State state = new();

        private ObservableCollection<Teams> homeTeamsList = [];
        private ObservableCollection<Teams> opposingTeamsList = [];
        public ObservableCollection<Teams> Teams
        {
            get => homeTeamsList;
            set
            {
                homeTeamsList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Teams> TeamsOpposing
        {
            get => opposingTeamsList;
            set
            {
                opposingTeamsList = value;
                OnPropertyChanged();
            }
        }

        public MatchSelectPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadSettingsAsync();
            LoadTeamsAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadSettingsAsync()
        {
            if (!userSettingsRepo.HasSavedSettings())
            {
                var settingsWindow = new SettingsWindow(state);
                settingsWindow.ShowDialog();
                return;
            }
            try
            {
                var userSettings = await userSettingsRepo.GetUserSettingsAsync();
                ApplySettingsToState(userSettings);
            }
            catch (Exception ex) { }
        }

        private void ApplySettingsToState(DAL.Models.UserSettings userSettings)
        {
            if (userSettings is null)
            {
                var settingsWindow = new SettingsWindow(state);
                settingsWindow.ShowDialog();
                return;
            }

            state.Language = userSettings.Language;
            state.Gender = userSettings.Gender;
            state.Resolution = userSettings.Resolution;
            state.FifaCode = userSettings.FifaCode;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(state);
            settingsWindow.ShowDialog();
        }

        private async Task LoadTeamsAsync()
        {
            //todo: some female teams have no opponents, handle that case
            var teams = await matchDataRepo.GetTeams(state.Gender);
            teams = teams.OrderBy(t => t.Country).ToArray();
            
            Teams = new ObservableCollection<Teams>(teams);
        }
        private async void cbHomeTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await LoadOpponentsAsync();
            await LoadPlayersAsync();
        }

        private async Task LoadOpponentsAsync()
        {
            if (cbHomeTeam.SelectedItem is not Teams selectedHomeTeam)
                return;

            var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
            var opps = matches.Where(m => m.HomeTeam.Code == selectedHomeTeam.FifaCode || m.AwayTeam.Code == selectedHomeTeam.FifaCode)
                             .Select(m => m.HomeTeam.Code == selectedHomeTeam.FifaCode ? m.AwayTeam : m.HomeTeam)
                             .Where(team => team.Code != selectedHomeTeam.FifaCode)
                             .DistinctBy(team => team.Code)
                             .OrderBy(team => team.Country)
                             .Select(team => new Teams 
                             { 
                                 Country = team.Country, 
                                 FifaCode = team.Code,
                             })
                             .ToList();

            TeamsOpposing = new ObservableCollection<Teams>(opps);
        }

        private async void cbOpposingTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHomeTeam.SelectedItem is not Teams selectedHomeTeam || 
                cbOpposingTeam.SelectedItem is not Teams selectedOpposingTeam)
            {
                lblVS.Content = "VS";
                ClearPlayerPositions();
                return;
            }

            try
            {
                var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
                var match = matches.FirstOrDefault(m => 
                    (m.HomeTeam.Code == selectedHomeTeam.FifaCode && m.AwayTeam.Code == selectedOpposingTeam.FifaCode) ||
                    (m.HomeTeam.Code == selectedOpposingTeam.FifaCode && m.AwayTeam.Code == selectedHomeTeam.FifaCode));

                if (match != null)
                {
                    if (match.HomeTeam.Code == selectedHomeTeam.FifaCode)
                    {
                        lblVS.Content = $"{match.HomeTeam.Goals}:{match.AwayTeam.Goals}";
                    }
                    else
                    {
                        lblVS.Content = $"{match.AwayTeam.Goals}:{match.HomeTeam.Goals}";
                    }
                    
                    await LoadPlayersForMatchAsync(match, selectedHomeTeam, selectedOpposingTeam);
                }
                else
                {
                    lblVS.Content = "VS";
                    ClearPlayerPositions();
                }
            }
            catch (Exception ex)
            {
                lblVS.Content = "VS";
                ClearPlayerPositions();
            }
        }

        private async Task LoadPlayersAsync()
        {
            if (cbHomeTeam.SelectedItem is Teams selectedHomeTeam && 
                cbOpposingTeam.SelectedItem is Teams selectedOpposingTeam)
            {
                try
                {
                    var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
                    var match = matches.FirstOrDefault(m => 
                        (m.HomeTeam.Code == selectedHomeTeam.FifaCode && m.AwayTeam.Code == selectedOpposingTeam.FifaCode) ||
                        (m.HomeTeam.Code == selectedOpposingTeam.FifaCode && m.AwayTeam.Code == selectedHomeTeam.FifaCode));

                    if (match != null)
                    {
                        await LoadPlayersForMatchAsync(match, selectedHomeTeam, selectedOpposingTeam);
                    }
                }
                catch (Exception ex)
                {
                    // Handle error silently or log it
                }
            }
        }

        private async Task LoadPlayersForMatchAsync(DAL.Models.Matches match, Teams homeTeam, Teams opposingTeam)
        {
            ClearPlayerPositions();

            try
            {
                // Determine which team is home and away in the match
                var isHomeTeamFirst = match.HomeTeam.Code == homeTeam.FifaCode;
                
                var homeMatchStats = isHomeTeamFirst ? match.HomeTeamStatistics : match.AwayTeamStatistics;
                var opposingMatchStats = isHomeTeamFirst ? match.AwayTeamStatistics : match.HomeTeamStatistics;

                // Load home team players
                await LoadTeamPlayers(homeMatchStats.StartingEleven, true);
                
                // Load opposing team players
                await LoadTeamPlayers(opposingMatchStats.StartingEleven, false);
            }
            catch (Exception ex)
            {
                // Handle error silently or log it
            }
        }

        private async Task LoadTeamPlayers(IEnumerable<StartingEleven> players, bool isHomeTeam)
        {
            var playersByPosition = players.GroupBy(p => p.Position).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var positionGroup in playersByPosition)
            {
                var position = positionGroup.Key;
                var positionPlayers = positionGroup.Value;

                StackPanel? targetPanel = position switch
                {
                    Position.Goalie => isHomeTeam ? HomeGoaliePosition : OpposingGoaliePosition,
                    Position.Defender => isHomeTeam ? HomeDefenderPositions : OpposingDefenderPositions,
                    Position.Midfield => isHomeTeam ? HomeMidfieldPositions : OpposingMidfieldPositions,
                    Position.Forward => isHomeTeam ? HomeForwardPositions : OpposingForwardPositions,
                    _ => null
                };

                if (targetPanel != null)
                {
                    foreach (var player in positionPlayers)
                    {
                        var playerCard = new PlayerCard(player);
                        playerCard.Margin = new Thickness(1); // Add some spacing between players
                        targetPanel.Children.Add(playerCard);
                    }
                }
            }
        }

        private void ClearPlayerPositions()
        {
            HomeGoaliePosition.Children.Clear();
            HomeDefenderPositions.Children.Clear();
            HomeMidfieldPositions.Children.Clear();
            HomeForwardPositions.Children.Clear();

            OpposingGoaliePosition.Children.Clear();
            OpposingDefenderPositions.Children.Clear();
            OpposingMidfieldPositions.Children.Clear();
            OpposingForwardPositions.Children.Clear();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (cbHomeTeam.SelectedItem is not Teams selectedHomeTeam || 
                cbOpposingTeam.SelectedItem is not Teams selectedOpposingTeam)
            {
                MessageBox.Show("Please select both home and opposing teams before viewing match details.", 
                               "Teams Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var matchDetailsPage = new MatchDetailsPage(state, selectedHomeTeam, selectedOpposingTeam);
            NavigationService?.Navigate(matchDetailsPage);
        }
    }
}
