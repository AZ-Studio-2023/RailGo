using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using RailGo.Contracts.Services;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;
using RailGo.Core.Query;
using RailGo.Core.Query.Online;

namespace RailGo.Services;

public class QueryService
{
    #region URL常量定义
    private const string TrainPreselect_url = "https://data.railgo.zenglingkun.cn/api/train/preselect";
    private const string TrainQuery_url = "https://data.railgo.zenglingkun.cn/api/train/query";
    private const string StationToStationQuery_url = "https://data.railgo.zenglingkun.cn/api/train/sts_query";
    private const string StationPreselect_url = "https://data.railgo.zenglingkun.cn/api/station/preselect";
    private const string StationQuery_url = "https://data.railgo.zenglingkun.cn/api/station/query";
    private const string EmuAssignmentQuery_url = "https://delay.data.railgo.zenglingkun.cn/api/trainAssignment/queryEmu";
    private const string TrainDelayQuery_url = "https://delay.data.railgo.zenglingkun.cn/api/trainDetails/queryTrainDelayDetails";
    private const string PlatformInfoQuery_url = "https://mobile.12306.cn/wxxcx/wechat/bigScreen/getExit";
    private const string GetBigScreenData_url = "https://screen.data.railgo.zenglingkun.cn";
    private const string EmuQuery_url = "https://emu.data.railgo.zenglingkun.cn";
    private const string DownloadEmuImage_url = "https://tp.railgo.zenglingkun.cn/api";
    #endregion

    #region 离线模式判断

    /// <summary>
    /// 判断是否使用离线模式
    /// </summary>
    private readonly IDataSourceService _dataSourceService;

    public QueryService(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
    }

    private async Task<bool> IsOfflineModeAsync()
    {
        var queryMode = await _dataSourceService.GetQueryModeAsync();
        return queryMode == "Offline";
    }

    /// <summary>
    /// 获取数据库路径
    /// </summary>
    private  string GetDatabasePath()
    {
        return DBGetService.GetLocalDatabasePath();
    }

    #endregion

    #region 车次查询接口

    /// <summary>
    /// 车次预选搜索
    /// </summary>
    public async Task<ObservableCollection<TrainPreselectResult>> QueryTrainPreselectAsync(string keyword)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : TrainPreselect_url;
        return await ApiService.TrainPreselectAsync(isOfflineMode, urlOrDbPath, keyword);
    }

    /// <summary>
    /// 车次详情查询
    /// </summary>
    public  async Task<Train> QueryTrainQueryAsync(string trainNumber)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : TrainQuery_url;
        return await ApiService.TrainQueryAsync(isOfflineMode, urlOrDbPath, trainNumber);
    }

    /// <summary>
    /// 站站查询
    /// </summary>
    public  async Task<List<Train>> QueryStationToStationQueryAsync(string from, string to, string date)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : StationToStationQuery_url;
        return await ApiService.StationToStationQueryAsync(isOfflineMode, urlOrDbPath, from, to, date);
    }

    #endregion

    #region 车站查询接口

    /// <summary>
    /// 车站预选搜索
    /// </summary>
    public  async Task<ObservableCollection<StationPreselectResult>> QueryStationPreselectAsync(string keyword)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : StationPreselect_url;
        return await ApiService.StationPreselectAsync(isOfflineMode, urlOrDbPath, keyword);
    }

    /// <summary>
    /// 车站详情查询
    /// </summary>
    public  async Task<StationQueryResponse> QueryStationQueryAsync(string telecode)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : StationQuery_url;
        return await ApiService.StationQueryAsync(isOfflineMode, urlOrDbPath, telecode);
    }

    /// <summary>
    /// 车站大屏数据
    /// </summary>
    public  async Task<BigScreenData> QueryGetBigScreenDataAsync(string stationName)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : GetBigScreenData_url;
        return await ApiService.GetBigScreenDataAsync(isOfflineMode, urlOrDbPath, stationName);
    }

    #endregion

    #region 动车组查询接口

    /// <summary>
    /// 动车组运行交路查询
    /// </summary>
    public  async Task<ObservableCollection<EmuOperation>> QueryEmuQueryAsync(string type, string keyword)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : EmuQuery_url;
        return await ApiService.EmuQueryAsync(isOfflineMode, urlOrDbPath, type, keyword);
    }

    /// <summary>
    /// 动车组配属查询
    /// </summary>
    public  async Task<ObservableCollection<EmuAssignment>> QueryEmuAssignmentQueryAsync(
        string type, string keyword, int cursor = 0, int count = 15)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : EmuAssignmentQuery_url;
        return await ApiService.EmuAssignmentQueryAsync(isOfflineMode, urlOrDbPath, type, keyword, cursor, count);
    }

    #endregion

    #region 实时数据接口

    /// <summary>
    /// 正晚点查询
    /// </summary>
    public  async Task<List<DelayInfo>> QueryTrainDelayAsync(string date, string trainNumber,
        string fromStation, string toStation)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : TrainDelayQuery_url;
        return await ApiService.QueryTrainDelayAsync(isOfflineMode, urlOrDbPath, date, trainNumber, fromStation, toStation);
    }

    /// <summary>
    /// 停台检票口查询
    /// </summary>
    public  async Task<PlatformInfo> QueryPlatformInfoAsync(string stationCode, string trainDate,
        string type, string stationTrainCode)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : PlatformInfoQuery_url;
        return await ApiService.QueryPlatformInfoAsync(isOfflineMode, urlOrDbPath, stationCode, trainDate, type, stationTrainCode);
    }

    #endregion

    #region 其他接口

    /// <summary>
    /// 下载动车组图片
    /// </summary>
    public  async Task<byte[]> QueryDownloadEmuImageAsync(string trainModel)
    {
        bool isOfflineMode = (await IsOfflineModeAsync());
        string urlOrDbPath = isOfflineMode ? GetDatabasePath() : DownloadEmuImage_url;
        return await ApiService.DownloadEmuImageAsync(isOfflineMode, urlOrDbPath, trainModel);
    }

    #endregion
}