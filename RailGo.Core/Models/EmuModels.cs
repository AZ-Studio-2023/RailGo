using System.Collections.Generic;
using Newtonsoft.Json;

namespace RailGo.Core.Models;

// 动车组运行交路
public class EmuOperation
{
    [JsonProperty("date")]
    public string Date
    {
        get; set;
    }

    [JsonProperty("train_no")]
    public string TrainNo
    {
        get; set;
    }

    [JsonProperty("emu_no")]
    public string EmuNo
    {
        get; set;
    }
}

// 动车组配属查询响应
public class EmuAssignmentResponse
{
    [JsonProperty("code")]
    public int Code
    {
        get; set;
    }

    [JsonProperty("data")]
    public PaginatedResponse<EmuAssignment> Data
    {
        get; set;
    }
}

// 正晚点响应
public class DelayResponse
{
    [JsonProperty("data")]
    public List<DelayInfo> Data
    {
        get; set;
    }
}