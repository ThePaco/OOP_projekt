using DAL.Models;

namespace DAL.Repository;

public interface IUserSettingsRepo
{
    public Task<UserSettings> GetUserSettingsAsync();
    public Task SaveUserSettingsAsync(UserSettings userSettings);
    public Task ClearUserSettingsAsync();
    public bool HasSavedSettings();
}
