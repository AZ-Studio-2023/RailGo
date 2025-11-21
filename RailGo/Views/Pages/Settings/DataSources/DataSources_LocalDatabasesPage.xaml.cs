using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings.DataSources;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_LocalDatabasesPage : Page
{
    public DataSources_LocalDatabasesViewModel ViewModel
    {
        get;
    }

    public DataSources_LocalDatabasesPage()
    {
        ViewModel = App.GetService<DataSources_LocalDatabasesViewModel>();
        InitializeComponent();
    }
}