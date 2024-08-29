using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace RailGo.Views;

public sealed partial class Station_InformationPage : Page
{
    public Station_InformationViewModel ViewModel
    {
        get;
    }

    public Station_InformationPage()
    {
        ViewModel = App.GetService<Station_InformationViewModel>();
        InitializeComponent();
    }
    private void GetstationSearchInfoBtn_Click(object sender, RoutedEventArgs e)
    {
        if (TrainNumberTextBox.Text != null)
        {
            // url = "https://api.rail.re/emu/" + EmuIdTextBox.Text;
            // ViewModel.GettrainNumberTripsInfosContent();
        }

    }
}
