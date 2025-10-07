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
    public ObservableCollection<EmuOperation> trainNumberEmuInfos = new();

    [ObservableProperty]
    private bool isLoading;

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
            TrainNumberEmuInfos = await ApiService.EmuQueryAsync("emu", DataFromLast.EmuNo);
            var TrainEmuFromWhereAll = await ApiService.EmuAssignmentQueryAsync("trainSerialNumber", DataFromLast.EmuNoCode);
            // 在您的查询方法调用后
            var targetEmu = FilterByTrainModel(TrainEmuFromWhereAll, DataFromLast.EmuNoModel);

            if (targetEmu != null)
            {
                // 找到了匹配的动车组配属信息
                Trace.WriteLine($"所属路局: {targetEmu.Bureau}");
                Trace.WriteLine($"所属段: {targetEmu.Department}");
                Trace.WriteLine($"制造商: {targetEmu.Manufacturer}");
            }
            else
            {
                // 没有找到匹配的车型
                Trace.WriteLine("未找到指定车型的配属信息");
            }
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