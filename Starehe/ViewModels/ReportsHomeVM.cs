using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ReportsHomeVM: ViewModelBase
    {
        public ReportsHomeVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "REPORT BUILDER";
        }

        protected override void CreateCommands()
        {
            OpenReportBuilderCommand = new RelayCommand(o =>
            {
                try
                {
                    string s = GetReportBuilderPath();
                    if (!string.IsNullOrWhiteSpace(s))
                        Process.Start(s);
                    else
                        MessageBox.Show("Report builder 3.0 could not be found on your system. \r\nPlease contact your system administrator for more info.",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                catch { }
            }, o => true);
        }

        private string GetReportBuilderPath()
        {
            string path;
            if (Environment.Is64BitOperatingSystem)
            {
                 path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Microsoft SQL Server\Report Builder 3.0\MSReportBuilder.exe";
                 if (File.Exists(path))
                     return path;
                 else return "";
            }
            else
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Microsoft SQL Server\Report Builder 3.0\MSReportBuilder.exe";
                if (File.Exists(path))
                    return path;
                else return "";
            }
        }

        public ICommand OpenReportBuilderCommand
        { get; private set; }

        public override void Reset()
        {
           
        }
    }
}
