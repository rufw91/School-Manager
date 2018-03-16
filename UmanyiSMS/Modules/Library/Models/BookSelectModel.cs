using System;
using System.Collections.Generic;
using UmanyiSMS.Modules.Library.Controller;

namespace UmanyiSMS.Modules.Library.Models
{
    public class BookSelectModel: BookModel
    {
        
        public BookSelectModel()
        {
            CheckErrors();
        }
        public override bool CheckErrors()
        {
            try
            {
                ClearAllErrors();
                if (BookID == 0)
                {
                    List<string> errors = new List<string>();
                    errors.Add("Book does not exist.");
                    SetErrors("BookID", errors);
                }
                else
                {
                    BookModel book = DataController.GetBook(BookID);
                    if (book.BookID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Book does not exist.");
                        SetErrors("BookID", errors);
                    }
                    else
                    {
                        ClearErrors("BookID");
                        this.Author = book.Author;
                        this.ISBN = book.ISBN;
                        this.Publisher = book.Publisher;
                        this.SPhoto = book.SPhoto;
                        this.Title = book.Title;
                    }
                }
            }
            catch (Exception e)
            {
                List<string> errors = new List<string>();
                errors.Add(e.Message);
                SetErrors("", errors);
            }
            NotifyPropertyChanged("HasErrors");
            return HasErrors;
        }
    }
}
