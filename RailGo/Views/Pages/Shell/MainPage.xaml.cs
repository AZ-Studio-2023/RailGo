using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Shell;

namespace RailGo.Views.Pages.Shell;

public sealed partial class MainPage : Page
{
    private const double BannerWidthRatio = 1938;
    private const double BannerHeightRatio = 585;
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateBannerHeight();
    }

    // 页面大小变化时动态更新轮播图高度
    private void OnContentAreaSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateBannerHeight();
    }

    // 计算并更新轮播图高度
    private void UpdateBannerHeight()
    {
        // 获取当前的页面宽度
        double pageWidth = ContentArea.ActualWidth;

        // 计算新的高度（宽度 / 长宽比）
        double newHeight = (pageWidth / BannerWidthRatio) * BannerHeightRatio;

        // 设置轮播图的高度
        BannerFlipView.Height = newHeight;
    }
}