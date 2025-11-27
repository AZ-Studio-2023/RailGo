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

    public DataSources_CustomSourcesPage()
    {
        ViewModel = App.GetService<DataSources_CustomSourcesViewModel>();
        InitializeComponent();
    }
    private async void OnSelectSourceClick(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedMethodItem == null) return;

        await SourceSelectionDialog.ShowAsync();
    }
    private async void OnStartCreateNewClick(object sender, RoutedEventArgs e)
    {
        await CreateNewDialog.ShowAsync();
    }
}