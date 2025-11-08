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
    public TrainPreselectResult DataFromLast => DataContext as TrainPreselectResult;
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
        train_no = DataFromLast.Number;
        date = System.DateTime.Now.ToString("yyyyMMdd");
        ViewModel.GetInformationCommand.Execute((train_no, date));
        this.Loaded -= OnLoad;
    }
}
