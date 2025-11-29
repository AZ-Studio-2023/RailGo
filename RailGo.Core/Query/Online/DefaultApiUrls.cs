using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailGo.Core.Query.Online;
public static class DefaultApiUrls
{
    private static readonly Dictionary<string, string> _urlMappings = new Dictionary<string, string>
    {
        { "TrainPreselect", "https://data.railgo.zenglingkun.cn/api/train/preselect" },
        { "TrainQuery", "https://data.railgo.zenglingkun.cn/api/train/query" },
        { "StationToStationQuery", "https://data.railgo.zenglingkun.cn/api/train/sts_query" },
        { "StationPreselect", "https://data.railgo.zenglingkun.cn/api/station/preselect" },
        { "StationQuery", "https://data.railgo.zenglingkun.cn/api/station/query" },
        { "EmuAssignmentQuery", "https://delay.data.railgo.zenglingkun.cn/api/trainAssignment/queryEmu" },
        { "TrainDelayQuery", "https://delay.data.railgo.zenglingkun.cn/api/trainDetails/queryTrainDelayDetails" },
        { "PlatformInfoQuery", "https://mobile.12306.cn/wxxcx/wechat/bigScreen/getExit" },
        { "GetBigScreenData", "https://screen.data.railgo.zenglingkun.cn" },
        { "EmuQuery", "https://emu.data.railgo.zenglingkun.cn" },
        { "DownloadEmuImage", "https://tp.railgo.zenglingkun.cn/api" }
    };

    public static string GetDefaultUrl(string methodName)
    {
        return _urlMappings.TryGetValue(methodName, out var url) ? url : null;
    }

    public static bool TryGetDefaultUrl(string methodName, out string url)
    {
        return _urlMappings.TryGetValue(methodName, out url);
    }
}
