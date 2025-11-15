using RailGo.ViewModels.Pages.Shell;

using Microsoft.UI.Xaml.Controls;

namespace RailGo.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
