﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using RailGo.Activation;
using RailGo.Contracts.Services;
using RailGo.Core.Contracts.Services;
using RailGo.Core.Services;
using RailGo.Helpers;
using RailGo.Core.Models;
using RailGo.Notifications;
using RailGo.Services;
using RailGo.ViewModels;
using RailGo.Views;
using System.Collections.ObjectModel;
using Windows.Storage;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;

namespace RailGo;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<EMU_RoutingDetailsPage>();
            services.AddTransient<StationDetailsViewModel>();
            services.AddTransient<StationDetailsPage>();
            services.AddTransient<TrainNumberTripDetailsPage>();
            services.AddTransient<TrainNumberTripDetailsViewModel>();
            services.AddTransient<Ticket_GenerateViewModel>();
            services.AddTransient<Ticket_GeneratePage>();
            services.AddTransient<StationToStationViewModel>();
            services.AddTransient<StationToStationPage>();
            services.AddTransient<Station_InformationViewModel>();
            services.AddTransient<Station_InformationPage>();
            services.AddTransient<Train_NumberViewModel>();
            services.AddTransient<Train_NumberPage>();
            services.AddTransient<EMU_RoutingViewModel>();
            services.AddTransient<EMU_RoutingPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }
    
    public static class Global
    {
        public static ObservableCollection<StationPreselectResult> StationsJson;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/stations.json"));
        var json = await FileIO.ReadTextAsync(file);
        var StationJson = JsonConvert.DeserializeObject<ObservableCollection<StationPreselectResult>>(json);
        App.Global.StationsJson = StationJson;

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
