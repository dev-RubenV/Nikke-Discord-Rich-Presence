using System.Diagnostics;
using H.NotifyIcon;
using H.NotifyIcon.Core;
using Microsoft.Maui.Controls;
using System.Windows.Forms;
using Microsoft.Maui;

namespace NikkeDRP.Services
{
    public class SystemTrayService
    {
        private static TaskbarIcon taskbarIcon;
        private static Window mainWindow;

        public static void Initialize(Window window)
        {
            mainWindow = window;
            Debug.WriteLine("Initializing SystemTrayService");

            // Initialize tray icon
            taskbarIcon = new TaskbarIcon
            {
                ToolTipText = "Nikke Discord Rich Presence",
                IconSource = new FileImageSource { File = "nikkedrp.ico" }
            };

            // Set icon (ensure "appicon.ico" exists in Resources\Images)
            try
            {
                taskbarIcon.IconSource = new FileImageSource { File = "nikkedrp.ico" };
                taskbarIcon.ForceCreate(); // Required for icon visibility
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting icon: {ex.Message}");
            }

            // Create context menu
         


            // Handle left-click
            taskbarIcon.LeftClickCommand = new Command(RestoreFromTray);

            Debug.WriteLine("SystemTrayService initialized");
        }

        public static void MinimizeToTray()
        {
            Debug.WriteLine("Minimizing to tray...");
            if (mainWindow != null)
            {
                mainWindow.Hide();
                Debug.WriteLine("Window hidden");
            }
            else
            {
                Debug.WriteLine("Main window is null");
            }
        }

        public static void RestoreFromTray()
        {
            Debug.WriteLine("Restoring from tray...");
            if (mainWindow != null)
            {
                mainWindow.Show();
                Debug.WriteLine("Window shown");
            }
            else
            {
                Debug.WriteLine("Main window is null");
            }
        }

        public static void Cleanup()
        {
            taskbarIcon?.Dispose();
        }
    }
}
