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

    // 删除方法的事件处理 - 现在什么都不做，因为方法不能删除
    private void OnDeleteMethodClick(object sender, RoutedEventArgs e)
    {
        // 方法不能删除，所以这个方法现在什么都不做
    }
}