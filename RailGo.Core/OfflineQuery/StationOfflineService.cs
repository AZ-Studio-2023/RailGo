using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using RailGo.Core.Models;
using Newtonsoft.Json;

namespace RailGo.Core.OfflineQuery;

public class StationOfflineService : BaseOfflineService
{
    public StationOfflineService(string databasePath) : base(databasePath) { }

    /// <summary>
    /// 车站预选搜索（离线版）
    /// </summary>
    public async Task<string> StationPreselectAsync(string keyword)
    {
        string sql = @"
            SELECT name, telecode, pinyin, pinyinTriple, type, bureau, belong
            FROM stations 
            WHERE name LIKE @keyword 
               OR pinyin LIKE @keyword 
               OR pinyinTriple LIKE @keyword
            LIMIT 20";

        var parameters = new[]
        {
            new SqliteParameter("@keyword", $"%{keyword}%")
        };

        var results = await QueryAsync(sql, reader => new StationPreselectResult
        {
            Name = reader["name"].ToString(),
            TeleCode = reader["telecode"].ToString(),
            Pinyin = reader["pinyin"].ToString(),
            PinyinTriple = reader["pinyinTriple"].ToString(),
            Type = JsonConvert.DeserializeObject<List<string>>(reader["type"].ToString() ?? "[]"),
            Bureau = reader["bureau"].ToString(),
            Belong = reader["belong"].ToString()
        }, parameters);

        var collection = new ObservableCollection<StationPreselectResult>(results);
        return SerializeToJson(collection);
    }

    /// <summary>
    /// 车站详情查询（离线版）
    /// </summary>
    public async Task<string> StationQueryAsync(string telecode)
    {
        // 查询车站基本信息
        string stationSql = "SELECT * FROM stations WHERE telecode = @telecode";
        var stationParameters = new[] { new SqliteParameter("@telecode", telecode) };

        var stations = await QueryAsync(stationSql, reader => new Station
        {
            Name = reader["name"].ToString(),
            Telecode = reader["telecode"].ToString(),
            Pinyin = reader["pinyin"].ToString(),
            PinyinTriple = reader["pinyinTriple"].ToString(),
            Type = JsonConvert.DeserializeObject<List<string>>(reader["type"].ToString() ?? "[]"),
            Bureau = reader["bureau"].ToString(),
            Belong = reader["belong"].ToString(),
            TrainList = JsonConvert.DeserializeObject<List<string>>(reader["trainList"].ToString() ?? "[]")
        }, stationParameters);

        var station = stations.FirstOrDefault();
        if (station == null) return SerializeToJson(new StationQueryResponse { Data = null, Trains = new ObservableCollection<StationTrain>() });

        // 查询经过该车站的车次
        string trainsSql = @"
            SELECT t.number, t.numberFull, t.numberKind, t.type,
                   json_extract(stop.value, '$.arrive') as arrive,
                   json_extract(stop.value, '$.depart') as depart,
                   json_extract(stop.value, '$.stopTime') as stopTime
            FROM trains t, json_each(t.timetable) as stop
            WHERE json_extract(stop.value, '$.stationTelecode') = @telecode";

        var stationTrains = await QueryAsync(trainsSql, reader => new StationTrain
        {
            Number = reader["number"].ToString(),
            NumberFull = JsonConvert.DeserializeObject<List<string>>(reader["numberFull"].ToString() ?? "[]"),
            NumberKind = reader["numberKind"].ToString(),
            Type = reader["type"].ToString(),
            ArriveTime = reader["arrive"].ToString(),
            DepartTime = reader["depart"].ToString(),
            StopTime = reader["stopTime"] == DBNull.Value ? 0 : Convert.ToInt32(reader["stopTime"])
        }, stationParameters);

        var response = new StationQueryResponse
        {
            Data = station,
            Trains = new ObservableCollection<StationTrain>(stationTrains)
        };

        return SerializeToJson(response);
    }
}