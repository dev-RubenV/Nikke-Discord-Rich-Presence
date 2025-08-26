using MauiWindow = Microsoft.Maui.Controls.Window;

namespace NikkeDRP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            const int newHeight = 600;
            const int newWidth = 800;

            var mainPageWindow = new Window();
            var page = new MainPage(mainPageWindow);
            mainPageWindow.Page = page;

            mainPageWindow.Height = newHeight;
            mainPageWindow.Width = newWidth;
            mainPageWindow.Title = "Nikke Discord Rich Presence";
            mainPageWindow.MaximumHeight = newHeight;
            mainPageWindow.MinimumHeight = newHeight;
            mainPageWindow.MaximumWidth = newWidth;
            mainPageWindow.MinimumWidth = newWidth;

            return mainPageWindow;
        }
    }
}
