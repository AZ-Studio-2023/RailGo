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
    private DataSourceGroup? _editingItem;

    [ObservableProperty]
    private DataSourceGroup? _originalItemBackup;

    [ObservableProperty]
    private bool _isEditing = false;

    [ObservableProperty]
    private bool _isCreatingNew = false;

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
    private void StartEdit()
    {
        if (SelectedItem == null) return;

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
            CustomSources.Add(EditingItem);
            SelectedItem = EditingItem;
        }

        await _dataSourceService.SetDataSourceGroupAsync(EditingItem);

        IsEditing = false;
        IsCreatingNew = false;
        EditingItem = null;
        OriginalItemBackup = null;

        await LoadDataSourcesAsync();
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

    [RelayCommand]
    private void AddNewMethod()
    {
        Debug.WriteLine("所有方法都是预定义的，使用默认方法列表");
    }

    [RelayCommand]
    private void DeleteMethod(DataSourceMethod method)
    {
        Debug.WriteLine($"方法 {method?.Name} 是预定义的，不能删除");
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
            return IsEditing ? DataGridRowDetailsVisibilityMode.VisibleWhenSelected : DataGridRowDetailsVisibilityMode.Collapsed;
        }
    }

    [RelayCommand]
    private async Task SelectSourceWithRootAsync(object parameter)
    {
        if (parameter is not object[] parameters || parameters.Length != 2) return;

        var method = parameters[0] as DataSourceMethod;
        var xamlRoot = parameters[1] as XamlRoot;

        if (method == null || xamlRoot == null || !IsEditing) return;

        var dialog = new SourceSelectionDialog(_dataSourceService)
        {
            XamlRoot = xamlRoot
        };

        dialog.IsOnlineMode = method.Mode?.ToLower() == "online";
        dialog.IsOfflineMode = method.Mode?.ToLower() == "offline";

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary && !string.IsNullOrEmpty(dialog.SelectedSourceName))
        {
            method.SourceName = dialog.SelectedSourceName;

            OnPropertyChanged(nameof(CurrentMethods));
        }
    }

    [RelayCommand]
    private async Task SelectSourceAsync(DataSourceMethod method)
    {
        if (method == null || !IsEditing) return;

        var dialog = new SourceSelectionDialog(_dataSourceService);

        dialog.IsOnlineMode = method.Mode?.ToLower() == "online";
        dialog.IsOfflineMode = method.Mode?.ToLower() == "offline";
        Trace.WriteLine("CAONIMT");

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary && !string.IsNullOrEmpty(dialog.SelectedSourceName))
        {
            method.SourceName = dialog.SelectedSourceName;

            OnPropertyChanged(nameof(CurrentMethods));
        }
    }

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