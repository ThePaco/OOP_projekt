using DAL.Models;
using System.Text.Json;

namespace DAL.Repository;

public class UserFavouritesRepo : IUserFavouritesRepo
{
    private const string FAVOURITES_FILE_PATH = @"..\..\..\..\WorldCupData\UserData\Favourites.json";

    public async Task<IEnumerable<StartingEleven>> GetFavouritePlayersAsync()
    {
        try
        {
            if (!File.Exists(FAVOURITES_FILE_PATH))
            {
                return new List<StartingEleven>();
            }

            var jsonContent = await File.ReadAllTextAsync(FAVOURITES_FILE_PATH);
            
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return new List<StartingEleven>();
            }

            var serOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var favouritePlayers = JsonSerializer.Deserialize<List<StartingEleven>>(jsonContent, serOptions);
            return favouritePlayers ?? new List<StartingEleven>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load favourite players: {ex.Message}", ex);
        }
    }

    public async Task SaveFavouritePlayersAsync(IEnumerable<StartingEleven> favouritePlayers)
    {
        try
        {
            var directory = Path.GetDirectoryName(FAVOURITES_FILE_PATH);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var serOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var jsonContent = JsonSerializer.Serialize(favouritePlayers, serOptions);
            await File.WriteAllTextAsync(FAVOURITES_FILE_PATH, jsonContent);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save favourite players: {ex.Message}", ex);
        }
    }

    public async Task ClearFavouritePlayersAsync()
    {
        try
        {
            if (File.Exists(FAVOURITES_FILE_PATH))
            {
                File.Delete(FAVOURITES_FILE_PATH);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to clear favourite players: {ex.Message}", ex);
        }
    }
}