using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RailGo.Core.Models.Messages;
using RailGo.Core.Models.QueryDatas;
using RailGo.Helpers;

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
}