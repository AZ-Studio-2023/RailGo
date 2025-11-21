using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings.DataSources;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_CustomSourcesPage : Page
{
    public DataSources_CustomSourcesViewModel ViewModel
    {
        get;
    }

    public DataSources_CustomSourcesPage()
    {
        ViewModel = App.GetService<DataSources_CustomSourcesViewModel>();
        InitializeComponent();
    }
}