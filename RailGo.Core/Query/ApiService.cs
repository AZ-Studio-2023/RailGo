using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;
using RailGo.Core.Query.Offline;
using RailGo.Core.Query.Online;
using RailGo.Core.Helpers;
using Newtonsoft.Json;
using System.Diagnostics;

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
        ObservableCollection<string> stringArray;

        if (IsOfflineMode())
        {
            var offlineService = GetOfflineService<TrainOfflineService>();
            var json = await offlineService.TrainPreselectAsync(keyword);
            stringArray = JsonConvert.DeserializeObject<ObservableCollection<string>>(json);
        }
        else
        {
            var url = $"{TrainPreselect_url}?keyword={System.Net.WebUtility.UrlEncode(keyword)}";
            stringArray = await HttpService.GetAsync<ObservableCollection<string>>(url);
        }

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

        var url = $"{TrainQuery_url}?train={System.Net.WebUtility.UrlEncode(trainNumber)}";
        return await HttpService.GetAsync<Train>(url);
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

        var url = $"{StationToStationQuery_url}?from={from}&to={to}&date={date}";
        return await HttpService.GetAsync<List<Train>>(url);
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

        var url = $"{StationPreselect_url}?keyword={System.Net.WebUtility.UrlEncode(keyword)}";
        return await HttpService.GetAsync<ObservableCollection<StationPreselectResult>>(url);
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

        var url = $"{StationQuery_url}?telecode={telecode}";
        return await HttpService.GetAsync<StationQueryResponse>(url);
    }

    /// <summary>
    /// 车站大屏数据
    /// </summary>
    public static async Task<BigScreenData> GetBigScreenDataAsync(string stationName)
    {
        if (!IsOfflineMode())
        {
            var nameWithoutSuffix = stationName.Replace("站", "");
            var url = $"{GetBigScreenData_url}/station/{System.Net.WebUtility.UrlEncode(nameWithoutSuffix)}";
            return await HttpService.GetAsync<BigScreenData>(url);
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
        var url = $"{EmuQuery_url}/{type}/{System.Net.WebUtility.UrlEncode(keyword)}";
        return await HttpService.GetAsync<ObservableCollection<EmuOperation>>(url);
    }

    /// <summary>
    /// 动车组配属查询
    /// </summary>
    public static async Task<ObservableCollection<EmuAssignment>> EmuAssignmentQueryAsync(
        string type, string keyword, int cursor = 0, int count = 15)
    {
        if (!IsOfflineMode())
        {
            var url = EmuAssignmentQuery_url;
            var formData = new List<KeyValuePair<string, string>>
            {
                new("type", type),
                new("keyword", keyword),
                new("trainCategory", "0"),
                new("cursor", cursor.ToString()),
                new("count", count.ToString())
            };

            var onlineResponse = await HttpService.PostFormAsync<EmuAssignmentResponse>(url, formData);
            return onlineResponse?.Data?.Data;
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
        var url = TrainDelayQuery_url;
        var data = new
        {
            date,
            trainNumber,
            fromStationName = fromStation,
            toStationName = toStation
        };

        var delayResponse = await HttpService.PostAsync<DelayResponse>(url, data);
        return delayResponse?.Data;
    }

    /// <summary>
    /// 停台检票口查询
    /// </summary>
    public static async Task<PlatformInfo> QueryPlatformInfoAsync(string stationCode, string trainDate,
        string type, string stationTrainCode)
    {
        var url = PlatformInfoQuery_url;
        var data = new
        {
            stationCode,
            trainDate,
            type,
            stationTrainCode
        };

        return await HttpService.PostAsync<PlatformInfo>(url, data);
    }

    #endregion

    #region 其他接口

    /// <summary>
    /// 下载动车组图片
    /// </summary>
    public static async Task<byte[]> DownloadEmuImageAsync(string trainModel)
    {
        var url = $"{DownloadEmuImage_url}/{System.Net.WebUtility.UrlEncode(trainModel)}.png";
        return await HttpService.DownloadFileAsync(url);
    }

    #endregion
}