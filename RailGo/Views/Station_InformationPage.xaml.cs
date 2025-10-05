using RailGo.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using RailGo.Core.Models;

namespace RailGo.Views;

public sealed partial class Station_InformationPage : Page
{
    public Station_InformationViewModel ViewModel
    {
        get;
    }

    // 保持原有的 _item 字段，类型改为 StationSearch
    public StationSearch _item;

    public Station_InformationPage()
    {
        ViewModel = App.GetService<Station_InformationViewModel>();
        InitializeComponent();
    }

    // 移除原来的同步点击事件，改用 Command
    // private void GetstationSearchInfoBtn_Click(object sender, RoutedEventArgs e)
    // {
    //     if (StationSearchTextBox.Text != null)
    //     {
    //         Trace.WriteLine(ViewModel.InputSearchStation);
    //         ViewModel.Stations = ViewModel.SearchData(App.Global.StationsJson, ViewModel.InputSearchStation);
    //     }
    // }

    private void StationDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_item == null)
            return;

        // 创建车站详情页面，传递车站电报码
        StationDetailsPage page = new()
        {
            // 这里需要根据您的 StationDetailsPage 的构造函数调整
            // 可能需要传递车站电报码而不是整个对象
        };

        TabViewItem tabViewItem = new()
        {
            Header = _item.Name + "站详情",
            Content = page,
            CanDrag = true,
            IconSource = new FontIconSource() { Glyph = "\uF161" }
        };

        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
    }
}