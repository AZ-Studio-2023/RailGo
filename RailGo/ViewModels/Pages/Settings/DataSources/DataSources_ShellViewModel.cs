using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RailGo.Contracts.Services;
using RailGo.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using RailGo.Core.Query.Online;
using RailGo.Services;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_ShellViewModel : ObservableRecipient
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private string querySourceCode;

    public DataSources_ShellViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
    }
    public async void CheckAvailableModes()
    {
        var MainViewModel = App.GetService<DataSources_MainViewModel>();
        MainViewModel.IfOfflineAvailable = DBGetService.LocalDatabaseExists();
        MainViewModel.IfCustomAvailable = false;
        var QueryModeSelcted = await _dataSourceService.GetQueryModeAsync();
        if (QueryModeSelcted == "Offline")
        {
            if (DBGetService.LocalDatabaseExists())
            {
                MainViewModel.QuerySource = "离线（RailGo默认）";
                QuerySourceCode = "OfflineRailGO";
            }
            else
            {
                MainViewModel.QuerySource = "在线（RailGo默认）";
                QuerySourceCode = "OnlineRailGO";
            }
        }
    }
}