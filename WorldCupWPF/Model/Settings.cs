using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Enums;

namespace WorldCupWPF.Model
{
    public class State
    {
        public Gender Gender { get; set; }
        public Language Language { get; set; }
        public Resolution Resolution { get; set; }
        public string? FifaCode { get; set; }
    }

    public static class Mapper
    {
        public static UserSettings ToUserSettings(this State state)
        {
            return new UserSettings()
                   {
                       Gender = state.Gender,
                       Language = state.Language,
                       Resolution = state.Resolution,
                       FifaCode = state.FifaCode,
                   };

        }
        public static State ToState(this UserSettings userSettings)
        {
            return new State();
        }
    }
}
