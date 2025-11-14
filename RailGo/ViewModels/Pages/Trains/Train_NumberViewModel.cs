using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using RailGo.Core.Models;
using RailGo.Core.OnlineQuery;

namespace RailGo.ViewModels;

public partial class Train_NumberViewModel : ObservableRecipient
{
    public Train_NumberViewModel()
    {
    }

    [ObservableProperty]
    public ObservableCollection<TrainPreselectResult> trainNumberTripsInfos;

    public string InputTrainTrips;
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    public async Task GettrainNumberTripsInfosContent()
    {
        progressBarVM.TaskIsInProgress = "Visible";
        try
        {
            TrainNumberTripsInfos = await ApiService.TrainPreselectAsync(InputTrainTrips);
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
