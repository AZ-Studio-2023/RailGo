using HCM.Core.Models;
using HCM.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using Microsoft.Windows.ApplicationModel.Resources;

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
    public string content;
    public string url;
    public async Task GetContent()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Button_Click();
                }
            }
            List<TrainInfo> trainInfos = JsonConvert.DeserializeObject<List<TrainInfo>>(content);
            dataGrid.ItemsSource = trainInfos;
        }
        catch
        {
            Button_Click();
        }
    }
    private void GetEmuBtn_Click(object sender, RoutedEventArgs e)
    {
        if (EmuIdTextBox.Text != null)
        {
            url = "https://api.rail.re/emu/" + EmuIdTextBox.Text;
            GetContent();
        }

    }
    
    //弹窗
    private void Button_Click()
    {
        NotificationQueue.Show(null, 2000);
    }
}
