using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace RailGo.Views;

public sealed partial class Train_NumberPage : Page
{
    public Train_NumberViewModel ViewModel
    {
        get;
    }

    public Train_NumberPage()
    {
        ViewModel = App.GetService<Train_NumberViewModel>();
        InitializeComponent();
    }
}
