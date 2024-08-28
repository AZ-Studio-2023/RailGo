using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls;
using RailGo.Models;
using RailGo.Models;
using Newtonsoft.Json;
using Windows.System;

namespace RailGo.ViewModels;

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
    public string InputEmuID;
    public string url = "https://api.rail.re/emu/";
    public string XGZurl = "http://www.xiaguanzhan.com/";

    public EMU_RoutingViewModel()
    {
    }
    public async Task GetContent()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url + InputEmuID);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    XGZurl = "http://www.xiaguanzhan.com/soso.asp?keyword=" + InputEmuID;
                    Trace.WriteLine(InputEmuID);
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
