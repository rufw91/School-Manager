﻿
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Library.Models
{
    public class BookModel:ModelBase
    {
        int bookID;
        string title;
        string isbn;
        string author;
        byte[] sPhoto;
        private string publisher;

        public BookModel()
        {
            BookID = 0;
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = new byte[0];
            Publisher = "";
        }

        public BookModel(BookModel book)
        {
            BookID = book.bookID;
            Title = book.title;
            Author = book.author;
            ISBN = book.isbn;
            SPhoto = book.sPhoto;
            Publisher = book.publisher;
        }
        
        public int BookID
        {
            get { return this.bookID; }

            set
            {
                if (value != this.bookID)
                {
                    this.bookID = value;
                    NotifyPropertyChanged("BookID");
                }
            }
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
        public string Publisher
        {
            get { return this.publisher; }

            set
            {
                if (value != this.publisher)
                {
                    this.publisher = value;
                    NotifyPropertyChanged("Publisher");
                }
            }
        }

        public override void Reset()
        {
            BookID = 0;
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = new byte[0];
            Publisher = "";
        }

        
    }
}
