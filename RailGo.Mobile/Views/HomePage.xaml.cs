namespace RailGo.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomeViewModel ViewModel
    {
        get;
    }
    public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
