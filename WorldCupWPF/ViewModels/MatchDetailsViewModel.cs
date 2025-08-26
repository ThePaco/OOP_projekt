using DAL.Models;
using DAL.Repository;
using WorldCupWPF.Models;

namespace WorldCupWPF.ViewModels;

public class MatchDetailsViewModel : BaseViewModel
{
    private readonly IMatchDataRepo matchDataRepo;
    private readonly State state;
    private readonly Teams homeTeam;
    private readonly Teams opponentTeam;

    public MatchDetailsViewModel(State state, Teams homeTeam, Teams opponentTeam, IMatchDataRepo matchDataRepo = null)
    {
        this.state = state;
        this.homeTeam = homeTeam;
        this.opponentTeam = opponentTeam;
        this.matchDataRepo = matchDataRepo ?? new LocalMatchDataRepo();
        
        LoadTeamDetailsAsync();
    }

    // Home Team Properties
    private string homeTeamName;

    public string HomeTeamName
    {
        get => homeTeamName;
        set
        {
            homeTeamName = value;
            OnPropertyChanged();
        }
    }

    private int homeWins;

    public int HomeWins
    {
        get => homeWins;
        set
        {
            homeWins = value;
            OnPropertyChanged();
        }
    }

    private int homeLosses;

    public int HomeLosses
    {
        get => homeLosses;
        set
        {
            homeLosses = value;
            OnPropertyChanged();
        }
    }

    private int homeDraws;

    public int HomeDraws
    {
        get => homeDraws;
        set
        {
            homeDraws = value;
            OnPropertyChanged();
        }
    }

    private int homeGoalsScored;

    public int HomeGoalsScored
    {
        get => homeGoalsScored;
        set
        {
            homeGoalsScored = value;
            OnPropertyChanged();
        }
    }

    private int homeGoalsTaken;

    public int HomeGoalsTaken
    {
        get => homeGoalsTaken;
        set
        {
            homeGoalsTaken = value;
            OnPropertyChanged();
        }
    }

    private int homeGoalDifference;

    public int HomeGoalDifference
    {
        get => homeGoalDifference;
        set
        {
            homeGoalDifference = value;
            OnPropertyChanged();
        }
    }

    // Opponent Team Properties
    private string opponentTeamName;

    public string OpponentTeamName
    {
        get => opponentTeamName;
        set
        {
            opponentTeamName = value;
            OnPropertyChanged();
        }
    }

    private int opponentWins;

    public int OpponentWins
    {
        get => opponentWins;
        set
        {
            opponentWins = value;
            OnPropertyChanged();
        }
    }

    private int opponentLosses;

    public int OpponentLosses
    {
        get => opponentLosses;
        set
        {
            opponentLosses = value;
            OnPropertyChanged();
        }
    }

    private int opponentDraws;

    public int OpponentDraws
    {
        get => opponentDraws;
        set
        {
            opponentDraws = value;
            OnPropertyChanged();
        }
    }

    private int opponentGoalsScored;

    public int OpponentGoalsScored
    {
        get => opponentGoalsScored;
        set
        {
            opponentGoalsScored = value;
            OnPropertyChanged();
        }
    }

    private int opponentGoalsTaken;

    public int OpponentGoalsTaken
    {
        get => opponentGoalsTaken;
        set
        {
            opponentGoalsTaken = value;
            OnPropertyChanged();
        }
    }

    private int opponentGoalDifference;

    public int OpponentGoalDifference
    {
        get => opponentGoalDifference;
        set
        {
            opponentGoalDifference = value;
            OnPropertyChanged();
        }
    }

    private async Task LoadTeamDetailsAsync()
    {
        try
        {
            HomeTeamName = homeTeam.Country;
            OpponentTeamName = opponentTeam.Country;

            var homeStats = await matchDataRepo.GetTeamStats(state.Gender, homeTeam.FifaCode);
            var opponentStats = await matchDataRepo.GetTeamStats(state.Gender, opponentTeam.FifaCode);

            HomeWins = (int)homeStats.Wins;
            HomeLosses = (int)homeStats.Losses;
            HomeDraws = (int)homeStats.Draws;
            HomeGoalsScored = (int)homeStats.GoalsFor;
            HomeGoalsTaken = (int)homeStats.GoalsAgainst;
            HomeGoalDifference = (int)homeStats.GoalDifferential;

            OpponentWins = (int)opponentStats.Wins;
            OpponentLosses = (int)opponentStats.Losses;
            OpponentDraws = (int)opponentStats.Draws;
            OpponentGoalsScored = (int)opponentStats.GoalsFor;
            OpponentGoalsTaken = (int)opponentStats.GoalsAgainst;
            OpponentGoalDifference = (int)opponentStats.GoalDifferential;
        }
        catch (Exception ex)
        {

        }
    }
}

