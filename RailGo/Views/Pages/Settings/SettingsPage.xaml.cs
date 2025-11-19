using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RailGo.ViewModels.Pages.Settings;

namespace RailGo.Views.Pages.Settings;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        this.Loaded += OnLoad;
    }

    public void OnLoad(object sender, RoutedEventArgs e)
    {
        InitializeThemeComboBox();
    }

    private void InitializeThemeComboBox()
    {
        if (ViewModel?.ElementTheme != null)
        {
            var theme = ViewModel.ElementTheme.ToString();

            foreach (ComboBoxItem item in ThemeComboBox.Items)
            {
                if (item.Tag?.ToString() == theme)
                {
                    ThemeComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var themeString = selectedItem.Tag?.ToString();

            if (Enum.TryParse<ElementTheme>(themeString, out var theme))
            {
                if (ViewModel?.ElementTheme != theme)
                {
                    ViewModel.SwitchThemeCommand.Execute(theme);
                }
            }
        }
    }
}