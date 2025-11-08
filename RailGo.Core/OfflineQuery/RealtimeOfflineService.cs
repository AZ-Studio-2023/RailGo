using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using RailGo.Core.Models;

namespace RailGo.Core.OfflineQuery;

public class RealtimeOfflineService : BaseOfflineService
{
    public RealtimeOfflineService(string databasePath) : base(databasePath) { }

    /// <summary>
    /// 获取车站大屏数据（离线版）- 基于时刻表生成模拟数据
    /// </summary>
    public async Task<string> GetBigScreenDataAsync(string stationName)
    {
        // 移除"站"后缀
        var nameWithoutSuffix = stationName.Replace("站", "");

        string sql = @"
            SELECT t.numberFull as trainNumber,
                   json_extract(first_stop.value, '$.station') as fromStation,
                   json_extract(last_stop.value, '$.station') as toStation,
                   json_extract(current_stop.value, '$.depart') as scheduleTime
            FROM trains t,
                 (SELECT value FROM json_each(t.timetable) ORDER BY json_extract(value, '$.day'), json_extract(value, '$.depart') LIMIT 1) as first_stop,
                 (SELECT value FROM json_each(t.timetable) ORDER BY json_extract(value, '$.day') DESC, json_extract(value, '$.depart') DESC LIMIT 1) as last_stop,
                 json_each(t.timetable) as current_stop
            WHERE json_extract(current_stop.value, '$.station') LIKE @stationName
            LIMIT 50";

        var parameters = new[] { new SqliteParameter("@stationName", $"%{nameWithoutSuffix}%") };

        var screenItems = await QueryAsync(sql, reader => new StationScreenItem
        {
            TrainNumber = reader["trainNumber"].ToString(),
            FromStation = reader["fromStation"].ToString(),
            ToStation = reader["toStation"].ToString(),
            ScheduleTime = reader["scheduleTime"].ToString(),
            WaitingArea = "候车室/检票口", // 模拟数据
            Status = "正点" // 模拟数据
        }, parameters);

        var bigScreenData = new BigScreenData
        {
            Station = stationName,
            Data = new ObservableCollection<StationScreenItem>(screenItems)
        };

        return SerializeToJson(bigScreenData);
    }
}