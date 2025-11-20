using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings.DataSources;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_ShellPage : Page
{
    public DataSources_ShellViewModel ViewModel
    {
        get;
    }

    public DataSources_ShellPage()
    {
        ViewModel = App.GetService<DataSources_ShellViewModel>();
        InitializeComponent();
    }
}