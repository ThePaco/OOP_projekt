using DAL.Models;
using System.Text.Json;
using DAL.Models.Enums;

namespace DAL.Repository;

public class LocalMatchDataRepo : IMatchDataRepo
{
    public const string PATH = @"..\..\..\..\WorldCupData\LocalMatchData\";

    public async Task<IEnumerable<Matches>> GetMatchesAsync(Gender gender)
    {
        string filePath = string.Concat(PATH, gender.ToString());

        try
        {
            filePath = Path.Combine(filePath, "matches.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Matches data file not found at: {filePath}");
            }

            var jsonContent = await File.ReadAllTextAsync(filePath);
            var matches = JsonSerializer.Deserialize<List<Matches>>(jsonContent);

            return matches ?? [];
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load matches data: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Matches>> GetMatchesByStageAsync(Gender gender, string stageName)
    {
        var allMatches = await GetMatchesAsync(gender);
        return allMatches.Where(m => m.StageName.ToString().Equals(stageName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Teams>> GetTeams(Gender gender)
    {
        var filePath = string.Concat(PATH, gender.ToString());

        try
        {
            filePath = Path.Combine(filePath, "teams.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Teams data file not found at: {filePath}");
            }

            var jsonContent = await File.ReadAllTextAsync(filePath);
            var teams = JsonSerializer.Deserialize<List<Teams>>(jsonContent);

            return teams ?? [];
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load teams data: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Teams>> GetTeamsByFifaCodeAsync(Gender gender, string fifaCode)
    {
        var allTeams = await GetTeams(gender);
        return allTeams.Where(t => t.FifaCode.Equals(fifaCode, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<StartingEleven>> GetPlayersByTeamAsync(Gender gender, string fifaCode)
    {
        try
        {
            var matchesPath = Path.Combine(PATH, gender.ToString(), "matches.json");

            if (!File.Exists(matchesPath))
            {
                throw new FileNotFoundException($"Matches data file not found at: {matchesPath}");
            }

            var jsonContent = await File.ReadAllTextAsync(matchesPath);
            var matches = JsonSerializer.Deserialize<List<Matches>>(jsonContent);

            var players = new List<StartingEleven>();

            if (matches != null)
            {
                foreach (var match in matches)
                {
                    // Check home team
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

                    // Check away team
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
            }

            // Remove duplicates
            return players.GroupBy(p => p.Name).Select(g => g.First()).ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load players for team {fifaCode}: {ex.Message}", ex);
        }
    }
    public async Task<Results> GetTeamStats(Gender gender, string fifaCode)
    {
        try
        {
            var resultsPath = Path.Combine(PATH, gender.ToString(), "results.json");

            if (!File.Exists(resultsPath))
            {
                throw new FileNotFoundException($"Matches data file not found at: {resultsPath}");
            }

            var jsonContent = await File.ReadAllTextAsync(resultsPath);
            var allResults = JsonSerializer.Deserialize<List<Results>>(jsonContent);

            return allResults?.FirstOrDefault(r => r.FifaCode.Equals(fifaCode, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load stats for team {fifaCode}: {ex.Message}", ex);
        }
    }
}
