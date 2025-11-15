using RailGo.ViewModels.Pages.Stations;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using RailGo.Core.Models;
using RailGo.Core.Models.QueryDatas;

namespace RailGo.Views.Pages.Stations;

public sealed partial class Station_InformationPage : Page
{
    public Station_InformationViewModel ViewModel
    {
        get;
    }

    // 保持原有的 _item 字段，类型改为 StationSearch
    public StationPreselectResult _item;

    public Station_InformationPage()
    {
        ViewModel = App.GetService<Station_InformationViewModel>();
        InitializeComponent();
    }

    private void StationDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_item == null)
            return;

        // 创建车站详情页面，传递车站电报码
        StationDetailsPage page = new()
        {
            DataContext = _item
        };

        TabViewItem tabViewItem = new()
        {
            Header = _item.Name,
            Content = page,
            CanDrag = true,
            IconSource = new FontIconSource() { Glyph = "\uF161" }
        };

        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
    }
}