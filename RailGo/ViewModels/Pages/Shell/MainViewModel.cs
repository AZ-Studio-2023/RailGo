using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Query;
using RailGo.ViewModels.Pages.Stations;
using RailGo.ViewModels.Pages.TrainEmus;
using RailGo.ViewModels.Pages.Trains;

namespace RailGo.ViewModels.Pages.Shell;

public partial class MainViewModel : ObservableRecipient
{
    public Contracts.Services.INavigationService navigationService = App.GetService<Contracts.Services.INavigationService>();
    public MainViewModel()
    {
    }

    [RelayCommand]
    private async Task NavigationAsync(object parameter)
    {
        string buttonName = parameter?.ToString() ?? string.Empty;

        switch (buttonName)
        {
            case "ToTrainEmusButton":
                navigationService.NavigateTo(typeof(EMU_RoutingViewModel).FullName!);
                break;
            case "ToTrainsButton":
                navigationService.NavigateTo(typeof(Train_NumberViewModel).FullName!);
                break;
            case "ToStationsButton":
                navigationService.NavigateTo(typeof(Station_InformationViewModel).FullName!);
                break;
            default:
                navigationService.NavigateTo(typeof(MainViewModel).FullName!);
                break;
        }
    }
}