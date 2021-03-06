﻿
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.MySystem.Views;
using UmanyiSMS.Modules.Purchases.ViewModels;

namespace UmanyiSMS.Modules.Purchases.Views
{
    /// <summary>
    /// Interaction logic for PaymentToSupplier.xaml
    /// </summary>
    public partial class PaymentToSupplier : UserControl
    {
        public PaymentToSupplier()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    PaymentToSupplierVM nfpvm = DataContext as PaymentToSupplierVM;
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
