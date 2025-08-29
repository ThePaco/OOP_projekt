using DAL.Models;
using DAL.Models.Enums;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        using (HttpClient client = new HttpClient())
        {
            var res =
                await client.GetAsync($"https://worldcup-vua.nullbit.hr/{gender.ToString()}/matches");

            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var matches = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Matches>>(content);
                var players = new List<StartingEleven>();

                foreach (var match in matches)
                {
                    // home team
                    if (match.HomeTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true &&
                        match.HomeTeamStatistics?.StartingEleven != null)
                    {
                        players.AddRange(match.HomeTeamStatistics.StartingEleven);
                        if (match.HomeTeamStatistics.Substitutes != null)
                        {
                            players.AddRange(match.HomeTeamStatistics.Substitutes);
                        }

                        break;
                    }

                    // away team
                    if (match.AwayTeam?.Code?.Equals(fifaCode, StringComparison.OrdinalIgnoreCase) == true &&
                        match.AwayTeamStatistics?.StartingEleven != null)
                    {
                        players.AddRange(match.AwayTeamStatistics.StartingEleven);
                        if (match.AwayTeamStatistics.Substitutes != null)
                        {
                            players.AddRange(match.AwayTeamStatistics.Substitutes);
                        }

                        break;
                    }
                }

                return players;
            }
            else
            {
                throw new Exception($"Failed to fetch players: {res.ReasonPhrase}");
            }
        }
    }
    public async Task<Results> GetTeamStats(Gender gender, string fifaCode)
    {

            using (HttpClient client = new HttpClient())
            {
                var res =
                    await client.GetAsync($"https://worldcup-vua.nullbit.hr/men/{gender.ToString()}/results");

                if (res.IsSuccessStatusCode)
                {
                    var jsonContent = await res.Content.ReadAsStringAsync();
                    var allResults = JsonSerializer.Deserialize<List<Results>>(jsonContent);
                    return allResults?.FirstOrDefault(r => r.FifaCode.Equals(fifaCode, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    throw new Exception($"Failed to fetch team stats: {res.ReasonPhrase}");
            }
        }

        }

    }

