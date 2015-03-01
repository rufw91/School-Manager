﻿using Helper;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class BackupVM : ViewModelBase
    {
        private string pathToFile;
        public BackupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {

        }

        protected override void CreateCommands()
        {
            BrowseCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                PathToFile = FileHelper.SaveFileAsBak();
                IsBusy = false;
            }, o => !IsBusy);
            BackupCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccessHelper.CreateBackupAsync(pathToFile);
                if (succ)
                    MessageBox.Show("Successfully completed operation.");
                else
                    MessageBox.Show("Operation failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Reset();
                IsBusy = false;
            }, o => !IsBusy && !string.IsNullOrEmpty(pathToFile));
        }

        public string PathToFile
        {
            get { return pathToFile; }
            set
            {
                if (value != this.pathToFile)
                {
                    this.pathToFile = value;
                    NotifyPropertyChanged("PathToFile");
                }
            }
        }

        public ICommand BrowseCommand
        { get; private set; }

        public ICommand BackupCommand
        { get; private set; }

        public override void Reset()
        {
            PathToFile = "";
        }
    }
}