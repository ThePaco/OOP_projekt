using DAL.Models.Enums;
using DAL.Repository;
using System.Drawing.Printing;
using System.Globalization;
using System.Resources;
using WorldCupWinForms.Model;

namespace WorldCupWinForms;

public partial class RankForm : Form
{
    private readonly State state;
    private readonly LocalMatchDataRepo matchDataDataRepo;
    private readonly RemoteMatchDataRepo repositoryAPI;
    private readonly ResourceManager resourceManager;

    public RankForm(State state)
    {
        this.state = state;
        Thread.CurrentThread.CurrentUICulture = state.SelectedLanguage == Language.Croatian
                                                    ? new("hr-HR") 
                                                    : new CultureInfo("en-US");

        InitializeComponent();

        resourceManager = new($"{typeof(RankForm).FullName}", typeof(RankForm).Assembly);

        cmbCategory.DataSource = ComboRankingItems.GetItemsForLanguage(resourceManager);
        matchDataDataRepo = new();
    }

    private void btnRank_Click(object sender, EventArgs e) => DisplayRankingsAsync(cmbCategory);

    private void btnPrint_Click(object sender, EventArgs e) => ExportRankingsToPDF(lbRankResults);

    private async void DisplayRankingsAsync(ComboBox cmComboBox)
    {
        lbRankResults.Items.Clear();
        var selectedRank = cmbCategory.SelectedValue as RankType?;

        if (selectedRank is null)
        {
            MessageBox.Show(resourceManager.GetString("RankForm_Please_select_a_category"));
            return;
        }

        if (selectedRank == RankType.Goals)
        {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);
                var playerGoals = new Dictionary<string, int>();

                foreach (var match in allMatches)
                {
                    // home check
                    if (match.HomeTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountGoalsFromEvents(match.HomeTeamEvents, playerGoals);
                    }

                    // away check
                    if (match.AwayTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountGoalsFromEvents(match.AwayTeamEvents, playerGoals);
                    }
                }

                var sortedPlayers = playerGoals.OrderByDescending(p => p.Value).ToList();
                if (sortedPlayers.Any())
                {
                    lbRankResults.Items.Add($"Goals ranking for {state.FifaCode}:");
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
                    lbRankResults.Items.Add($"No goal data found for team {state.FifaCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading goals data: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        else if (selectedRank == RankType.YellowCards)
        {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);
                var playerYellowCards = new Dictionary<string, int>();

                foreach (var match in allMatches)
                {
                    // home check
                    if (match.HomeTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountYellowCardsFromEvents(match.HomeTeamEvents, playerYellowCards);
                    }

                    // away check
                    if (match.AwayTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        CountYellowCardsFromEvents(match.AwayTeamEvents, playerYellowCards);
                    }
                }

                var sortedPlayers = playerYellowCards.OrderByDescending(p => p.Value).ToList();
                if (sortedPlayers.Any())
                {
                    lbRankResults.Items.Add($"Yellow cards ranking for {state.FifaCode}:");
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
                    lbRankResults.Items.Add($"No yellow card data found for team {state.FifaCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading yellow cards data: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        else if (selectedRank == RankType.Attendance)
        {
            try
            {
                var allMatches = await matchDataDataRepo.GetMatchesAsync(state.SelectedGender);

                // Filter po timu (home/away)
                var teamMatches = allMatches.Where(match =>
                                                       (match.HomeTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true) ||
                                                       (match.AwayTeam?.Code?.Equals(state.FifaCode, StringComparison.OrdinalIgnoreCase) == true))
                                            .ToList();

                if (teamMatches.Any())
                {
                    var sortedMatches = teamMatches.OrderByDescending(m => m.Attendance).ToList();

                    lbRankResults.Items.Add($"Matches by attendance for {state.FifaCode}:");

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
                    lbRankResults.Items.Add($"No match data found for team {state.FifaCode}");
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
        foreach (var playerName in from teamEvent in teamEvents
                                   where teamEvent.TypeOfEvent == TypeOfEvent.Goal ||
                                         teamEvent.TypeOfEvent == TypeOfEvent.GoalPenalty ||
                                         teamEvent.TypeOfEvent == TypeOfEvent.GoalOwn
                                   select teamEvent.Player?.Trim()
                                   into playerName
                                   where !string.IsNullOrEmpty(playerName)
                                   select playerName)
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
        foreach (var playerName in from teamEvent in teamEvents
                                   where teamEvent.TypeOfEvent == TypeOfEvent.YellowCard
                                   select teamEvent.Player?.Trim()
                                   into playerName
                                   where !string.IsNullOrEmpty(playerName)
                                   select playerName)
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
                MessageBox.Show(resourceManager.GetString("No_data_to_export"), "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDocument printDoc = new();

            printDoc.DocumentName = "Rankings Report";
            printDoc.PrintPage += (sender, e) => PrintPage_Handler(sender, e, listBox);

            using PrintDialog printDialog = new();
            printDialog.Document = printDoc;
            printDialog.UseEXDialog = true;
            printDialog.Document.DocumentName = $"Rankings_{state.FifaCode}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            printDoc.Print();
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
            // stylizing 
            Graphics graphics = e.Graphics;
            Font titleFont = new("Arial", 16, FontStyle.Bold);
            Font contentFont = new("Arial", 10, FontStyle.Regular);
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
            var metadata = $"Team: {state.FifaCode} | Gender: {state.SelectedGender} | Date: {DateTime.Now:yyyy-MM-dd HH:mm}";
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

public class ComboRankingItems
{
    public RankType Id { get; set; }

    public string Text { get; set; } = null!;

    public static ComboRankingItems[] GetItemsForLanguage(ResourceManager resourceManager)
    {
        return
        [
            new() { Id = RankType.Goals, Text = resourceManager.GetString("ComboRankItem_1")! },
            new() { Id = RankType.YellowCards, Text = resourceManager.GetString("ComboRankItem_2")! },
            new() { Id = RankType.Attendance, Text = resourceManager.GetString("ComboRankItem_3")! }
        ];
    }

    private ComboRankingItems()
    {
        
    }
}

public enum RankType
{
    Goals = 1,
    YellowCards = 2,
    Attendance = 3
}
