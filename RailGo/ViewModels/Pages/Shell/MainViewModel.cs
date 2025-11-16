using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RailGo.Core.Query.Online;
using RailGo.ViewModels.Pages.Stations;
using RailGo.ViewModels.Pages.TrainEmus;
using RailGo.ViewModels.Pages.Trains;
using System.Collections.ObjectModel;

namespace RailGo.ViewModels.Pages.Shell;

public partial class MainViewModel : ObservableObject
{
    public Contracts.Services.INavigationService navigationService = App.GetService<Contracts.Services.INavigationService>();

    [ObservableProperty]
    private ObservableCollection<string> _bannerImages = new ObservableCollection<string>();

    private const string DefaultBannerImage = "https://api.state.railgo.zenglingkun.cn/uploads/index_banner_1.png";

    public MainViewModel()
    {
        // 先添加默认图片
        _bannerImages.Add(DefaultBannerImage);

        // 异步加载网络图片
        _ = LoadBannerImagesAsync();
    }

    private async Task LoadBannerImagesAsync()
    {
        try
        {
            var images = await SettingsAPIService.GetBannerImagesAsync();

            if (images?.Count > 0)
            {
                // 清空并添加新图片
                _bannerImages.Clear();
                foreach (var imageUrl in images)
                {
                    _bannerImages.Add(imageUrl);
                }
            }
            // 如果加载失败或为空，就保持默认图片
        }
        catch
        {
            // 发生异常时保持默认图片
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