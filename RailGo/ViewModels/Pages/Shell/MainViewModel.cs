using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using RailGo.Core.Query.Online;
using RailGo.ViewModels.Pages.Stations;
using RailGo.ViewModels.Pages.TrainEmus;
using RailGo.ViewModels.Pages.Trains;
using System.Collections.ObjectModel;

namespace RailGo.ViewModels.Pages.Shell;

public partial class MainViewModel : ObservableRecipient
{
    public Contracts.Services.INavigationService navigationService = App.GetService<Contracts.Services.INavigationService>();

    [ObservableProperty]
    private ObservableCollection<string> bannerImages = new ObservableCollection<string>();

    [ObservableProperty]
    private bool isLoadingBanners = true;

    public MainViewModel()
    {
        LoadBannerImages();
    }

    [RelayCommand]
    private async Task LoadBannerImages()
    {
        try
        {
            IsLoadingBanners = true;
            var images = await SettingsAPIService.GetBannerImagesAsync();

            BannerImages.Clear();
            foreach (var imageUrl in images)
            {
                BannerImages.Add(imageUrl);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"加载轮播图失败: {ex.Message}");
            // 可以添加一些默认图片作为fallback
            BannerImages.Clear();
            BannerImages.Add("https://via.placeholder.com/800x300?text=默认图片1");
            BannerImages.Add("https://via.placeholder.com/800x300?text=默认图片2");
        }
        finally
        {
            IsLoadingBanners = false;
        }
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