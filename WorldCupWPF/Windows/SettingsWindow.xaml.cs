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
using System.Windows.Shapes;
using DAL.Models.Enums;
using DAL.Repository;
using WorldCupWPF.Model;

namespace WorldCupWPF.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly State state;
        private readonly IUserSettingsRepo userSettingsRepo = new UserSettingsRepo();
        public SettingsWindow(State state)
        {
            this.state = state;
            InitializeComponent();
            LoadSettings();
        }

        private async Task LoadSettings()
        {
            
            if (!userSettingsRepo.HasSavedSettings())
            {
                cmbGender.SelectedIndex = -1;
                cmbLanguage.SelectedIndex = -1;
                cmbResolution.SelectedIndex = -1;
                return;
            }
            try
            {
                var settings = await userSettingsRepo.GetUserSettingsAsync();
                cmbGender.SelectedIndex = (int)settings.Gender;
                cmbLanguage.SelectedIndex = (int)settings.Language;
                cmbResolution.SelectedIndex = (int)settings.Resolution;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to load settings: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveSettings()
        {
            if (cmbGender.SelectedIndex < 0 || cmbLanguage.SelectedIndex < 0 || cmbResolution.SelectedIndex < 0)
            {
                lblWarning.Visibility = Visibility.Visible;
            }
            else
            {
                var userSettings = new DAL.Models.UserSettings
                                          {
                                              Gender = cmbGender.SelectedIndex == 0 ? Gender.Men : Gender.Women,
                                              DataSource = DataSource.Local,
                                              Language = cmbLanguage.SelectedIndex == 0 ? DAL.Models.Enums.Language.English : DAL.Models.Enums.Language.Croatian,
                                              Resolution = cmbResolution.SelectedIndex switch
                                              {
                                                  0 => Resolution.Fullscreen,
                                                    1 => Resolution.w1920_h1080,
                                                  2 => Resolution.w1280_h720,
                                                    3 => Resolution.w900_h720,
                                                  _ => Resolution.w1920_h1080,
                                              },
                                          };

                state.Language = userSettings.Language;
                state.Gender = userSettings.Gender;
                state.Resolution = userSettings.Resolution;
                state.FifaCode = userSettings.FifaCode;

                try
                {
                    await userSettingsRepo.SaveUserSettingsAsync(userSettings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Close();
                }
            }
            
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e) => await SaveSettings();

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}