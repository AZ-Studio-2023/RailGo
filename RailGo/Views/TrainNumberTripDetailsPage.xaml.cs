using Microsoft.UI.Xaml.Controls;

using RailGo.Models;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainTripsInfo ViewModel => DataContext as TrainTripsInfo;

    public TrainNumberTripDetailsPage()
    {
        InitializeComponent();
    }
}
