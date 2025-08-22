using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL.Models;
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

        public PlayerCard(StartingEleven player)
        {
            InitializeComponent();
            this.player = player;
            lblName.Content = player.Name;
            lblShirtNumber.Content = player.ShirtNumber;
            LoadImage(player.Name);

            // Add click event handler
            this.MouseLeftButtonUp += PlayerCard_MouseLeftButtonUp;
            // Make the cursor indicate it's clickable
            this.Cursor = Cursors.Hand;
        }

        private void PlayerCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var playerDetailsWindow = new PlayerDetailsWindow(player);
            playerDetailsWindow.ShowDialog();
        }

        private async Task LoadImage(string playerName)
        {
            try
            {
                var imageBytes = await imageRepo.GetImageAsync(playerName);
                var bitmapImage = new BitmapImage();

                using (var stream = new System.IO.MemoryStream(imageBytes))
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

