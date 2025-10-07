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
    public string ifHighspeed = "Collapsed";

    [ObservableProperty]
    public string ifPassenger = "Collapsed";

    [ObservableProperty]
    public string ifCargo = "Collapsed";

    [ObservableProperty]
    private bool ifBigscreen = false;

    [ObservableProperty]
    private bool isLoading;

    // 存储当前车站的电报码，用于查找停靠信息
    private string currentStationTelecode;

    [RelayCommand]
    public async Task GetInformationAsync((string StationName, string TeleCode, List<string> Type) stationInfo)
    {
        string teleCode = stationInfo.TeleCode;
        string stationName = stationInfo.StationName;
        List<string> type = stationInfo.Type;
        if (string.IsNullOrEmpty(teleCode) || string.IsNullOrEmpty(stationName))
            return;

        try
        {
            IsLoading = true;
            progressBarVM.TaskIsInProgress = "Visible";

            StationNameLook = stationName;
            // 设置车站类型标签
            if (type != null)
            {
                if (type.Contains("高"))
                {
                    IfHighspeed = "Visible";
                }

                if (type.Contains("客"))
                {
                    IfPassenger = "Visible";
                    IfBigscreen = true;
                }

                if (type.Contains("货"))
                {
                    IfCargo = "Visible";
                }

            }
            var stationResponse = new StationQueryResponse();
            var screenResponse = new BigScreenData();
            // 调用车站详情API
            if (IfBigscreen)
            {
                var stationTask = ApiService.StationQueryAsync(teleCode);
                var screenTask = ApiService.GetBigScreenDataAsync(stationName);
                await Task.WhenAll(stationTask, screenTask);
                stationResponse = stationTask.Result;
                screenResponse = screenTask.Result;
            }

            if (stationResponse?.Data != null)
            {
                // 设置车站基本信息
                var stationData = stationResponse.Data;
                StationPinyin = stationData.Pinyin;
                StationBelong = $"{stationData.Bureau ?? "未知"} {stationData.Belong} 辖";
                StationCodes = $"{stationData.PinyinTriple}/-{stationData.Telecode}";

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
    public StationTrain FindstationTrainsByTrainNumber(string trainNumber)
    {
        return stationTrains.FirstOrDefault(item => item.Number == trainNumber);
    }

}