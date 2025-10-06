﻿using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Models;
using RailGo.ViewModels;
using RailGo.Core.OnlineQuery;

namespace RailGo.Views;

public sealed partial class StationDetailsPage : Page
{
    public StationPreselectResult DataFromLast => DataContext as StationPreselectResult;
    public StationScreenItem _item_bigscreen;
    public StationTrain _item_trains;

    public StationDetailsViewModel ViewModel
    {
        get;
    }

    public StationDetailsPage()
    {
        InitializeComponent();
        this.Loaded += OnLoad;
        ViewModel = App.GetService<StationDetailsViewModel>();
        TrainsDataGrid.Visibility = Visibility.Visible;
        BigScreenDataGrid.Visibility = Visibility.Collapsed;
    }

    public void OnLoad(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("OnLoad");
        if (DataFromLast != null)
        {
            Trace.WriteLine("GetDataFromLast");
            // 使用电报码获取详细信息
            ViewModel.GetInformationCommand.Execute((DataFromLast.Name, DataFromLast.TeleCode, DataFromLast.Type));
        }
        this.Loaded -= OnLoad;
    }

    private void OnNavButtonChecked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && TrainsDataGrid != null && BigScreenDataGrid != null)
        {
            // 根据选择的导航按钮切换右侧内容
            switch (radioButton.Content.ToString())
            {
                case "途经车次":
                    Trace.WriteLine("途经车次");
                    TrainsDataGrid.Visibility = Visibility.Visible;
                    BigScreenDataGrid.Visibility = Visibility.Collapsed;
                    // 其他页面隐藏
                    break;
                case "车站大屏":
                    Trace.WriteLine("车站大屏");
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    BigScreenDataGrid.Visibility = Visibility.Visible;
                    // 显示页面二
                    break;
                case "路线":
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    BigScreenDataGrid.Visibility = Visibility.Collapsed;
                    // 显示页面三
                    break;
            }
        }
    }

    private async Task DetailBtnClick(object sender, RoutedEventArgs e)
    {
        if (sender is HyperlinkButton button)
        {
            string BarHeader = null;
            string icon = null;
            Page page = null;
            StationTrain Details = null;
            bool Await = false;
            // 根据选择的导航按钮切换右侧内容
            switch (button.Name.ToString())
            {
                case "StationTrains_TrainInformation":
                    icon = "\uE7C0";
                    BarHeader = _item_trains.Number;
                    page = new TrainNumberTripDetailsPage()
                    {
                        DataContext = new TrainPreselectResult { FullNumber = _item_trains.Number }
                    };
                    break;
                case "StationTrains_FromStation":
                    icon = "\uF161";
                    BarHeader = _item_trains.FromStation.Station;
                    page = new StationDetailsPage()
                    {
                        DataContext = new StationPreselectResult { Name = _item_trains.FromStation.Station, TeleCode = _item_trains.FromStation.StationTelecode}
                    };
                    break;
                case "StationTrains_ToStation":
                    icon = "\uF161";
                    BarHeader = _item_trains.ToStation.Station;
                    page = new StationDetailsPage()
                    {
                        DataContext = new StationPreselectResult { Name = _item_trains.ToStation.Station, TeleCode = _item_trains.ToStation.StationTelecode }
                    };
                    break;
                case "BigScreen_TrainInformation":
                    icon = "\uE7C0";
                    BarHeader = _item_bigscreen.TrainNumber;
                    page = new TrainNumberTripDetailsPage()
                    {
                        DataContext = new TrainPreselectResult { FullNumber = _item_bigscreen.TrainNumber }
                    };
                    break;
                case "BigScreen_FromStation":
                    icon = "\uF161";
                    BarHeader = _item_bigscreen.FromStation;
                    Details = ViewModel.FindstationTrainsByTrainNumber(_item_bigscreen.TrainNumber);
                    page = new StationDetailsPage()
                    {
                        DataContext = new StationPreselectResult { Name = Details.FromStation.Station, TeleCode = Details.FromStation.StationTelecode }
                    };
                    break;
                case "BigScreen_ToStation":
                    icon = "\uF161";
                    BarHeader = _item_bigscreen.ToStation;
                    Details = ViewModel.FindstationTrainsByTrainNumber(_item_bigscreen.TrainNumber);
                    page = new StationDetailsPage()
                    {
                        DataContext = new StationPreselectResult { Name = Details.ToStation.Station, TeleCode = Details.ToStation.StationTelecode }
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