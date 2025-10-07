using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using RailGo.Core.Models;

namespace RailGo.Core.OnlineQuery;

public class ApiService
{
    private const string BaseUrl = "https://data.railgo.zenglingkun.cn/api";
    private const string DelayBaseUrl = "https://delay.data.railgo.zenglingkun.cn/api";
    private const string ScreenBaseUrl = "https://screen.data.railgo.zenglingkun.cn";
    private const string EmuBaseUrl = "https://emu.data.railgo.zenglingkun.cn";

    #region 车次查询接口

    /// <summary>
    /// 车次预选搜索
    /// </summary>
    // 修改 ApiService 方法
    public static async Task<ObservableCollection<TrainPreselectResult>> TrainPreselectAsync(string keyword)
    {
        var url = $"{BaseUrl}/train/preselect?keyword={System.Net.WebUtility.UrlEncode(keyword)}";

        // 先获取字符串数组
        var stringArray = await HttpService.GetAsync<ObservableCollection<string>>(url);

        // 转换为 TrainPreselectResult
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
        var url = $"{BaseUrl}/train/query?train={System.Net.WebUtility.UrlEncode(trainNumber)}";
        return await HttpService.GetAsync<Train>(url);
    }

    /// <summary>
    /// 站站查询
    /// </summary>
    public static async Task<List<Train>> StationToStationQueryAsync(string from, string to, string date)
    {
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
        var url = $"{BaseUrl}/station/preselect?keyword={System.Net.WebUtility.UrlEncode(keyword)}";
        return await HttpService.GetAsync<ObservableCollection<StationPreselectResult>>(url);
    }

    /// <summary>
    /// 车站详情查询
    /// </summary>
    public static async Task<StationQueryResponse> StationQueryAsync(string telecode)
    {
        var url = $"{BaseUrl}/station/query?telecode={telecode}";
        return await HttpService.GetAsync<StationQueryResponse>(url);
    }

    /// <summary>
    /// 车站大屏数据
    /// </summary>
    public static async Task<BigScreenData> GetBigScreenDataAsync(string stationName)
    {
        var nameWithoutSuffix = stationName.Replace("站", "");
        var url = $"{ScreenBaseUrl}/station/{System.Net.WebUtility.UrlEncode(nameWithoutSuffix)}";
        return await HttpService.GetAsync<BigScreenData>(url);
    }

    #endregion

    #region 动车组查询接口

    /// <summary>
    /// 动车组运行交路查询
    /// </summary>
    public static async Task<List<EmuOperation>> EmuQueryAsync(string type, string keyword)
    {
        var url = $"{EmuBaseUrl}/{type}/{System.Net.WebUtility.UrlEncode(keyword)}";
        return await HttpService.GetAsync<List<EmuOperation>>(url);
    }

    /// <summary>
    /// 动车组配属查询
    /// </summary>
    public static async Task<PaginatedResponse<EmuAssignment>> EmuAssignmentQueryAsync(
        string type, string keyword, int cursor = 0, int count = 15)
    {
        var url = $"{DelayBaseUrl}/trainAssignment/queryEmu";
        var data = new
        {
            type,
            keyword,
            trainCategory = 1,
            cursor,
            count
        };

        var response = await HttpService.PostAsync<EmuAssignmentResponse>(url, data);
        return response?.Data;
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

        var response = await HttpService.PostAsync<DelayResponse>(url, data);
        return response?.Data;
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