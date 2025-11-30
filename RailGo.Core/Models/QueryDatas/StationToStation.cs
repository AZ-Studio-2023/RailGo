using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace RailGo.Core.Models.QueryDatas
{
    public class TrainRunInfo
    {
        [JsonProperty("bureauName")]
        public string BureauName
        {
            get; set;
        }

        [JsonProperty("car")]
        public string Car
        {
            get; set;
        }

        [JsonProperty("carOwner")]
        public string CarOwner
        {
            get; set;
        }

        [JsonProperty("dayDiff")]
        public int DayDiff
        {
            get; set;
        }

        // 数组内容为数字？字符串？你给的是空数组，所以我只能用 object
        [JsonProperty("diagram")]
        public ObservableCollection<object> Diagram
        {
            get; set;
        }

        [JsonProperty("fromDepart")]
        public string FromDepart
        {
            get; set;
        }

        [JsonProperty("fromPos")]
        public int FromPos
        {
            get; set;
        }

        [JsonProperty("fromStationTelecode")]
        public string FromStationTelecode
        {
            get; set;
        }

        [JsonProperty("number")]
        public string Number
        {
            get; set;
        }

        [JsonProperty("numberFull")]
        public ObservableCollection<string> NumberFull
        {
            get; set;
        }

        [JsonProperty("numberKind")]
        public string NumberKind
        {
            get; set;
        }

        [JsonProperty("passTime")]
        public string PassTime
        {
            get; set;
        }

        [JsonProperty("rundays")]
        public ObservableCollection<string> RunDays
        {
            get; set;
        }

        [JsonProperty("runner")]
        public string Runner
        {
            get; set;
        }

        [JsonProperty("timetable")]
        public ObservableCollection<TimetableItem> Timetable
        {
            get; set;
        }

        [JsonProperty("toArrive")]
        public string ToArrive
        {
            get; set;
        }

        [JsonProperty("toPos")]
        public int ToPos
        {
            get; set;
        }

        [JsonProperty("toStationTelecode")]
        public string ToStationTelecode
        {
            get; set;
        }

        [JsonProperty("type")]
        public string Type
        {
            get; set;
        }
    }
}
