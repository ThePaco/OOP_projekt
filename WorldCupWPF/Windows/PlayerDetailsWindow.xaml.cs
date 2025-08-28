using DAL.Models;
using DAL.Models.Enums;
using System.Windows;
using System.Windows.Input;
using WorldCupWPF.ViewModels;

namespace WorldCupWPF.Windows
{
    /// <summary>
    /// Interaction logic for PlayerDetailsWindow.xaml
    /// </summary>
    public partial class PlayerDetailsWindow : Window
    {
        public PlayerDetailsWindow(StartingEleven player, Gender gender, int matchId)
        {
            InitializeComponent();
            var viewModel= new PlayerDetailsViewModel()
                           {
                               Player = player,
                               Gender = gender,
                               MatchId = matchId
            };
            DataContext = viewModel;

            this.KeyDown += PlayerWindow_KeyDown;
            this.Loaded += PlayerWindow_Loaded;

        }
        private void PlayerWindow_Loaded(object sender, RoutedEventArgs e) => this.Focus();
        private void PlayerWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        private void btnOK_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
