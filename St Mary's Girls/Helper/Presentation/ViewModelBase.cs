using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Helper
{
    public abstract class ViewModelBase : NotifiesPropertyChanged, IDisposable
    {
        string title;
        bool isBusy;
        public ViewModelBase()
        {
            Title = "";
            IsBusy = false;
        }
        public void Dispose()
        {
            Reset();
        }

        protected abstract void InitVars();

        protected abstract void CreateCommands();

        public abstract void Reset();

        public string Title
        {
            get { return title; }
            set
            {
                if (value != this.title)
                {
                    this.title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (value != this.isBusy)
                {
                    this.isBusy = value;
                    NotifyPropertyChanged("IsBusy");
                }
            }
        }

    }

}
