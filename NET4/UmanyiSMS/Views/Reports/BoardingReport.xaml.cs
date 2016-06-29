﻿using Helper.Controls;
using System;
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
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for BoardingReport.xaml
    /// </summary>
    public partial class BoardingReport : UserControl
    {
        public BoardingReport()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
            {
                BoardingReportVM srvm = DataContext as BoardingReportVM;
                if (srvm == null)
                    return;
                srvm.ShowFullPreviewAction = (d) =>
                {
                    CustomWindow w = new CustomWindow();
                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;

                    w.Content = new PrintDialog(d);
                    w.ShowDialog();
                };
            };
        }
    }
}
