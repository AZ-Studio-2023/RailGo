using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using RailGo.Contracts.Services;
using RailGo.Core.Models.Settings;

namespace RailGo.ViewModels.Pages.Settings.DataSources;

public partial class DataSources_CustomSourcesViewModel : ObservableRecipient
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private DataSourceGroup? _selectedItem;

    [ObservableProperty]
    private ObservableCollection<DataSourceGroup> _customSources = new();

    [ObservableProperty]
    private DataSourceGroup? _editingItem;

    [ObservableProperty]
    private DataSourceGroup? _originalItemBackup;

    [ObservableProperty]
    private bool _isEditing = false;

    [ObservableProperty]
    private bool _isCreatingNew = false;

    [ObservableProperty]
    private ObservableCollection<string> _availableModes = new() { "online", "offline" };

    // 预定义的所有查询方法
    private readonly List<DataSourceMethod> _predefinedMethods = new()
    {
        // 车次查询接口
        new DataSourceMethod { Name = "QueryTrainPreselectAsync", Mode = "online", SourceName = "TrainPreselect" },
        new DataSourceMethod { Name = "QueryTrainQueryAsync", Mode = "online", SourceName = "TrainQuery" },
        new DataSourceMethod { Name = "QueryStationToStationQueryAsync", Mode = "online", SourceName = "StationToStationQuery" },
        
        // 车站查询接口
        new DataSourceMethod { Name = "QueryStationPreselectAsync", Mode = "online", SourceName = "StationPreselect" },
        new DataSourceMethod { Name = "QueryStationQueryAsync", Mode = "online", SourceName = "StationQuery" },
        new DataSourceMethod { Name = "QueryGetBigScreenDataAsync", Mode = "online", SourceName = "GetBigScreenData" },
        
        // 动车组查询接口
        new DataSourceMethod { Name = "QueryEmuQueryAsync", Mode = "online", SourceName = "EmuQuery" },
        new DataSourceMethod { Name = "QueryEmuAssignmentQueryAsync", Mode = "online", SourceName = "EmuAssignmentQuery" },
        
        // 实时数据接口
        new DataSourceMethod { Name = "QueryTrainDelayAsync", Mode = "online", SourceName = "TrainDelayQuery" },
        new DataSourceMethod { Name = "QueryPlatformInfoAsync", Mode = "online", SourceName = "PlatformInfoQuery" },
        
        // 其他接口
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
        SelectedItem = null; // 清空选择
    }

    [RelayCommand]
    private void StartEdit()
    {
        if (SelectedItem == null) return;

        // 备份原始数据
        OriginalItemBackup = CloneDataSourceGroup(SelectedItem);
        EditingItem = SelectedItem;
        IsCreatingNew = false;
        IsEditing = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (EditingItem == null) return;

        if (IsCreatingNew)
        {
            // 新建：添加到列表
            CustomSources.Add(EditingItem);
            SelectedItem = EditingItem;
        }

        // 保存到服务
        await _dataSourceService.SetDataSourceGroupAsync(EditingItem);

        IsEditing = false;
        IsCreatingNew = false;
        EditingItem = null;
        OriginalItemBackup = null;

        // 重新加载数据确保同步
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        if (IsCreatingNew)
        {
            // 新建取消：完全丢弃
            EditingItem = null;
        }
        else if (OriginalItemBackup != null && SelectedItem != null)
        {
            // 编辑取消：恢复原始数据
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

        // 保存更改到服务
        await _dataSourceService.SaveDataSourcesToSettingsAsync(CustomSources);
        await LoadDataSourcesAsync();
    }

    [RelayCommand]
    private void AddNewMethod()
    {
        // 由于方法都是预定义的，这个命令现在可能不需要了
        // 或者可以用于添加自定义方法（如果需要的话）
        Debug.WriteLine("所有方法都是预定义的，使用默认方法列表");
    }

    [RelayCommand]
    private void DeleteMethod(DataSourceMethod method)
    {
        // 方法不能删除，所以这个方法现在什么都不做
        Debug.WriteLine($"方法 {method?.Name} 是预定义的，不能删除");
    }

    // 计算属性 - 需要手动通知更新
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

    public Visibility EditorPanelVisibility =>
        (SelectedItem != null || EditingItem != null) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility NoSelectionPanelVisibility =>
        (SelectedItem == null && EditingItem == null) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility EditButtonVisibility =>
        (!IsEditing && SelectedItem != null) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility SaveButtonVisibility =>
        IsEditing ? Visibility.Visible : Visibility.Collapsed;

    public Visibility CancelButtonVisibility =>
        IsEditing ? Visibility.Visible : Visibility.Collapsed;

    public Visibility DeleteButtonVisibility =>
        (!IsCreatingNew && !IsEditing && SelectedItem != null) ? Visibility.Visible : Visibility.Collapsed;

    public ObservableCollection<DataSourceMethod> CurrentMethods
    {
        get
        {
            if (EditingItem != null)
                return EditingItem.Data;
            if (SelectedItem != null)
                return SelectedItem.Data;
            return new ObservableCollection<DataSourceMethod>();
        }
    }

    public string CurrentItemName
    {
        get
        {
            if (EditingItem != null)
                return EditingItem.Name ?? "";
            if (SelectedItem != null)
                return SelectedItem.Name ?? "";
            return "";
        }
        set
        {
            if (EditingItem != null)
                EditingItem.Name = value;
            else if (SelectedItem != null)
                SelectedItem.Name = value;
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
    public DataGridRowDetailsVisibilityMode RowDetailsVisibility
    {
        get
        {
            // 只有在编辑模式下才允许展开 RowDetails
            return IsEditing ? DataGridRowDetailsVisibilityMode.VisibleWhenSelected : DataGridRowDetailsVisibilityMode.Collapsed;
        }
    }

    // 当相关属性改变时，手动通知计算属性更新
    partial void OnSelectedItemChanged(DataSourceGroup? value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(EditorPanelVisibility));
        OnPropertyChanged(nameof(NoSelectionPanelVisibility));
        OnPropertyChanged(nameof(EditButtonVisibility));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
        OnPropertyChanged(nameof(CurrentMethods));
        OnPropertyChanged(nameof(CurrentItemName));
        OnPropertyChanged(nameof(RowDetailsVisibility));
    }

    partial void OnEditingItemChanged(DataSourceGroup? value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(EditorPanelVisibility));
        OnPropertyChanged(nameof(NoSelectionPanelVisibility));
        OnPropertyChanged(nameof(CurrentMethods));
        OnPropertyChanged(nameof(CurrentItemName));
        OnPropertyChanged(nameof(RowDetailsVisibility));
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(EditButtonVisibility));
        OnPropertyChanged(nameof(SaveButtonVisibility));
        OnPropertyChanged(nameof(CancelButtonVisibility));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
        OnPropertyChanged(nameof(CurrentItemName));
        OnPropertyChanged(nameof(RowDetailsVisibility));
    }

    partial void OnIsCreatingNewChanged(bool value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
        OnPropertyChanged(nameof(CurrentItemName));
        OnPropertyChanged(nameof(RowDetailsVisibility));
    }
}