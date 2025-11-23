using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Contracts.Services;
using RailGo.Core.Models.Settings;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RailGo.Views.ContentDialogs;

[ObservableObject]
public sealed partial class SourceSelectionDialog : ContentDialog
{
    private readonly IDataSourceService _dataSourceService;

    [ObservableProperty]
    private ObservableCollection<object> _currentSources = new();

    [ObservableProperty]
    private object _selectedSource;

    [ObservableProperty]
    private bool _isOnlineMode = true;

    [ObservableProperty]
    private bool _isOfflineMode;

    public string SelectedSourceName
    {
        get; private set;
    }

    public SourceSelectionDialog(IDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await LoadSourcesAsync();
    }

    // 模式改变时重新加载数据源
    partial void OnIsOnlineModeChanged(bool value)
    {
        if (value) _ = LoadSourcesAsync();
    }

    partial void OnIsOfflineModeChanged(bool value)
    {
        if (value) _ = LoadSourcesAsync();
    }

    private async Task LoadSourcesAsync()
    {
        CurrentSources.Clear();

        // 添加默认项
        var defaultItem = new { Name = "RailGoDefault", Address = "RailGo默认" };
        CurrentSources.Add(defaultItem);

        if (IsOnlineMode)
        {
            var onlineSources = await _dataSourceService.GetOnlineApiSourcesAsync();
            foreach (var source in onlineSources)
            {
                CurrentSources.Add(source);
            }
        }
        else
        {
            var offlineSources = await _dataSourceService.GetLocalDatabaseSourcesAsync();
            foreach (var source in offlineSources)
            {
                CurrentSources.Add(source);
            }
        }
    }

    private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        // 确保有选中项时才允许确认
        if (SelectedSource == null)
        {
            args.Cancel = true;
            return;
        }

        // 设置选中的源名称
        if (SelectedSource is { } anonymous && anonymous.GetType().GetProperty("Name")?.GetValue(anonymous) is string defaultName)
        {
            SelectedSourceName = defaultName;
        }
        else if (SelectedSource is OnlineApiSource onlineSource)
        {
            SelectedSourceName = onlineSource.Name;
        }
        else if (SelectedSource is LocalDatabaseSource offlineSource)
        {
            SelectedSourceName = offlineSource.Name;
        }
    }
}