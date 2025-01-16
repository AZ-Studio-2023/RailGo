using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using RailGo.Models;

namespace RailGo.ViewModels;

public partial class Train_NumberViewModel : ObservableRecipient
{
    public Train_NumberViewModel()
    {
    }

    [ObservableProperty]
    public ObservableCollection<TrainTripsInfo> trainNumberTripsInfos;

    public string InputTrainTrips;
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    public async Task GettrainNumberTripsInfosContent()
    {
        progressBarVM.TaskIsInProgress = "Visible";
        var httpClient = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/train/" + InputTrainTrips);
        var response = await httpClient.SendAsync(requestMessage);
        var data = await response.Content.ReadAsStringAsync();
        var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
        TrainNumberTripsInfos = newTrainInfos;
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
}
