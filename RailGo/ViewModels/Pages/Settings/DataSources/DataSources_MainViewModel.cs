using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Contracts.Services;
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
    private string querySource;

    [ObservableProperty]
    private bool allowCustomSource;

    [ObservableProperty]
    private ObservableCollection<string>? dataSourceGroups;

    [ObservableProperty]
    private string? selectedDataSourceGroup;

    public DataSources_MainViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
        _ = LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        try
        {
            IsLoading = true;
            QueryMode = await _dataSourceService.GetQueryModeAsync();
            AllowCustomSource = await _dataSourceService.GetIfAllowCustomSourceAsync();
            DataSourceGroups = await _dataSourceService.GetAllGroupNamesAsync();
            SelectedDataSourceGroup = await _dataSourceService.GetSelectedDataSourceAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnQueryModeChanged(string? value)
    {
        if (!IsLoading && !string.IsNullOrEmpty(value))
        {
            _ = _dataSourceService.SetQueryModeAsync(value);
        }
    }

    partial void OnAllowCustomSourceChanged(bool value)
    {
        if (!IsLoading)
        {
            _ = _dataSourceService.SetIfAllowCustomSourceAsync(value);
        }
    }

    partial void OnSelectedDataSourceGroupChanged(string? value)
    {
        if (!IsLoading && !string.IsNullOrEmpty(value))
        {
            _ = _dataSourceService.SetSelectedDataSourceAsync(value);
        }
    }
}