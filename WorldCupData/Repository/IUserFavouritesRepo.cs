using DAL.Models;

namespace DAL.Repository;

public interface IUserFavouritesRepo
{
    Task<IEnumerable<StartingEleven>> GetFavouritePlayersAsync();
    Task SaveFavouritePlayersAsync(IEnumerable<StartingEleven> favouritePlayers);
    Task ClearFavouritePlayersAsync();
}