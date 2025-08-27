using System.Windows;
using DAL.Models;
using DAL.Models.Enums;
using WorldCupWPF.ViewModels;

namespace WorldCupWPF.Windows
{
    /// <summary>
    /// Interaction logic for PlayerDetailsWindow.xaml
    /// </summary>
    public partial class PlayerDetailsWindow : Window
    {
        public PlayerDetailsWindow(StartingEleven player)
        {
            InitializeComponent();
            var viewModel= new PlayerDetailsViewModel(player);
            DataContext = viewModel;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
