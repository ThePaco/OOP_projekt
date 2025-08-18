using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Enums;

namespace WorldCupWinForms.Model;
public class State
{
    public Language SelectedLanguage { get; set; }
    public Gender SelectedGender { get; set; }
    public DataSource SelectedSource { get; set; }
    public string SelectedFifaCode { get; set; }
}