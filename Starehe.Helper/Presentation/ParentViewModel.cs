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
private  ViewModelBase activeView;
        public ParentViewModel()
        {
            menuItems = new ObservableCollection<ViewModelBase>();
            NotifyPropertyChanged("MenuItems");
            menuItems.CollectionChanged += (o, e) =>
            {
                if (menuItems.Count > 0)
                    ActiveView = menuItems[0];
            };
        }
        protected override void InitVars()
        {
           
        }

        public ViewModelBase ActiveView
        {
            get
            {
                if (activeView != null)
                    return (ViewModelBase)Activator.CreateInstance(activeView.GetType());
                else return activeView;
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

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
