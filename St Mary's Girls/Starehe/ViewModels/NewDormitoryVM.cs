using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class NewDormitoryVM:ViewModelBase
    {
        private DormModel newDormitory;
        public NewDormitoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "NEW DORMITORY";
            NewDormitory = new DormModel();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewDormitory(newDormitory);
                if (succ)
                {
                    MessageBox.Show("Succefully saved details", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(newDormitory.NameOfDormitory);
        }

        public DormModel NewDormitory
        {
            get { return this.newDormitory; }

            set
            {
                if (value != this.newDormitory)
                {
                    this.newDormitory = value;
                    NotifyPropertyChanged("NewDormitory");
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
            NewDormitory.Reset();
        }
    }
}
