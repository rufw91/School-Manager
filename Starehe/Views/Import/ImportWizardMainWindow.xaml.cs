using Helper.Controls;
using System;
using System.Windows;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class ImportWizardMainWindow : CustomWindow
    {
        public ImportWizardMainWindow()
        {
            InitializeComponent();
            DataContext = new ImportWizardMainWindowVM();
            ImportWizardMainWindowVM iwmwvm = DataContext as ImportWizardMainWindowVM;

            iwmwvm.CloseAction = new Action(() => 
            {
                if (!iwmwvm.IsBusy)
                    this.Close();
                else
                    MessageBox.Show("Please wait while the wizard completes the current action.",
                        "Please wait...", MessageBoxButton.OK, MessageBoxImage.Warning);
            });

            iwmwvm.ShowErrorsAction = (errors) =>
            {
                new ShowErrors(errors).Show();
            };

            this.Closing += (o, e) =>
                {
                    if (iwmwvm.IsBusy)
                    {                        
                        MessageBox.Show("Please wait while the current operation is completed.", "Busy",
                             MessageBoxButton.OK, MessageBoxImage.Information);
                        e.Cancel = true;
                    }
                };
        }
    }
}
