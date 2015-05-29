using Helper;
using Helper.Controls;
using Helper.Converters;
using Microsoft.Win32;
using OpenXmlPackaging;
using UmanyiSMS.ViewModels;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.Presentation
{
    public static class CommonCommands
    {
        static CommonCommands()
        {
            ExportToPDFCommand = new RelayCommand(o =>
            {
                if ((o == null) || (o == DependencyProperty.UnsetValue))
                {
                    MessageBox.Show("Cannot export empty dataset.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string s="";

                if (o.GetType()==typeof(FixedDocument))
                 s = XPSHelper.CreatePDFromFixedDocument((FixedDocument)o);

                if (o.GetType() == typeof(DataGrid))
                    s = XPSHelper.CreatePDFFromDataGrid((DataGrid)o);
                if (string.IsNullOrWhiteSpace(s))
                {
                    MessageBox.Show("Unable to obtain export data.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                CustomWindow w = new CustomWindow();
                w.MinHeight = 610;
                w.MinWidth = 810;
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                w.WindowState = WindowState.Maximized;

                w.Content = new PDFViewVM(s);
                w.ShowDialog();
            }, o => true);

            ExportToExcelCommand = new RelayCommand(o =>
            {
                if ((o == null) || (o == DependencyProperty.UnsetValue))
                {
                    MessageBox.Show("Cannot export empty dataset.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string s = "";

                if ((o.GetType() != typeof(DataGrid))&&(o.GetType()!=typeof(DataTable)))
                {
                    MessageBox.Show("Only data tables can be exported to Excel.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (o.GetType() == typeof(DataGrid))
                    s = ExportDataGrid((DataGrid)o);
                if (o.GetType() == typeof(DataTable))
                    s = ExportTable((DataTable)o);
                if (string.IsNullOrWhiteSpace(s))
                {
                    MessageBox.Show("Unable to export data.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    Process.Start(s);
                }
                catch { }
                
            }, o => true);
        }
        public static ICommand ExportToPDFCommand
        { get; private set; }

        public static ICommand ExportToExcelCommand
        { get; private set; }

        public static string ExportTable(DataTable Items)
        {
            if (Items.Rows.Count == 0)
                return "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select location to save excel file";
            saveDialog.Filter = "Excel Files|*.xlsx";
            if ((saveDialog.ShowDialog() == true) &&
                (new System.IO.FileInfo(saveDialog.FileName).Extension.ToLower() == ".xlsx"))
            {
                string name = saveDialog.FileName;
                try
                {
                    using (var doc = new SpreadsheetDocument(name))
                    {
                        Worksheet sheet1 = doc.Worksheets.Add("Worksheet1");
                        sheet1.ImportDataTable(Items, "A1", true);
                        sheet1.AutoFitColumns();
                        return name;
                    }
                }
                catch { }
                
            }
            return "";
        }

        public static string ExportDataGrid(DataGrid dataGrid)
        {
            try
            {
                var d = DataGridToDataTable.Convert(dataGrid);
                return ExportTable(d);
            }
            catch { }
            return "";
        }
    }
}
