using DiscordRPC.Logging;
using DiscordRPC;

namespace DiscordRP
{
    public class NikkeRichPresence
    {
        public class DiscordRichPresence
        {
            private DiscordRpcClient client;

            // Called when your application first starts.
            // For example, just before your main loop, or OnEnable for Unity.
            public void Initialize(string largeImageKey, string smallImageKey, string details, string state)
            {
                // Create a Discord client
                // NOTE: If you are using Unity3D, you must use the full constructor and define the pipe connection.
                client = new DiscordRpcClient("1344627328562626621");

                // Set the logger
                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

                // Subscribe to events
                client.OnReady += (sender, e) =>
                {
                    Console.WriteLine("Received Ready from user {0}", e.User.Username);
                };

                client.OnPresenceUpdate += (sender, e) =>
                {
                    Console.WriteLine("Received Update! {0}", e.Presence);
                };

                // Connect to the RPC
                client.Initialize();

                // Set the rich presence
                // Call this as many times as you want and anywhere in your code.
                client.SetPresence(new RichPresence()
                {
                    Details = details,
                    State = state,
                    Assets = new Assets()
                    {
                        LargeImageKey = largeImageKey,
                        LargeImageText = "Goddess of Victory: NIKKE",
                        SmallImageKey = smallImageKey,
                    }
                });
            }

            // The main loop of your application, or some sort of timer. Literally the Update function in Unity3D
            public void Update()
            {
                // Invoke all the events, such as OnPresenceUpdate
                client.Invoke();
            }

            // Called when your application terminates.
            // For example, just after your main loop, or OnDisable for Unity.
            public void Deinitialize()
            {
                client.ClearPresence();

                client.Dispose();
            }
        }
    }
}
