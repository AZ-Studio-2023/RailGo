using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

using RailGo.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

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
        AlongTime = $"约{hours}小时{minutes}分钟";

        if (realdata.TrainType == "高速")
        {
            ifHighSpeed = "Visible";
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
        routing = realDetailsData.Routing.RoutingItems; // 车组交路
        TrainModel = realDetailsData.Routing.TrainModel; // 车组型号
    }

}
