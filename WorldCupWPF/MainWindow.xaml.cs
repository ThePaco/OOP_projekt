using DAL.Models.Enums;
using DAL.Repository;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Navigation;
using WorldCupWPF.Views;

namespace WorldCupWPF;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : NavigationWindow
{
    public MainWindow()
    {
        InitializeComponent();
        NavigationService.Navigate(new MatchSelectPage());
    }
}