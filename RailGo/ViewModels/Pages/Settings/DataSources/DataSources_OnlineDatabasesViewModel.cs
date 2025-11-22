using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RailGo.Contracts.Services;
using RailGo.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Microsoft.UI.Xaml.Controls;
using RailGo.Services;
using RailGo.Core.Models.QueryDatas;
using RailGo.ViewModels.Pages.Shell;
using System.Collections.ObjectModel;
using RailGo.Core.Query.Online;
using RailGo.Core.Models.Settings;
using RailGo.Views.Windows;
using RailGo.ViewModels.Windows;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_OnlineDatabasesViewModel : ObservableRecipient
{
    public DataSources_OnlineDatabasesViewModel(IThemeSelectorService themeSelectorService)
    {

    }
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    private VersionInfo remoteDBInfo = new VersionInfo() { Db = "未获取" };

    [ObservableProperty]
    public InfoBarSeverity remoteDBInfoBarSeverity = InfoBarSeverity.Informational;


    [RelayCommand]
    private async Task GetRemoteDBInfoAsync()
    {

        try
        {
            progressBarVM.TaskIsInProgress = "Visible";
            RemoteDBInfo = await DBGetService.GetVersionInfoAsync();
            var DownloadDBWindowViewModel = App.GetService<GetOfflineDatabaseWindowViewModel>();
            DownloadDBWindowViewModel.SetRemoteDatabaaseVersion(RemoteDBInfo);
        }
        catch (Exception ex)
        {
            RemoteDBInfoBarSeverity = InfoBarSeverity.Error;
            RemoteDBInfo = new VersionInfo() { Db = "获取失败" };
        }
        finally
        {
            progressBarVM.TaskIsInProgress = "Collapsed";
        }
    }

    [RelayCommand]
    private async Task DownloadRemoteDBAsync()
    {
        GetOfflineDatabaseWindow downloadDBWindow = new GetOfflineDatabaseWindow();
        downloadDBWindow.Activate();
    }
}