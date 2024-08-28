using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace RailGo.Models;
public class TrainTripsInfo
{
    [JsonProperty("emu_no")]
    public string 车组号
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string 担当车次
    {
        get; set;
    }
    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime 日期
    {
        get; set;
    }
}
public class TrainNumberEmuInfo
{
    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime 日期
    {
        get; set;
    }
    [JsonProperty("emu_no")]
    public string 动车组编号
    {
        get; set;
    }
    [JsonProperty("train_no")]
    public string 车次
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
