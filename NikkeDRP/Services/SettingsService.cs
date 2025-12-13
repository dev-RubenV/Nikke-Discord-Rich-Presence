using System.Drawing.Printing;
using System.Text.Json;
using NikkeDRP.Models;

namespace NikkeDRP.Services
{
    public static class SettingsService
    {
        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "userSettings.json");

        public static UserSettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    var defaults = new UserSettings();
                    Save(defaults);
                    return defaults;
                }

                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
            }
            catch
            {
                return new UserSettings();
            }
        }

        public static void Save(UserSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
    }
}