using H.NotifyIcon;
using Microsoft.Maui.Controls;
using Microsoft.Maui.LifecycleEvents;
using NikkeDRP.Models;
using NikkeDRP.Services;
using System;
using System.Linq;
using System.Windows.Input;
using MauiWindow = Microsoft.Maui.Controls.Window;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using WinUIWindow = Microsoft.UI.Xaml.Window;
using System.Runtime.InteropServices;
#endif

namespace NikkeDRP
{
    public partial class MainPage : ContentPage
    {
        public ICommand ShowHideWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }
        private MauiWindow _mauiWindow;
        private UserSettings _settings;

#if WINDOWS
                private AppWindow _appWindow;
#endif

        // Default constructor required for XAML instantiation
        public MainPage()
            : this(Application.Current?.Windows.FirstOrDefault())
        {
        }

        // Custom constructor
        public MainPage(MauiWindow mauiWindow)
        {
            InitializeComponent();

            _mauiWindow = mauiWindow;
            _settings = SettingsService.Load();
            

            ShowHideWindowCommand = new Command(ToggleWindowVisibility);
            ExitApplicationCommand = new Command(() => Application.Current.Quit());

            BindingContext = this;
            Loaded += MainPage_Loaded;

            #if WINDOWS
                        Loaded += OnLoaded;
            #endif
        }


#if WINDOWS
                private void OnLoaded(object sender, EventArgs e)
                {
                    var nativeWin = _mauiWindow?.Handler?.PlatformView as WinUIWindow;
                    if (nativeWin is null) return;

                    var hwnd = WindowNative.GetWindowHandle(nativeWin);
                    var winId = Win32Interop.GetWindowIdFromWindow(hwnd);
                    _appWindow = AppWindow.GetFromWindowId(winId);

                    HookWindowMessages(nativeWin);
                }
#endif

        public void ToggleWindowVisibility()
        {
#if WINDOWS
            if (_appWindow is null) return;

            if (_appWindow.Presenter is OverlappedPresenter)
            {
                if (_appWindow.IsVisible)
                    _appWindow.Hide();
                else
                    _appWindow.Show();
            }
#else

#endif
        }

        private async void MainPage_Loaded(object sender, EventArgs e)
        {
#if WINDOWS           
            var webView2 = blazorWebView?.Handler?.PlatformView as Microsoft.UI.Xaml.Controls.WebView2;
            if (webView2 != null)
            {
                await webView2.EnsureCoreWebView2Async();
                webView2.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView2.CoreWebView2.Settings.IsZoomControlEnabled = false;
            }
#endif
        }
#if WINDOWS
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_MINIMIZE = 0xF020;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WndProc newProc);

        private delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private WndProc _newWndProc;
        private IntPtr _oldWndProc;

        private void HookWindowMessages(WinUIWindow nativeWin)
        {
            var hwnd = WindowNative.GetWindowHandle(nativeWin);

            _newWndProc = new WndProc(CustomWndProc);
            _oldWndProc = SetWindowLongPtr(hwnd, -4, _newWndProc); // -4 = GWLP_WNDPROC
        }

        private IntPtr CustomWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_SYSCOMMAND && wParam.ToInt32() == SC_MINIMIZE)
            {
                _settings = SettingsService.Load();

                if(_settings.MinimizeToTray){
                // Instead of minimizing, hide to tray
                ToggleWindowVisibility();
                return IntPtr.Zero; // swallow the message
                }
                else{
                // Default behavior
                return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
            }
            }

            // Default behavior
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
#endif
    }

}
