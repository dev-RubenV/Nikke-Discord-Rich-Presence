using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace NikkeDRP.Services
{
    public class StartupManagerService
    {
        public static void SetStartup(bool setting)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (setting) rk.SetValue("NikkeDiscordRichPresence", exePath);
                else rk.DeleteValue("NikkeDiscordRichPresence", false);
            }
                }
    }
}
