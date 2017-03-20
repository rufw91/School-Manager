﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Library.Models;

namespace UmanyiSMS.Modules.Library.Controller
{
    public class DataController
    {
        public static bool SearchAllBookProperties(BookModel book, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(book.ISBN, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(book.Title, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(book.Author, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(book.Publisher, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(book.Price.ToString(), searchText, RegexOptions.IgnoreCase).Success;
        }

        public static Task<ObservableCollection<BookModel>> GetAllBooksAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<BookModel>>(delegate
            {
                ObservableCollection<BookModel> observableCollection = new ObservableCollection<BookModel>();
                string commandText = "SELECT ISBN, Name,Author,Publisher,SPhoto,BookID,Price FROM [Book]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new BookModel
                    {
                        ISBN = dataRow[0].ToString(),
                        Title = dataRow[1].ToString(),
                        Author = dataRow[2].ToString(),
                        Publisher = dataRow[3].ToString(),
                        SPhoto = (dataRow[4] != null && !(dataRow[4] is DBNull)) ? ((byte[])dataRow[4]) : new byte[0],
                        BookID = int.Parse(dataRow[5].ToString()),
                        Price = decimal.Parse(dataRow[6].ToString())
                    });
                }
                return observableCollection;
            });
        }


        public static Task<bool> SaveNewBookAsync(BookModel book)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Book] (Name,ISBN,Author,Price,Publisher,SPhoto) VALUES('",
                    book.Title,
                    "','",
                    book.ISBN,
                    "','",
                    book.Author,
                    "',",
                    book.Price,
                    ",'",
                    book.Publisher,
                    "',@Photo)"
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new List<SqlParameter>
                {
                    new SqlParameter("@Photo", book.SPhoto)
                });
            });
        }


        public static Task<bool> SaveNewBookIssueAsync(BookIssueModel bim)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('dbo.BookIssueHeader') INSERT INTO [BookIssueHeader] (BookIssueID,StudentID,DateIssued) VALUES (@id,",
                    bim.StudentID,
                    ",'",
                    bim.DateIssued.ToString("g"),
                    "')\r\n"
                });
                foreach (BookModel current in bim.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [BookIssueDetail] (BookIssueID,BookID) VALUES (@id,",
                        current.BookID,
                        ")\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }


        public static Task<ObservableCollection<BookModel>> GetUnreturnedBooksAsync(int studenID)
        {
            return Task.Factory.StartNew<ObservableCollection<BookModel>>(delegate
            {
                ObservableCollection<BookModel> observableCollection = new ObservableCollection<BookModel>();
                string commandText = string.Concat(new object[]
                {
                    "SELECT x.BookID,b.ISBN,b.Name,b.Author,b.Publisher,b.SPhoto,b.Price FROM ((SELECT bid.BookID FROM [BookIssueDetail] bid INNER JOIN [BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE bih.StudentID=",
                    studenID,
                    " AND NOT EXISTS(SELECT brd.BookID FROM [BookReturnDetail] brd INNER JOIN [BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID AND brh.StudentID=",
                    studenID,
                    ")) x LEFT OUTER JOIN [Book] b ON (x.BookID=b.BookID))"
                });
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new BookModel
                    {
                        BookID = int.Parse(dataRow[0].ToString()),
                        ISBN = dataRow[1].ToString(),
                        Title = dataRow[2].ToString(),
                        Author = dataRow[3].ToString(),
                        Publisher = dataRow[4].ToString(),
                        SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                        Price = decimal.Parse(dataRow[6].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<UnreturnedBookModel>> GetUnreturnedBooksAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<UnreturnedBookModel>>(delegate
            {
                ObservableCollection<UnreturnedBookModel> observableCollection = new ObservableCollection<UnreturnedBookModel>();
                string commandText = "SELECT x.BookID,b.ISBN,b.Name,b.Author,b.Publisher,b.SPhoto,b.Price,dbo.GetUnreturnedCopies(x.BookID) FROM ((SELECT bid.BookID FROM [BookIssueDetail] bid INNER JOIN [BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE NOT EXISTS(SELECT brd.BookID FROM [BookReturnDetail] brd INNER JOIN [BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID)) x LEFT OUTER JOIN [Book] b ON (x.BookID=b.BookID))";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new UnreturnedBookModel
                    {
                        BookID = int.Parse(dataRow[0].ToString()),
                        ISBN = dataRow[1].ToString(),
                        Title = dataRow[2].ToString(),
                        Author = dataRow[3].ToString(),
                        Publisher = dataRow[4].ToString(),
                        SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                        Price = decimal.Parse(dataRow[6].ToString()),
                        UnreturnedCopies = decimal.Parse(dataRow[7].ToString())
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewBookReturnAsync(BookReturnModel bim)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\ndeclare @id int; SET @id = [dbo].GetNewID('dbo.BookReturnHeader') INSERT INTO [BookReturnHeader] (BookReturnID,StudentID,DateReturned) VALUES (@id,",
                    bim.StudentID,
                    ",'",
                    bim.DateReturned.ToString("g"),
                    "')\r\n"
                });
                foreach (BookModel current in bim.Entries)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [BookReturnDetail] (BookReturnID,BookID) VALUES (@id,",
                        current.BookID,
                        ")\r\n"
                    });
                }
                text += " COMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text);
            });
        }

        internal static BookModel GetBook(int bookID)
        {
            string commandText = "SELECT BookID,ISBN,Name,Author,Publisher,SPhoto,b.Price FROM [Book] WHERE BookID=" + bookID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            BookModel result;
            if (dataTable.Rows.Count == 0)
            {
                result = new BookModel();
            }
            else
            {
                BookModel bookModel = new BookModel();
                DataRow dataRow = dataTable.Rows[0];
                result = new BookModel
                {
                    BookID = int.Parse(dataRow[0].ToString()),
                    ISBN = dataRow[1].ToString(),
                    Title = dataRow[2].ToString(),
                    Author = dataRow[3].ToString(),
                    Publisher = dataRow[4].ToString(),
                    SPhoto = (dataRow[5] != null && !(dataRow[5] is DBNull)) ? ((byte[])dataRow[5]) : new byte[0],
                    Price = decimal.Parse(dataRow[6].ToString())
                };
            }
            return result;
        }

        public static Task<bool> UpdateBookAsync(BookSelectModel book)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Book] SET ISBN='",
                    book.ISBN,
                    "', Name='",
                    book.Title,
                    "', Author='",
                    book.Author,
                    "', Publisher='",
                    book.Publisher,
                    "', Price='",
                    book.Price,
                    "', SPhoto=@photo WHERE BookID=",
                    book.BookID
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new List<SqlParameter>
                {
                    new SqlParameter("@photo", book.SPhoto)
                });
            });
        }
    }
}
