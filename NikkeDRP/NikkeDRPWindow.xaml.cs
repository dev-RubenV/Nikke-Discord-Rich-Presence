using Microsoft.Maui.Controls;
using NikkeDRP.Services;
using System.Runtime.InteropServices;

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

        public NikkeDRPWindow()
        {
            InitializeComponent();

            // Initialize the system tray service
                SystemTrayService.Initialize(this);

                // Hook into the window created event
                this.Created += OnWindowCreated;
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
                        
                        // Use the appropriate function based on process architecture
                        if (Environment.Is64BitProcess)
                            _oldWndProc = SetWindowLongPtr64(_hWnd, GWL_WNDPROC, functionPointer);
                        else
                            _oldWndProc = SetWindowLongPtr32(_hWnd, GWL_WNDPROC, functionPointer);
                            
                        System.Diagnostics.Debug.WriteLine("Window hook set up successfully");
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
                // Intercept minimize command
                if (msg == WM_SYSCOMMAND && (wParam.ToInt32() & 0xFFF0) == SC_MINIMIZE)
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