using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

using RailGo.Models;
using Newtonsoft.Json;

namespace RailGo.Views;

public sealed partial class TrainNumberTripDetailsPage : Page
{
    public TrainTripsInfo ViewModel => DataContext as TrainTripsInfo;
    public string url = "https://rail.moefactory.com/api/trainNumber/query";
    public string train_no;
    public string date;
    public TrainDetail realdata;
    public bool isload = false;

    public TrainNumberTripDetailsPage()
    {
        this.Loaded += GetImformation;
    }

    private void GetImformation(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Trace.WriteLine("山东省实验中学");
        Trace.WriteLine(ViewModel.emu_no);
        Trace.WriteLine(ViewModel.train_no);
        Trace.WriteLine(ViewModel.date.ToString("yyyyMMdd"));
        train_no = ViewModel.train_no;
        date = ViewModel.date.ToString("yyyyMMdd");

        var httpClient = new HttpClient();
        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "date",date },
            {"trainNumber",train_no },

        });
        var response = httpClient.PostAsync(new Uri(url), body).Result;
        var data = response.Content.ReadAsStringAsync().Result;

        Trace.WriteLine(data);
        //var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainNumberOutput>>(data);
        TrainNumberTripDetailsModel trainInfo = JsonConvert.DeserializeObject<TrainNumberTripDetailsModel>(data);
        Trace.WriteLine(trainInfo.Data.DataList[0].TrainIndex);
        realdata = trainInfo.Data.DataList[0];
        InitializeComponent();
        this.Loaded -= GetImformation;
    }
    
}
