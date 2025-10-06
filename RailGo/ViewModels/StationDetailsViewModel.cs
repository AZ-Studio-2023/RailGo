using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Core.OnlineQuery;
using RailGo.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace RailGo.ViewModels;

public partial class StationDetailsViewModel : ObservableRecipient
{
    public StationDetailsViewModel()
    {
    }

    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    private string stationNameLook;

    [ObservableProperty]
    private string stationPinyin;

    [ObservableProperty]
    private string stationBureau;

    [ObservableProperty]
    private string stationBelong;

    [ObservableProperty]
    private string stationType;

    [ObservableProperty]
    private string stationCodes;

    [ObservableProperty]
    private ObservableCollection<StationTrain> stationTrains;

    [ObservableProperty]
    private ObservableCollection<StationScreenItem> stationBigScreen;

    [ObservableProperty]
    private bool isLoading;

    // 存储当前车站的电报码，用于查找停靠信息
    private string currentStationTelecode;

    [RelayCommand]
    public async Task GetInformationAsync((string StationName, string TeleCode) stationInfo)
    {
        string teleCode = stationInfo.TeleCode;
        string stationName = stationInfo.StationName;
        if (string.IsNullOrEmpty(teleCode) || string.IsNullOrEmpty(stationName))
            return;

        try
        {
            IsLoading = true;
            progressBarVM.TaskIsInProgress = "Visible";

            // 调用车站详情API
            var stationTask = ApiService.StationQueryAsync(teleCode);
            var screenTask = ApiService.GetBigScreenDataAsync(stationName);
            await Task.WhenAll(stationTask, screenTask);
            var stationResponse = stationTask.Result;
            var screenResponse = screenTask.Result;

            if (stationResponse?.Data != null)
            {
                // 设置车站基本信息
                var stationData = stationResponse.Data;
                StationNameLook = stationName;
                StationPinyin = stationData.Pinyin;
                StationBureau = stationData.Bureau ?? "未知";
                StationBelong = stationData.Belong ?? "未知";
                StationType = stationData.Type != null ? string.Join("、", stationData.Type) : "未知";
                StationCodes = $"{stationData.PinyinTriple}/{stationData.Telecode}";

                // 保存当前车站电报码
                currentStationTelecode = stationData.Telecode;

                // 设置车次信息
                if (stationResponse.Trains != null && stationResponse.Trains.Any())
                {
                    Trace.WriteLine("GetTrains");
                    StationTrains = new ObservableCollection<StationTrain>(stationResponse.Trains);
                }
                else
                {
                    StationTrains = new ObservableCollection<StationTrain>();
                }
            }

            if (screenResponse?.Data != null)
            {
                StationBigScreen = new ObservableCollection<StationScreenItem>(screenResponse.Data);
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            progressBarVM.IfShowErrorInfoBarOpen = true;
            progressBarVM.ShowErrorInfoBarContent = ex.Message;
            progressBarVM.ShowErrorInfoBarTitle = "Error";
            WaitCloseInfoBar();
            StationTrains = new ObservableCollection<StationTrain>();
        }
        finally
        {
            IsLoading = false;
            progressBarVM.TaskIsInProgress = "Collapsed";
        }
    }
    private async void WaitCloseInfoBar()
    {
        await Task.Delay(3000);
        progressBarVM.IfShowErrorInfoBarOpen = false;
    }

}