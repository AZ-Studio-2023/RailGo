using Microsoft.UI.Xaml.Controls;

using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class StationDetailsPage : Page
{
    public StationDetailsViewModel ViewModel
    {
        get;
    }

    public StationDetailsPage()
    {
        ViewModel = App.GetService<StationDetailsViewModel>();
        InitializeComponent();
    }
}
