using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Enums;

namespace DAL.Models;
public class UserSettings
{
    public Gender Gender { get; set; }
    public Language Language { get; set; }
    public DataSource DataSource { get; set; }
    public Resolution Resolution { get; set; }
}
