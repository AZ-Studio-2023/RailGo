using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using RailGo.Core.Helpers;
using RailGo.Core.Models;
using static System.Collections.Specialized.BitVector32;

namespace RailGo.ViewModels;

public partial class Station_InformationViewModel : ObservableRecipient
{
    public Station_InformationViewModel()
    {
    }

    [ObservableProperty]
    public ObservableCollection<StationSearch> stations = App.Global.StationsJson;
    [ObservableProperty]
    public string inputSearchStation;

    public ObservableCollection<StationSearch> SearchData(ObservableCollection<StationSearch> sourceCollection, string queryText)
    {
        string normalizedQuery = queryText.Replace(" ", "");

        var filteredItems = sourceCollection
            .Where(item =>
                item.Name.Replace(" ", "").Contains(normalizedQuery, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        return new ObservableCollection<StationSearch>(filteredItems);
    }
}
