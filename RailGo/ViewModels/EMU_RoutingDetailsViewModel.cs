using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Core.Models;
using RailGo.Core.OnlineQuery;

namespace RailGo.ViewModels;

public partial class EMU_RoutingDetailsViewModel : ObservableRecipient
{
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    public ObservableCollection<EmuOperation> trainEmuInfos = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string trainEmuModel;

    [ObservableProperty]
    private string trainEmuCode;

    [ObservableProperty]
    private string trainBelong;

    [ObservableProperty]
    private string trainMaker;


    public EMU_RoutingDetailsViewModel()
    {
    }

    [RelayCommand]
    private async Task SearchEmuDetailsAsync(EmuOperation DataFromLast)
    {
        try
        {
            IsLoading = true;
            progressBarVM.TaskIsInProgress = "Visible";

            // 调用 API 进行搜索
            TrainEmuInfos = await ApiService.EmuQueryAsync("emu", DataFromLast.EmuNo);
            var TrainEmuFromWhereAll = await ApiService.EmuAssignmentQueryAsync("trainSerialNumber", DataFromLast.EmuNoCode);
            // 在您的查询方法调用后
            var targetEmu = FilterByTrainModel(TrainEmuFromWhereAll, DataFromLast.EmuNoModel);

            TrainBelong = $"{targetEmu.Bureau ?? "未知"} {targetEmu.Department ?? "未知"}段";
            TrainMaker = $"{targetEmu.Manufacturer ?? "未知"} 制造";
            TrainEmuModel = DataFromLast.EmuNoModel;
            TrainEmuCode = DataFromLast.EmuNoCode;
        }
        catch (Exception ex)
        {
            progressBarVM.IfShowErrorInfoBarOpen = true;
            Trace.WriteLine(ex.Message);
            Trace.WriteLine(ex);
            progressBarVM.ShowErrorInfoBarContent = ex.Message;
            progressBarVM.ShowErrorInfoBarTitle = "Error";
            WaitCloseInfoBar();
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
    public EmuAssignment FilterByTrainModel(ObservableCollection<EmuAssignment> assignments, string targetTrainModel)
    {
        if (assignments == null || string.IsNullOrEmpty(targetTrainModel))
            return null;

        // 精确匹配车型
        return assignments.FirstOrDefault(a => a.TrainModel == targetTrainModel);
    }
}