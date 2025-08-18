using DAL.Models;
using DAL.Models.Enums;

namespace DAL.Repository;

public class RemoteMatchDataRepo : IMatchDataRepo
{
    public async Task<IEnumerable<Matches>> GetMatchesAsync(Gender gender)
    {
        using (HttpClient client = new HttpClient())
        {
            var res =
                await client.GetAsync($"https://worldcup-vua.nullbit.hr/{gender.ToString()}/matches");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Matches>>(content);
            }
            else
            {
                throw new Exception($"Failed to fetch matches: {res.ReasonPhrase}");
            }
        }
    }
    public async Task<IEnumerable<Matches>> GetMatchesByStageAsync(Gender gender, string stageName)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Teams>> GetTeams(Gender gender)
    {
        using (HttpClient client = new HttpClient())
        {
            var res =
                await client.GetAsync($"https://worldcup-vua.nullbit.hr/{gender.ToString()}/team/results");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Teams>>(content);
            }
            else
            {
                throw new Exception($"Failed to fetch teams: {res.ReasonPhrase}");
            }
        }
    }
    public async Task<IEnumerable<Teams>> GetTeamsByFifaCodeAsync(Gender gender, string fifaCode)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<StartingEleven>> GetPlayersByTeamAsync(Gender gender, string fifaCode)
    {
        throw new NotImplementedException();
    }
}
