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

    // 动车组型号（从EmuNo中提取）
    [JsonIgnore]
    public string EmuNoModel
    {
        get
        {
            if (string.IsNullOrEmpty(EmuNo))
                return null;

            // 提取型号部分（去掉最后4位数字）
            if (EmuNo.Length > 4)
            {
                return EmuNo.Substring(0, EmuNo.Length - 4);
            }
            return EmuNo;
        }
    }

    // 动车组车号（从EmuNo中提取最后4位数字）
    [JsonIgnore]
    public string EmuNoCode
    {
        get
        {
            if (string.IsNullOrEmpty(EmuNo))
                return null;

            // 提取最后4位数字作为车号
            if (EmuNo.Length >= 4)
            {
                return EmuNo.Substring(EmuNo.Length - 4);
            }
            return EmuNo;
        }
    }

    // 完整格式：车型-车号
    [JsonIgnore]
    public string EmuNoFull
    {
        get
        {
            if (string.IsNullOrEmpty(EmuNo))
                return null;

            return $"{EmuNoModel}-{EmuNoCode}";
        }
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