namespace NikkeDRP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            const int newHeight = 600;
            const int newWidth = 800;

            var newWindow = new Window(MainPage)
            {
                Height = newHeight,
                Width = newWidth,
                Title = "Nikke Discord Rich Presence",
                MaximumHeight = newHeight,
                MinimumHeight = newHeight,
                MaximumWidth = newWidth,
                MinimumWidth = newWidth,
            };

            return newWindow;
        }
    }
}
