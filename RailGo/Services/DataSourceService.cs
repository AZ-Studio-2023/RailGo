using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RailGo.Contracts.Services;
using RailGo.Core.Models.Settings;

namespace RailGo.Services;

public class DataSourceService : IDataSourceService
{
    private const string DataSourcesSettingsKey = "DataSources";
    private const string SelectedDataSourceSettingsKey = "SelectedDataSource"; // 新增设置键
    private readonly ILocalSettingsService _localSettingsService;

    private ObservableCollection<DataSourceGroup> _dataSources = new ObservableCollection<DataSourceGroup>();

    public DataSourceService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        var dataSources = await LoadDataSourcesFromSettingsAsync();
        _dataSources = new ObservableCollection<DataSourceGroup>(dataSources);
        await Task.CompletedTask;
    }

    #region 读取操作

    public async Task<ObservableCollection<DataSourceGroup>> GetAllDataSourcesAsync()
    {
        await InitializeAsync();
        return _dataSources;
    }

    public async Task<DataSourceGroup?> GetDataSourceGroupAsync(string groupName)
    {
        await InitializeAsync();
        return _dataSources.FirstOrDefault(g => g.Name == groupName);
    }

    public async Task<DataSourceMethod?> GetDataSourceMethodAsync(string groupName, string methodName)
    {
        var group = await GetDataSourceGroupAsync(groupName);
        return group?.Data.FirstOrDefault(m => m.Name == methodName);
    }

    #endregion

    #region 读取所有值的方法

    public async Task<ObservableCollection<DataSourceMethod>> GetAllMethodsAsync()
    {
        await InitializeAsync();

        var allMethods = new ObservableCollection<DataSourceMethod>();
        foreach (var group in _dataSources)
        {
            foreach (var method in group.Data)
            {
                allMethods.Add(method);
            }
        }

        return allMethods;
    }

    public async Task<ObservableCollection<string>> GetAllGroupNamesAsync()
    {
        await InitializeAsync();
        return new ObservableCollection<string>(_dataSources.Select(g => g.Name));
    }

    public async Task<ObservableCollection<DataSourceMethod>> GetMethodsByModeAsync(string mode)
    {
        await InitializeAsync();

        var methods = new ObservableCollection<DataSourceMethod>();
        foreach (var group in _dataSources)
        {
            foreach (var method in group.Data.Where(m => m.Mode == mode))
            {
                methods.Add(method);
            }
        }

        return methods;
    }

    #endregion

    #region 设置操作

    public async Task SetDataSourceGroupAsync(DataSourceGroup group)
    {
        await InitializeAsync();

        var existingGroup = _dataSources.FirstOrDefault(g => g.Name == group.Name);
        if (existingGroup != null)
        {
            _dataSources.Remove(existingGroup);
        }

        _dataSources.Add(group);
        await SaveDataSourcesToSettingsAsync(_dataSources);
    }

    public async Task SetDataSourceMethodAsync(string groupName, DataSourceMethod method)
    {
        await InitializeAsync();

        var group = _dataSources.FirstOrDefault(g => g.Name == groupName);
        if (group == null)
        {
            group = new DataSourceGroup { Name = groupName };
            _dataSources.Add(group);
        }

        var existingMethod = group.Data.FirstOrDefault(m => m.Name == method.Name);
        if (existingMethod != null)
        {
            group.Data.Remove(existingMethod);
        }

        group.Data.Add(method);
        await SaveDataSourcesToSettingsAsync(_dataSources);
    }

    public async Task UpdateDataSourceMethodAsync(string groupName, string methodName, string mode, string sources)
    {
        var method = new DataSourceMethod
        {
            Name = methodName,
            Mode = mode,
            Sources = sources
        };

        await SetDataSourceMethodAsync(groupName, method);
    }

    #endregion

    #region 选择的DataSource

    public async Task<string?> GetSelectedDataSourceAsync()
    {
        return await _localSettingsService.ReadSettingAsync<string>(SelectedDataSourceSettingsKey);
    }

    public async Task SetSelectedDataSourceAsync(string selectedDataSource)
    {
        await _localSettingsService.SaveSettingAsync(SelectedDataSourceSettingsKey, selectedDataSource);
    }

    public async Task<DataSourceGroup?> GetSelectedDataSourceGroupAsync()
    {
        var selectedName = await GetSelectedDataSourceAsync();
        return string.IsNullOrEmpty(selectedName) ? null : await GetDataSourceGroupAsync(selectedName);
    }

    public async Task<DataSourceMethod?> GetSelectedDataSourceMethodAsync(string methodName)
    {
        var selectedName = await GetSelectedDataSourceAsync();
        return string.IsNullOrEmpty(selectedName) ? null : await GetDataSourceMethodAsync(selectedName, methodName);
    }

    #endregion

    #region 辅助方法

    private async Task<List<DataSourceGroup>> LoadDataSourcesFromSettingsAsync()
    {
        var dataSources = await _localSettingsService.ReadSettingAsync<List<DataSourceGroup>>(DataSourcesSettingsKey);
        return dataSources ?? new List<DataSourceGroup>();
    }

    private async Task SaveDataSourcesToSettingsAsync(ObservableCollection<DataSourceGroup> dataSources)
    {
        await _localSettingsService.SaveSettingAsync(DataSourcesSettingsKey, dataSources.ToList());
    }

    #endregion
}