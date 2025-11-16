using RailGo.ViewModels.Pages.Shell;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Dispatching;

namespace RailGo.Views.Pages.Shell;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    private DispatcherQueueTimer _autoFlipTimer;

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        // 初始化自动轮播定时器
        InitializeAutoFlipTimer();
    }

    private void InitializeAutoFlipTimer()
    {
        _autoFlipTimer = DispatcherQueue.CreateTimer();
        _autoFlipTimer.Interval = TimeSpan.FromSeconds(5);
        _autoFlipTimer.Tick += AutoFlipTimer_Tick;

        // 当有图片时启动定时器
        if (ViewModel.BannerImages.Count > 1)
        {
            _autoFlipTimer.Start();
        }
    }

    private void AutoFlipTimer_Tick(DispatcherQueueTimer sender, object e)
    {
        if (BannerFlipView.Items.Count > 1)
        {
            int nextIndex = (BannerFlipView.SelectedIndex + 1) % BannerFlipView.Items.Count;
            BannerFlipView.SelectedIndex = nextIndex;
        }
    }

    // 图片加载失败处理
    private void BannerImage_ImageFailed(object sender, Microsoft.UI.Xaml.ExceptionRoutedEventArgs e)
    {
        if (sender is Image image)
        {
            // 可以在这里处理图片加载失败的情况
            System.Diagnostics.Debug.WriteLine($"图片加载失败: {e.ErrorMessage}");
        }
    }

    // 页面导航处理
    protected override void OnNavigatedFrom(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        // 离开页面时停止定时器
        _autoFlipTimer?.Stop();
    }

    protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        // 进入页面时如果有多个图片，启动定时器
        if (ViewModel.BannerImages.Count > 1)
        {
            _autoFlipTimer?.Start();
        }
    }
}