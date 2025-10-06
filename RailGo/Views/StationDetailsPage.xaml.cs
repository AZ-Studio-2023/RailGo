using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Models;
using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class StationDetailsPage : Page
{
    public StationSearch DataFromLast => DataContext as StationSearch;
    public StationDetailsViewModel ViewModel
    {
        get;
    }

    public StationDetailsPage()
    {
        InitializeComponent();
        this.Loaded += OnLoad;
        ViewModel = App.GetService<StationDetailsViewModel>();
    }

    public void OnLoad(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("OnLoad");
        if (DataFromLast != null)
        {
            Trace.WriteLine("GetDataFromLast");
            // 使用电报码获取详细信息
            ViewModel.GetInformationCommand.Execute((DataFromLast.TeleCode));
        }
        this.Loaded -= OnLoad;
    }

    private void OnNavButtonChecked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && TrainsDataGrid != null)
        {
            // 根据选择的导航按钮切换右侧内容
            switch (radioButton.Content.ToString())
            {
                case "车次信息":
                    TrainsDataGrid.Visibility = Visibility.Visible;
                    // 其他页面隐藏
                    break;
                case "页面二":
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    // 显示页面二
                    break;
                case "页面三":
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    // 显示页面三
                    break;
            }
        }
    }
}