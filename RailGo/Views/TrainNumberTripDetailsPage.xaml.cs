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
    public TimetableItem _item_ViaStaion;
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
    private async void DetailBtnClick(object sender, RoutedEventArgs e)
    {
        if (sender is HyperlinkButton button)
        {
            string BarHeader = null;
            string icon = null;
            Page page = null;
            StationTrain Details = null;
            List<string> StationType = new() { "SearchingST" };
            bool Await = false;
            // 根据选择的导航按钮切换右侧内容
            switch (button.Name.ToString())
            {
                case "ViaStationsStationInformation":
                    icon = "\uF161";
                    BarHeader = _item_ViaStaion.Station;
                    page = new StationDetailsPage()
                    {
                        DataContext = new StationPreselectResult { Name = _item_ViaStaion.Station, TeleCode = _item_ViaStaion.StationTelecode, Type = StationType }
                    };
                    break;
            }

            TabViewItem tabViewItem = new()
            {
                Header = BarHeader,
                Content = page,
                CanDrag = true,
                IconSource = new FontIconSource() { Glyph = icon }
            };
            MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
            MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;

            if (Await)
            {

                //var StationDetails = await ApiService.StationPreselectAsync(BarHeader);
                //page = new StationDetailsPage()
                // {
                // DataContext = new StationPreselectResult { Name = Details.ToStation.Station, TeleCode = Details.ToStation.StationTelecode, Type = StationDetails }
                // };
            }
        }

    }
}
