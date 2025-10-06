using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

// 正晚点信息
public class DelayInfo
{
    [JsonProperty("stationName")]
    public string StationName
    {
        get; set;
    }

    [JsonProperty("delayMinutes")]
    public int? DelayMinutes
    {
        get; set;
    }

    [JsonProperty("status")]
    public int? Status
    {
        get; set;
    }

    [JsonProperty("arrivalTime")]
    public string ArrivalTime
    {
        get; set;
    }

    [JsonProperty("departureTime")]
    public string DepartureTime
    {
        get; set;
    }

    [JsonProperty("stopMinutes")]
    public int? StopMinutes
    {
        get; set;
    }

    [JsonProperty("arrivalDate")]
    public string ArrivalDate
    {
        get; set;
    }
}

// 停台检票口信息
public class PlatformInfo
{
    [JsonProperty("status")]
    public bool Status
    {
        get; set;
    }

    [JsonProperty("data")]
    public PlatformData Data
    {
        get; set;
    }

    [JsonProperty("errorMsg")]
    public string ErrorMsg
    {
        get; set;
    }
}

public class PlatformData
{
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

// 大屏数据
public class BigScreenData
{
    [JsonProperty("data")]
    public List<string[]> Data
    {
        get; set;
    }
}

// 车站查询响应
public class StationQueryResponse
{
    [JsonProperty("data")]
    public Station Data
    {
        get; set;
    }

    [JsonProperty("trains")]
    public ObservableCollection<StationTrain> Trains
    {
        get; set;
    }
}

public class StationTrain
{
    [JsonProperty("arrive")]
    public string ArriveTime
    {
        get; set;
    }

    [JsonProperty("depart")]
    public string DepartTime
    {
        get; set;
    }

    [JsonProperty("fromStation")]
    public StationTrainStationInfo FromStation
    {
        get; set;
    }

    [JsonProperty("indexStopThere")]
    public int IndexStopThere
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

    [JsonProperty("stopTime")]
    public int StopTime
    {
        get; set;
    }

    [JsonProperty("toStation")]
    public StationTrainStationInfo ToStation
    {
        get; set;
    }

    [JsonProperty("type")]
    public string Type
    {
        get; set;
    }
}

public class StationTrainStationInfo
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
}