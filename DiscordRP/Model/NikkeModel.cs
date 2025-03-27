using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordRP.Model
{
    public class LargeImageKeyModel
    {
        public string Name { get; set; }
        public string LargeImageKey { get; set; }
        public string LargeImageKeyUrl { get; set; }

    }

    public class SmallImageKeyModel
    {
        public string Name { get; set; }
        public string SmallImageKey { get; set; }
        public string SmallImageKeyUrl { get; set; }
    }

    public static class Data
    {
        public static List<LargeImageKeyModel> LargeImageKeyData = new List<LargeImageKeyModel>
        {
            new LargeImageKeyModel { Name = "Nikke Original", LargeImageKey = "nikke", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817718329434.png" },
            new LargeImageKeyModel { Name = "Nikke Dark",  LargeImageKey = "nikke-dark", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817026535528.png" },
            new LargeImageKeyModel { Name = "Naga: Last Girlhood", LargeImageKey = "nagalg", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344652783986413579.png" },
            new LargeImageKeyModel { Name = "Naga: Last Girlhood 2",LargeImageKey = "nagalg2", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344651584763854960.png" },
            new LargeImageKeyModel { Name = "Scarlet: Black Shadow (Longing Flower)", LargeImageKey = "scarletlf", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344649684479770737.png" },
            new LargeImageKeyModel { Name = "Doro", LargeImageKey = "doro", LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1354794141426651160.png" },

        };

        public static List<SmallImageKeyModel> SmallImageKeyData = new List<SmallImageKeyModel>
        {
            new SmallImageKeyModel { Name = "None", SmallImageKey = "", SmallImageKeyUrl = "" },
            new SmallImageKeyModel { Name = "Nikke Original", SmallImageKey = "nikke", SmallImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817718329434.png" },
            new SmallImageKeyModel { Name = "Nikke Dark", SmallImageKey = "nikke-dark", SmallImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817026535528.png" },
        };

    }
}
