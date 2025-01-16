using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using RailGo.Models;
using RailGo.Models;
using Newtonsoft.Json;
using Windows.System;

namespace RailGo.ViewModels;

public partial class EMU_RoutingViewModel : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<TrainTripsInfo> trainNumberEmuInfos;

    public string InputEmuID;
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    public EMU_RoutingViewModel()
    {
    }
    public async Task GettrainNumberEmuInfosContent()
    {
        progressBarVM.TaskIsInProgress = "Visible";
        var httpClient = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/emu/" + InputEmuID);
        var response = await httpClient.SendAsync(requestMessage);
        var data = await response.Content.ReadAsStringAsync();
        var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
        TrainNumberEmuInfos = newTrainInfos;
        Trace.WriteLine(data);
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
}
