using DAL.Models;
using DAL.Repository;
using System.Windows.Media.Imaging;
using DAL.Models.Enums;

namespace WorldCupWPF.ViewModels;

public class PlayerDetailsViewModel : BaseViewModel
{
    private readonly IImagesRepo imageRepo = new ImagesRepo();
    private readonly IMatchDataRepo matchDataRepo = new LocalMatchDataRepo();

    public PlayerDetailsViewModel(StartingEleven player)
    {
        Player = player;
        LoadImage();
        CalcGoalsAndYellowCards();
    }

    private StartingEleven player;

    public StartingEleven Player
    {
        get => player;
        set
        {
            if (player == value)
                return;

            player = value;
            OnPropertyChanged();
        }
    }

    private string captainStar;
    public string CaptainStar
    {
        //todo zast
        get => Player.Captain ? "★" : string.Empty;
    }

    private int goals;
    public int Goals
    {
        get => goals;
        set
        {
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
            yellowCards = value;
            OnPropertyChanged();
        }
    }

    private string imagePath;
    public string ImagePath
    {
        get => imagePath;
        set
        {
            imagePath = value;
            OnPropertyChanged();
        }
    }

    private async void CalcGoalsAndYellowCards()
    {
        try
        {
            var allMatches = await matchDataRepo.GetMatchesAsync(Gender.Men);
            var playerGoals = 0;
            var playerYellowCards = 0;


        }
        catch (Exception ex) { }
    }

    private async Task LoadImage() => ImagePath = await imageRepo.GetImagePathAsync(Player.Name);
}
