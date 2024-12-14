using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

using RailGo.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainTripsInfo ViewModel => DataContext as TrainTripsInfo;
    public string train_no;
    public string date;

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

    private static DateTime nowdateTime = DateTime.Now;
    public string TrainTripscontent;
    public string InputTrainTrips;
    public string url = "https://api.rail.re/train/";
    public ObservableCollection<TrainTripsInfo> trainNumberTripsInfos = new();

    public TrainNumberTripDetailsPage()
    {
        this.Loaded += GetImformation;
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
                CrTypeLabelBorderBrush = "#718bdc";
                CrTypeLabelBackground = "#e9edfa";
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
        GettrainNumberTripsInfosContent();
    }

    public async Task GettrainNumberTripsInfosContent()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url + ViewModel.train_no);
                if (response.IsSuccessStatusCode)
                {
                    TrainTripscontent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // NotificationQueue.Show(null, 2000);
                }

            }
            var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(TrainTripscontent);
            foreach (var trainInfo in newTrainInfos)
            {
                trainNumberTripsInfos.Add(trainInfo);
            }
            DongCheZuJiaoLu.ItemsSource = trainNumberTripsInfos;
        }
        catch
        {
            // NotificationQueue.Show(null, 2000);
        }
    }

}
