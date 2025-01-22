using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using RailGo.Core.Models;

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
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/train/" + InputTrainTrips);
            var response = await httpClient.SendAsync(requestMessage);
            var data = await response.Content.ReadAsStringAsync();
            var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
            TrainNumberTripsInfos = newTrainInfos;
        }
        catch (Exception ex) 
        {
            progressBarVM.IfShowErrorInfoBarOpen = true;
            progressBarVM.ShowErrorInfoBarContent = ex.Message;
            progressBarVM.ShowErrorInfoBarTitle = "Error";
            WaitCloseInfoBar();
        }
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
    private async void WaitCloseInfoBar()
    {
        await Task.Delay(3000);
        progressBarVM.IfShowErrorInfoBarOpen = false;
    }
}
