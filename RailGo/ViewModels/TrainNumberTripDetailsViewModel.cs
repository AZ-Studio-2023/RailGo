using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using RailGo.Core.Models;
using RailGo.Views;
using Windows.Media.Protection.PlayReady;

namespace RailGo.ViewModels;

public partial class TrainNumberTripDetailsViewModel : ObservableRecipient
{
    public TrainNumberTripDetailsViewModel()
    {
    }

    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    public TrainDetail realdata;

    [ObservableProperty]
    public string trainIndex;

    [ObservableProperty]
    public string alongTime;


    [ObservableProperty]
    public TrainDetailsData realDetailsData;

    [ObservableProperty]
    public ObservableCollection<ViaStation> viaStations;

    [ObservableProperty]
    public ObservableCollection<RoutingItem> routing;

    [ObservableProperty]
    public string trainModel;

    [ObservableProperty]
    public ObservableCollection<TrainTripsInfo> trainNumberTripsInfos;

    [ObservableProperty]
    public string ifHighSpeed = "Collapsed";

    [ObservableProperty]
    public string crType = " -- ";

    [ObservableProperty]
    public string ifCrType = "Collapsed";

    [ObservableProperty]
    public string crTypeLabelBorderBrush = "#ffffff";

    [ObservableProperty]
    public string crTypeLabelBackground = "#ffffff";

    public async Task GetImformation(string train_no, string date)
    {
        progressBarVM.TaskIsInProgress = "Visible";
        string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/132.0.0.0 Safari/537.36 Edge/132.0.0.0";
        var contentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "utf-8"
        };

        var httpClient = new HttpClient();
        var formData = new Dictionary<string, string>
        {
            { "date",date },
            {"trainNumber",train_no }

        };
        var content = new FormUrlEncodedContent(formData); ;
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://rail.moefactory.com/api/trainNumber/query")
        {
            Content = content
            // (这个破玩意竟然不支持3个参数的构造，太坏了）
        };
        requestMessage.Headers.Add("User-Agent", UserAgent);
        requestMessage.Content.Headers.ContentType = contentType;
        var response = await httpClient.SendAsync(requestMessage);
        var data = await response.Content.ReadAsStringAsync();
        TrainNumberTripDetailsModel trainInfo = JsonConvert.DeserializeObject<TrainNumberTripDetailsModel>(data);
        Realdata = trainInfo.Data.DataList[0];
        TrainIndex = realdata.TrainIndex.ToString();
        int hours = realdata.DurationMinutes / 60;
        int minutes = realdata.DurationMinutes % 60;
        AlongTime = $"约{hours}时{minutes}分";
        if (Realdata.TrainType == "高速" || Realdata.TrainType == "动车")
        {
            IfHighSpeed = "Visible";
        }
        switch (Realdata.CrType)
        {
            case 1:
                IfCrType = "Visible";
                CrType = "复兴号CR400";
                CrTypeLabelBorderBrush = "#f09b7d";
                CrTypeLabelBackground = "#fdefeb";
                break;
            case 2:
                IfCrType = "Visible";
                CrType = "复兴号CR300";
                CrTypeLabelBorderBrush = "#718bdc";
                CrTypeLabelBackground = "#e9edfa";
                break;

            case 3:
                IfCrType = "Visible";
                CrType = "复兴号CR200";
                CrTypeLabelBorderBrush = "#a8d9e9";
                CrTypeLabelBackground = "#e9f5fa";
                break;
        }
        formData = new Dictionary<string, string>
        {
            { "date",date },
            {"trainIndex",trainIndex },
            {"includeCheckoutNames","true" }
        };
        content = new FormUrlEncodedContent(formData);
        requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://rail.moefactory.com/api/trainDetails/query")
        {
            Content = content
        };
        requestMessage.Headers.Add("User-Agent", UserAgent);
        requestMessage.Content.Headers.ContentType = contentType;
        response = await httpClient.SendAsync(requestMessage);
        data = await response.Content.ReadAsStringAsync();
        TrainDetailsInfoModel trainDetailsInfo = JsonConvert.DeserializeObject<TrainDetailsInfoModel>(data);
        RealDetailsData = trainDetailsInfo.Data; // 最外层数据，包含路局、餐车等
        ViaStations = realDetailsData.ViaStations; // 经过的站点集合
        try
        {
            Routing = realDetailsData.Routing.RoutingItems; // 车组交路
            TrainModel = realDetailsData.Routing.TrainModel; // 车组型号
        }
        catch { }
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
    public async Task GetEmuImformation(string train_no)
    {
        var httpClient = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/train/" + train_no);
        var response = await httpClient.SendAsync(requestMessage);
        var data = await response.Content.ReadAsStringAsync();
        var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
        TrainNumberTripsInfos = newTrainInfos;
    }
}
