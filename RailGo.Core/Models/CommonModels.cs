using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

// 车站基本信息
public class Station
{
    [JsonProperty("name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("telecode")]
    public string Telecode
    {
        get; set;
    }

    [JsonProperty("pinyin")]
    public string Pinyin
    {
        get; set;
    }

    [JsonProperty("pinyinTriple")]
    public string PinyinTriple
    {
        get; set;
    }

    [JsonProperty("type")]
    public List<string> Type
    {
        get; set;
    }

    [JsonProperty("bureau")]
    public string Bureau
    {
        get; set;
    }

    [JsonProperty("belong")]
    public string Belong
    {
        get; set;
    }

    [JsonProperty("trainList")]
    public List<string> TrainList
    {
        get; set;
    }
}

// 分页响应
public class PaginatedResponse<T>
{
    [JsonProperty("data")]
    public ObservableCollection<T> Data
    {
        get; set;
    }

    [JsonProperty("hasMore")]
    public bool HasMore
    {
        get; set;
    }

    [JsonProperty("totalCount")]
    public int TotalCount
    {
        get; set;
    }
}