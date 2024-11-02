using Microsoft.UI.Xaml.Controls;

using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class EMU_RoutingDetailsPage : Page
{
    public EMU_RoutingDetailsViewModel ViewModel
    {
        get;
    }

    public EMU_RoutingDetailsPage()
    {
        ViewModel = App.GetService<EMU_RoutingDetailsViewModel>();
        InitializeComponent();
    }
}
