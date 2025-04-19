using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using RailGo.Core.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace RailGo.ViewModels;

public partial class StationDetailsViewModel : ObservableRecipient
{
    public StationDetailsViewModel()
    {
    }
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    public string stationNameLook;

    [ObservableProperty]
    public ObservableCollection<StationTrainsInfo> stationTrainsInfoList;

    public async Task GetImformation(string StationName, string TeleCode)
    {
        progressBarVM.TaskIsInProgress = "Visible";
        string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        var contentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "utf-8"
        };

        var httpClient = new HttpClient();
        var payload = new
        {
            @params = new
            {
                stationCode = TeleCode,
                type = "D"
            },
            isSign = 0
        };
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://tripapi.ccrgt.com/crgt/trip-server-app/screen/getStationScreenByStationCode")
        {
            Content = content
            // (这个破玩意竟然不支持3个参数的构造，太坏了）
        };
        requestMessage.Headers.Add("User-Agent", UserAgent);
        var response = await httpClient.SendAsync(requestMessage);
        var data = await response.Content.ReadAsStringAsync();
        StationTrainsInfoApiResponse StationTrainsInfoApiResponse = JsonConvert.DeserializeObject<StationTrainsInfoApiResponse>(data);
        StationTrainsInfoList = StationTrainsInfoApiResponse.Data.List;
        StationNameLook = StationName;
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
}