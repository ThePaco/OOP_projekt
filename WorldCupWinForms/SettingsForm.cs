using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Models.Enums;
using WorldCupWinForms.Model;

namespace WorldCupWinForms;

public partial class SettingsForm : Form
{
    private readonly State state;
    
    public SettingsForm(State state)
    {
        InitializeComponent();
        this.state = state;

        foreach (var control in gbLanguage.Controls)
        {
            if (control is not RadioButton rb)
                continue;

            rb.Checked = state.SelectedLanguage switch
            {
                Language.Croatian => rb.Text == "Croatian",
                Language.English => rb.Text == "English",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        foreach (var control in gbCategory.Controls)
        {
            if (control is not RadioButton rb)
                continue;

            rb.Checked = state.SelectedGender switch
            {
                Gender.Men => rb.Text == "Men",
                Gender.Women => rb.Text == "Women",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        foreach (var control in gbSource.Controls)
        {
            if (control is not RadioButton rb)
                continue;

            rb.Checked = state.SelectedSource switch
            {
                DataSource.Local => rb.Text == "Local File",
                DataSource.Api => rb.Text == "Web Api",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Enter:
                btnSave_Click(this, EventArgs.Empty);
                return true;
            case Keys.Escape:
                btnCancel_Click(this, EventArgs.Empty);
                return true;
            default:
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    private void label1_Click(object sender, EventArgs e) { }

    private void rb_CheckedChanged(object sender, EventArgs e) { }

    private void btnCancel_Click(object sender, EventArgs e) => Close();

    private void btnSave_Click(object sender, EventArgs e)
    {
        foreach (var control in gbLanguage.Controls)
        {
            if (control is not RadioButton { Checked: true } rb)
                continue;

            state.SelectedLanguage = rb.Text switch
                {
                    "Croatian" => Language.Croatian,
                    "English" => Language.English,
                    _ => throw new ArgumentOutOfRangeException()
                }
                ;
            break;
        }

        foreach (var control in gbCategory.Controls)
        {
            if (control is not RadioButton { Checked: true } rb)
                continue;

            state.SelectedGender = rb.Text switch
                {
                    "Men" => Gender.Men,
                    "Women" => Gender.Women,
                    _ => throw new ArgumentOutOfRangeException()
                }
                ;
            break;
        }

        foreach (var control in gbSource.Controls)
        {
            if (control is not RadioButton { Checked: true } rb)
                continue;

            state.SelectedSource = rb.Text switch
                {
                    "Local File" => DataSource.Local,
                    "Web Api" => DataSource.Api,
                    _ => throw new ArgumentOutOfRangeException()
                }
                ;
            break;
        }

        Close();
    }
}
