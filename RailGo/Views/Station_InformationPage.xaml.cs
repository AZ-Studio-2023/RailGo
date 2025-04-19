using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;
using System.Collections.ObjectModel;
using RailGo.Core.Models;

namespace RailGo.Views;

public sealed partial class Station_InformationPage : Page
{
    public Station_InformationViewModel ViewModel
    {
        get;
    }
    public StationSearch _item;

    public Station_InformationPage()
    {
        ViewModel = App.GetService<Station_InformationViewModel>();
        InitializeComponent();
    }
    private void GetstationSearchInfoBtn_Click(object sender, RoutedEventArgs e)
    {
        if (StationSearchTextBox.Text != null)
        {
            Trace.WriteLine(ViewModel.InputSearchStation);
            ViewModel.Stations = ViewModel.SearchData(App.Global.StationsJson, ViewModel.InputSearchStation);
        }

    }
    private void StationDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
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
