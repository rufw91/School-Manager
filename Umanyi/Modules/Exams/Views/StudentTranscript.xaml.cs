﻿

using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Exams.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Exams.Views
{
    public partial class StudentTranscript : UserControl
    {
        public StudentTranscript()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    StudentReportFormVM nfpvm = DataContext as StudentReportFormVM;
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