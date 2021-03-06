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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FluentApi.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            userControlEmployees.Content = new EmployeeUserControl();
            userControlTime.Content = new TimeUserControl();
            userControlTeams.Content = new TeamsUserControl();
            userControlProjects.Content = new ProjectsUserControl();
            userControlDetails.Content = new DetailsUserControl();
        }

        private void OnTabSelectedDetails(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                userControlDetails.Content = new DetailsUserControl();
            }
        }

        private void OnTabSelectedTime(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                userControlTime.Content = new TimeUserControl();
            }
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
