using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Starehe.ViewModels
{
    public class ModifyBookVM: ViewModelBase
    {
        BookSelectorModel book;
        public ModifyBookVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "MODIFY BOOK";
        }

        protected override void CreateCommands()
        {
            /*SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;

                bool succ = await DataAccess.UpdateStaffAsync(newStaff);
                if (succ)
                {
                    MessageBox.Show("Succesfully updated staff.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this Time.", "", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                Reset();
                IsBusy = false;
            }, o => !IsBusy&&CanSave());
            ClearImageCommand = new RelayCommand(o => { newStaff.SPhoto = null; }, o => true);
            BrowseCommand = new RelayCommand(o => { newStaff.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);*/
        }

        private bool CanSave()
        {
            /*newStaff.CheckErrors();
            if(!newStaff.HasErrors)
                Role = UsersHelper.GetUserRole(newStaff.StaffID);
            return !newStaff.HasErrors && ValidateStaff();*/
            return false;
        }

        public override void Reset()
        {
            
        }
    }
}
