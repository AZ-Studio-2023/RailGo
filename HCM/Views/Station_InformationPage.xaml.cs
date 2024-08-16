using HCM.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace HCM.Views;

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
}
