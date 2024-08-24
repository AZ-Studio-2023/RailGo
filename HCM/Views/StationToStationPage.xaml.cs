using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace RailGo.Views;

public sealed partial class StationToStationPage : Page
{
    public StationToStationViewModel ViewModel
    {
        get;
    }

    public StationToStationPage()
    {
        ViewModel = App.GetService<StationToStationViewModel>();
        InitializeComponent();
    }
}
