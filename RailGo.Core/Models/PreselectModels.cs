using System.Collections.Generic;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

// 车次预选搜索结果
public class TrainPreselectResult
{
    [JsonProperty("fullNumber")]
    public string FullNumber
    {
        get; set;
    }
    [JsonIgnore]
    public string Number
    {
        get
        {
            if (string.IsNullOrEmpty(FullNumber))
                return string.Empty;

            var parts = FullNumber.Split('/');
            return parts.Length > 0 ? parts[0] : FullNumber;
        }
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
    public string TeleCode
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
}