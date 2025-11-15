using RailGo.ViewModels.Pages.StationToStation;

using Microsoft.UI.Xaml.Controls;

namespace RailGo.Views.Pages.StationToStation;

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
