using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// 用于：https://rail.moefactory.com/api/trainNumber/query
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

// 用于：https://rail.moefactory.com/api/trainDetails/query
public class TrainDetailsInfoModel
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

public class TrainDetailsData
{
    [JsonProperty("trainNumber")]
    public string TrainNumber
    {
        get; set;
    }

    [JsonProperty("trainType")]
    public string TrainType
    {
        get; set;
    }

    [JsonProperty("companyName")]
    public string CompanyName
    {
        get; set;
    }

    [JsonProperty("foodCoachName")]
    public string FoodCoachName
    {
        get; set;
    }

    [JsonProperty("viaStations")]
    public List<ViaStation> ViaStations
    {
        get; set;
    }

    [JsonProperty("crType")]
    public int CrType
    {
        get; set;
    }

    [JsonProperty("routing")]
    public Routing Routing
    {
        get; set;
    }
}

public class ViaStation
{
    [JsonProperty("stationName")]
    public string StationName
    {
        get; set;
    }

    [JsonProperty("stationTelegramCode")]
    public string StationTelegramCode
    {
        get; set;
    }

    [JsonProperty("trainNumber")]
    public string TrainNumber
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
    public int StopMinutes
    {
        get; set;
    }

    [JsonProperty("distance")]
    public int Distance
    {
        get; set;
    }

    [JsonProperty("checkoutName")]
    public string CheckoutName
    {
        get; set;
    }

    [JsonProperty("speed")]
    public int? Speed
    {
        get; set;
    }

    [JsonProperty("dayIndex")]
    public int DayIndex
    {
        get; set;
    }

    [JsonProperty("companyName")]
    public string CompanyName
    {
        get; set;
    }

    [JsonProperty("province")]
    public string Province
    {
        get; set;
    }

    [JsonProperty("district")]
    public string District
    {
        get; set;
    }

    [JsonProperty("outOfDateFlag")]
    public int OutOfDateFlag
    {
        get; set;
    }

    [JsonProperty("isTurn")]
    public bool IsTurn
    {
        get; set;
    }
}

public class Routing
{
    [JsonProperty("routingItems")]
    public List<RoutingItem> RoutingItems
    {
        get; set;
    }

    [JsonProperty("trainModel")]
    public string TrainModel
    {
        get; set;
    }
}

public class RoutingItem
{
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
}


