using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using System.Windows;
using System.Windows.Input;
using WorldCupWPF.Commands;
using WorldCupWPF.Models;

namespace WorldCupWPF.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly IUserSettingsRepo userSettingsRepo;

    public SettingsViewModel(State state, IUserSettingsRepo userSettingsRepo)
    {
        this.state = state;
        this.userSettingsRepo = userSettingsRepo;

        SaveCommand = new RelayCommand(async () => await SaveSettingsAsync(),
                                       () => SelectedGenderIndex >= 0 && SelectedLanguageIndex >= 0 && SelectedResolutionIndex >= 0);
        
        CancelCommand = new RelayCommand(() => ExitRequested?.Invoke(this, false), userSettingsRepo.HasSavedSettings);

        LoadSettingsAsync();
    }

    public event EventHandler<bool>? ExitRequested;

    private readonly State state;
    public State State => state;

    private int selectedGenderIndex = -1;

    public int SelectedGenderIndex
    {
        get => selectedGenderIndex;
        set
        {
            selectedGenderIndex = value;
            OnPropertyChanged();
        }
    }

    private int selectedLanguageIndex = -1;

    public int SelectedLanguageIndex
    {
        get => selectedLanguageIndex;
        set
        {
            selectedLanguageIndex = value;
            OnPropertyChanged();
        }
    }

    private int selectedResolutionIndex = -1;

    public int SelectedResolutionIndex
    {
        get => selectedResolutionIndex;
        set
        {
            selectedResolutionIndex = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand SaveCommand { get; }

    public ICommand CancelCommand { get; }

    private async Task LoadSettingsAsync()
    {
        if (!userSettingsRepo.HasSavedSettings())
        {
            SelectedGenderIndex = -1;
            SelectedLanguageIndex = -1;
            SelectedResolutionIndex = -1;
            return;
        }

        try
        {
            var settings = await userSettingsRepo.GetUserSettingsAsync();
            SelectedGenderIndex = (int)settings.Gender;
            SelectedLanguageIndex = (int)settings.Language;
            SelectedResolutionIndex = (int)settings.Resolution;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Failed to load settings: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveSettingsAsync()
    {
        var userSettings = new UserSettings
                           {
                               Gender = SelectedGenderIndex == 0 ? Gender.Men : Gender.Women,
                               DataSource = DataSource.Local,
                               Language = SelectedLanguageIndex == 0 ? Language.English : Language.Croatian,
                               Resolution = SelectedResolutionIndex switch
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
            ExitRequested?.Invoke(this, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
