using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class TrainNumberTripDetailsModel
{
    [JsonProperty("code")]
    public int Code
    {
        get; set;
    }

    [JsonProperty("data")]
    public TrainData Data
    {
        get; set;
    }

    [JsonProperty("message")]
    public object Message
    {
        get; set;
    }
}

public class TrainData
{
    [JsonProperty("pageIndex")]
    public int PageIndex
    {
        get; set;
    }

    [JsonProperty("pageSize")]
    public int PageSize
    {
        get; set;
    }

    [JsonProperty("totalPages")]
    public int TotalPages
    {
        get; set;
    }

    [JsonProperty("totalCount")]
    public int TotalCount
    {
        get; set;
    }

    [JsonProperty("data")]
    public List<TrainDetail> DataList
    {
        get; set;
    }
}

public class TrainDetail
{
    [JsonProperty("trainIndex")]
    public int TrainIndex
    {
        get; set;
    }

    [JsonProperty("trainNumber")]
    public string TrainNumber
    {
        get; set;
    }

    [JsonProperty("beginStationName")]
    public string BeginStationName
    {
        get; set;
    }

    [JsonProperty("departureTime")]
    public string DepartureTime
    {
        get; set;
    }

    [JsonProperty("endStationName")]
    public string EndStationName
    {
        get; set;
    }

    [JsonProperty("arrivalTime")]
    public string ArrivalTime
    {
        get; set;
    }

    [JsonProperty("dayCount")]
    public int DayCount
    {
        get; set;
    }

    [JsonProperty("durationMinutes")]
    public int DurationMinutes
    {
        get; set;
    }

    [JsonProperty("distance")]
    public int Distance
    {
        get; set;
    }

    [JsonProperty("trainType")]
    public string TrainType
    {
        get; set;
    }

    [JsonProperty("crType")]
    public int CrType
    {
        get; set;
    }

    [JsonProperty("outOfDateFlag")]
    public int OutOfDateFlag
    {
        get; set;
    }
}

