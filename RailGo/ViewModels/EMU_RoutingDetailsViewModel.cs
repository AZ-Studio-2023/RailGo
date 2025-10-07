using System.Collections.ObjectModel;
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
            var TrainEmuFromWhere = await ApiService.EmuAssignmentQueryAsync("trainSerialNumber", DataFromLast.EmuNoCode);

        }
        catch (Exception ex)
        {
            progressBarVM.IfShowErrorInfoBarOpen = true;
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
}