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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavForm));
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
        SuspendLayout();
        // 
        // lblSelection
        // 
        resources.ApplyResources(lblSelection, "lblSelection");
        lblSelection.Name = "lblSelection";
        // 
        // cmbTeams
        // 
        cmbTeams.FormattingEnabled = true;
        resources.ApplyResources(cmbTeams, "cmbTeams");
        cmbTeams.Name = "cmbTeams";
        cmbTeams.SelectedIndexChanged += cmbTeams_SelectedIndexChanged_1;
        // 
        // pnlTeamPlayers
        // 
        resources.ApplyResources(pnlTeamPlayers, "pnlTeamPlayers");
        pnlTeamPlayers.Name = "pnlTeamPlayers";
        // 
        // pnlFavPlayers
        // 
        resources.ApplyResources(pnlFavPlayers, "pnlFavPlayers");
        pnlFavPlayers.Name = "pnlFavPlayers";
        // 
        // btnFavAdd
        // 
        resources.ApplyResources(btnFavAdd, "btnFavAdd");
        btnFavAdd.Name = "btnFavAdd";
        btnFavAdd.UseVisualStyleBackColor = true;
        // 
        // btnFavRemove
        // 
        resources.ApplyResources(btnFavRemove, "btnFavRemove");
        btnFavRemove.Name = "btnFavRemove";
        btnFavRemove.UseVisualStyleBackColor = true;
        // 
        // lblPlayers
        // 
        resources.ApplyResources(lblPlayers, "lblPlayers");
        lblPlayers.Name = "lblPlayers";
        // 
        // lblFavPlayers
        // 
        resources.ApplyResources(lblFavPlayers, "lblFavPlayers");
        lblFavPlayers.Name = "lblFavPlayers";
        // 
        // lblRankings
        // 
        resources.ApplyResources(lblRankings, "lblRankings");
        lblRankings.Name = "lblRankings";
        // 
        // lblSettings
        // 
        resources.ApplyResources(lblSettings, "lblSettings");
        lblSettings.Name = "lblSettings";
        // 
        // btnSettings
        // 
        resources.ApplyResources(btnSettings, "btnSettings");
        btnSettings.Name = "btnSettings";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // btnRankings
        // 
        resources.ApplyResources(btnRankings, "btnRankings");
        btnRankings.Name = "btnRankings";
        btnRankings.UseVisualStyleBackColor = true;
        btnRankings.Click += btnRankings_Click;
        // 
        // FavForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
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
}