using System.Windows.Controls;
using WorldCupWPF.ViewModels;
using WorldCupWPF.Models;
using WorldCupWPF.UserControls;
using DAL.Models;

namespace WorldCupWPF.Views
{
    /// <summary>
    /// Interaction logic for MatchSelectPage.xaml
    /// </summary>
    public partial class MatchSelectPage : Page
    {
        private MatchSelectViewModel ViewModel => (MatchSelectViewModel)DataContext;

        public MatchSelectPage()
        {
            InitializeComponent();

            var viewModel = new MatchSelectViewModel();
            DataContext = viewModel;
            viewModel.InitializeAsync();

            //navigation events subs
            viewModel.NavigateToSettingsRequested += OnNavigateToSettings;
            viewModel.NavigateToDetailsRequested += OnNavigateToDetails;

            //collection changes for all player positions
            ViewModel.HomeGoaliePlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(HomeGoaliePositions, ViewModel.HomeGoaliePlayers);
            ViewModel.HomeDefenderPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(HomeDefenderPositions, ViewModel.HomeDefenderPlayers);
            ViewModel.HomeMidfieldPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(HomeMidfieldPositions, ViewModel.HomeMidfieldPlayers);
            ViewModel.HomeForwardPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(HomeForwardPositions, ViewModel.HomeForwardPlayers);

            ViewModel.OpposingGoaliePlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(OpposingGoaliePositions, ViewModel.OpposingGoaliePlayers);
            ViewModel.OpposingDefenderPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(OpposingDefenderPositions, ViewModel.OpposingDefenderPlayers);
            ViewModel.OpposingMidfieldPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(OpposingMidfieldPositions, ViewModel.OpposingMidfieldPlayers);
            ViewModel.OpposingForwardPlayers.CollectionChanged += (s, e) => UpdatePlayerPositions(OpposingForwardPositions, ViewModel.OpposingForwardPlayers);
        }

        public MatchSelectPage(State state) : this()
        {

        }

        private void UpdatePlayerPositions(StackPanel stackPanel, System.Collections.ObjectModel.ObservableCollection<StartingEleven> players)
        {
            stackPanel.Children.Clear();

            foreach (var player in players)
            {
                var playerCard = new PlayerCard(player);
                stackPanel.Children.Add(playerCard);
            }
        }

        private void OnNavigateToSettings(object? sender, EventArgs e)
        {
            var settingsPage = new SettingsPage(ViewModel.State);
            NavigationService?.Navigate(settingsPage);
        }

        private void OnNavigateToDetails(object? sender, NavigateToDetailsEventArgs e)
        {
            var matchDetailsPage = new MatchDetailsPage(ViewModel.State, e.HomeTeam, e.OpposingTeam);
            NavigationService?.Navigate(matchDetailsPage);
        }

        private void cbHomeTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MatchSelectViewModel)?.OnHomeTeamSelectionChanged();
        }

        private void cbOppTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MatchSelectViewModel)?.OnOpposingTeamSelectionChanged();
        }
    }
}
