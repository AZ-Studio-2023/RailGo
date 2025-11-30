using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.StationToStation;
using RailGo.Views.Pages.Stations;
using Windows.Foundation.Metadata;

namespace RailGo.Views.Pages.StationToStation;

public sealed partial class StationToStationPage : Page
{
    public StationToStationViewModel ViewModel
    {
        get;
    }

    public Station_InformationPage FromStationSelector;
    public Station_InformationPage ToStationSelector;

    public StationToStationPage()
    {
        ViewModel = App.GetService<StationToStationViewModel>();
        InitializeComponent();
        FromStationSelector = App.GetService<Station_InformationPage>();
        ToStationSelector = App.GetService<Station_InformationPage>();
        FromStationSelector.SetMode("StationToStation_SearchFromStation");
        ToStationSelector.SetMode("StationToStation_SearchToStation");
    }

    private void OpenStationSelector_FromStation_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        PageFrame.Content = FromStationSelector;
        ViewModel.ContentText = "选择始发车站（点击车站行里面的“查看详情”按钮即可选择）";
        ViewModel.SelectTeachingTipIsOpen = true;
    }

    private void OpenStationSelector_ToStation_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        PageFrame.Content = ToStationSelector;
        ViewModel.ContentText = "选择终到车站（点击车站行里面的“查看详情”按钮即可选择）";
        ViewModel.SelectTeachingTipIsOpen = true;
    }

    private void SwitchFromAndToButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var From = ViewModel.FromStation;
        var To = ViewModel.ToStation;
        ViewModel.FromStation = To;
        ViewModel.ToStation = From;
    }

    private void CloseStationChooseButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectTeachingTipIsOpen = false;
    }
}
