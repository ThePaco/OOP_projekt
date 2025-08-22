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
using System.Windows.Shapes;
using DAL.Models;
using DAL.Repository;

namespace WorldCupWPF.Windows
{
    /// <summary>
    /// Interaction logic for PlayerDetailsWindow.xaml
    /// </summary>
    public partial class PlayerDetailsWindow : Window
    {
        private readonly StartingEleven player;
        private readonly IImagesRepo imageRepo = new ImagesRepo();
        private readonly IMatchDataRepo matchDataRepo = new LocalMatchDataRepo();
        public PlayerDetailsWindow(StartingEleven player)
        {
            this.player = player;
            InitializeComponent();
            lblName.Content = player.Name;
            lblShirtNumber.Content = player.ShirtNumber;
            lblPosition.Content = player.Position.ToString();
            lblKapetan.Visibility = player.Captain ? Visibility.Visible : Visibility.Hidden;

            //todo: implement yellow card and goal counter
            //utakmicu po id bi trebalo hvatati
            LoadImage(player.Name);
            CalcGoalsAndYellowCards();
        }
        private void CalcGoalsAndYellowCards()
        {
            
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

        private void btnOK_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
