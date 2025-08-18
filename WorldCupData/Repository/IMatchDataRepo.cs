using DAL.Models;
using DAL.Models.Enums;

namespace DAL.Repository;

public interface IMatchDataRepo
{
    public Task<IEnumerable<Matches>> GetMatchesAsync(Gender gender);
    public Task<IEnumerable<Matches>> GetMatchesByStageAsync(Gender gender, string stageName);
    public Task<IEnumerable<Teams>> GetTeams(Gender gender);
    public Task<IEnumerable<Teams>> GetTeamsByFifaCodeAsync(Gender gender, string fifaCode);
    public Task<IEnumerable<StartingEleven>> GetPlayersByTeamAsync(Gender gender, string fifaCode);
}
