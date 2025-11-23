using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.Contracts.Services;
using RailGo.Core.Models.Settings;
using RailGo.ViewModels.Pages.Settings.DataSources;
using RailGo.Views.ContentDialogs;

namespace RailGo.Views.Pages.Settings.DataSources;

public sealed partial class DataSources_CustomSourcesPage : Page
{
    public DataSources_CustomSourcesViewModel ViewModel
    {
        get;
    }

    public ObservableCollection<string> AvailableModes => ViewModel.AvailableModes;
    public IRelayCommand<DataSourceMethod> SelectSourceCommand => ViewModel.SelectSourceCommand;

    public DataSources_CustomSourcesPage()
    {
        ViewModel = App.GetService<DataSources_CustomSourcesViewModel>();
        InitializeComponent();
    }
    private void OnSelectSourceClick(object sender, RoutedEventArgs e)
    {
        if (!ViewModel.IsEditing) return;

        if (sender is Button button && button.Tag is DataSourceMethod method)
        {
            var parameters = new object[] { method, this.XamlRoot };
            ViewModel.SelectSourceWithRootCommand.Execute(parameters);
        }
    }
}