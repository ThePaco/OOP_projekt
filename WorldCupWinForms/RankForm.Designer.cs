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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RankForm));
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
        resources.ApplyResources(label1, "label1");
        label1.Name = "label1";
        // 
        // cmbCategory
        // 
        cmbCategory.DisplayMember = "Text";
        cmbCategory.FormattingEnabled = true;
        resources.ApplyResources(cmbCategory, "cmbCategory");
        cmbCategory.Name = "cmbCategory";
        cmbCategory.ValueMember = "Id";
        // 
        // lbRankResults
        // 
        lbRankResults.FormattingEnabled = true;
        resources.ApplyResources(lbRankResults, "lbRankResults");
        lbRankResults.Name = "lbRankResults";
        // 
        // btnRank
        // 
        resources.ApplyResources(btnRank, "btnRank");
        btnRank.Name = "btnRank";
        btnRank.UseVisualStyleBackColor = true;
        btnRank.Click += btnRank_Click;
        // 
        // label2
        // 
        resources.ApplyResources(label2, "label2");
        label2.Name = "label2";
        // 
        // btnPrint
        // 
        resources.ApplyResources(btnPrint, "btnPrint");
        btnPrint.Name = "btnPrint";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += btnPrint_Click;
        // 
        // RankForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(btnPrint);
        Controls.Add(label2);
        Controls.Add(btnRank);
        Controls.Add(lbRankResults);
        Controls.Add(cmbCategory);
        Controls.Add(label1);
        Name = "RankForm";
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