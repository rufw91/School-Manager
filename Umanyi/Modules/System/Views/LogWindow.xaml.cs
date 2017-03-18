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
using UmanyiSMS.Modules.System.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : UserControl
    {
        public LogWindow()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
              {
                  if (DataContext!=null)
                  {
                      LogWindowVM lwvm = DataContext as LogWindowVM;
                      lwvm.CloseAction = () =>
                      {
                          Window.GetWindow(this).Close();
                      };
                  }
              };
        }
    }
}
