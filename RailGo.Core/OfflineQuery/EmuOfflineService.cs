using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using RailGo.Core.Models;

namespace RailGo.Core.OfflineQuery;

public class EmuOfflineService : BaseOfflineService
{
    public EmuOfflineService(string databasePath) : base(databasePath) { }

    /// <summary>
    /// 动车组配属查询（离线版）
    /// </summary>
    public async Task<string> EmuAssignmentQueryAsync(string type, string keyword, int cursor = 0, int count = 15)
    {
        // 注意：离线数据库可能没有完整的动车组配属数据
        // 这里需要根据您的实际数据结构调整查询逻辑

        string sql = @"
            SELECT DISTINCT car as trainModel, carOwner as bureau
            FROM trains 
            WHERE car LIKE @keyword 
               AND type IN ('G', 'D', 'C')
            LIMIT @count OFFSET @cursor";

        var parameters = new[]
        {
            new SqliteParameter("@keyword", $"%{keyword}%"),
            new SqliteParameter("@count", count),
            new SqliteParameter("@cursor", cursor)
        };

        var results = await QueryAsync(sql, reader => new EmuAssignment
        {
            TrainModel = reader["trainModel"].ToString(),
            Bureau = reader["bureau"].ToString(),
            Department = reader["bureau"].ToString() // 简化处理
        }, parameters);

        var response = new EmuAssignmentResponse
        {
            Code = 200,
            Message = "Success",
            Data = new EmuAssignmentPagedData
            {
                Data = new ObservableCollection<EmuAssignment>(results),
                Cursor = cursor + results.Count,
                Count = results.Count,
                HasMore = results.Count == count,
                TotalCount = results.Count
            }
        };

        return SerializeToJson(response);
    }
}