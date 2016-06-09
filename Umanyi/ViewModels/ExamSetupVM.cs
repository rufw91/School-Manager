using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class ExamSetupVM:ViewModelBase
    {
        ExamSettingsModel settings;
        public ExamSetupVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "EXAMS & GRADES SETUP";
            settings = new ExamSettingsModel();
            settings.CopyFrom(App.AppExamSettings);
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewExamSettingsAsync(settings);
                App.AppExamSettings.CopyFrom(settings);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
            }, o => true);
        }

        public ExamSettingsModel Settings
        {
            get { return settings; }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }
    }
}
