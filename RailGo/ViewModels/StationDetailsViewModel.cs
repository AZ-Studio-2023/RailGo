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
    private bool isLoading;

    // 存储当前车站的电报码，用于查找停靠信息
    private string currentStationTelecode;

    [RelayCommand]
    private async Task GetInformationAsync(string teleCode)
    {
        Trace.WriteLine("搜索车站中。。。。。");
        if (string.IsNullOrEmpty(teleCode))
            return;

        try
        {
            IsLoading = true;
            progressBarVM.TaskIsInProgress = "Visible";

            // 调用车站详情API
            var stationResponse = await ApiService.StationQueryAsync(teleCode);

            if (stationResponse?.Data != null)
            {
                // 设置车站基本信息
                var stationData = stationResponse.Data;
                StationNameLook = stationData.Name;
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
        }
        catch (Exception ex)
        {
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

    // 重载方法，接受名称和电报码
    public async Task GetInformationAsync(string stationName, string teleCode)
    {
        StationNameLook = stationName; // 先设置名称
        await GetInformationAsync(teleCode);
    }
}