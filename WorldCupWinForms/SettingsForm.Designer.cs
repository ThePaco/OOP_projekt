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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
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
        rbApi = new RadioButton();
        rbLocalFiles = new RadioButton();
        gbLanguage.SuspendLayout();
        gbCategory.SuspendLayout();
        gbSource.SuspendLayout();
        SuspendLayout();
        // 
        // label1
        // 
        resources.ApplyResources(label1, "label1");
        label1.Name = "label1";
        // 
        // btnSave
        // 
        resources.ApplyResources(btnSave, "btnSave");
        btnSave.Name = "btnSave";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        resources.ApplyResources(btnCancel, "btnCancel");
        btnCancel.Name = "btnCancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // gbLanguage
        // 
        gbLanguage.Controls.Add(rbEng);
        gbLanguage.Controls.Add(rbCro);
        resources.ApplyResources(gbLanguage, "gbLanguage");
        gbLanguage.Name = "gbLanguage";
        gbLanguage.TabStop = false;
        // 
        // rbEng
        // 
        resources.ApplyResources(rbEng, "rbEng");
        rbEng.Name = "rbEng";
        rbEng.TabStop = true;
        rbEng.UseVisualStyleBackColor = true;
        // 
        // rbCro
        // 
        resources.ApplyResources(rbCro, "rbCro");
        rbCro.Name = "rbCro";
        rbCro.TabStop = true;
        rbCro.UseVisualStyleBackColor = true;
        // 
        // gbCategory
        // 
        gbCategory.Controls.Add(rbWomen);
        gbCategory.Controls.Add(rbMen);
        resources.ApplyResources(gbCategory, "gbCategory");
        gbCategory.Name = "gbCategory";
        gbCategory.TabStop = false;
        // 
        // rbWomen
        // 
        resources.ApplyResources(rbWomen, "rbWomen");
        rbWomen.Name = "rbWomen";
        rbWomen.TabStop = true;
        rbWomen.UseVisualStyleBackColor = true;
        // 
        // rbMen
        // 
        resources.ApplyResources(rbMen, "rbMen");
        rbMen.Name = "rbMen";
        rbMen.TabStop = true;
        rbMen.UseVisualStyleBackColor = true;
        // 
        // gbSource
        // 
        gbSource.Controls.Add(rbApi);
        gbSource.Controls.Add(rbLocalFiles);
        resources.ApplyResources(gbSource, "gbSource");
        gbSource.Name = "gbSource";
        gbSource.TabStop = false;
        // 
        // rbApi
        // 
        resources.ApplyResources(rbApi, "rbApi");
        rbApi.Name = "rbApi";
        rbApi.TabStop = true;
        rbApi.UseVisualStyleBackColor = true;
        // 
        // rbLocalFiles
        // 
        resources.ApplyResources(rbLocalFiles, "rbLocalFiles");
        rbLocalFiles.Name = "rbLocalFiles";
        rbLocalFiles.TabStop = true;
        rbLocalFiles.UseVisualStyleBackColor = true;
        // 
        // SettingsForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbSource);
        Controls.Add(gbCategory);
        Controls.Add(gbLanguage);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(label1);
        Name = "SettingsForm";
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