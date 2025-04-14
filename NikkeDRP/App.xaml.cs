using Microsoft.Maui.Controls;
using MudBlazor;
using NikkeDRP.Services;
using System.Security.Cryptography.X509Certificates;

namespace NikkeDRP
{
    public partial class App : Application
    {
        public NikkeDRPWindow TitleWindow { get; }

        public App(NikkeDRPWindow titleWindow)
        {
            InitializeComponent();
            TitleWindow = titleWindow;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            const int newHeight = 600;
            const int newWidth = 800;

            TitleWindow.Page = new AppShell();
            SystemTrayService.Initialize(TitleWindow);

            TitleWindow.Height = newHeight;
            TitleWindow.Width = newWidth;
            TitleWindow.MaximumHeight = newHeight;
            TitleWindow.MinimumHeight = newHeight;
            TitleWindow.MaximumWidth = newWidth;
            TitleWindow.MinimumWidth = newWidth;

            return TitleWindow;
        }

    }
}
