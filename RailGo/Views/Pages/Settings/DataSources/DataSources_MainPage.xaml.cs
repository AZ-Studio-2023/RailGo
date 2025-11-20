using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings.DataSources;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_MainPage : Page
{
    public DataSources_MainViewModel ViewModel
    {
        get;
    }

    public DataSources_MainPage()
    {
        ViewModel = App.GetService<DataSources_MainViewModel>();
        InitializeComponent();
    }
}