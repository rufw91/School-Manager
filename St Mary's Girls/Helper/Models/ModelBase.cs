using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Helper
{
    public enum ErrorCheckingStatus
    { Incomplete, Complete }
    public abstract class ModelBase : NotifiesPropertyChanged, INotifyDataErrorInfo
    {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Helper.ErrorCheckingStatus errorCheckingStatus;


        public ModelBase()
        {
            this.ErrorCheckingStatus = ErrorCheckingStatus.Complete;
        }

        public ErrorCheckingStatus ErrorCheckingStatus
        {
            get { return this.errorCheckingStatus; }

            set
            {
                if (value != this.errorCheckingStatus)
                {
                    this.errorCheckingStatus = value;
                    NotifyPropertyChanged("ErrorCheckingStatus");
                }
            }
        }

        public abstract void Reset();

        public virtual bool CheckErrors()
        {
             ClearAllErrors();
             return false;
        }

        public void SetErrors(string propertyName, List<string> propertyErrors)
        {
            errors.Remove(propertyName);
            errors.Add(propertyName, propertyErrors);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors(string propertyName)
        {
            errors.Remove(propertyName);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearAllErrors()
        {
            errors.Clear();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return (errors.Values);
            }
            else
            {
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool HasErrors
        {
            get
            {
                return (errors.Count > 0);
            }
        }
    }

}
