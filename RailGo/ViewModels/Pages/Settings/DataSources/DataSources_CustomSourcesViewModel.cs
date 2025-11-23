using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RailGo.Contracts.Services;
using RailGo.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using RailGo.Core.Models.Settings;
using System.Collections.ObjectModel;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_CustomSourcesViewModel : ObservableRecipient
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private OnlineApiSource item;

    [ObservableProperty]
    public ObservableCollection<DataSourceGroup> customSources = new();

    public DataSources_CustomSourcesViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
    }
}