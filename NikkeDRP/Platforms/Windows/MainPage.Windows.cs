using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using MauiWindow = Microsoft.Maui.Controls.Window;
using WinUIWindow = Microsoft.UI.Xaml.Window;


namespace NikkeDRP
{
    public partial class MainPage
    {
        private AppWindow _appWindow;

        private void OnLoaded(object sender, EventArgs e)
        {
            var nativeWin = _mauiWindow?.Handler?.PlatformView as WinUIWindow;
            if (nativeWin is null) return;

            var hwnd = WindowNative.GetWindowHandle(nativeWin);
            var winId = Win32Interop.GetWindowIdFromWindow(hwnd);
            _appWindow = AppWindow.GetFromWindowId(winId);
        }

        partial void ToggleWindowVisibilityImpl()
        {
            if (_appWindow is null) return;

            if (_appWindow.Presenter is OverlappedPresenter)
            {
                if (_appWindow.IsVisible)
                    _appWindow.Hide();
                else
                    _appWindow.Show();
            }
        }

    }
}
