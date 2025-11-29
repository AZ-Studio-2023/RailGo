using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using RailGo.Contracts.Services;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;
using RailGo.Core.Models.Settings;
using RailGo.Core.Query;
using RailGo.Core.Query.Online;

namespace RailGo.Services;

public class QueryService : IQueryService
{
    private readonly IDataSourceService _dataSourceService;

    public QueryService(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
    }

    private async Task<GetPathModel> GetPath(string MethodName)
    {
        var GroupName = await _dataSourceService.GetSelectedDataSourceAsync();
        var method = await _dataSourceService.GetDataSourceMethodAsync(GroupName, MethodName);
        bool isOfflineMode = method.IsOfflineMode;
        string urlOrDbPath;

        if (method.SourceName == "RailGoDefalt")
        {
            if(isOfflineMode)
            {
                urlOrDbPath = GetDatabasePath();
            }
            else
            {
                urlOrDbPath = DefaultApiUrls.GetDefaultUrl(MethodName);
            }
        }
        else
        {
            if (isOfflineMode)
            {
                urlOrDbPath = await _dataSourceService.GetLocalDatabaseSourceAddressAsync(method.SourceName);
            }
            else
            {
                urlOrDbPath = await _dataSourceService.GetOnlineApiSourceAddressAsync(method.SourceName);
            }
        }
        return new GetPathModel
        {
            IsOfflineMode = isOfflineMode,
            Path = urlOrDbPath
        };
    }

    /// <summary>
    /// 获取数据库路径
    /// </summary>
    private string GetDatabasePath()
    {
        return DBGetService.GetLocalDatabasePath();
    }

    #region 车次查询接口

    /// <summary>
    /// 车次预选搜索
    /// </summary>
    public async Task<ObservableCollection<TrainPreselectResult>> QueryTrainPreselectAsync(string keyword)
    {
        var GotPath = await GetPath("QueryTrainPreselect");
        var SelectedDataSourceGroup = await _dataSourceService.GetSelectedDataSourceAsync();
        return await ApiService.TrainPreselectAsync(GotPath.IsOfflineMode, GotPath.Path, keyword);
    }

    /// <summary>
    /// 车次详情查询
    /// </summary>
    public async Task<Train> QueryTrainQueryAsync(string trainNumber)
    {
        var GotPath = await GetPath("QueryTrainQuery");
        return await ApiService.TrainQueryAsync(GotPath.IsOfflineMode, GotPath.Path, trainNumber);
    }

    /// <summary>
    /// 站站查询
    /// </summary>
    public async Task<List<Train>> QueryStationToStationQueryAsync(string from, string to, string date)
    {
        var GotPath = await GetPath("QueryStationToStationQuery");
        return await ApiService.StationToStationQueryAsync(GotPath.IsOfflineMode, GotPath.Path, from, to, date);
    }

    #endregion

    #region 车站查询接口

    /// <summary>
    /// 车站预选搜索
    /// </summary>
    public async Task<ObservableCollection<StationPreselectResult>> QueryStationPreselectAsync(string keyword)
    {
        var GotPath = await GetPath("QueryStationPreselect");
        return await ApiService.StationPreselectAsync(GotPath.IsOfflineMode, GotPath.Path, keyword);
    }

    /// <summary>
    /// 车站详情查询
    /// </summary>
    public async Task<StationQueryResponse> QueryStationQueryAsync(string telecode)
    {
        var GotPath = await GetPath("QueryStationQuery");
        return await ApiService.StationQueryAsync(GotPath.IsOfflineMode, GotPath.Path, telecode);
    }

    /// <summary>
    /// 车站大屏数据
    /// </summary>
    public async Task<BigScreenData> QueryGetBigScreenDataAsync(string stationName)
    {
        var GotPath = await GetPath("QueryGetBigScreenData");
        return await ApiService.GetBigScreenDataAsync(GotPath.IsOfflineMode, GotPath.Path, stationName);
    }

    #endregion

    #region 动车组查询接口

    /// <summary>
    /// 动车组运行交路查询
    /// </summary>
    public async Task<ObservableCollection<EmuOperation>> QueryEmuQueryAsync(string type, string keyword)
    {
        var GotPath = await GetPath("QueryEmuQuery");
        return await ApiService.EmuQueryAsync(GotPath.IsOfflineMode, GotPath.Path, type, keyword);
    }

    /// <summary>
    /// 动车组配属查询
    /// </summary>
    public async Task<ObservableCollection<EmuAssignment>> QueryEmuAssignmentQueryAsync(string type, string keyword, int cursor = 0, int count = 15)
    {
        var GotPath = await GetPath("QueryEmuAssignmentQuery");
        return await ApiService.EmuAssignmentQueryAsync(GotPath.IsOfflineMode, GotPath.Path, type, keyword, cursor, count);
    }

    #endregion

    #region 实时数据接口

    /// <summary>
    /// 正晚点查询
    /// </summary>
    public async Task<List<DelayInfo>> QueryTrainDelayAsync(string date, string trainNumber, string fromStation, string toStation)
    {
        var GotPath = await GetPath("QueryTrainDelay");
        return await ApiService.QueryTrainDelayAsync(GotPath.IsOfflineMode, GotPath.Path, date, trainNumber, fromStation, toStation);
    }

    /// <summary>
    /// 停台检票口查询
    /// </summary>
    public async Task<PlatformInfo> QueryPlatformInfoAsync(string stationCode, string trainDate, string type, string stationTrainCode)
    {
        var GotPath = await GetPath("QueryPlatformInfo");
        return await ApiService.QueryPlatformInfoAsync(GotPath.IsOfflineMode, GotPath.Path, stationCode, trainDate, type, stationTrainCode);
    }

    #endregion

    #region 其他接口

    /// <summary>
    /// 下载动车组图片
    /// </summary>
    public async Task<byte[]> QueryDownloadEmuImageAsync(string trainModel)
    {
        var GotPath = await GetPath("QueryDownloadEmuImage");
        return await ApiService.DownloadEmuImageAsync(GotPath.IsOfflineMode, GotPath.Path, trainModel);
    }

    #endregion
}