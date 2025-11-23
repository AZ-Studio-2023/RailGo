using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Models.Settings;
using RailGo.ViewModels.Pages.Settings.DataSources;
using System.Collections.ObjectModel;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_CustomSourcesPage : Page
{
    public DataSources_CustomSourcesViewModel ViewModel
    {
        get;
    }

    public ObservableCollection<string> AvailableModes => ViewModel.AvailableModes;

    public DataSources_CustomSourcesPage()
    {
        ViewModel = App.GetService<DataSources_CustomSourcesViewModel>();
        InitializeComponent();
    }

    // 删除方法的事件处理
    private void OnDeleteMethodClick(object sender, RoutedEventArgs e)
    {
        // 只有在编辑模式下才能删除
        if (!ViewModel.IsEditing) return;

        if (sender is Button button && button.Tag is DataSourceMethod method)
        {
            ViewModel.DeleteMethodCommand.Execute(method);
        }
    }
    public string GetCurrentItemName()
    {
        if (ViewModel.EditingItem != null)
            return ViewModel.EditingItem.Name ?? "";
        if (ViewModel.SelectedItem != null)
            return ViewModel.SelectedItem.Name ?? "";
        return "";
    }
}