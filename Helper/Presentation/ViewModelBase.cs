using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Helper
{
    public abstract class ViewModelBase : NotifiesPropertyChanged
    {
        string title;
        bool isBusy;
        private bool isActive;
        private bool isVisible;
        private ViewModelBase parent;
        public ViewModelBase()
        {
            Title = "";
            IsBusy = false;
            IsActive = false;
            IsVisible = true;
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

        public ViewModelBase Parent
        {
            get { return parent; }
            set
            {
                if (value != this.parent)
                {
                    this.parent = value;
                    NotifyPropertyChanged("Parent");
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
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value != this.isVisible)
                {
                    this.isVisible = value;
                    NotifyPropertyChanged("IsVisible");
                }
            }
        }
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }
    }

}
