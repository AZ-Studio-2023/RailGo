using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

using RailGo.Core.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using RailGo.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainTripsInfo DataFromLast => DataContext as TrainTripsInfo;
    public TrainNumberTripDetailsViewModel ViewModel
    {
        get;
    }
    public string train_no;
    public string date;
    public TrainNumberTripDetailsPage()
    {
        this.Loaded += OnLoad;
        ViewModel = App.GetService<TrainNumberTripDetailsViewModel>();
        InitializeComponent();
    }

    public void OnLoad(object sender, RoutedEventArgs e)
    {
        train_no = DataFromLast.train_no;
        date = DataFromLast.date.ToString("yyyyMMdd");
        ViewModel.GetImformation(train_no, date);
        ViewModel.GetEmuImformation(train_no);
        this.Loaded -= OnLoad;
    }
}
