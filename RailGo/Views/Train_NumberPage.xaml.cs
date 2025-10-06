﻿using RailGo.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using RailGo.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RailGo.Views;

public sealed partial class Train_NumberPage : Page
{
    public Train_NumberViewModel ViewModel
    {
        get;
    }

    public TrainPreselectResult _item;

    public Train_NumberPage()
    {
        ViewModel = App.GetService<Train_NumberViewModel>();
        InitializeComponent();
    }

    private void GetTrainNumberBtn_Click(object sender, RoutedEventArgs e)
    {
        if (TrainNumberTextBox.Text != null)
        {
            ViewModel.GettrainNumberTripsInfosContent();
        }

    }
    private void TrainNumberDetailsBtn_Click(object sender, RoutedEventArgs e)
    {
        TrainNumberTripDetailsPage page = new()
        {
            DataContext = _item
        };

        TabViewItem tabViewItem = new()
        {
            Header = _item.Number,
            Content = page,
            CanDrag = true,
            IconSource = new FontIconSource() { Glyph = "\uE7C0" }
        };
        MainWindow.Instance.MainTabView.TabItems.Add(tabViewItem);
        MainWindow.Instance.MainTabView.SelectedItem = tabViewItem;
    }
}
