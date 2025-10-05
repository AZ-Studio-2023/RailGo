using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Core.OnlineQuery;
using RailGo.Core.Models;
using System.Threading.Tasks;
using System.Linq;

namespace RailGo.ViewModels;

public partial class Station_InformationViewModel : ObservableRecipient
{
    public Station_InformationViewModel()
    {
    }

    [ObservableProperty]
    private ObservableCollection<StationSearch> stations = new ObservableCollection<StationSearch>();

    [ObservableProperty]
    private string inputSearchStation;

    [ObservableProperty]
    private bool isLoading;

    // 使用 AsyncRelayCommand 替代原来的同步方法
    [RelayCommand]
    private async Task SearchStationsAsync()
    {
        if (string.IsNullOrWhiteSpace(InputSearchStation))
        {
            Stations.Clear();
            return;
        }

        try
        {
            IsLoading = true;

            // 调用 API 进行搜索
            var results = await ApiService.StationPreselectAsync(InputSearchStation);

            // 清空现有数据
            Stations.Clear();

            // 转换并添加新数据
            if (results != null && results.Any())
            {
                foreach (var result in results)
                {
                    var stationSearch = StationSearch.FromPreselectResult(result);
                    Stations.Add(stationSearch);
                }
            }

            Debug.WriteLine($"搜索到 {Stations.Count} 个车站");
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine($"搜索失败: {ex.Message}");
            // 可以在这里添加错误处理，比如显示提示信息
        }
        finally
        {
            IsLoading = false;
        }
    }

    // 保留原有的同步搜索方法作为备用（如果需要）
    public ObservableCollection<StationSearch> SearchData(ObservableCollection<StationSearch> sourceCollection, string queryText)
    {
        if (string.IsNullOrWhiteSpace(queryText))
            return new ObservableCollection<StationSearch>();

        string normalizedQuery = queryText.Replace(" ", "");

        var filteredItems = sourceCollection
            .Where(item =>
                item.Name?.Replace(" ", "").Contains(normalizedQuery, StringComparison.InvariantCultureIgnoreCase) == true)
            .ToList();

        return new ObservableCollection<StationSearch>(filteredItems);
    }
}