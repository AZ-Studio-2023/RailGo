using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace RailGo.Models;
public class TrainNumberEmuInfo
{
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
    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime date
    {
        get; set;
    }
}
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
public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd HH:mm";
    }
}
