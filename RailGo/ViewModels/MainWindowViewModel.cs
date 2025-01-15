using CommunityToolkit.Mvvm.ComponentModel;

namespace RailGo.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    public MainWindowViewModel()
    {
    }

    [ObservableProperty]
    public string taskIsInProgress;
}
