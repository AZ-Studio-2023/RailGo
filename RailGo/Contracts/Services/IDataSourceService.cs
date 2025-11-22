using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RailGo.Core.Models.Settings;

namespace RailGo.Contracts.Services;

public interface IDataSourceService
{
    Task InitializeAsync();

    // 读取操作
    Task<ObservableCollection<DataSourceGroup>> GetAllDataSourcesAsync();
    Task<DataSourceGroup?> GetDataSourceGroupAsync(string groupName);
    Task<DataSourceMethod?> GetDataSourceMethodAsync(string groupName, string methodName);

    // 读取所有值的方法
    Task<ObservableCollection<DataSourceMethod>> GetAllMethodsAsync();
    Task<ObservableCollection<string>> GetAllGroupNamesAsync();
    Task<ObservableCollection<DataSourceMethod>> GetMethodsByModeAsync(string mode);

    // 设置操作
    Task SetDataSourceGroupAsync(DataSourceGroup group);
    Task SetDataSourceMethodAsync(string groupName, DataSourceMethod method);
    Task UpdateDataSourceMethodAsync(string groupName, string methodName, string mode, string sources);

    // 选择的DataSource
    Task<string?> GetSelectedDataSourceAsync();
    Task SetSelectedDataSourceAsync(string selectedDataSource);

    // 基于 SelectedDataSource 的便捷方法
    Task<DataSourceGroup?> GetSelectedDataSourceGroupAsync();
    Task<DataSourceMethod?> GetSelectedDataSourceMethodAsync(string methodName);

    // 本地数据库源管理
    Task<ObservableCollection<LocalDatabaseSource>> GetLocalDatabaseSourcesAsync();
    Task SaveLocalDatabaseSourceAsync(LocalDatabaseSource source);
    Task DeleteLocalDatabaseSourceAsync(string sourceName);

    // 在线API源管理
    Task<ObservableCollection<OnlineApiSource>> GetOnlineApiSourcesAsync();
    Task SaveOnlineApiSourceAsync(OnlineApiSource source);
    Task DeleteOnlineApiSourceAsync(string sourceName);

    // 查询模式管理
    Task<string?> GetQueryModeAsync();
    Task SetQueryModeAsync(string queryMode);

    // 离线数据库版本管理
    Task<OfflineDatabaseVersion?> GetOfflineDatabaseVersionAsync();
    Task SetOfflineDatabaseVersionAsync(OfflineDatabaseVersion version);
    Task UpdateOfflineDatabaseVersionAsync(string version, int sequence);
}