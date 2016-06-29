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
    public abstract class ModelBase : NotifiesPropertyChanged, IDataErrorInfo
    {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        
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
        }

        protected void ClearErrors(string propertyName)
        {
            errors.Remove(propertyName);
        }

        protected void ClearAllErrors()
        {
            errors.Clear();
        }

        private string GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            else
            {
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName].Count == 0 ? "" : errors[propertyName][0]);
                }
                else
                {
                    return null;
                }
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get { return GetErrors(columnName); }
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
