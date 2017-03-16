using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;

namespace Helper
{
    public class StartUpModel
    {
        ObservableCollection<Error> errors;
        public StartUpModel()
        {
            errors = new ObservableCollection<Error>();
            IsSQLInstalled = false;
            IsSQLInstanceInstalled = false;
            IsDefaultLoginOK = false;
            IsDbOK = false;
            IsDbObjectsOK = false;
        }

        public bool IsSQLInstalled
        { get; set; }

        public bool IsSQLInstanceInstalled
        { get; set; }

        public bool IsDefaultLoginOK
        { get; set; }

        public bool IsDbOK
        { get; set; }

        public bool IsDbObjectsOK
        { get; set; }

        public bool IsOK
        {
            get { return GetIsOK(); }
        }

        public ObservableCollection<Error> Errors
        {
            get { GetErrors(); return errors; }
        }

        private void GetErrors()
        {
            errors.Clear();
            if (!IsSQLInstalled)
                errors.Add(new Error("0x0001", "SQL Server is not Installed."));
            if (!IsSQLInstanceInstalled)
                errors.Add(new Error("0x0002", "Required SQL Server Instance (Umanyi) does not exist."));
            if (!IsDefaultLoginOK)
                errors.Add(new Error("0x0003", "Error logging in with default login."));
            if (!IsDbOK)
                errors.Add(new Error("0x0004", "The database [UmanyiSMS] does not exist."));
            if (!IsDbObjectsOK)
                errors.Add(new Error("0x0005", "Database objects are not properly created."));
        }

        private bool GetIsOK()
        {
            return IsSQLInstalled && IsSQLInstanceInstalled && IsDefaultLoginOK && IsDbOK && IsDbObjectsOK;
        }

    }

    public class Error : ModelBase
    {
        string _message;
        string _title;
        public Error()
        {
            Title = "";
            Message = "";
        }
        public Error(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public string Title
        {
            get { return _title; }
            private set
            {
                if (value != this._title)
                {
                    this._title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Message
        {
            get { return _message; }
            private set
            {
                if (value != this._message)
                {
                    this._message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public override void Reset()
        {
        }
    }
}
