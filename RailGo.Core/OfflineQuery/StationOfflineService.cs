using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using RailGo.Core.Models;
using Newtonsoft.Json;
using RailGo.Core.OnlineQuery;

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

        // 查询经过该车站的所有车次详细信息
        var stationTrains = new ObservableCollection<StationTrain>();

        if (station.TrainList != null && station.TrainList.Any())
        {
            // 获取 TrainOfflineService 实例
            var trainService = ApiService.GetOfflineService<TrainOfflineService>();

            foreach (var trainNumber in station.TrainList)
            {
                try
                {
                    // 调用 TrainQueryAsync 获取车次详细信息
                    var trainJson = await trainService.TrainQueryAsync(trainNumber);
                    var train = JsonConvert.DeserializeObject<Train>(trainJson);

                    if (train?.Timetable != null && train.Timetable.Any())
                    {
                        // 找到该车次在当前车站的停靠信息
                        var stopInfo = train.Timetable.FirstOrDefault(t =>
                            t.StationTelecode == telecode);

                        if (stopInfo != null)
                        {
                            // 计算 IndexStopThere（停靠站序）
                            var timetableList = train.Timetable.ToList();
                            var indexStopThere = timetableList.IndexOf(stopInfo);

                            // 获取始发站信息（第一个车站）
                            var firstStop = timetableList.First();
                            var fromStation = new StationTrainStationInfo
                            {
                                Station = firstStop.Station,
                                StationTelecode = firstStop.StationTelecode
                            };

                            // 获取终点站信息（最后一个车站）
                            var lastStop = timetableList.Last();
                            var toStation = new StationTrainStationInfo
                            {
                                Station = lastStop.Station,
                                StationTelecode = lastStop.StationTelecode
                            };

                            var stationTrain = new StationTrain
                            {
                                Number = train.Number,
                                NumberFull = train.NumberFull,
                                NumberKind = train.NumberKind,
                                Type = train.Type,
                                ArriveTime = stopInfo.Arrive,
                                DepartTime = stopInfo.Depart,
                                StopTime = stopInfo.StopTime,
                                IndexStopThere = indexStopThere,
                                FromStation = fromStation,
                                ToStation = toStation
                            };
                            stationTrains.Add(stationTrain);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理其他车次
                    Console.WriteLine($"查询车次 {trainNumber} 时出错: {ex.Message}");
                }
            }
        }

        var response = new StationQueryResponse
        {
            Data = station,
            Trains = stationTrains
        };

        return SerializeToJson(response);
    }
}