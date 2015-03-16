using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
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
                    BookModel book = DataAccess.GetBook(BookID);
                    if (book.BookID == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Book does not exist.");
                        SetErrors("BookID", errors);
                    }
                    else
                    {
                        ClearErrors("BookID");
                        this.BookID = book.BookID;
                        this.Author = book.Author;
                        this.ISBN = book.ISBN;
                        this.Price = book.Price;
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
