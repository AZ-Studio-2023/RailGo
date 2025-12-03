using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using RailGo.Core.Models.Messages;
using RailGo.Core.Models.QueryDatas;
using RailGo.Services;

namespace RailGo.ViewModels.Pages.StationToStation;

public partial class StationToStationViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string contentText = "最近查询";

    [ObservableProperty]
    private StationPreselectResult fromStation;

    [ObservableProperty]
    private StationPreselectResult toStation;

    [ObservableProperty]
    private bool selectTeachingTipIsOpen;

    [ObservableProperty]
    private bool allowCitysIsOpen = true;

    [ObservableProperty]
    private DateTimeOffset startDate = new DateTimeOffset(DateTime.Now);

    [ObservableProperty]
    public ObservableCollection<TrainRunInfo> trainResults;

    // 预留 API URL
    private const string ApiUrl = "https://data.railgo.zenglingkun.cn/api/train/sts_query?from={0}&to={1}&date=20251130";

    public StationToStationViewModel()
    {
        WeakReferenceMessenger.Default.Register<StationSelectedInStationToStationMessagerModel>(this, (recipient, message) =>
        {
            if (message != null && message.MessagerName == "StationToStation_SearchFromStation")
            {
                FromStation = message.Data;
                SelectTeachingTipIsOpen = false;
            }
            else if (message != null && message.MessagerName == "StationToStation_SearchToStation")
            {
                ToStation = message.Data;
                SelectTeachingTipIsOpen = false;
            }
        });
    }

    public async Task QueryTrainListAsync()
    {
        if (FromStation == null || ToStation == null)
        {
            ContentText = "请选择始发站和终到站";
            return;
        }

        string url = string.Format(ApiUrl, FromStation.TeleCode, ToStation.TeleCode);

        try
        {
            var queryService = App.GetService<QueryService>();
            TrainResults = await queryService.QueryStationToStationQueryAsync(FromStation.TeleCode, ToStation.TeleCode, StartDate.ToString("yyyyMMdd"), AllowCitysIsOpen);
            Trace.WriteLine($"查询到 {TrainResults.Count} 条结果");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"查询失败：{ex.Message}");
        }
    }
}
