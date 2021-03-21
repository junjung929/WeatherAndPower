﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.ViewModels.AddWindow;

namespace WeatherAndPower.UI.Views
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow(PlaceholderViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new MainViewModel(viewModel);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel) DataContext; }
        }
    }
}