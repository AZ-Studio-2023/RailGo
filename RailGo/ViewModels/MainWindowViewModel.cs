using CommunityToolkit.Mvvm.ComponentModel;

namespace RailGo.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    public MainWindowViewModel()
    {
    }

    [ObservableProperty]
    public string taskIsInProgress;

    [ObservableProperty]
    public bool ifShowErrorInfoBarOpen;

    [ObservableProperty]
    public string showErrorInfoBarTitle;

    [ObservableProperty]
    public string showErrorInfoBarContent;
}
