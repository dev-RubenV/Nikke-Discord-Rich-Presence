using System.Windows.Input;
using Microsoft.Maui.Controls;
using MauiWindow = Microsoft.Maui.Controls.Window;

namespace NikkeDRP
{
    public partial class MainPage : ContentPage
    {
        public ICommand ShowHideWindowCommand { get; }
        public ICommand ExitApplicationCommand { get; }
        private MauiWindow _mauiWindow;

        public MainPage(MauiWindow mauiWindow)
        {
            InitializeComponent();
            _mauiWindow = mauiWindow;

            ShowHideWindowCommand = new Command(ToggleWindowVisibility);
            ExitApplicationCommand = new Command(() => Application.Current.Quit());

            BindingContext = this;
            #if WINDOWS
                Loaded += OnLoaded;
            #endif
        }
        partial void ToggleWindowVisibilityImpl();

        private void ToggleWindowVisibility()
        {
            ToggleWindowVisibilityImpl();
        }
    }
}
