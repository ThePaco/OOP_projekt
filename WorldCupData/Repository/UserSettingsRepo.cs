using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Text.Json;
using DAL.Models.Enums;

namespace DAL.Repository;
public class UserSettingsRepo : IUserSettingsRepo
{
    private const string SETTINGS_FILE_PATH = @"..\..\..\..\WorldCupData\UserData\Settings.json";
    
    public async Task<UserSettings> GetUserSettingsAsync()
    {
        try
        {
            if (!File.Exists(SETTINGS_FILE_PATH))
            {
                return GetDefaultSettings();
            }

            var jsonContent = await File.ReadAllTextAsync(SETTINGS_FILE_PATH);
            
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return GetDefaultSettings();
            }

            var serOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var userSettings = JsonSerializer.Deserialize<UserSettings>(jsonContent, serOptions);
            return userSettings ?? GetDefaultSettings();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load user settings: {ex.Message}", ex);
        }
    }

    public async Task SaveUserSettingsAsync(UserSettings userSettings)
    {
        try
        {
            var directory = Path.GetDirectoryName(SETTINGS_FILE_PATH);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var serOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var jsonContent = JsonSerializer.Serialize(userSettings, serOptions);
            await File.WriteAllTextAsync(SETTINGS_FILE_PATH, jsonContent);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save user settings: {ex.Message}", ex);
        }
    }

    public async Task ClearUserSettingsAsync()
    {
        try
        {
            if (File.Exists(SETTINGS_FILE_PATH))
            {
                File.Delete(SETTINGS_FILE_PATH);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to clear user settings: {ex.Message}", ex);
        }
    }

    public bool HasSavedSettings()
    {
        return File.Exists(SETTINGS_FILE_PATH) && new FileInfo(SETTINGS_FILE_PATH).Length > 0;
    }

    private static UserSettings GetDefaultSettings()
    {
        return new UserSettings
        {
            Language = Language.English,
            Gender = Gender.Men,
            DataSource = DataSource.Local,
            Resolution = Resolution.w1920_h1080,
            FifaCode = null
        };
    }
}
