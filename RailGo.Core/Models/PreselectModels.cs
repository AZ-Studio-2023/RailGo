using System.Collections.Generic;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

// 车次预选搜索结果
public class TrainPreselectResult
{
    [JsonProperty("numberFull")]
    public List<string> NumberFull
    {
        get; set;
    }

    [JsonProperty("fromStation")]
    public Station FromStation
    {
        get; set;
    }

    [JsonProperty("toStation")]
    public Station ToStation
    {
        get; set;
    }
}

// 车站预选搜索结果
public class StationPreselectResult
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
}