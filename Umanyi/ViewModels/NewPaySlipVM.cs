﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class NewPaySlipVM:ViewModelBase
    {
        PayslipModel newSlip;
        private FeesStructureEntryModel newEntry;
private  FixedDocument fd;
private int selectedYear;
private string selectedMonth;
        public NewPaySlipVM()
        {
           
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "NEW PAYSLIP";
            
            NewSlip = new PayslipModel();
            

            NewEntry = new FeesStructureEntryModel();
            newSlip.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName != "StaffID")
                    return;
                if (newSlip.StaffID > 0)
                    newSlip.CheckErrors();
            };
            PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "SelectedMonth" || e.PropertyName == "SelectedYear")
                        newSlip.PaymentPeriod = selectedMonth + " " + selectedYear;
                };
            SelectedMonth = DateTime.Now.ToString("MMMM");
            SelectedYear = DateTime.Now.Year;
        }

        public List<string> MonthsOfTheYear
        {
            get { return new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }; }
        }

        public List<int> FutureYears
        {
            get
            {
                List<int> temp = new List<int>();
                for (int i = 2015; i < 2025; i++)
                    temp.Add(i);
                 return temp;
            }
        }

        protected override void CreateCommands()
        {
            AddEntryCommand = new RelayCommand(o =>
            {
                newSlip.Entries.Add(newEntry);
                NewEntry = new FeesStructureEntryModel();
            },
                o => CanAddNewEntry());

            RemoveEntryCommand = new RelayCommand(o =>
            {
                newSlip.Entries.RemoveAt((int)o);
            }, o =>
            {
                if (o != null)
                    return ((int)o) > -1;
                return false;
            });

            PreviewCommand = new RelayCommand(o =>
            {
                IsBusy = true;
                newSlip.RefreshTotal();

                MessageBox.Show("Don't forget to save the transaction!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                Document = DocumentHelper.GenerateDocument(newSlip);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
                IsBusy = false;
            }, o => CanSave());
            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = true;

                succ = await DataAccess.SaveNewPayslip(newSlip);

                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();

                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsBusy = false;
            },
               o => CanSave());

            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = true;

                succ = await DataAccess.SaveNewPayslip(newSlip);

                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    newSlip.RefreshTotal();
                    Document = DocumentHelper.GenerateDocument(newSlip);
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(Document);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                IsBusy = false;
            },
               o => CanSave());
        }         
            public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        private bool CanSave()
        {
            return !newSlip.HasErrors &&
                newSlip.AmountPaid > 0;
        }

        private bool CanAddNewEntry()
        {
            return (!string.IsNullOrWhiteSpace(newEntry.Name)
                && newEntry.Amount > 0) && !newSlip.HasErrors;
        }

        public FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public string SelectedMonth
        {
            get { return this.selectedMonth; }

            set
            {
                if (value != this.selectedMonth)
                {
                    this.selectedMonth = value;
                    NotifyPropertyChanged("SelectedMonth");
                }
            }
        }

        public int SelectedYear
        {
            get { return this.selectedYear; }

            set
            {
                if (value != this.selectedYear)
                {
                    this.selectedYear = value;
                    NotifyPropertyChanged("SelectedYear");
                }
            }
        }

        public FeesStructureEntryModel NewEntry
        {
            get { return newEntry; }
            set
            {
                if (value != newEntry)
                {
                    newEntry = value;
                    NotifyPropertyChanged("NewEntry");
                }
            }
        }

        public PayslipModel NewSlip
        {
            get { return newSlip; }

            set
            {
                if (value != newSlip)
                {
                    newSlip = value;
                    NotifyPropertyChanged("NewSlip");
                }
            }
        }

        public ICommand PreviewCommand
        {
            get;
            private set;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SaveAndPrintCommand
        {
            get;
            private set;
        }

        public ICommand AddEntryCommand
        { get; private set; }

        public ICommand RemoveEntryCommand
        { get; private set; }

        public override void Reset()
        {
            newSlip.Reset();
        }
    }
}