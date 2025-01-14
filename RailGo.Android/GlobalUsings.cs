global using System.Collections.Immutable;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using RailGo.Android.Models;
global using RailGo.Android.Presentation;
global using RailGo.Android.DataContracts;
global using RailGo.Android.DataContracts.Serialization;
global using RailGo.Android.Services.Caching;
global using RailGo.Android.Services.Endpoints;
#if MAUI_EMBEDDING
global using RailGo.MauiControls;
#endif
global using ApplicationExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;
