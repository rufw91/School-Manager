﻿
using System.Windows;
using System.Windows.Controls;

using System.Data.SqlClient;
using UmanyiSMS.Modules.System.ViewModels;
using UmanyiSMS.Lib.Controls;

namespace UmanyiSMS.Views
{
    public partial class AdvancedSettings : UserControl
    {
        public AdvancedSettings()
        {
            Window w = null;
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                AdvancedSettingsVM dbtvm = this.DataContext as AdvancedSettingsVM;
                if (dbtvm == null)
                    return;
                dbtvm.OpenTaskWindowAction = (p) =>
                {
                    w = new CustomWindow();

                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;


                    w.Content = p;
                    w.ShowDialog();
                };
                dbtvm.OpenTaskWindowAction2 = (p) =>
                {
                    w = new CustomWindow();

                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;


                    w.Content = p;
                    w.Show();
                };
            };
        }

        
    }
}
