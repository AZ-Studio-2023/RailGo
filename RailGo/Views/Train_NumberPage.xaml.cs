using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace RailGo.Views;

public sealed partial class Train_NumberPage : Page
{
    public Train_NumberViewModel ViewModel
    {
        get;
    }

    public Train_NumberPage()
    {
        ViewModel = App.GetService<Train_NumberViewModel>();
        InitializeComponent();
    }

    private void GetTrainNumberBtn_Click(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("12345");
        if (EmuIdTextBox.Text != null)
        {
            Trace.WriteLine("37890");
            // url = "https://api.rail.re/emu/" + EmuIdTextBox.Text;
            ViewModel.GettrainNumberTripsInfosContent();
        }

    }
}
