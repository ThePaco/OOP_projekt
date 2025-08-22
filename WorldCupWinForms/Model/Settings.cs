using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Enums;

namespace WorldCupWinForms.Model;
public class State
{
    public event EventHandler? OnSettingsChanged;

    private Language selectedLanguage;

    public Language SelectedLanguage
    {
        get => selectedLanguage;
        set
        {
            if(selectedLanguage == value)
                return;

            selectedLanguage = value;
            OnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public Gender SelectedGender { get; set; }
    public DataSource SelectedSource { get; set; }
    public string? FifaCode { get; set; }
}

public static class Mapper
{
    public static UserSettings ToUserSettings(this State state)
    {
        return new UserSettings
               { };
    }

    public static State ToState(this UserSettings userSettings)
    {
        return new State()
               { };
    }
}
