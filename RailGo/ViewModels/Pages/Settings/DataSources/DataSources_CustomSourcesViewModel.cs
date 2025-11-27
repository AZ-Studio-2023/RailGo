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
    private DataSourceGroup? _selectedItem;

    [ObservableProperty]
    private ObservableCollection<DataSourceGroup> _customSources = new();

    [ObservableProperty]
    private ObservableCollection<DataSourceMethod> selectingMethodItem; // ListView选中的方法加载出来的DataGrid信息

    [ObservableProperty]
    private DataSourceMethod selectedMethodItem; // ListView选中的方法加载出来的DataGrid中选中的项

    // 编辑时的ContentDialog
    [ObservableProperty]
    private ObservableCollection<DataSourceMethod> methodModeSelectIsOnline; // ContentDialog中的选择的是否在线

    [ObservableProperty]
    private ObservableCollection<DataSourceMethod> methodModeSelectIsOffline; // ContentDialog中的选择的是否离线

    [ObservableProperty]
    private object currentSelectSources_Editing = new(); // ContentDialog中的选择的数据源

    [ObservableProperty]
    private ObservableCollection<object> currentSources_Editing = new(); // ContentDialog中的所有的数据源






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
    private void StartCreateNew()
    {
        EditingItem = new DataSourceGroup
        {
            Name = "新数据源组",
            Data = new ObservableCollection<DataSourceMethod>(_predefinedMethods.Select(m => new DataSourceMethod
            {
                Name = m.Name,
                Mode = m.Mode,
                SourceName = m.SourceName
            }))
        };
        OriginalItemBackup = null;
        IsCreatingNew = true;
        IsEditing = true;
        SelectedItem = null;
    }

    [RelayCommand]
    private void SelectionItemAsync()
    {
        if (SelectedItem == null) return;

        SelectingMethodItem = SelectedItem.Data;
    }

    [RelayCommand]
    private void StartEdit()
    {
        if (SelectedItem == null) return;

        OriginalItemBackup = CloneDataSourceGroup(SelectedItem);
        EditingItem = SelectedItem;
        IsCreatingNew = false;
        IsEditing = true;
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
        if (MethodModeSelectIsOnline != null && MethodModeSelectIsOnline.Count > 0)
        {
            Mode = "online";
        }
        if (MethodModeSelectIsOffline != null && MethodModeSelectIsOffline.Count > 0)
        {
            Mode = "offline";
        }

        DataSourceMethod EditedMethod = new DataSourceMethod
        {
            Name = SelectedMethodItem.Name,
            Mode = SelectedMethodItem.Mode,
            SourceName = SourceName
        };

        await _dataSourceService.SetDataSourceMethodAsync(SelectedMethodItem.Name, EditedMethod);
    }

    [RelayCommand]
    private void CancelEdit()
    {
        if (IsCreatingNew)
        {
            EditingItem = null;
        }
        else if (OriginalItemBackup != null && SelectedItem != null)
        {
            SelectedItem.Name = OriginalItemBackup.Name;
            SelectedItem.Data = new ObservableCollection<DataSourceMethod>(OriginalItemBackup.Data);
        }

        IsEditing = false;
        IsCreatingNew = false;
        EditingItem = null;
        OriginalItemBackup = null;
    }

    [RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedItem == null) return;

        CustomSources.Remove(SelectedItem);
        SelectedItem = null;

        IsEditing = false;
        IsCreatingNew = false;
        EditingItem = null;

        await _dataSourceService.SaveDataSourcesToSettingsAsync(CustomSources);
        await LoadDataSourcesAsync();
    }

    public string EditorTitle
    {
        get
        {
            if (IsCreatingNew) return "新建数据源组";
            if (EditingItem != null) return EditingItem.Name;
            if (SelectedItem != null) return SelectedItem.Name;
            return "数据源组";
        }
    }
    private DataSourceGroup CloneDataSourceGroup(DataSourceGroup original)
    {
        return new DataSourceGroup
        {
            Name = original.Name,
            Data = new ObservableCollection<DataSourceMethod>(
                original.Data.Select(m => new DataSourceMethod
                {
                    Name = m.Name,
                    Mode = m.Mode,
                    SourceName = m.SourceName
                }))
        };
    }

}