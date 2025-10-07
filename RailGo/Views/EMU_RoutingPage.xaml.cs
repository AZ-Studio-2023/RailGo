﻿using RailGo.Core.Models;
using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.UI.Windowing;
using CommunityToolkit.WinUI.Controls;

namespace RailGo.Views;

public sealed partial class EMU_RoutingPage : Page
{
    public EMU_RoutingViewModel ViewModel
    {
        get;
    }

    public EMU_RoutingPage()
    {
        ViewModel = App.GetService<EMU_RoutingViewModel>();
        InitializeComponent();
    }

    public EmuOperation _item;
    private void TrainNumberDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        // 显示车次Details

        TrainNumberTripDetailsPage page = new()
        {
            DataContext = new TrainPreselectResult { FullNumber = _item.TrainNo }
        };

        TabViewItem tabViewItem = new()
        {
            Header = _item.TrainNo,
            Content = page,
            CanDrag = true,
            IconSource = new FontIconSource() { Glyph = "\uE7C0" }
        };
        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
    }
    private void TrainEmuDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        // 显示车组Details

        EMU_RoutingDetailsPage page = new()
        {
            DataContext = _item
        };

        TabViewItem tabViewItem = new()
        {
            Header = _item.EmuNo,
            Content = page,
            CanDrag = true,
            IconSource = new FontIconSource() { Glyph = "\uEB4D" }
        };
        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
    }
}
