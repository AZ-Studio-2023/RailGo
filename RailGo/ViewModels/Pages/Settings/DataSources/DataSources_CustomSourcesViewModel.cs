using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Contracts.Services;
using RailGo.Core.Models.Settings;
using RailGo.Views.ContentDialogs;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_CustomSourcesViewModel : ObservableRecipient
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private DataSourceGroup? selectedItem; // ListView选中的数据源组

    [ObservableProperty]
    private ObservableCollection<DataSourceGroup> customSources = new(); // 所有自定义数据源组

    [ObservableProperty]
    private ObservableCollection<DataSourceMethod> selectingMethodItem; // ListView选中的方法加载出来的DataGrid信息

    [ObservableProperty]
    private DataSourceMethod selectedMethodItem; // ListView选中的方法加载出来的DataGrid中选中的项

    [ObservableProperty]
    private string currentItemName = "数据源组"; // 当前的方法名称

    [ObservableProperty]
    private bool isTitleEditing = false; // 是否正在编辑标题

    // 编辑时的ContentDialog
    [ObservableProperty]
    private bool methodModeSelectIsOnline; // ContentDialog中的选择的是否在线

    [ObservableProperty]
    private bool methodModeSelectIsOffline; // ContentDialog中的选择的是否离线

    [ObservableProperty]
    private object currentSelectSources_Editing = new(); // ContentDialog中的选择的数据源

    [ObservableProperty]
    private ObservableCollection<object> currentSources_Editing = new(); // ContentDialog中的所有的数据源

    // 可见性
    [ObservableProperty]
    private Visibility isContentOpen = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility isContentClose = Visibility.Visible;

    // 新建数据源
    [ObservableProperty]
    private string newDataSourceGroupName;

    [ObservableProperty]
    private ObservableCollection<string> _availableModes = new() { "online", "offline" };

    private readonly List<DataSourceMethod> _predefinedMethods = new()
    {
        new DataSourceMethod { Name = "QueryTrainPreselectAsync", Mode = "online", SourceName = "TrainPreselect" },
        new DataSourceMethod { Name = "QueryTrainQueryAsync", Mode = "online", SourceName = "TrainQuery" },
        new DataSourceMethod { Name = "QueryStationToStationQueryAsync", Mode = "online", SourceName = "StationToStationQuery" },
        
        new DataSourceMethod { Name = "QueryStationPreselectAsync", Mode = "online", SourceName = "StationPreselect" },
        new DataSourceMethod { Name = "QueryStationQueryAsync", Mode = "online", SourceName = "StationQuery" },
        new DataSourceMethod { Name = "QueryGetBigScreenDataAsync", Mode = "online", SourceName = "GetBigScreenData" },
        
        new DataSourceMethod { Name = "QueryEmuQueryAsync", Mode = "online", SourceName = "EmuQuery" },
        new DataSourceMethod { Name = "QueryEmuAssignmentQueryAsync", Mode = "online", SourceName = "EmuAssignmentQuery" },
        
        new DataSourceMethod { Name = "QueryTrainDelayAsync", Mode = "online", SourceName = "TrainDelayQuery" },
        new DataSourceMethod { Name = "QueryPlatformInfoAsync", Mode = "online", SourceName = "PlatformInfoQuery" },
        
        new DataSourceMethod { Name = "QueryDownloadEmuImageAsync", Mode = "online", SourceName = "DownloadEmuImage" }
    };

    public DataSources_CustomSourcesViewModel(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private async Task LoadDataSourcesAsync()
    {
        var sources = await _dataSourceService.GetAllDataSourcesAsync();
        CustomSources = new ObservableCollection<DataSourceGroup>(sources);
    }

    [RelayCommand]
    private async void StartCreateNew()
    {
        var NewItem = new DataSourceGroup
        {
            Name = NewDataSourceGroupName,
            Data = new ObservableCollection<DataSourceMethod>(_predefinedMethods.Select(m => new DataSourceMethod
            {
                Name = m.Name,
                Mode = m.Mode,
                SourceName = m.SourceName
            }))
        };
        await _dataSourceService.SetDataSourceGroupAsync(NewItem);
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private async Task SelectionItemAsync()
    {
        if (SelectedItem == null) return;

        SelectingMethodItem = SelectedItem.Data;
        CurrentItemName = SelectedItem.Name;
        IsContentOpen = Visibility.Visible;
        IsContentClose = Visibility.Collapsed;
    }

    [RelayCommand]
    private async Task SaveSelectedMethodAsync()
    {
        if (SelectingMethodItem == null) return;

        var SourceName = string.Empty;
        if (CurrentSelectSources_Editing is OnlineApiSource)
        {
            SourceName = (CurrentSelectSources_Editing as OnlineApiSource)?.Name;
        }
        if (CurrentSelectSources_Editing is LocalDatabaseSource)
        {
            SourceName = (CurrentSelectSources_Editing as LocalDatabaseSource)?.Name;
        }

        var Mode = string.Empty;
        if (MethodModeSelectIsOnline != null)
        {
            Mode = "online";
        }
        else if (MethodModeSelectIsOffline != null)
        {
            Mode = "offline";
        }
        else
        {
            Mode = SelectedMethodItem.Mode;
        }

            DataSourceMethod EditedMethod = new DataSourceMethod
            {
                Name = SelectedMethodItem.Name,
                Mode = SelectedMethodItem.Mode,
                SourceName = SourceName
            };

        await _dataSourceService.SetDataSourceMethodAsync(SelectedMethodItem.Name, EditedMethod);
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedItem == null) return;

        CustomSources.Remove(SelectedItem);
        SelectedItem = null;

        await _dataSourceService.SaveDataSourcesToSettingsAsync(CustomSources);
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private async Task EditSelectedItemAsync()
    {
        if (SelectedItem == null) return;

        IsTitleEditing = true;
    }

    [RelayCommand]
    private async Task EditSelectedItemOKAsync()
    {
        if (SelectedItem == null) return;

        IsTitleEditing = false;
        var EditedItem = new DataSourceGroup
        {
            Name = CurrentItemName,
            Data = SelectingMethodItem
        };
        CustomSources.Remove(SelectedItem);
        CustomSources.Add(EditedItem);

        await _dataSourceService.SaveDataSourcesToSettingsAsync(CustomSources);
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private async Task EditSelectedItemNOAsync()
    {
        if (SelectedItem == null) return;

        IsTitleEditing = false;
    }
}