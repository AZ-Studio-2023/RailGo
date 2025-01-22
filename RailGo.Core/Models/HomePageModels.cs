using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

public class TrainTripsInfo
{
    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime date
    {
        get; set;
    }
    [JsonProperty("emu_no")]
    public string emu_no
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string train_no
    {
        get; set;
    }
}
public class StationSearch
{
    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public string StationName
    {
        get; set;
    }
    [JsonProperty("emu_no")]
    public string LuJu
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string Province
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string Telegraph
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string PinYin
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string TMIS
    {
        get; set;
    }
}
public class moefactory_train_number_FirstOutTable_TrainInfo
{
    public int TrainIndex
    {
        get; set;
    }
    public string TrainNumber
    {
        get; set;
    }
    public string BeginStationName
    {
        get; set;
    }
    public string DepartureTime
    {
        get; set;
    }
    public string EndStationName
    {
        get; set;
    }
    public string ArrivalTime
    {
        get; set;
    }
    public int DayCount
    {
        get; set;
    }
    public int DurationMinutes
    {
        get; set;
    }
    public int Distance
    {
        get; set;
    }
    public string TrainType
    {
        get; set;
    }
    public int CrType
    {
        get; set;
    }
    public int OutOfDateFlag
    {
        get; set;
    }
}

public class moefactory_train_number_FirstOutTable_Data
{
    public int PageIndex
    {
        get; set;
    }
    public int PageSize
    {
        get; set;
    }
    public int TotalPages
    {
        get; set;
    }
    public int TotalCount
    {
        get; set;
    }
    public List<moefactory_train_number_FirstOutTable_TrainInfo> DataList
    {
        get; set;
    }
}

public class moefactory_train_number_FirstOutTable_Root
{
    public int Code
    {
        get; set;
    }
    public moefactory_train_number_FirstOutTable_Data Data
    {
        get; set;
    }
    public string Message
    {
        get; set;
    }
}

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd HH:mm";
    }
}
