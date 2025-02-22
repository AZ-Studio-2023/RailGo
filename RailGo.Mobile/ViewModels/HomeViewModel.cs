using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RailGo.Core.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace RailGo.Mobile.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        // Observable properties for x:Bind
        [ObservableProperty]
        public ObservableCollection<TrainTripsInfo> trainNumberEmuInfos;

        [ObservableProperty]
        public string inputEmuID;

        [ObservableProperty]
        public bool showErrorInfoBar;

        [ObservableProperty]
        public string errorMessage;

        // Command for Get EMU
        public IAsyncRelayCommand GetEmuCommand
        {
            get;
        }

        public HomeViewModel()
        {
            // Initialize the command
            GetEmuCommand = new AsyncRelayCommand(GettrainNumberEmuInfosContent);
        }

        public async Task GettrainNumberEmuInfosContent()
        {
            try
            {
                var httpClient = new HttpClient();
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rail.re/emu/" + InputEmuID);
                var response = await httpClient.SendAsync(requestMessage);
                var data = await response.Content.ReadAsStringAsync();
                var newTrainInfos = JsonConvert.DeserializeObject<ObservableCollection<TrainTripsInfo>>(data);
                TrainNumberEmuInfos = newTrainInfos;
                Trace.WriteLine(TrainNumberEmuInfos.Count);
            }
            catch (Exception ex)
            {
                ShowErrorInfoBar = true;
                ErrorMessage = ex.Message;
            }
        }
    }
}
