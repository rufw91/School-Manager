﻿using Helper.Controls;
using UmanyiSMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class LeavingCertificate : UserControl
    {
        public LeavingCertificate()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    LeavingCertificateVM nfpvm = DataContext as LeavingCertificateVM;
                    nfpvm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new PrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
