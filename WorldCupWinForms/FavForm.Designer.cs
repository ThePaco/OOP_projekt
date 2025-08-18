namespace WorldCupWinForms;

partial class FavForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblSelection = new Label();
        cmbTeams = new ComboBox();
        pnlTeamPlayers = new Panel();
        pnlFavPlayers = new Panel();
        btnFavAdd = new Button();
        btnFavRemove = new Button();
        lblPlayers = new Label();
        lblFavPlayers = new Label();
        lblRankings = new Label();
        lblSettings = new Label();
        btnSettings = new Button();
        btnRankings = new Button();
        lblState = new Label();
        SuspendLayout();
        // 
        // lblSelection
        // 
        lblSelection.AutoSize = true;
        lblSelection.Location = new Point(12, 39);
        lblSelection.Name = "lblSelection";
        lblSelection.Size = new Size(134, 15);
        lblSelection.TabIndex = 0;
        lblSelection.Text = "Select team from menu:";
        // 
        // cmbTeams
        // 
        cmbTeams.FormattingEnabled = true;
        cmbTeams.Location = new Point(152, 36);
        cmbTeams.Name = "cmbTeams";
        cmbTeams.Size = new Size(189, 23);
        cmbTeams.TabIndex = 1;
        cmbTeams.SelectedIndexChanged += cmbTeams_SelectedIndexChanged_1;
        // 
        // pnlTeamPlayers
        // 
        pnlTeamPlayers.Location = new Point(12, 87);
        pnlTeamPlayers.Name = "pnlTeamPlayers";
        pnlTeamPlayers.Size = new Size(329, 355);
        pnlTeamPlayers.TabIndex = 2;
        // 
        // pnlFavPlayers
        // 
        pnlFavPlayers.Location = new Point(523, 87);
        pnlFavPlayers.Name = "pnlFavPlayers";
        pnlFavPlayers.Size = new Size(329, 355);
        pnlFavPlayers.TabIndex = 3;
        // 
        // btnFavAdd
        // 
        btnFavAdd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnFavAdd.Location = new Point(382, 169);
        btnFavAdd.Name = "btnFavAdd";
        btnFavAdd.Size = new Size(107, 23);
        btnFavAdd.TabIndex = 4;
        btnFavAdd.Text = "->";
        btnFavAdd.UseVisualStyleBackColor = true;
        // 
        // btnFavRemove
        // 
        btnFavRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnFavRemove.Location = new Point(382, 281);
        btnFavRemove.Name = "btnFavRemove";
        btnFavRemove.Size = new Size(107, 23);
        btnFavRemove.TabIndex = 5;
        btnFavRemove.Text = "<-";
        btnFavRemove.UseVisualStyleBackColor = true;
        // 
        // lblPlayers
        // 
        lblPlayers.AutoSize = true;
        lblPlayers.Location = new Point(12, 69);
        lblPlayers.Name = "lblPlayers";
        lblPlayers.Size = new Size(75, 15);
        lblPlayers.TabIndex = 6;
        lblPlayers.Text = "Team players";
        // 
        // lblFavPlayers
        // 
        lblFavPlayers.AutoSize = true;
        lblFavPlayers.Location = new Point(523, 69);
        lblFavPlayers.Name = "lblFavPlayers";
        lblFavPlayers.Size = new Size(121, 15);
        lblFavPlayers.TabIndex = 7;
        lblFavPlayers.Text = "Your favourite players";
        // 
        // lblRankings
        // 
        lblRankings.AutoSize = true;
        lblRankings.Location = new Point(371, 446);
        lblRankings.Name = "lblRankings";
        lblRankings.Size = new Size(118, 15);
        lblRankings.TabIndex = 8;
        lblRankings.Text = "View player rankings:";
        // 
        // lblSettings
        // 
        lblSettings.AutoSize = true;
        lblSettings.Location = new Point(676, 40);
        lblSettings.Name = "lblSettings";
        lblSettings.Size = new Size(95, 15);
        lblSettings.TabIndex = 9;
        lblSettings.Text = "Change settings:";
        // 
        // btnSettings
        // 
        btnSettings.Location = new Point(777, 36);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(75, 23);
        btnSettings.TabIndex = 10;
        btnSettings.Text = "Settings";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // btnRankings
        // 
        btnRankings.Location = new Point(394, 464);
        btnRankings.Name = "btnRankings";
        btnRankings.Size = new Size(75, 23);
        btnRankings.TabIndex = 11;
        btnRankings.Text = "Rankings";
        btnRankings.UseVisualStyleBackColor = true;
        btnRankings.Click += btnRankings_Click;
        // 
        // lblState
        // 
        lblState.AutoSize = true;
        lblState.Location = new Point(676, 9);
        lblState.Name = "lblState";
        lblState.Size = new Size(38, 15);
        lblState.TabIndex = 12;
        lblState.Text = "label6";
        // 
        // FavForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(864, 601);
        Controls.Add(lblState);
        Controls.Add(btnRankings);
        Controls.Add(btnSettings);
        Controls.Add(lblSettings);
        Controls.Add(lblRankings);
        Controls.Add(lblFavPlayers);
        Controls.Add(lblPlayers);
        Controls.Add(btnFavRemove);
        Controls.Add(btnFavAdd);
        Controls.Add(pnlFavPlayers);
        Controls.Add(pnlTeamPlayers);
        Controls.Add(cmbTeams);
        Controls.Add(lblSelection);
        Name = "FavForm";
        Text = "FavForm";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblSelection;
    private ComboBox cmbTeams;
    private Panel pnlTeamPlayers;
    private Panel pnlFavPlayers;
    private Button btnFavAdd;
    private Button btnFavRemove;
    private Label lblPlayers;
    private Label lblFavPlayers;
    private Label lblRankings;
    private Label lblSettings;
    private Button btnSettings;
    private Button btnRankings;
    private Label lblState;
}