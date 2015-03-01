﻿using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class MainWindowVM : ViewModelBase    
    {
        ViewModelBase source;
        ObservableCollection<ViewModelBase> backJournal;
        bool navigatingBack;
        public MainWindowVM() 
        {
            InitVars();
            CreateCommands();            
        }

        public ViewModelBase Source
        {
            get { return this.source; }

            set
            {
                if (value != this.source)
                {                   
                    this.source = value;                    
                    NotifyPropertyChanged("Source");
                    if (!navigatingBack)
                        BackJournal.Add(source);
                }
            }
        }
        
        public ObservableCollection<ViewModelBase> BackJournal
        { get { return backJournal; } }

        protected override void InitVars()
        {
            navigatingBack = false;
            backJournal = new ObservableCollection<ViewModelBase>();
            Source = new HomePageVM();
            MessageBox.Show("This is a demo version of Starehe MS. Please contact your System Administrator for more information", "Info",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        protected override void CreateCommands()
        {
            AboutCommand = new RelayCommand(o =>
            {
                if (AboutAction != null)
                    AboutAction.Invoke();
            }, o => true);
            DisplayVATAnalysisCommand = new RelayCommand(o =>
            {
                this.Source = new VATAnalysisVM();
            }, o => true);
            DisplayNewVATRateCommand = new RelayCommand(o =>
            {
                this.Source = new NewVATRateVM();
            }, o => true);
            DisplayStudentDetailCommand = new RelayCommand(o =>
            {
                this.Source = new StudentDetailsVM((StudentModel)o);
            }, o => (o is StudentModel));

            DisplayStaffDetailCommand = new RelayCommand(o =>
            {
                this.Source = new StaffDetailsVM((StaffModel)o);
            }, o => (o is StudentModel));

            DisplayStudentsCommand = new RelayCommand(o =>
            {
                this.Source = new StudentsVM();
            }, o => true);

            DisplayStaffCommand = new RelayCommand(o =>
            {
                this.Source = new StaffVM();
            }, o => true);

            DisplayFeesCommand = new RelayCommand(o =>
            {
                this.Source = new FeesVM();
            }, o => true);

            DisplayInstitutionCommand = new RelayCommand(o =>
            {
                this.Source = new InstitutionVM();
            }, o => true);

            DisplayTimeTableCommand = new RelayCommand(o =>
            {
                this.Source = new TimeTableVM();
            }, o => true);

            DisplayExamsCommand = new RelayCommand(o =>
            {
                this.Source = new ExamsVM();
            }, o => true);

            DisplayEventsCommand = new RelayCommand(o =>
            {
                this.Source = new LibraryVM();
            }, o => true);

            DisplayGalleryCommand = new RelayCommand(o =>
            {
                this.Source = new GalleryVM();
            }, o => true);

            DisplaySettingsCommand = new RelayCommand(o =>
            {
                this.Source = new SettingsVM();
            }, o => true);

            DisplayInventoryMgmtCommand = new RelayCommand(o =>
            {
                this.Source = new InventoryMgmtVM();
            }, o => true);

            DisplayBoardingCommand = new RelayCommand(o =>
            {
                this.Source = new BoardingVM();
            }, o => true);

            DisplayPayrollCommand = new RelayCommand(o =>
            {
                this.Source = new PayrollVM();
            }, o => true);

            #region Inventory Mgmt
            DisplayNewItemCommand = new RelayCommand(o =>
            {
                this.Source = new NewItemVM();
            }, o => true);

            DisplayNewItemCategoryCommand = new RelayCommand(o =>
            {
                this.Source = new NewItemCategoryVM();
            }, o => true);

            DisplayReceiveItemsCommand = new RelayCommand(o =>
            {
                this.Source = new ReceiveItemsVM();
            }, o => true);

            DisplayItemListCommand = new RelayCommand(o =>
            {
                this.Source = new ItemListVM();
            }, o => true);

            DisplayModifyItemCommand = new RelayCommand(o =>
            {
                this.Source = new ModifyItemVM();
            }, o => true);

            DisplayItemReceiptHistoryCommand = new RelayCommand(o =>
            {
                this.Source = new ItemReceiptHistoryVM();
            }, o => true);

            DisplaySupplierListCommand = new RelayCommand(o =>
            {
                this.Source = new SupplierListVM();
            }, o => true);

            DisplayModifySupplierCommand = new RelayCommand(o =>
        {
            this.Source = new ModifySupplierVM();
        }, o => true);
            DisplayNewSupplierPaymentCommand = new RelayCommand(o =>
            {
                this.Source = new PaymentToSupplierVM();
            }, o => true);

            DisplayNewSupplierCommand = new RelayCommand(o =>
            {
                this.Source = new NewSupplierVM();
            }, o => true);

            DisplaySupplierPaymentsHistoryCommand = new RelayCommand(o =>
            {
                this.Source = new PaymentToSupplierHistoryVM();
            }, o => true);

            DisplayStockTakeResultsCommand = new RelayCommand(o =>
            {
                this.Source = new StockTakeResultsVM();
            }, o => true);

            DisplayNewStockTakingCommand = new RelayCommand(o =>
            {
                this.Source = new StockTakingVM();
            }, o => true);
            
            #endregion

            BackCommand = new RelayCommand(o =>
            {
                navigatingBack = true;
                int index = backJournal.Count - 2;
                backJournal.RemoveAt(index + 1);
                this.Source = backJournal[index];
                navigatingBack = false;
            }, o => CanGoBack());
        }

        private bool CanGoBack()
        {
            if (backJournal.Count < 1)
                return false;
            else return true;
        }

        #region Inventory Mgmt
        public ICommand DisplayVATAnalysisCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewVATRateCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStockTakeResultsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewStockTakingCommand
        {
            get;
            private set;
        }
            
        public ICommand DisplaySupplierPaymentsHistoryCommand
        {
            get;
            private set;
        }

        public ICommand DisplaySupplierListCommand
        {
            get;
            private set;
        }

        public ICommand DisplayModifySupplierCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewSupplierPaymentCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewSupplierCommand
        {
            get;
            private set;
        }

        public ICommand DisplayItemReceiptHistoryCommand
        {
            get;
            private set;
        }
        
        public ICommand DisplayReceiveItemsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewItemCommand
        {
            get;
            private set;
        }

        public ICommand DisplayItemListCommand
        {
            get;
            private set;
        }

        public ICommand DisplayNewItemCategoryCommand
        {
            get;
            private set;
        }

        public ICommand DisplayModifyItemCommand
        {
            get;
            private set;
        }
        #endregion

        public Action AboutAction
        {
            get;
            set;
        }

        public ICommand AboutCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStudentDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStaffDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStudentsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStaffCommand
        {
            get;
            private set;
        }

        public ICommand DisplayFeesCommand
        {
            get;
            private set;
        }

        public ICommand DisplayInstitutionCommand
        {
            get;
            private set;
        }
        
        public ICommand DisplayTimeTableCommand
        {
            get;
            private set;
        }

        public ICommand DisplayExamsCommand
        {
            get;
            private set;
        }
                
        public ICommand DisplayEventsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayGalleryCommand
        {
            get;
            private set;
        }

        public ICommand DisplaySettingsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayInventoryMgmtCommand
        {
            get;
            private set;
        }

        public ICommand DisplayBoardingCommand
        {
            get;
            private set;
        }

        public ICommand DisplayPayrollCommand
        {
            get;
            private set;
        }
        
        public ICommand BackCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }

    }
}