using System;
using System.Windows;
using System.Windows.Controls;
using DAL.Models;
using WorldCupWPF.Models;
using WorldCupWPF.ViewModels;

namespace WorldCupWPF.Views
{
    /// <summary>
    /// Interaction logic for MatchDetailsPage.xaml
    /// </summary>
    public partial class MatchDetailsPage : Page
    {
        public MatchDetailsPage(State state, Teams homeTeam, Teams opTeam)
        {
            InitializeComponent();
            DataContext = new MatchDetailsViewModel(state, homeTeam, opTeam);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hr-HR");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
    }
}

