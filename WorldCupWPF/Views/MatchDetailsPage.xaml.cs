using System;
using System.Collections.Generic;
using System.Linq;
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
using DAL.Repository;
using WorldCupWPF.Model;

namespace WorldCupWPF.Views
{
    /// <summary>
    /// Interaction logic for MatchDetailsPage.xaml
    /// </summary>
    public partial class MatchDetailsPage : Page
    {
        private readonly State state;
        private readonly Teams homeTeam;
        private readonly Teams opTeam;
        private readonly IMatchDataRepo matchDataRepo = new LocalMatchDataRepo();
        public MatchDetailsPage(State state,Teams homeTeam, Teams opTeam)
        {
            this.state = state;
            this.homeTeam = homeTeam;
            this.opTeam = opTeam;
            InitializeComponent();
            FillDetails();
        }

        public async Task FillDetails()
        {
            lblTeam.Content = homeTeam.Country;
            lblTeamOpps.Content = opTeam.Country;

            var homeStats = await matchDataRepo.GetTeamStats(state.Gender, homeTeam.FifaCode);
            var opStats = await matchDataRepo.GetTeamStats(state.Gender, opTeam.FifaCode);

            lblWins.Content = homeStats.Wins;
            lblLosses.Content = homeStats.Losses;
            lblDraws.Content = homeStats.Draws;
            lblGoalsScored.Content = homeStats.GoalsFor;
            lblGoalsTaken.Content = homeStats.GoalsAgainst;
            lblGoalsDiff.Content = homeStats.GoalDifferential;

            lblWinsOpps.Content = opStats.Wins;
            lblLossesOpps.Content = opStats.Losses;
            lblDrawsOpps.Content = opStats.Draws;
            lblGoalsScoredOpps.Content = opStats.GoalsFor;
            lblGoalsTakenOpps.Content = opStats.GoalsAgainst;
            lblGoalsDiffOpps.Content = opStats.GoalDifferential;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            //todo: return to previous state
        }
    }
}

