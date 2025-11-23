using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
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
            Data = new ObservableCollection<DataSourceMethod>()
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
        if (EditingItem == null || !IsEditing) return;

        var newMethod = new DataSourceMethod
        {
            Name = "新方法",
            Mode = "online",
            SourceName = "源名称"
        };

        EditingItem.Data.Add(newMethod);
    }

    [RelayCommand]
    private void DeleteMethod(DataSourceMethod method)
    {
        if (!IsEditing) return; // 只有在编辑模式下才能删除
        EditingItem?.Data.Remove(method);
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

    // 当相关属性改变时，手动通知计算属性更新
    partial void OnSelectedItemChanged(DataSourceGroup? value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(EditorPanelVisibility));
        OnPropertyChanged(nameof(NoSelectionPanelVisibility));
        OnPropertyChanged(nameof(EditButtonVisibility));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
        OnPropertyChanged(nameof(CurrentMethods));
    }

    partial void OnEditingItemChanged(DataSourceGroup? value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(EditorPanelVisibility));
        OnPropertyChanged(nameof(NoSelectionPanelVisibility));
        OnPropertyChanged(nameof(CurrentMethods));
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(EditButtonVisibility));
        OnPropertyChanged(nameof(SaveButtonVisibility));
        OnPropertyChanged(nameof(CancelButtonVisibility));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
    }

    partial void OnIsCreatingNewChanged(bool value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(DeleteButtonVisibility));
    }
}