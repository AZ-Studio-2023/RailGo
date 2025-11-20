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
}