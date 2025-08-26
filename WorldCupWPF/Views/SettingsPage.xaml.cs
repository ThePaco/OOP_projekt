using DAL.Repository;
using System.Windows;
using System.Windows.Controls;
using WorldCupWPF.Models;
using WorldCupWPF.ViewModels;

namespace WorldCupWPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly State state;
        private readonly IUserSettingsRepo userSettingsRepo = new UserSettingsRepo();
        private readonly SettingsViewModel viewModel;

        public SettingsPage(State state)
        {
            this.state = state;
            InitializeComponent();
            viewModel = new SettingsViewModel(state, new UserSettingsRepo());
            DataContext = viewModel;

            viewModel.ExitRequested += (_, e) => OnCancelRequested(e);
        }

        private void OnCancelRequested(bool shouldNavigateToMatchSelect)
        {
            if (shouldNavigateToMatchSelect)
            {
                SetResolution();
                NavigationService?.Navigate(new MatchSelectPage());
            }
            else
            {
                NavigationService?.GoBack();
            }
        }

        private void SetResolution()
        {
            var resolution = ((SettingsViewModel)DataContext).State.Resolution;

            if (resolution == DAL.Models.Enums.Resolution.Fullscreen)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                switch (resolution)
                {
                    case DAL.Models.Enums.Resolution.w1920_h1080:
                        Application.Current.MainWindow.Height = 1080;
                        Application.Current.MainWindow.Width = 1920;
                        break;
                    case DAL.Models.Enums.Resolution.w1280_h720:
                        Application.Current.MainWindow.Height = 720;
                        Application.Current.MainWindow.Width = 1280;
                        break;
                    case DAL.Models.Enums.Resolution.w900_h720:
                        Application.Current.MainWindow.Height = 720;
                        Application.Current.MainWindow.Width = 900;
                        break;
                }
            }
        }
    }
}