using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

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
    public int Day
    {
        get; set;
    } 

    [JsonProperty("stopTime")]
    public int StopTime
    {
        get; set;
    }  
}

// 车次基本信息
public class Train
{
    [JsonProperty("number")]
    public string Number => NumberFull != null && NumberFull.Count > 0
        ? string.Join("/", NumberFull)
        : null;

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
    public ObservableCollection<TimetableItem> Timetable
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
    public ObservableCollection<TrainDiagram> Diagram
    {
        get; set;
    }
}

// 交路信息
public class TrainDiagram
{
    [JsonProperty("number")]
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

    [JsonIgnore]
    public string FromStation => From?.Count > 0 ? From[0] : null;

    [JsonIgnore]
    public string FromTime => From?.Count > 1 ? From[1] : null;

    [JsonIgnore]
    public string ToStation => To?.Count > 0 ? To[0] : null;

    [JsonIgnore]
    public string ToTime => To?.Count > 1 ? To[1] : null;
}