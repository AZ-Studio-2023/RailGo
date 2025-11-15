using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;
using RailGo.Core.OfflineQuery;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RailGo.Core.OnlineQuery;

public class ApiService
{
    private const string BaseUrl = "https://data.railgo.zenglingkun.cn/api";
    private const string DelayBaseUrl = "https://delay.data.railgo.zenglingkun.cn/api";
    private const string ScreenBaseUrl = "https://screen.data.railgo.zenglingkun.cn";
    private const string EmuBaseUrl = "https://emu.data.railgo.zenglingkun.cn";

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
            var url = $"{BaseUrl}/train/preselect?keyword={System.Net.WebUtility.UrlEncode(keyword)}";
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

        var url = $"{BaseUrl}/train/query?train={System.Net.WebUtility.UrlEncode(trainNumber)}";
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

        var url = $"{BaseUrl}/train/sts_query?from={from}&to={to}&date={date}";
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

        var url = $"{BaseUrl}/station/preselect?keyword={System.Net.WebUtility.UrlEncode(keyword)}";
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

        var url = $"{BaseUrl}/station/query?telecode={telecode}";
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
            var url = $"{ScreenBaseUrl}/station/{System.Net.WebUtility.UrlEncode(nameWithoutSuffix)}";
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
        var url = $"{EmuBaseUrl}/{type}/{System.Net.WebUtility.UrlEncode(keyword)}";
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
            var url = $"{DelayBaseUrl}/trainAssignment/queryEmu";
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
        var url = $"{DelayBaseUrl}/trainDetails/queryTrainDelayDetails";
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
        var url = "https://mobile.12306.cn/wxxcx/wechat/bigScreen/getExit";
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
        var url = $"https://tp.railgo.zenglingkun.cn/api/{System.Net.WebUtility.UrlEncode(trainModel)}.png";
        return await HttpService.DownloadFileAsync(url);
    }

    #endregion
}