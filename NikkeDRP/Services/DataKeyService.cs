using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DiscordRP.Model;

namespace NikkeDRP.Services
{
    public class DataKeyService
    {
        public static ImageKeyData ImageKeys { get; set; }

        static DataKeyService()
        {
            string json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "imageKeyData.json"));
            ImageKeys = JsonSerializer.Deserialize<ImageKeyData>(json);
        }

        public List<LargeImageKeyModel> LargeImages = ImageKeys.LargeImageKeyData;
        public List<SmallImageKeyModel> SmallImages = ImageKeys.SmallImageKeyData;
    }
}
