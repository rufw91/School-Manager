using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class ParentViewModel: ViewModelBase
    {
        ObservableCollection<ViewModelBase> menuItems;
        ObservableCollection<ViewModelBase> hiddenMenuItems;
        private  ViewModelBase activeView;
        private bool persistViews;
        public ParentViewModel()
        {
            menuItems = new ObservableCollection<ViewModelBase>();
            hiddenMenuItems = new ObservableCollection<ViewModelBase>();
            NotifyPropertyChanged("MenuItems");
            menuItems.CollectionChanged += (o, e) =>
            {
                if (menuItems.Count > 0)
                    ActiveView = menuItems[0];
            };
            PersistViews = false;
        }
        protected override void InitVars()
        {
           
        }

        public ViewModelBase ActiveView
        {
            get
            {
                if (persistViews)
                    return activeView;
                else return (ViewModelBase)Activator.CreateInstance(activeView.GetType());
                 
            }
            set
            {
                if (activeView != value)
                {
                    if (activeView!=null)
                    activeView.IsActive = false;
                    activeView = value;
                    if (activeView != null)
                    activeView.IsActive = true;
                    NotifyPropertyChanged("ActiveView");
                }
            }
        }

        public bool PersistViews
        {
            get
            {
                return persistViews;
            }
            set
            {
                if (persistViews != value)
                {
                    persistViews = value;
                    NotifyPropertyChanged("PersistViews");
                }
            }
        }

        public ReadOnlyObservableCollection<ViewModelBase> MenuItems
        {
            get { return new ReadOnlyObservableCollection<ViewModelBase>(menuItems); }
        }

        public void TryAddChild(Type typeOfViewModel)
        {
            try
            {
                var temp=Activator.CreateInstance(typeOfViewModel) as ViewModelBase;
                menuItems.Add(temp);
            }
            catch { }
            NotifyPropertyChanged("MenuItems");
        }

        public void TryAddChild(Type typeOfViewModel,bool isVisible)
        {
            try
            {
                var temp = Activator.CreateInstance(typeOfViewModel) as ViewModelBase;
                temp.IsVisible = IsVisible;
                if (isVisible)
                    menuItems.Add(temp);                
                else
                    hiddenMenuItems.Add(temp);
            }
            catch { }
            NotifyPropertyChanged("MenuItems");
        }

        private void TryAddChild2(Type typeOfViewModel)
        {
            try
            {
                var temp = Activator.CreateInstance(typeOfViewModel) as ViewModelBase;
                temp.IsVisible = false;
                    menuItems.Add(temp);
            }
            catch { }
            NotifyPropertyChanged("MenuItems");
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }

        public void ToggleMenuVisibility()
        {

            if (menuItems.Any(c => c.IsVisible == false))
            {
                for (int i = menuItems.Count-1; i >=0; i--)
                {
                    if (!menuItems[i].IsVisible)
                        menuItems.RemoveAt(i);
                }
                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (!menuItems[i].IsVisible)
                        menuItems.RemoveAt(i);
                }
            }
            else {
                for (int i = 0; i < hiddenMenuItems.Count; i++)
                    TryAddChild2(hiddenMenuItems[i].GetType());
            }
            
        }
    }
}
