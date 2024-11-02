using Microsoft.UI.Xaml.Controls;

using RailGo.Models;

namespace RailGo.Views;

public sealed partial class EMU_RoutingDetailsPage : Page
{
    public TrainNumberEmuInfo ViewModel => DataContext as TrainNumberEmuInfo;

    public EMU_RoutingDetailsPage()
    {
        InitializeComponent();
    }
}
