using System;
using System.Collections.Immutable;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
    public class LogWindowVM : ViewModelBase
    {
        public LogWindowVM()
        {
            InitVars();
            CreateCommands();
        }
        public override void Reset()
        {
        }

        protected override void CreateCommands()
        {
            CloseCommand = new RelayCommand(o => 
            {
                if (CloseAction != null)
                    CloseAction.Invoke();

            }, o => true);
        }

        protected override void InitVars()
        {
        }

        public ICommand CloseCommand
        { get; private set; }

        public Action CloseAction
        { get; set; }

        public IImmutableList<string>Entries
        { get { return ((App)Application.Current).LogEntries; } }
    }
}
