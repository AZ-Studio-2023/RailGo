using Microsoft.UI.Xaml.Controls;

using RailGo.Core.Models;

namespace RailGo.Views;

public sealed partial class EMU_RoutingDetailsPage : Page
{
    public TrainTripsInfo ViewModel => DataContext as TrainTripsInfo;

    public EMU_RoutingDetailsPage()
    {
        InitializeComponent();
    }
}
