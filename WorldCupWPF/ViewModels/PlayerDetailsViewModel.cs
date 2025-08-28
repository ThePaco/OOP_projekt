using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using WorldCupWPF.Models;

namespace WorldCupWPF.ViewModels;

public class PlayerDetailsViewModel() : BaseViewModel
{
    private readonly IImagesRepo imageRepo = new ImagesRepo();
    private readonly IMatchDataRepo matchDataRepo = new LocalMatchDataRepo();
    public Gender Gender { get; set; }
    public int MatchId { get; set; }

    private StartingEleven player;
    public StartingEleven Player
    {
        get => player;
        set
        {
            player = value;
            ImagePath = imageRepo.GetImagePath(Player.Name);
            CalcGoalsAndYellowCards();
        }
    }

    public string CaptainStar => Player.Captain ? "★" : string.Empty;
    public string ImagePath { get; set; }

    private int goals;
    public int Goals
    {
        get => goals;
        set
        {
            if (value == goals)
            {
                return;
            }

            goals = value;
            OnPropertyChanged();
        }
    }

    private int yellowCards;

    public int YellowCards  
    {
        get => yellowCards;
        set
        {
            if (value == yellowCards)
            {
                return;
            }

            yellowCards = value;
            OnPropertyChanged();
        }
    }

    //private void CalcGoalsAndYellowCards()
    //{
    //    Task.Run(async () =>
    //    {
    //        if (state is null)
    //        {
    //            Goals = 0;
    //            YellowCards = 0;
    //            return;
    //        }

    //        var matches = await matchDataRepo.GetMatchesAsync(state.Gender);
            
    //    });
    //}

    private async void CalcGoalsAndYellowCards()
    {
        try
        {
            var allMatches = await matchDataRepo.GetMatchesAsync(Gender);
            var match = allMatches.FirstOrDefault(m => m.FifaId == MatchId);

            if (match == null)
            {
                Goals = 0;
                YellowCards = 0;
                return;
            }

            var playerGoals = 0;
            var playerYellowCards = 0;

            if (match.HomeTeamEvents != null)
            {
                playerGoals += CountPlayerGoalsFromEvents(match.HomeTeamEvents, player.Name);
                playerYellowCards += CountPlayerYellowCardsFromEvents(match.HomeTeamEvents, player.Name);
            }

            if (match.AwayTeamEvents != null)
            {
                playerGoals += CountPlayerGoalsFromEvents(match.AwayTeamEvents, player.Name);
                playerYellowCards += CountPlayerYellowCardsFromEvents(match.AwayTeamEvents, player.Name);
            }

            Goals = playerGoals;
            YellowCards = playerYellowCards;
        }
        catch (Exception)
        {
            Goals = 0;
            YellowCards = 0;
        }
    }

    private int CountPlayerGoalsFromEvents(List<TeamEvent> teamEvents, string playerName)
    {
        if (teamEvents == null || string.IsNullOrEmpty(playerName)) return 0;

        return teamEvents.Count(teamEvent =>
                                    (teamEvent.TypeOfEvent == TypeOfEvent.Goal ||
                                     teamEvent.TypeOfEvent == TypeOfEvent.GoalPenalty ||
                                     teamEvent.TypeOfEvent == TypeOfEvent.GoalOwn) &&
                                    string.Equals(teamEvent.Player?.Trim(), playerName.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private int CountPlayerYellowCardsFromEvents(List<TeamEvent> teamEvents, string playerName)
    {
        if (teamEvents == null || string.IsNullOrEmpty(playerName)) return 0;

        return teamEvents.Count(teamEvent =>
                                    teamEvent.TypeOfEvent == TypeOfEvent.YellowCard &&
                                    string.Equals(teamEvent.Player?.Trim(), playerName.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
