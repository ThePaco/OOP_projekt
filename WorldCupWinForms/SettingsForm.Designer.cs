namespace WorldCupWinForms;

partial class SettingsForm
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
        btnSave = new Button();
        btnCancel = new Button();
        gbLanguage = new GroupBox();
        rbEng = new RadioButton();
        rbCro = new RadioButton();
        gbCategory = new GroupBox();
        rbWomen = new RadioButton();
        rbMen = new RadioButton();
        gbSource = new GroupBox();
        rbLocalFiles = new RadioButton();
        rbApi = new RadioButton();
        gbLanguage.SuspendLayout();
        gbCategory.SuspendLayout();
        gbSource.SuspendLayout();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 24F);
        label1.Location = new Point(266, 38);
        label1.Name = "label1";
        label1.Size = new Size(249, 45);
        label1.TabIndex = 0;
        label1.Text = "Change settings";
        label1.Click += label1_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(266, 324);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(94, 41);
        btnSave.TabIndex = 7;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(420, 324);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(94, 41);
        btnCancel.TabIndex = 8;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // gbLanguage
        // 
        gbLanguage.Controls.Add(rbEng);
        gbLanguage.Controls.Add(rbCro);
        gbLanguage.Location = new Point(55, 122);
        gbLanguage.Name = "gbLanguage";
        gbLanguage.Size = new Size(200, 100);
        gbLanguage.TabIndex = 9;
        gbLanguage.TabStop = false;
        gbLanguage.Text = "Language";
        // 
        // rbEng
        // 
        rbEng.AutoSize = true;
        rbEng.Location = new Point(13, 27);
        rbEng.Name = "rbEng";
        rbEng.Size = new Size(63, 19);
        rbEng.TabIndex = 1;
        rbEng.TabStop = true;
        rbEng.Text = "English";
        rbEng.UseVisualStyleBackColor = true;
        rbEng.CheckedChanged += rb_CheckedChanged;
        // 
        // rbCro
        // 
        rbCro.AutoSize = true;
        rbCro.Location = new Point(13, 63);
        rbCro.Name = "rbCro";
        rbCro.Size = new Size(70, 19);
        rbCro.TabIndex = 0;
        rbCro.TabStop = true;
        rbCro.Text = "Croatian";
        rbCro.UseVisualStyleBackColor = true;
        rbCro.CheckedChanged += rb_CheckedChanged;
        // 
        // gbCategory
        // 
        gbCategory.Controls.Add(rbWomen);
        gbCategory.Controls.Add(rbMen);
        gbCategory.Location = new Point(303, 122);
        gbCategory.Name = "gbCategory";
        gbCategory.Size = new Size(200, 100);
        gbCategory.TabIndex = 10;
        gbCategory.TabStop = false;
        gbCategory.Text = "Gender Category";
        // 
        // rbWomen
        // 
        rbWomen.AutoSize = true;
        rbWomen.Location = new Point(6, 63);
        rbWomen.Name = "rbWomen";
        rbWomen.Size = new Size(67, 19);
        rbWomen.TabIndex = 1;
        rbWomen.TabStop = true;
        rbWomen.Text = "Women";
        rbWomen.UseVisualStyleBackColor = true;
        // 
        // rbMen
        // 
        rbMen.AutoSize = true;
        rbMen.Location = new Point(6, 27);
        rbMen.Name = "rbMen";
        rbMen.Size = new Size(49, 19);
        rbMen.TabIndex = 0;
        rbMen.TabStop = true;
        rbMen.Text = "Men";
        rbMen.UseVisualStyleBackColor = true;
        // 
        // gbSource
        // 
        gbSource.Controls.Add(rbApi);
        gbSource.Controls.Add(rbLocalFiles);
        gbSource.Location = new Point(552, 122);
        gbSource.Name = "gbSource";
        gbSource.Size = new Size(200, 100);
        gbSource.TabIndex = 11;
        gbSource.TabStop = false;
        gbSource.Text = "Data Source";
        // 
        // rbLocalFiles
        // 
        rbLocalFiles.AutoSize = true;
        rbLocalFiles.Location = new Point(21, 25);
        rbLocalFiles.Name = "rbLocalFiles";
        rbLocalFiles.Size = new Size(74, 19);
        rbLocalFiles.TabIndex = 0;
        rbLocalFiles.TabStop = true;
        rbLocalFiles.Text = "Local File";
        rbLocalFiles.UseVisualStyleBackColor = true;
        // 
        // rbApi
        // 
        rbApi.AutoSize = true;
        rbApi.Location = new Point(21, 63);
        rbApi.Name = "rbApi";
        rbApi.Size = new Size(70, 19);
        rbApi.TabIndex = 1;
        rbApi.TabStop = true;
        rbApi.Text = "Web Api";
        rbApi.UseVisualStyleBackColor = true;
        // 
        // SettingsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(gbSource);
        Controls.Add(gbCategory);
        Controls.Add(gbLanguage);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(label1);
        Name = "SettingsForm";
        Text = "SettingsForm";
        gbLanguage.ResumeLayout(false);
        gbLanguage.PerformLayout();
        gbCategory.ResumeLayout(false);
        gbCategory.PerformLayout();
        gbSource.ResumeLayout(false);
        gbSource.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button btnSave;
    private Button btnCancel;
    private GroupBox gbLanguage;
    private GroupBox gbCategory;
    private RadioButton rbEng;
    private RadioButton rbCro;
    private RadioButton rbWomen;
    private RadioButton rbMen;
    private GroupBox gbSource;
    private RadioButton rbApi;
    private RadioButton rbLocalFiles;
}