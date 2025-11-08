using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using RailGo.Core.Models;
using Newtonsoft.Json;

namespace RailGo.Core.OfflineQuery;

public class TrainOfflineService : BaseOfflineService
{
    public TrainOfflineService(string databasePath) : base(databasePath) { }

    /// <summary>
    /// 车次预选搜索（离线版）
    /// </summary>
    public async Task<string> TrainPreselectAsync(string keyword)
    {
        string sql = @"
            SELECT DISTINCT numberFull 
            FROM trains 
            WHERE number LIKE @keyword 
               OR numberFull LIKE @keyword
            LIMIT 20";

        var parameters = new[]
        {
            new SqliteParameter("@keyword", $"%{keyword}%")
        };

        var results = await QueryAsync(sql, reader =>
            new TrainPreselectResult { FullNumber = reader["numberFull"].ToString() },
            parameters);

        var collection = new ObservableCollection<TrainPreselectResult>(results);
        return SerializeToJson(collection);
    }

    /// <summary>
    /// 车次详情查询（离线版）
    /// </summary>
    public async Task<string> TrainQueryAsync(string trainNumber)
    {
        string sql = @"
            SELECT * FROM trains 
            WHERE number = @trainNumber 
               OR numberFull LIKE @trainNumberPattern";

        var parameters = new[]
        {
            new SqliteParameter("@trainNumber", trainNumber),
            new SqliteParameter("@trainNumberPattern", $"%{trainNumber}%")
        };

        var trains = await QueryAsync(sql, reader => new Train
        {
            NumberFull = JsonConvert.DeserializeObject<List<string>>(reader["numberFull"].ToString() ?? "[]"),
            NumberKind = reader["numberKind"].ToString(),
            Type = reader["type"].ToString(),
            BureauName = reader["bureauName"].ToString(),
            Runner = reader["runner"].ToString(),
            CarOwner = reader["carOwner"].ToString(),
            Car = reader["car"].ToString(),
            Rundays = JsonConvert.DeserializeObject<List<string>>(reader["randays"].ToString() ?? "[]"),
            Timetable = JsonConvert.DeserializeObject<ObservableCollection<TimetableItem>>(reader["timetable"].ToString() ?? "[]"),
            Diagram = JsonConvert.DeserializeObject<ObservableCollection<TrainDiagram>>(reader["diagram"].ToString() ?? "[]")
        }, parameters);

        var train = trains.FirstOrDefault();
        return SerializeToJson(train);
    }

    /// <summary>
    /// 站站查询（离线版）
    /// </summary>
    public async Task<string> StationToStationQueryAsync(string from, string to, string date)
    {
        // 简化实现：查询包含这两个车站的车次
        string sql = @"
            SELECT t.* 
            FROM trains t
            WHERE EXISTS (
                SELECT 1 FROM json_each(t.timetable) AS stop1
                WHERE json_extract(stop1.value, '$.station') = @fromStation
            )
            AND EXISTS (
                SELECT 1 FROM json_each(t.timetable) AS stop2
                WHERE json_extract(stop2.value, '$.station') = @toStation
            )";

        var parameters = new[]
        {
            new SqliteParameter("@fromStation", from),
            new SqliteParameter("@toStation", to)
        };

        var trains = await QueryAsync(sql, reader => new Train
        {
            NumberFull = JsonConvert.DeserializeObject<List<string>>(reader["numberFull"].ToString() ?? "[]"),
            NumberKind = reader["numberKind"].ToString(),
            Type = reader["type"].ToString(),
            BureauName = reader["bureauName"].ToString(),
            Timetable = JsonConvert.DeserializeObject<ObservableCollection<TimetableItem>>(reader["timetable"].ToString() ?? "[]")
        }, parameters);

        return SerializeToJson(trains);
    }
}