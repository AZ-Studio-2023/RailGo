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

// 时刻表项
public class TimetableItem
{
    [JsonProperty("station")]
    public string Station
    {
        get; set;
    }

    [JsonProperty("stationTelecode")]
    public string StationTelecode
    {
        get; set;
    }

    [JsonProperty("trainCode")]
    public string TrainCode
    {
        get; set;
    }

    [JsonProperty("arrive")]
    public string Arrive
    {
        get; set;
    }

    [JsonProperty("depart")]
    public string Depart
    {
        get; set;
    }

    [JsonProperty("distance")]
    public string Distance
    {
        get; set;
    }

    [JsonProperty("speed")]
    public double? Speed
    {
        get; set;
    }

    [JsonProperty("day")]
    public string Day
    {
        get; set;
    }

    [JsonProperty("stopTime")]
    public string StopTime
    {
        get; set;
    }

    [JsonProperty("platform")]
    public string Platform
    {
        get; set;
    }

    [JsonProperty("wicket")]
    public string Wicket
    {
        get; set;
    }
}

// 车次基本信息
public class Train
{
    [JsonProperty("code")]
    public string Code
    {
        get; set;
    }

    [JsonProperty("number")]
    public string Number
    {
        get; set;
    }

    [JsonProperty("numberFull")]
    public List<string> NumberFull
    {
        get; set;
    }

    [JsonProperty("numberKind")]
    public string NumberKind
    {
        get; set;
    }

    [JsonProperty("type")]
    public string Type
    {
        get; set;
    }

    [JsonProperty("timetable")]
    public List<TimetableItem> Timetable
    {
        get; set;
    }

    [JsonProperty("bureauName")]
    public string BureauName
    {
        get; set;
    }

    [JsonProperty("runner")]
    public string Runner
    {
        get; set;
    }

    [JsonProperty("carOwner")]
    public string CarOwner
    {
        get; set;
    }

    [JsonProperty("car")]
    public string Car
    {
        get; set;
    }

    [JsonProperty("rundays")]
    public List<string> Rundays
    {
        get; set;
    }

    [JsonProperty("diagram")]
    public List<TrainDiagram> Diagram
    {
        get; set;
    }
}

// 交路信息
public class TrainDiagram
{
    [JsonProperty("train_num")]
    public string TrainNum
    {
        get; set;
    }

    [JsonProperty("from")]
    public List<string> From
    {
        get; set;
    }

    [JsonProperty("to")]
    public List<string> To
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