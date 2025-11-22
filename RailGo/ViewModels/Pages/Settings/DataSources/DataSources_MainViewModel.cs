using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Contracts.Services;
using System.Threading.Tasks;
using RailGo.Core.Query.Online;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_MainViewModel : ObservableObject
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private string? _queryMode;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool ifOfflineAvailable;

    [ObservableProperty]
    private bool ifCustomAvailable;

    public DataSources_MainViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
        _ = LoadSettingsAsync();
    }

    public void CheckAvailableModes()
    {
        IfOfflineAvailable = DBGetService.LocalDatabaseExists();
        IfCustomAvailable = false;
    }

    private async Task LoadSettingsAsync()
    {
        try
        {
            IsLoading = true;
            QueryMode = await _dataSourceService.GetQueryModeAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    // 当 QueryMode 属性变化时自动保存
    partial void OnQueryModeChanged(string? value)
    {
        if (!IsLoading && !string.IsNullOrEmpty(value))
        {
            _ = _dataSourceService.SetQueryModeAsync(value);
        }
    }
}