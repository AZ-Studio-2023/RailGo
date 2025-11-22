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
    private readonly IDataSourceService _dataSourceService;
    public DataSources_OnlineDatabasesViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
        _ = GetRemoteDBInfoAsync();
        _ = GetLocalDBInfoAsync();
    }
    public MainWindowViewModel progressBarVM = App.GetService<MainWindowViewModel>();

    [ObservableProperty]
    private VersionInfo remoteDBInfo = new VersionInfo() { Db = "未获取" };

    [ObservableProperty]
    private OfflineDatabaseVersion localDBInfo = new OfflineDatabaseVersion() { Version = "未下载" };

    [ObservableProperty]
    private string remoteDBRefreshedDate;

    [ObservableProperty]
    public InfoBarSeverity remoteDBInfoBarSeverity = InfoBarSeverity.Informational;

    [ObservableProperty]
    public InfoBarSeverity localDBInfoBarSeverity = InfoBarSeverity.Informational;


    [RelayCommand]
    public async Task GetRemoteDBInfoAsync()
    {

        try
        {
            progressBarVM.TaskIsInProgress = "Visible";
            RemoteDBInfo = await DBGetService.GetVersionInfoAsync();
            var DownloadDBWindowViewModel = App.GetService<GetOfflineDatabaseWindowViewModel>();
            DownloadDBWindowViewModel.SetRemoteDatabaaseVersion(RemoteDBInfo);
            RemoteDBRefreshedDate = DateTime.Now.ToString("yy.MM.dd HH:mm:ss");
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
    public async Task GetLocalDBInfoAsync()
    {

        try
        {
            progressBarVM.TaskIsInProgress = "Visible";
            LocalDBInfo = await _dataSourceService.GetOfflineDatabaseVersionAsync();
        }
        catch (Exception ex)
        {
            LocalDBInfoBarSeverity = InfoBarSeverity.Warning;
            LocalDBInfo = new OfflineDatabaseVersion() { Version = "获取失败或未下载" };
        }
        finally
        {
            progressBarVM.TaskIsInProgress = "Collapsed";
        }
    }

    [RelayCommand]
    private async Task DownloadRemoteDBAsync()
    {
        var downloadDBWindow = App.GetService<GetOfflineDatabaseWindow>();
        downloadDBWindow.Activate();
    }
}