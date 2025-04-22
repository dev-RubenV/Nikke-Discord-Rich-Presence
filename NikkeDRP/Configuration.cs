using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using DiscordRP.Model;

namespace NikkeDRP
{
    public class Configuration
    {

        public void AppSettings(bool MinimizeToTray, bool RunOnStartup, bool RichPresenceEnabledOnStartup, LargeImageKeyModel selectedLargeImage, SmallImageKeyModel selectedSmallImage, string Details, string State)
        {
            var settings = new
            {
                MinimizeToTray = MinimizeToTray,
                RunOnStartup = RunOnStartup,
                RichPresenceEnabledOnStartup = RichPresenceEnabledOnStartup,
                SelectedLargeImage = selectedLargeImage,
                SelectedSmallImage = selectedSmallImage,
                Details = Details,
                State = State,
            };

            string jsonString = JsonSerializer.Serialize(settings);
            File.WriteAllText("appsettings.json", jsonString);
        }
    }
}
