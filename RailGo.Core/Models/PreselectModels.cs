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

    [JsonIgnore]
    public string Prefix
    {
        get
        {
            if (string.IsNullOrEmpty(FullNumber))
                return "";

            // 提取前缀字母
            if (FullNumber.Length > 0 && char.IsLetter(FullNumber[0]))
            {
                return FullNumber[0].ToString();
            }
            return "";
        }
    }

    [JsonIgnore]
    public string NumberPart
    {
        get
        {
            if (string.IsNullOrEmpty(FullNumber))
                return "";

            // 处理 G1202/G1203 这种情况
            if (FullNumber.Contains('/'))
            {
                var parts = FullNumber.Split('/');
                if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0)
                {
                    var firstChar1 = parts[0][0];
                    var firstChar2 = parts[1][0];

                    if (char.IsLetter(firstChar1) && char.IsLetter(firstChar2) && firstChar1 == firstChar2)
                    {
                        return $"{parts[0].Substring(1)}/{parts[1].Substring(1)}";
                    }
                }
            }

            // 普通情况：去掉前缀
            if (FullNumber.Length > 0 && char.IsLetter(FullNumber[0]))
            {
                return FullNumber.Substring(1);
            }
            return FullNumber;
        }
    }

    [JsonIgnore]
    public string PrefixColor
    {
        get
        {
            if (string.IsNullOrEmpty(Prefix))
                return "#000000";

            return Prefix.ToUpper() switch
            {
                "G" => "#DC2626", // 红色-高铁
                "D" => "#2563EB", // 蓝色-动车
                "C" => "#16A34A", // 绿色-城际
                "Z" => "#7C3AED", // 紫色-直达
                "T" => "#EA580C", // 橙色-特快
                "K" => "#854D0E", // 棕色-快速
                "S" => "#0891B2", // 青色-市域
                "L" => "#CA8A04", // 黄色-临客
                _ => "#000000"    // 黑色-其他
            };
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