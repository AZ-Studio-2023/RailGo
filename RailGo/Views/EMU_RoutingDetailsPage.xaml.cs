using Microsoft.UI.Xaml.Controls;

using RailGo.Core.Models;
using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class EMU_RoutingDetailsPage : Page
{
    public EmuOperation DataFromLast => DataContext as EmuOperation;
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
