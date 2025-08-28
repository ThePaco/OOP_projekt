using DAL.Repository;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            System.Threading.Thread.CurrentThread.CurrentUICulture = state.Language == DAL.Models.Enums.Language.English ?
                                                                         new System.Globalization.CultureInfo("en-US") :
                                                                         new System.Globalization.CultureInfo("hr-HR");

            viewModel.ExitRequested += (_, e) => OnCancelRequested(e);

            this.Focusable = true;
            this.KeyDown += SettingsPage_KeyDown;
            this.Loaded += SettingsPage_Loaded;
        }
        private void SettingsPage_Loaded(object sender, RoutedEventArgs e) => this.Focus();

        private void SettingsPage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (viewModel.CancelCommand.CanExecute(null))
                    {
                        viewModel.CancelCommand.Execute(null);
                    }
                    e.Handled = true;
                    break;

                case Key.Enter:
                    if (viewModel.SaveCommand.CanExecute(null))
                    {
                        viewModel.SaveCommand.Execute(null);
                    }
                    e.Handled = true;
                    break;
            }
        }

        private void OnCancelRequested(bool shouldNavigateToMatchSelect)
        {
            if (shouldNavigateToMatchSelect)
            {
                SetResolution();
                NavigationService?.Navigate(new MatchSelectPage(state));
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