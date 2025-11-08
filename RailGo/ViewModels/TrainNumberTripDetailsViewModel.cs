using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RailGo.Core.Models;
using RailGo.Core.OnlineQuery;
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
    public Train realdata;

    [ObservableProperty]
    public string trainName;

    [ObservableProperty]
    public string alongTime;

    [ObservableProperty]
    public ObservableCollection<TimetableItem> viaStations;

    [ObservableProperty]
    public ObservableCollection<TrainDiagram> routing;

    [ObservableProperty]
    public string trainModel;

    [ObservableProperty]
    public ObservableCollection<EmuOperation> trainEmuInfos = new();

    [ObservableProperty]
    public string crType = " -- ";

    [ObservableProperty]
    public string ifEmuRouting = "Collapsed";

    [ObservableProperty]
    public string crTypeLabelBorderBrush = "#ffffff";

    [ObservableProperty]
    public string crTypeLabelBackground = "#ffffff";

    [ObservableProperty]
    public string bureauName;

    [ObservableProperty]
    public string arrivalTime;

    [ObservableProperty]
    public string endStationName;

    [ObservableProperty]
    public string fromTime;

    [ObservableProperty]
    public string beginStationName;

    [RelayCommand]
    public async Task GetInformationAsync((string train_no, string date) parameters)
    {
        var train_no = parameters.train_no;
        var date = parameters.date;
        progressBarVM.TaskIsInProgress = "Visible";
        var TrainTask = ApiService.TrainQueryAsync(train_no);
        var TrainEmuInfosTask = ApiService.EmuQueryAsync("train", train_no);
        try
        {
            await Task.WhenAll(TrainTask, TrainEmuInfosTask);
            TrainEmuInfos = TrainEmuInfosTask.Result;
            IfEmuRouting = "Visible";
        }
        catch
        {
            await Task.WhenAll(TrainTask);
        }
        var Realdata = TrainTask.Result;
        // 获取第一个和最后一个时刻表项
        var firstItem = Realdata.Timetable.First();
        var lastItem = Realdata.Timetable.Last();

        BeginStationName = firstItem.Station;
        FromTime = firstItem.Depart;
        EndStationName = lastItem.Station;
        ArrivalTime = lastItem.Arrive;

        // 计算运行时间
        TimeSpan startTime = TimeSpan.Parse(FromTime);
        TimeSpan endTime = TimeSpan.Parse(ArrivalTime);

        // 考虑天数差异
        int dayDifference = lastItem.Day - firstItem.Day;
        if (dayDifference > 0)
        {
            endTime = endTime.Add(TimeSpan.FromDays(dayDifference));
        }

        TimeSpan duration = endTime - startTime;
        int hours = (int)duration.TotalHours;
        int minutes = duration.Minutes;
        AlongTime = $"约{hours}时{minutes}分";
        BureauName = Realdata.BureauName + Realdata.CarOwner;
        TrainModel = Realdata.Car;
        TrainName = Realdata.Number;
        CrType = Realdata.Type;
        switch (CrType)
        {
            case "高速":
                CrTypeLabelBorderBrush = "#f09b7d";
                CrTypeLabelBackground = "#fdefeb";
                break;
            case "动车":
                CrTypeLabelBorderBrush = "#718bdc";
                CrTypeLabelBackground = "#e9edfa";
                break;

            case "新空调快速":
                CrTypeLabelBorderBrush = "#a8d9e9";
                CrTypeLabelBackground = "#e9f5fa";
                break;
        }
        Routing = Realdata.Diagram;
        ViaStations = Realdata.Timetable;
        progressBarVM.TaskIsInProgress = "Collapsed";
    }
}
