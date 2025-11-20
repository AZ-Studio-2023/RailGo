using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RailGo.Core.Models.Settings;

public class DataSourceMethod
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mode")]
    public string Mode { get; set; }

    [JsonProperty("sources")]
    public string Sources { get; set; }
}

public class DataSourceGroup
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("data")]
    public ObservableCollection<DataSourceMethod> Data { get; set; } = new ObservableCollection<DataSourceMethod>();
}