using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class BookModel:ModelBase
    {
        string title;
        string isbn;
        string author;
        byte[] sPhoto;

        public BookModel()
        {
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = null;
        }
        public string Title
        {
            get { return this.title; }

            set
            {
                if (value != this.title)
                {
                    this.title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Author
        {
            get { return this.author; }

            set
            {
                if (value != this.author)
                {
                    this.author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

        public string ISBN
        {
            get { return this.isbn; }

            set
            {
                if (value != this.isbn)
                {
                    this.isbn = value;
                    NotifyPropertyChanged("ISBN");
                }
            }
        }

        public byte[] SPhoto
        {
            get { return this.sPhoto; }

            set
            {
                if (value != this.sPhoto)
                {
                    this.sPhoto = value;
                    NotifyPropertyChanged("SPhoto");
                }
            }
        }
        public override void Reset()
        {
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = null;
        }
    }
}
