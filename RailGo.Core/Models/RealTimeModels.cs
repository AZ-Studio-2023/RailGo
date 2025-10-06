using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    [JsonProperty("station")]
    public string Station
    {
        get; set;
    }

    [JsonProperty("data")]
    [JsonConverter(typeof(StationScreenItemListConverter))]
    public ObservableCollection<StationScreenItem> Data
    {
        get; set;
    }
}

// 自定义转换器
public class StationScreenItemListConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ObservableCollection<StationScreenItem>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var collection = new ObservableCollection<StationScreenItem>();

        if (reader.TokenType == JsonToken.StartArray)
        {
            var array = JArray.Load(reader);

            foreach (var item in array)
            {
                if (item is JArray jArray && jArray.Count >= 6)
                {
                    var screenItem = new StationScreenItem
                    {
                        TrainNumber = jArray[0]?.ToString(),
                        FromStation = jArray[1]?.ToString(),
                        ToStation = jArray[2]?.ToString(),
                        ScheduleTime = jArray[3]?.ToString(),
                        WaitingArea = jArray[4]?.ToString(),
                        Status = jArray[5]?.ToString()
                    };
                    collection.Add(screenItem);
                }
            }
        }

        return collection;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

// 车站大屏项
public class StationScreenItem
{
    [JsonProperty("0")]
    public string TrainNumber
    {
        get; set;
    }          // 车次

    [JsonProperty("1")]
    public string FromStation
    {
        get; set;
    }          // 始发站

    [JsonProperty("2")]
    public string ToStation
    {
        get; set;
    }            // 终到站

    [JsonProperty("3")]
    public string ScheduleTime
    {
        get; set;
    }         // 计划时间

    [JsonProperty("4")]
    public string WaitingArea
    {
        get; set;
    }          // 候车区域

    [JsonProperty("5")]
    public string Status
    {
        get; set;
    }               // 状态

    // 计算属性 - 格式化时间（只显示时分）
    [JsonIgnore]
    public string DisplayTime
    {
        get
        {
            if (DateTime.TryParse(ScheduleTime, out var dateTime))
                return dateTime.ToString("HH:mm");
            return ScheduleTime;
        }
    }

    // 计算属性 - 分离候车室
    [JsonIgnore]
    public string DisplayWaitingRoom
    {
        get
        {
            if (string.IsNullOrEmpty(WaitingArea))
                return string.Empty;

            var parts = WaitingArea.Split('/');
            return parts.Length > 0 ? parts[0] : WaitingArea;
        }
    }

    // 计算属性 - 分离检票口
    [JsonIgnore]
    public string DisplayTicketGate
    {
        get
        {
            if (string.IsNullOrEmpty(WaitingArea))
                return string.Empty;

            var parts = WaitingArea.Split('/');
            return parts.Length > 1 ? parts[1] : string.Empty;
        }
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