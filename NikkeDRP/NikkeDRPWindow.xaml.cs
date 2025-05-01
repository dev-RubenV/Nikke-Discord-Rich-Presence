using Microsoft.Maui.Controls;
using NikkeDRP.Services;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.IO;
using DiscordRP.Model;
using H.NotifyIcon;

namespace NikkeDRP
{
    public partial class NikkeDRPWindow : Window
    {
        // Windows message constants
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;

        // Import native methods for window message handling
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        // For 32-bit processes running on 64-bit Windows
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        // For 64-bit processes
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        private const int GWL_WNDPROC = -4;
        private IntPtr _oldWndProc;
        private IntPtr _hWnd;
        private WndProcDelegate _wndProcDelegate;
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        // Add a private field to track the current state
        private bool _minimizeToTrayEnabled;
        private bool _richPresenceEnabledOnStartup;

        public NikkeDRPWindow()
        {
            InitializeComponent();

            const string filePath = "appsettings.json";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                // Define default settings
                var defaultSettings = new
                {
                    MinimizeToTray = true,
                    RunOnStartup = false,
                    RichPresenceEnabledOnStartup = true,
                    SelectedLargeImage = new LargeImageKeyModel
                    {
                        Name = "Nikke Original",
                        LargeImageKey = "nikke",
                        LargeImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817718329434.png"
                    },
                    SelectedSmallImage = new SmallImageKeyModel
                    {
                        Name = "Default Small Image",
                        SmallImageKey = "nikke-dark",
                        SmallImageKeyUrl = "https://cdn.discordapp.com/app-assets/1344627328562626621/1344648817026535528.png"
                    },
                    Details = string.Empty,
                    State = string.Empty
                };

                // Serialize and create the file
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(defaultSettings, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);
            }

            // Read the JSON file
            var jsonContent = File.ReadAllText(filePath);
            dynamic settings = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonContent);

            // Set the initial state
            _minimizeToTrayEnabled = settings.MinimizeToTray == true || settings.MinimizeToTray is null;
            _richPresenceEnabledOnStartup = settings.RichPresenceEnabledOnStartup == true;

            // Initialize the system tray service regardless of the setting
            // (we'll need it even if minimize-to-tray is disabled initially but enabled later)
            SystemTrayService.Initialize(this);

            // Always hook into the window created event
            this.Created += OnWindowCreated;

            this.Activated += (s, e) =>
            {
                if (_minimizeToTrayEnabled && _richPresenceEnabledOnStartup)
                {
                    this.Hide();
                }
            };
        }

        // Add a public method to update the minimize-to-tray setting
        public void UpdateMinimizeToTraySetting(bool enabled)
        {
            _minimizeToTrayEnabled = enabled;

            // No need to modify window hooks as we'll check _minimizeToTrayEnabled in WndProc
            System.Diagnostics.Debug.WriteLine($"Minimize to tray setting updated: {enabled}");
        }

        private void OnWindowCreated(object sender, EventArgs e)
        {
        #if WINDOWS
            try
            {
                // Get the window handle using WinUI interop
                var handler = this.Handler;
                if (handler?.PlatformView is Microsoft.UI.Xaml.Window platformWindow)
                {
                    _hWnd = WinRT.Interop.WindowNative.GetWindowHandle(platformWindow);
            
                    if (_hWnd != IntPtr.Zero)
                    {
                        // Set up window procedure hook
                        _wndProcDelegate = new WndProcDelegate(WndProc);
                        IntPtr functionPointer = Marshal.GetFunctionPointerForDelegate(_wndProcDelegate);
                
                        if (Environment.Is64BitProcess)
                            _oldWndProc = SetWindowLongPtr64(_hWnd, GWL_WNDPROC, functionPointer);
                        else
                            _oldWndProc = SetWindowLongPtr32(_hWnd, GWL_WNDPROC, functionPointer);
                
                        System.Diagnostics.Debug.WriteLine("Window hook set up successfully");

                        // ? ADD THIS BLOCK INSIDE THE _hWnd CHECK ?
                        if (_minimizeToTrayEnabled && _richPresenceEnabledOnStartup)
                        {
                            Dispatcher.Dispatch(() => 
                            {
                                SystemTrayService.MinimizeToTray();
                                System.Diagnostics.Debug.WriteLine("Minimized to tray on startup");
                            });
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to get window handle");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("PlatformView is not a Window");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting up window hook: {ex}");
            }
        #endif
                }

#if WINDOWS
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                // Only intercept minimize command if minimize-to-tray is enabled
                if (_minimizeToTrayEnabled && msg == WM_SYSCOMMAND && (wParam.ToInt32() & 0xFFF0) == SC_MINIMIZE)
                {
                    System.Diagnostics.Debug.WriteLine("Minimize command intercepted");
                    // Instead of minimizing, minimize to tray
                    SystemTrayService.MinimizeToTray();
                    return IntPtr.Zero; // We handled this message
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in WndProc: {ex}");
            }
            
            // Call the original window procedure for other messages
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }
#endif

        protected override void OnDestroying()
        {
            base.OnDestroying();

#if WINDOWS
            // Restore the original window procedure when closing
            if (_hWnd != IntPtr.Zero && _oldWndProc != IntPtr.Zero)
            {
                try
                {
                    if (Environment.Is64BitProcess)
                        SetWindowLongPtr64(_hWnd, GWL_WNDPROC, _oldWndProc);
                    else
                        SetWindowLongPtr32(_hWnd, GWL_WNDPROC, _oldWndProc);
                        
                    System.Diagnostics.Debug.WriteLine("Window hook removed");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error removing window hook: {ex}");
                }
            }
#endif
        }
    }
}