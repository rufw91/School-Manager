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
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewBookVM: ViewModelBase
    {
        BookModel book;
        public NewBookVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "NEW BOOK";
            book = new BookModel();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewBookAsync(book);
                if (succ)
                {
                    MessageBox.Show("Succesfully saved details.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("An error occured. Could not save details at this time.","Error", MessageBoxButton.OK,
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
            return (!string.IsNullOrEmpty(book.Author)) && (!string.IsNullOrEmpty(book.ISBN)) && (!string.IsNullOrEmpty(book.Title));
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
