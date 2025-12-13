using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MudBlazor.Services;
using NikkeDRP.Services;
using NikkeDRP.Models;

#if WINDOWS
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Microsoft.Maui.Platform; // for Win32Interop
#endif

namespace NikkeDRP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
#if WINDOWS
                .ConfigureLifecycleEvents(events =>
                {
                    events.AddWindows(windows =>
                    {
                        windows.OnWindowCreated(window =>
                        {
                            var hwnd = WindowNative.GetWindowHandle(window);
                            var winId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                            var appWindow = AppWindow.GetFromWindowId(winId);

                            var settings = SettingsService.Load();
                            if (settings.RunOnStartup)
                            {
                                window.DispatcherQueue.TryEnqueue(() => appWindow.Hide());
                            }
                        });
                    });
                })
#endif
                ;

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<NikkeDRPWindow>();

            return builder.Build();
        }
    }
}
