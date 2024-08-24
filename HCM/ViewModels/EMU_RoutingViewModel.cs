using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using HCM.Models;
using HCM.Models;
using Newtonsoft.Json;
using Windows.System;

namespace HCM.ViewModels;

public partial class EMU_RoutingViewModel : ObservableObject
{
    ObservableCollection<TrainInfo> _trainInfos = new ObservableCollection<TrainInfo>();
    public ObservableCollection<TrainInfo> trainInfos
    {

        get
        {
            return _trainInfos;
        }
        set
        {
            _trainInfos = value;
            OnPropertyChanged("trainInfos");
        }
    }
    public string content;
    public string url = "https://api.rail.re/emu/CR400BF5033";
    public EMU_RoutingViewModel()
    {
    }
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
                    // XGZurl = "http://www.xiaguanzhan.com/soso.asp?keyword=" + EmuIdTextBox.Text;
                }
                else
                {
                    // NotificationQueue.Show(null, 2000);
                }

            }
            var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainInfo>>(content);
            trainInfos.Clear();
            foreach (var trainInfo in newTrainInfos)
            {
                trainInfos.Add(trainInfo);
            }
        }
        catch
        {
            // NotificationQueue.Show(null, 2000);
        }
    }
}
