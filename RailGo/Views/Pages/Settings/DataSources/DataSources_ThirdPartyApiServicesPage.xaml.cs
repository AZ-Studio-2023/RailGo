using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings.DataSources;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_ThirdPartyApiServicesPage : Page
{
    public DataSources_ThirdPartyApiServicesViewModel ViewModel
    {
        get;
    }

    public DataSources_ThirdPartyApiServicesPage()
    {
        ViewModel = App.GetService<DataSources_ThirdPartyApiServicesViewModel>();
        InitializeComponent();
    }
}