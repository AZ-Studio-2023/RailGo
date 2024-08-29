using Microsoft.UI.Xaml.Controls;

using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainNumberTripDetailsViewModel ViewModel
    {
        get;
    }

    public TrainNumberTripDetailsPage()
    {
        ViewModel = App.GetService<TrainNumberTripDetailsViewModel>();
        InitializeComponent();
    }
}
