using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper.Models
{
    public class BookModel:ModelBase
    {
        int bookID;
        string title;
        string isbn;
        string author;
        byte[] sPhoto;
        private string publisher;
        private decimal price;

        public BookModel()
        {
            BookID = 0;
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = SetNoImage();
            Publisher = "";
            Price = 0;
        }

        private byte[] SetNoImage()
        {
            byte[] b = new byte[0];
            try
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    Application.GetResourceStream(new Uri("pack://application:,,,/Resources/noImage.png")).Stream.CopyTo(mem, 1024);                    
                    mem.Seek(0, System.IO.SeekOrigin.Begin);
                    b = mem.ToArray();
                    mem.Dispose();
                }
            }
            catch { }
            return b;
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

        public decimal Price
        {
            get { return this.price; }

            set
            {
                if (value != this.price)
                {
                    this.price = value;
                    NotifyPropertyChanged("Price");
                }
            }
        }
        public override void Reset()
        {
            BookID = 0;
            Title = "";
            Author = "";
            ISBN = "";
            SPhoto = SetNoImage();
            Publisher = "";
            Price = 0;
        }

        
    }
}
