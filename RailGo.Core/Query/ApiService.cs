using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;
using RailGo.Core.Query.Offline;
using RailGo.Core.Query.Online;
using Newtonsoft.Json;

namespace RailGo.Core.Query;

public class ApiService
{
    #region URL常量定义
    public const string TrainPreselect_url = "https://data.railgo.zenglingkun.cn/api/train/preselect";
    public const string TrainQuery_url = "https://data.railgo.zenglingkun.cn/api/train/query";
    public const string StationToStationQuery_url = "https://data.railgo.zenglingkun.cn/api/train/sts_query";
    public const string StationPreselect_url = "https://data.railgo.zenglingkun.cn/api/station/preselect";
    public const string StationQuery_url = "https://data.railgo.zenglingkun.cn/api/station/query";
    public const string EmuAssignmentQuery_url = "https://delay.data.railgo.zenglingkun.cn/api/trainAssignment/queryEmu";
    public const string TrainDelayQuery_url = "https://delay.data.railgo.zenglingkun.cn/api/trainDetails/queryTrainDelayDetails";
    public const string PlatformInfoQuery_url = "https://mobile.12306.cn/wxxcx/wechat/bigScreen/getExit";
    private const string GetBigScreenData_url = "https://screen.data.railgo.zenglingkun.cn";
    private const string EmuQuery_url = "https://emu.data.railgo.zenglingkun.cn";
    private const string DownloadEmuImage_url = "https://tp.railgo.zenglingkun.cn/api";
    #endregion

    #region 离线模式判断

    /// <summary>
    /// 判断是否使用离线模式
    /// </summary>
    private static bool IsOfflineMode()
    {
        return DBGetService.LocalDatabaseExists();
    }

    /// <summary>
    /// 获取离线服务实例
    /// </summary>
    public static T GetOfflineService<T>() where T : BaseOfflineService
    {
        var databasePath = DBGetService.GetLocalDatabasePath();
        return (T)Activator.CreateInstance(typeof(T), databasePath);
    }

    #endregion

    #region 车次查询接口

    /// <summary>
    /// 车次预选搜索
    /// </summary>
    public static async Task<ObservableCollection<TrainPreselectResult>> TrainPreselectAsync(string keyword)
    {
        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<TrainOfflineService>();
            var json = await offlineService.TrainPreselectAsync(keyword);
            var stringArray = JsonConvert.DeserializeObject<ObservableCollection<string>>(json);

            var result = new ObservableCollection<TrainPreselectResult>();
            if (stringArray != null)
            {
                foreach (var fullNumber in stringArray)
                {
                    result.Add(new TrainPreselectResult { FullNumber = fullNumber });
                }
            }
            return result;
        }
        else
        {
            var stringArray = await OnlineApiService.TrainPreselectAsync(keyword, TrainPreselect_url);
            var result = new ObservableCollection<TrainPreselectResult>();
            if (stringArray != null)
            {
                foreach (var fullNumber in stringArray)
                {
                    result.Add(new TrainPreselectResult { FullNumber = fullNumber });
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 车次详情查询
    /// </summary>
    public static async Task<Train> TrainQueryAsync(string trainNumber)
    {
        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<TrainOfflineService>();
            var json = await offlineService.TrainQueryAsync(trainNumber);
            return JsonConvert.DeserializeObject<Train>(json);
        }
        else
        {
            return await OnlineApiService.TrainQueryAsync(trainNumber, TrainQuery_url);
        }
    }

    /// <summary>
    /// 站站查询
    /// </summary>
    public static async Task<List<Train>> StationToStationQueryAsync(string from, string to, string date)
    {
        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<TrainOfflineService>();
            var json = await offlineService.StationToStationQueryAsync(from, to, date);
            return JsonConvert.DeserializeObject<List<Train>>(json);
        }
        else
        {
            return await OnlineApiService.StationToStationQueryAsync(from, to, date, StationToStationQuery_url);
        }
    }

    #endregion

    #region 车站查询接口

    /// <summary>
    /// 车站预选搜索
    /// </summary>
    public static async Task<ObservableCollection<StationPreselectResult>> StationPreselectAsync(string keyword)
    {
        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<StationOfflineService>();
            var json = await offlineService.StationPreselectAsync(keyword);
            return JsonConvert.DeserializeObject<ObservableCollection<StationPreselectResult>>(json);
        }
        else
        {
            return await OnlineApiService.StationPreselectAsync(keyword, StationPreselect_url);
        }
    }

    /// <summary>
    /// 车站详情查询
    /// </summary>
    public static async Task<StationQueryResponse> StationQueryAsync(string telecode)
    {
        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<StationOfflineService>();
            var json = await offlineService.StationQueryAsync(telecode);
            return JsonConvert.DeserializeObject<StationQueryResponse>(json);
        }
        else
        {
            return await OnlineApiService.StationQueryAsync(telecode, StationQuery_url);
        }
    }

    /// <summary>
    /// 车站大屏数据
    /// </summary>
    public static async Task<BigScreenData> GetBigScreenDataAsync(string stationName)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.GetBigScreenDataAsync(stationName, GetBigScreenData_url);
        }
        return null;
    }

    #endregion

    #region 动车组查询接口

    /// <summary>
    /// 动车组运行交路查询
    /// </summary>
    public static async Task<ObservableCollection<EmuOperation>> EmuQueryAsync(string type, string keyword)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.EmuQueryAsync(type, keyword, EmuQuery_url);
        }
        return null;
    }

    /// <summary>
    /// 动车组配属查询
    /// </summary>
    public static async Task<ObservableCollection<EmuAssignment>> EmuAssignmentQueryAsync(
        string type, string keyword, int cursor = 0, int count = 15)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.EmuAssignmentQueryAsync(type, keyword, cursor, count, EmuAssignmentQuery_url);
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region 实时数据接口

    /// <summary>
    /// 正晚点查询
    /// </summary>
    public static async Task<List<DelayInfo>> QueryTrainDelayAsync(string date, string trainNumber,
        string fromStation, string toStation)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.QueryTrainDelayAsync(date, trainNumber, fromStation, toStation, TrainDelayQuery_url);
        }
        return null;
    }

    /// <summary>
    /// 停台检票口查询
    /// </summary>
    public static async Task<PlatformInfo> QueryPlatformInfoAsync(string stationCode, string trainDate,
        string type, string stationTrainCode)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.QueryPlatformInfoAsync(stationCode, trainDate, type, stationTrainCode, PlatformInfoQuery_url);
        }
        return null;
    }

    #endregion

    #region 其他接口

    /// <summary>
    /// 下载动车组图片
    /// </summary>
    public static async Task<byte[]> DownloadEmuImageAsync(string trainModel)
    {
        if (!IsOfflineMode())
        {
            return await OnlineApiService.DownloadEmuImageAsync(trainModel, DownloadEmuImage_url);
        }
        return null;
    }

    #endregion
}