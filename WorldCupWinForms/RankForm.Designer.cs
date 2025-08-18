namespace WorldCupWinForms;

partial class RankForm
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
        label1 = new Label();
        cmbCategory = new ComboBox();
        lbRankResults = new ListBox();
        btnRank = new Button();
        label2 = new Label();
        btnPrint = new Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(14, 15);
        label1.Name = "label1";
        label1.Size = new Size(155, 15);
        label1.TabIndex = 0;
        label1.Text = "Choose category to rank by:";
        // 
        // cmbCategory
        // 
        cmbCategory.FormattingEnabled = true;
        cmbCategory.Items.AddRange(new object[] { "Players by most goals", "Players by most yellow cards", "Matches by most attendance" });
        cmbCategory.Location = new Point(175, 12);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(193, 23);
        cmbCategory.TabIndex = 1;
        // 
        // lbRankResults
        // 
        lbRankResults.FormattingEnabled = true;
        lbRankResults.ItemHeight = 15;
        lbRankResults.Location = new Point(14, 47);
        lbRankResults.Name = "lbRankResults";
        lbRankResults.Size = new Size(684, 289);
        lbRankResults.TabIndex = 2;
        // 
        // btnRank
        // 
        btnRank.Location = new Point(384, 12);
        btnRank.Name = "btnRank";
        btnRank.Size = new Size(75, 23);
        btnRank.TabIndex = 3;
        btnRank.Text = "Rank!";
        btnRank.UseVisualStyleBackColor = true;
        btnRank.Click += btnRank_Click;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(590, 362);
        label2.Name = "label2";
        label2.Size = new Size(108, 15);
        label2.TabIndex = 4;
        label2.Text = "Export data to PDF:";
        // 
        // btnPrint
        // 
        btnPrint.Location = new Point(623, 380);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(75, 23);
        btnPrint.TabIndex = 6;
        btnPrint.Text = "Print";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += btnPrint_Click;
        // 
        // RankForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(713, 450);
        Controls.Add(btnPrint);
        Controls.Add(label2);
        Controls.Add(btnRank);
        Controls.Add(lbRankResults);
        Controls.Add(cmbCategory);
        Controls.Add(label1);
        Name = "RankForm";
        Text = "RankForm";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private ComboBox cmbCategory;
    private ListBox lbRankResults;
    private Button btnRank;
    private Label label2;
    private Button btnPrint;
}