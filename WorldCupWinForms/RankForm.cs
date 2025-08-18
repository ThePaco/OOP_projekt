using DAL.Models.Enums;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldCupWinForms.Model;

namespace WorldCupWinForms;
public partial class RankForm : Form
{
    private readonly State state;
    private readonly IMatchDataRepo matchDataDataRepo;
    public RankForm(State state)
    {
        if (state.SelectedLanguage == Language.Croatian)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hr-HR");
        }
        else
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        InitializeComponent();
        this.state = state;
        matchDataDataRepo = new LocalMatchDataRepo();
    }

    private void btnRank_Click(object sender, EventArgs e)
    {
        var selected = cmbCategory.SelectedItem as string;
        DisplayRankingsAsync(selected);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
        ExportRankingsToPDF(lbRankResults);
    }
    
    private async void DisplayRankingsAsync(string? selected)
    {
        lbRankResults.Items.Clear();
        if (selected == null)
        {
            MessageBox.Show("Please select a category.");
            return;
        }

        if (selected == "Players by most goals")
        {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);
                var playerGoals = new Dictionary<string, int>();
                
                foreach (var match in allMatches)
                {
                    // home check
                    if (match.HomeTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountGoalsFromEvents(match.HomeTeamEvents, playerGoals);
                    }

                    // away check
                    if (match.AwayTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountGoalsFromEvents(match.AwayTeamEvents, playerGoals);
                    }
                }
                
                var sortedPlayers = playerGoals.OrderByDescending(p => p.Value).ToList();
                if (sortedPlayers.Any())
                {
                    lbRankResults.Items.Add($"Goals ranking for {state.SelectedFifaCode}:");
                    lbRankResults.Items.Add("".PadRight(40, '-'));
                    
                    for (int i = 0; i < sortedPlayers.Count; i++)
                    {
                        var player = sortedPlayers[i];
                        var rank = i + 1;
                        var goalText = player.Value == 1 ? "goal" : "goals";
                        lbRankResults.Items.Add($"{rank}. {player.Key} - {player.Value} {goalText}");
                    }
                }
                else
                {
                    lbRankResults.Items.Add($"No goal data found for team {state.SelectedFifaCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading goals data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        else if (selected == "Players by most yellow cards") {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);
                var playerYellowCards = new Dictionary<string, int>();
                
                foreach (var match in allMatches)
                {
                    // home check
                    if (match.HomeTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountYellowCardsFromEvents(match.HomeTeamEvents, playerYellowCards);
                    }

                    // away check
                    if (match.AwayTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountYellowCardsFromEvents(match.AwayTeamEvents, playerYellowCards);
                    }
                }
                
                var sortedPlayers = playerYellowCards.OrderByDescending(p => p.Value).ToList();
                if (sortedPlayers.Any())
                {
                    lbRankResults.Items.Add($"Yellow cards ranking for {state.SelectedFifaCode}:");
                    lbRankResults.Items.Add("".PadRight(40, '-'));
                    
                    for (int i = 0; i < sortedPlayers.Count; i++)
                    {
                        var player = sortedPlayers[i];
                        var rank = i + 1;
                        var cardText = player.Value == 1 ? "yellow card" : "yellow cards";
                        lbRankResults.Items.Add($"{rank}. {player.Key} - {player.Value} {cardText}");
                    }
                }
                else
                {
                    lbRankResults.Items.Add($"No yellow card data found for team {state.SelectedFifaCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading yellow cards data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        else if (selected == "Matches by most attendance")
        {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);
                
                // Filter po timu (home/away)
                var teamMatches = allMatches.Where(match => 
                    (match.HomeTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true) ||
                    (match.AwayTeam?.Code?.Equals(state.SelectedFifaCode, StringComparison.OrdinalIgnoreCase) == true))
                    .ToList();
                
                if (teamMatches.Any())
                {
                    var sortedMatches = teamMatches.OrderByDescending(m => m.Attendance).ToList();
                    
                    lbRankResults.Items.Add($"Matches by attendance for {state.SelectedFifaCode}:");
                    
                    for (int i = 0; i < sortedMatches.Count; i++)
                    {
                        var match = sortedMatches[i];
                        var rank = i + 1;
                        var homeTeam = match.HomeTeam?.Country ?? "Unknown";
                        var awayTeam = match.AwayTeam?.Country ?? "Unknown";
                        var venue = !string.IsNullOrEmpty(match.Venue) ? match.Venue : "Unknown Venue";
                        var attendanceText = match.Attendance.ToString("N0");
                        
                        lbRankResults.Items.Add($"{rank}. {homeTeam} vs {awayTeam}");
                        lbRankResults.Items.Add($"    Venue: {venue}");
                        lbRankResults.Items.Add($"    Attendance: {attendanceText}");
                        lbRankResults.Items.Add(""); // padding
                    }
                }
                else
                {
                    lbRankResults.Items.Add($"No match data found for team {state.SelectedFifaCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            // kako doci do ovoga? nepotreban guard clause
            MessageBox.Show("Invalid selection.");
        }
    }
    
    private void CountGoalsFromEvents(List<DAL.Models.TeamEvent> teamEvents, Dictionary<string, int> playerGoals)
    {
        if (teamEvents == null) return;

        //hvala resharper
        foreach (var playerName in from teamEvent in teamEvents where teamEvent.TypeOfEvent == TypeOfEvent.Goal || 
                                                                      teamEvent.TypeOfEvent == TypeOfEvent.GoalPenalty || 
                                                                      teamEvent.TypeOfEvent == TypeOfEvent.GoalOwn 
                                   select teamEvent.Player?.Trim() into playerName 
                                   where !string.IsNullOrEmpty(playerName) select playerName)
        {
            if (playerGoals.ContainsKey(playerName))
            {
                playerGoals[playerName]++;
            }
            else
            {
                playerGoals[playerName] = 1;
            }
        }
    }

    private void CountYellowCardsFromEvents(List<DAL.Models.TeamEvent> teamEvents, Dictionary<string, int> playerYellowCards)
    {
        if (teamEvents == null) return;

        //hvala resharper 2
        foreach (var playerName in from teamEvent in teamEvents where teamEvent.TypeOfEvent == TypeOfEvent.YellowCard 
                                   select teamEvent.Player?.Trim() into playerName 
                                   where !string.IsNullOrEmpty(playerName) select playerName)
        {
            if (playerYellowCards.ContainsKey(playerName))
            {
                playerYellowCards[playerName]++;
            }
            else
            {
                playerYellowCards[playerName] = 1;
            }
        }
    }

    private void ExportRankingsToPDF(ListBox listBox)
    {
        try
        {
            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDocument printDoc = new PrintDocument();

            printDoc.DocumentName = "Rankings Report";
            printDoc.PrintPage += (sender, e) => PrintPage_Handler(sender, e, listBox);

            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.Document = printDoc;
                printDialog.UseEXDialog = true;
                printDialog.Document.DocumentName = $"Rankings_{state.SelectedFifaCode}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                printDoc.Print();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Export Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PrintPage_Handler(object sender, PrintPageEventArgs e, ListBox listBox)
    {
        try
        {
            // stylizing tools
            Graphics graphics = e.Graphics;
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font contentFont = new Font("Arial", 10, FontStyle.Regular);
            Brush blackBrush = Brushes.Black;
            Brush grayBrush = Brushes.Gray;
            
            float yPosition = 50;
            float leftMargin = 50;
            float rightMargin = e.PageBounds.Width - 50;
            float pageWidth = rightMargin - leftMargin;

            // naziv
            var title = "World Cup Report";
            SizeF titleSize = graphics.MeasureString(title, titleFont);
            graphics.DrawString(title, titleFont, blackBrush, leftMargin + (pageWidth - titleSize.Width) / 2, yPosition);
            yPosition += titleSize.Height + 20;

            // metadata
            var metadata = $"Team: {state.SelectedFifaCode} | Gender: {state.SelectedGender} | Date: {DateTime.Now:yyyy-MM-dd HH:mm}";
            graphics.DrawString(metadata, contentFont, grayBrush, leftMargin, yPosition);
            yPosition += 40;

            foreach (var item in listBox.Items)
            {
                var text = item.ToString();
                graphics.DrawString(text, contentFont, blackBrush, leftMargin, yPosition);
                yPosition += 15;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error during printing: {ex.Message}", "Print Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
