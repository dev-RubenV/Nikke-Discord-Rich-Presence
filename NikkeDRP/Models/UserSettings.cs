using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRP.Model;

namespace NikkeDRP.Models
{
    public class UserSettings
    {
        public bool MinimizeToTray { get; set; } = true;
        public bool RunOnStartup { get; set; } = false;
        public bool RichPresenceEnabledOnStartup { get; set; } = false;
        public LargeImageKeyModel SelectedLargeImage { get; set; } = new LargeImageKeyModel();
        public SmallImageKeyModel SelectedSmallImage { get; set; } = new SmallImageKeyModel();
        public string Details { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
