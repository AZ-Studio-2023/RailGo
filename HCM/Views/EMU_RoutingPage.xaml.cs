using HCM.Models;
using HCM.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.UI.Windowing;
using CommunityToolkit.WinUI.Controls;

namespace HCM.Views;

public sealed partial class EMU_RoutingPage : Page
{
    public EMU_RoutingViewModel ViewModel
    {
        get;
    }

    public EMU_RoutingPage()
    {
        ViewModel = App.GetService<EMU_RoutingViewModel>();
        InitializeComponent();
    }

    // 这一部分没有遵循MVVM设计模式
    // 不管了，马上开学了，能跑就行（
    // 会抽空重构这个破玩意的
    public string XGZurl = "http://www.xiaguanzhan.com/";

    private void GetEmuBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EmuIdTextBox.Text != null)
        {
            // url = "https://api.rail.re/emu/" + EmuIdTextBox.Text;
            ViewModel.GetContent();
        }

    }
    private async void OpenWebsite(object sender, RoutedEventArgs e)
    {
        switch ((sender as SettingsCard).Name)
        {
            case "OpenXGZ":
                await Windows.System.Launcher.LaunchUriAsync(new Uri(XGZurl));
                break;

            case "ChinaEmuCn":
                await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.china-emu.cn/Trains"));
                break;

        }
    }
}
