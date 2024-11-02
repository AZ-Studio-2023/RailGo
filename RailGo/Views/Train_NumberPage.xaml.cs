using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using RailGo.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RailGo.Views;

public sealed partial class Train_NumberPage : Page
{
    public Train_NumberViewModel ViewModel
    {
        get;
    }

    public TrainTripsInfo item;

    public Train_NumberPage()
    {
        ViewModel = App.GetService<Train_NumberViewModel>();
        InitializeComponent();
    }

    private void GetTrainNumberBtn_Click(object sender, RoutedEventArgs e)
    {
        if (TrainNumberTextBox.Text != null)
        {
            // url = "https://api.rail.re/emu/" + EmuIdTextBox.Text;
            ViewModel.GettrainNumberTripsInfosContent();
        }

    }
    private void TrainNumberDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        // 显示Details
        // 使用绑定
        var _item = DataGridInTrainNumber.SelectedItem as TrainTripsInfo;

        TrainNumberTripDetailsPage page = new()
        {
            DataContext = _item
        };
        Trace.WriteLine(_item.emu_no.ToString());
        //在这里准备好数据，到时候直接绑定即可
        //Trace.WriteLine(item.emu_no.ToString());
        TabViewItem tabViewItem = new()
        {
            Header = _item.train_no,
            Content = page,
            CanDrag = true,
            IconSource = new BitmapIconSource() { UriSource = new System.Uri("ms-appx:///Assets/StoreLogo.png") }
        };
        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
        //WindowEx NewWindow = new WindowEx();
        //NewWindow.Content = page;
        //NewWindow.Activate();
    }
}
