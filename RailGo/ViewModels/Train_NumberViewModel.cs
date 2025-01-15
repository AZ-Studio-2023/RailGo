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
    ObservableCollection<TrainTripsInfo> _TrainNumberTripsInfo = new ObservableCollection<TrainTripsInfo>();
    public ObservableCollection<TrainTripsInfo> trainNumberTripsInfos
    {

        get
        {
            return _TrainNumberTripsInfo;
        }
        set
        {
            _TrainNumberTripsInfo = value;
            OnPropertyChanged("trainNumberTripsInfos");
        }
    }

    private static DateTime nowdateTime = DateTime.Now;
    public string TrainTripscontent;
    public string InputTrainTrips;
    public string url = "https://api.rail.re/train/";
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    public async Task GettrainNumberTripsInfosContent()
    {
        progressBarVM.TaskIsInProgress = "Visible";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url + InputTrainTrips);
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
            trainNumberTripsInfos.Clear();
            foreach (var trainInfo in newTrainInfos)
            {
                trainNumberTripsInfos.Add(trainInfo);
            }
        }
        catch
        {
            // NotificationQueue.Show(null, 2000);
        }
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
}
