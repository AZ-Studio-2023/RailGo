using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

using RailGo.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainTripsInfo ViewModel => DataContext as TrainTripsInfo;
    public string train_no;
    public string date;
    public int ListViewSelectItem = 0;
    //public string IfTrainEmuVisible = "Collapsed";
    //public string HitoryDepartureTimeIfRight = "Collapsed";

    public TrainDetail realdata;
    public string trainIndex;
    public string AlongTime;

    public TrainDetailsData realDetailsData;
    public ObservableCollection<ViaStation> viaStations;
    public ObservableCollection<RoutingItem> routing;
    public string TrainModel;

    public string ifHighSpeed = "Collapsed";
    public string CrType = " -- ";
    public string ifCrType = "Collapsed";
    public string CrTypeLabelBorderBrush = "#ffffff";
    public string CrTypeLabelBackground = "#ffffff";

    public TrainNumberTripDetailsPage()
    {
        this.Loaded += GetImformation;
    }

    // 刷新Bind
    public static readonly DependencyProperty IfViaStationsProperty =
        DependencyProperty.Register(
            nameof(IfViaStations),
            typeof(bool),
            typeof(TrainNumberTripDetailsPage),
            new PropertyMetadata(false, OnVisibilityChanged));
    public static readonly DependencyProperty IfTrainEmuProperty =
        DependencyProperty.Register(
            nameof(IfTrainEmuVisible),
            typeof(bool),
            typeof(TrainNumberTripDetailsPage),
            new PropertyMetadata(false, OnVisibilityChanged));
    public static readonly DependencyProperty HitoryDepartureTimeIfRightProperty =
        DependencyProperty.Register(
            nameof(HitoryDepartureTimeIfRight),
            typeof(bool),
            typeof(TrainNumberTripDetailsPage),
            new PropertyMetadata(false, OnVisibilityChanged));

    public Visibility IfViaStationsVisibility =>
        IfViaStations ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IfTrainEmuVisibility =>
        IfTrainEmuVisible ? Visibility.Visible : Visibility.Collapsed;
    public Visibility HitoryDepartureTimeIfRightVisibility =>
        HitoryDepartureTimeIfRight ? Visibility.Visible : Visibility.Collapsed;

    private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var page = d as TrainNumberTripDetailsPage;
        page?.UpdateVisibility();
    }
    private void UpdateVisibility()
    {
        Bindings.Update();
    }

    public bool IfViaStations
    {
        get => (bool)GetValue(IfViaStationsProperty);
        set => SetValue(IfViaStationsProperty, value);
    }
    public bool IfTrainEmuVisible
    {
        get => (bool)GetValue(IfTrainEmuProperty);
        set => SetValue(IfTrainEmuProperty, value);
    }
    public bool HitoryDepartureTimeIfRight
    {
        get => (bool)GetValue(HitoryDepartureTimeIfRightProperty);
        set => SetValue(HitoryDepartureTimeIfRightProperty, value);
    }

    private void ImfomationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView listView = sender as ListView;
        IfViaStations = false;
        IfTrainEmuVisible = false;
        HitoryDepartureTimeIfRight = false;
        switch (listView.Items.IndexOf(listView.SelectedItem))
        {
            case 0:
                IfViaStations = true;
                break;

            case 1:
                IfTrainEmuVisible = true;
                break;

            case 2:
                HitoryDepartureTimeIfRight = true;
                break;
        }
    }

    private void GetImformation(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        train_no = ViewModel.train_no;
        date = ViewModel.date.ToString("yyyyMMdd");
        InitializeComponent();
        this.Loaded -= GetImformation;

        var httpClient = new HttpClient();
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "date",date },
            {"trainNumber",train_no }

        });
        var response = httpClient.PostAsync(new Uri("https://rail.moefactory.com/api/trainNumber/query"), body).Result;
        var data = response.Content.ReadAsStringAsync().Result;
        TrainNumberTripDetailsModel trainInfo = JsonConvert.DeserializeObject<TrainNumberTripDetailsModel>(data);
        realdata = trainInfo.Data.DataList[0];
        trainIndex = realdata.TrainIndex.ToString();
        int hours = realdata.DurationMinutes / 60;
        int minutes = realdata.DurationMinutes % 60;
        AlongTime = $"约{hours}时{minutes}分";

        if (realdata.TrainType == "高速" || realdata.TrainType == "动车")
        {
            ifHighSpeed = "Visible";
        }
        switch (realdata.CrType)
        {
            case 1:
                ifCrType = "Visible";
                CrType = "复兴号CR400";
                CrTypeLabelBorderBrush = "#f09b7d";
                CrTypeLabelBackground = "#fdefeb";
                break;
            case 2:
                ifCrType = "Visible";
                CrType = "复兴号CR300";
                CrTypeLabelBorderBrush = "#d57df0";
                CrTypeLabelBackground = "#f8ebfd";
                break;

            case 3:
                ifCrType = "Visible";
                CrType = "复兴号CR200";
                CrTypeLabelBorderBrush = "#a8d9e9";
                CrTypeLabelBackground = "#e9f5fa";
                break;
        }

        body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "date",date },
            {"trainIndex",trainIndex },
            {"includeCheckoutNames","true" }
        });
        response = httpClient.PostAsync(new Uri("https://rail.moefactory.com/api/trainDetails/query"), body).Result;
        data = response.Content.ReadAsStringAsync().Result;
        TrainDetailsInfoModel trainDetailsInfo = JsonConvert.DeserializeObject<TrainDetailsInfoModel>(data);
        realDetailsData = trainDetailsInfo.Data; // 最外层数据，包含路局、餐车等
        viaStations = realDetailsData.ViaStations; // 经过的站点集合
        try
        {
            routing = realDetailsData.Routing.RoutingItems; // 车组交路
            TrainModel = realDetailsData.Routing.TrainModel; // 车组型号
        }
        catch { }
    }

}
