using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Models;
using RailGo.ViewModels;

namespace RailGo.Views;

public sealed partial class StationDetailsPage : Page
{
    public StationPreselectResult DataFromLast => DataContext as StationPreselectResult;
    public StationDetailsViewModel ViewModel
    {
        get;
    }

    public StationDetailsPage()
    {
        InitializeComponent();
        this.Loaded += OnLoad;
        ViewModel = App.GetService<StationDetailsViewModel>();
        TrainsDataGrid.Visibility = Visibility.Visible;
        BigScreenDataGrid.Visibility = Visibility.Collapsed;
    }

    public void OnLoad(object sender, RoutedEventArgs e)
    {
        Trace.WriteLine("OnLoad");
        if (DataFromLast != null)
        {
            Trace.WriteLine("GetDataFromLast");
            // 使用电报码获取详细信息
            ViewModel.GetInformationCommand.Execute((DataFromLast.Name, DataFromLast.TeleCode));
        }
        this.Loaded -= OnLoad;
    }

    private void OnNavButtonChecked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton radioButton && TrainsDataGrid != null && BigScreenDataGrid != null)
        {
            Trace.WriteLine("RadioButtonChanges");
            // 根据选择的导航按钮切换右侧内容
            switch (radioButton.Content.ToString())
            {
                case "途经车次":
                    Trace.WriteLine("途经车次");
                    TrainsDataGrid.Visibility = Visibility.Visible;
                    BigScreenDataGrid.Visibility = Visibility.Collapsed;
                    // 其他页面隐藏
                    break;
                case "车站大屏":
                    Trace.WriteLine("车站大屏");
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    BigScreenDataGrid.Visibility = Visibility.Visible;
                    // 显示页面二
                    break;
                case "路线":
                    TrainsDataGrid.Visibility = Visibility.Collapsed;
                    BigScreenDataGrid.Visibility = Visibility.Collapsed;
                    // 显示页面三
                    break;
            }
        }
    }
}