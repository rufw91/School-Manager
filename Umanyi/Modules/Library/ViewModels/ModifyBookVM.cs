

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Library.Controller;
using UmanyiSMS.Modules.Library.Models;

namespace UmanyiSMS.Modules.Library.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ModifyBookVM : ViewModelBase
    {
        BookSelectModel book;
        public ModifyBookVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "MODIFY BOOK";
            book = new BookSelectModel();
            book.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "BookID")
                    book.CheckErrors();
            };
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataController.UpdateBookAsync(book);
                if (succ)
                {
                    MessageBox.Show("Succesfully saved details.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this time.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                IsBusy = false;
            }, o => !IsBusy && CanSave());
            BrowseCommand = new RelayCommand(o => { book.SPhoto = FileHelper.BrowseImageAsByteArray(); }, o => true);
        }

        public BookModel Book
        {
            get { return book; }
        }

        private bool CanSave()
        {
            return !book.HasErrors&&(!string.IsNullOrEmpty(book.Author)) && (!string.IsNullOrEmpty(book.ISBN)) && (!string.IsNullOrEmpty(book.Title));
        }

        public ICommand BrowseCommand
        {
            get;
            private set;
        }
        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
            book.Reset();
        }
    }
}
