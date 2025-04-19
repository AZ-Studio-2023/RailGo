using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Models;
using RailGo.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RailGo.Views;

public sealed partial class StationDetailsPage : Page
{
    public StationSearch DataFromLast => DataContext as StationSearch;
    public StationDetailsViewModel ViewModel
    {
        get;
    }
    public string StationName;
    public string TeleCode;

    public StationDetailsPage()
    {
        this.Loaded += OnLoad;
        ViewModel = App.GetService<StationDetailsViewModel>();
        InitializeComponent();
    }


    public void OnLoad(object sender, RoutedEventArgs e)
    {
        StationName = DataFromLast.Name;
        TeleCode = DataFromLast.TeleCode;
        ViewModel.GetImformation(StationName, TeleCode);
        this.Loaded -= OnLoad;
    }
}
