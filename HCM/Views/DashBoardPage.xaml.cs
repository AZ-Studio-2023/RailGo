using HCM.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace HCM.Views;

public sealed partial class DashBoardPage : Page
{
    public DashBoardViewModel ViewModel
    {
        get;
    }

    public DashBoardPage()
    {
        ViewModel = App.GetService<DashBoardViewModel>();
        InitializeComponent();
    }
}
