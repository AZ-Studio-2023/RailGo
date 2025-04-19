using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RailGo.Core.Models;
public class StationTrainsInfo
{
    [JsonProperty("trainCode")]
    public string TrainCode
    {
        get; set;
    }

    [JsonProperty("isTravel")]
    public int IsTravel
    {
        get; set;
    }

    [JsonProperty("startDepartTime")]
    public long StartDepartTimeUnix
    {
        get; set;
    } // 存储原始 Unix 时间戳

    // 计算属性：将 Unix 时间戳转为 DateTime
    public DateTime StartDepartTime => DateTimeOffset.FromUnixTimeSeconds(StartDepartTimeUnix).DateTime;

    [JsonProperty("startStation")]
    public string StartStation
    {
        get; set;
    }

    [JsonProperty("endStation")]
    public string EndStation
    {
        get; set;
    }

    [JsonProperty("waitingRoom")]
    public string WaitingRoom
    {
        get; set;
    }

    [JsonProperty("wicket")]
    public string Wicket
    {
        get; set;
    }

    [JsonProperty("exit")]
    public string Exit
    {
        get; set;
    }

    [JsonProperty("platform")]
    public string Platform
    {
        get; set;
    }

    [JsonProperty("status")]
    public int Status
    {
        get; set;
    }

    [JsonProperty("delay")]
    public int Delay
    {
        get; set;
    }

    [JsonProperty("travelId")]
    public string TravelId
    {
        get; set;
    }

    [JsonProperty("stop")]
    public bool Stop
    {
        get; set;
    }

    [JsonProperty("stopTitle")]
    public string StopTitle
    {
        get; set;
    }

    [JsonProperty("stopText")]
    public string StopText
    {
        get; set;
    }

    [JsonProperty("jumpUrl")]
    public string JumpUrl
    {
        get; set;
    }

    [JsonProperty("startDate")]
    public string StartDate
    {
        get; set;
    }
}

public class StationTrainsInfoStationData
{
    [JsonProperty("list")]
    public ObservableCollection<StationTrainsInfo> List
    {
        get; set;
    }

    [JsonProperty("stationCode")]
    public string StationCode
    {
        get; set;
    }

    [JsonProperty("stationName")]
    public string StationName
    {
        get; set;
    }

    [JsonProperty("type")]
    public string Type
    {
        get; set;
    }
}

public class StationTrainsInfoApiResponse
{
    [JsonProperty("code")]
    public int Code
    {
        get; set;
    }

    [JsonProperty("errmsg")]
    public string ErrMsg
    {
        get; set;
    }

    [JsonProperty("data")]
    public StationTrainsInfoStationData Data
    {
        get; set;
    }

    [JsonProperty("isSign")]
    public int IsSign
    {
        get; set;
    }
}
