using DAL.Models.Enums;

namespace DAL.Models;
public class UserSettings
{
    public Gender Gender { get; set; }
    public Language Language { get; set; }
    public DataSource DataSource { get; set; }
    public Resolution Resolution { get; set; }
    public string? FifaCode { get; set; }

}
