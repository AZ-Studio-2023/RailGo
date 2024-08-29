using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RailGo.Models;

namespace RailGo.ViewModels;

public partial class Station_InformationViewModel : ObservableRecipient
{
    public Station_InformationViewModel()
    {
    }
    public string InputSearchStation;
    ObservableCollection<StationSearch> _StationSearch = new ObservableCollection<StationSearch>();
    public ObservableCollection<StationSearch> stationSearchInfo
    {

        get
        {
            return _StationSearch;
        }
        set
        {
            _StationSearch = value;
            OnPropertyChanged("stationSearchInfo");
        }
    }
}
