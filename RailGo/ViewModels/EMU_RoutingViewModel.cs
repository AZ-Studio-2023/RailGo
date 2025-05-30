﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using RailGo.Core.Models;
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
        try
        {
            var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/emu/" + InputEmuID);
            var response = await httpClient.SendAsync(requestMessage);
            var data = await response.Content.ReadAsStringAsync();
            var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
            TrainNumberEmuInfos = newTrainInfos;
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
