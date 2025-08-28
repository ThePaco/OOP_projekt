using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DAL.Models;
using DAL.Models.Enums;
using DAL.Repository;
using WorldCupWPF.Windows;

namespace WorldCupWPF.UserControls
{
    /// <summary>
    /// Interaction logic for PlayerCard.xaml
    /// </summary>
    public partial class PlayerCard : UserControl
    {
        private readonly IImagesRepo imageRepo = new ImagesRepo();
        private readonly StartingEleven player;
        private readonly Gender gender;
        private readonly int matchId;


        public PlayerCard(StartingEleven player, Gender gender, int matchId)
        {
            InitializeComponent();
            this.player = player;
            this.gender = gender;
            this.matchId = matchId;
            lblName.Content = player.Name;
            lblShirtNumber.Content = player.ShirtNumber;
            LoadImage(player.Name);

            this.MouseLeftButtonUp += PlayerCard_MouseLeftButtonUp;
            this.Cursor = Cursors.Hand;
        }

        private void PlayerCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var playerDetailsWindow = new PlayerDetailsWindow(player, gender, matchId);
            playerDetailsWindow.ShowDialog();
        }

        private async Task LoadImage(string playerName)
        {
            try
            {
                var imageBytes = await imageRepo.GetImageAsync(playerName);
                var bitmapImage = new BitmapImage();

                using (var stream = new MemoryStream(imageBytes))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // Important for cross-thread access
                }

                imgIcon.Source = bitmapImage;
            }
            catch (Exception e)
            {
                //ne bi trebalo doći do ovoga osim ako ne nestane Avatar.png iz DAL/Images
                Console.WriteLine(e);
            }
        }
    }
}

