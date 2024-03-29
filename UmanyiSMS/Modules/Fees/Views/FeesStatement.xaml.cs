﻿

using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Fees.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Fees.Views
{
    public partial class FeesStatement : UserControl
    {
        public FeesStatement()
        {
            InitializeComponent();
            viewer.FitToWidth();

            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    FeesStatementVM nfpvm = DataContext as FeesStatementVM;
                    nfpvm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new MyPrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
