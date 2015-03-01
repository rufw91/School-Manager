﻿using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class ReprintReceiptVM:ViewModelBase
    {
        private FeePaymentModel currentPayment;
        private FixedDocument fd;
        ObservableCollection<FeePaymentModel> recentPayments;
        private StudentBaseModel currentStudent;

        public ReprintReceiptVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            Title = "REPRINT RECEIPT";
            CurrentStudent = new StudentBaseModel();
            CurrentPayment = new FeePaymentModel();
            RecentPayments = new ObservableCollection<FeePaymentModel>();
            currentStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                        RefreshRecentPayments();

                };
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                var fs = await DataAccess.GetSaleAsync(currentPayment.FeePaymentID);
                currentPayment.NameOfStudent = currentStudent.NameOfStudent;
                var temp = await DataAccess.GetReceiptAsync(currentPayment, new ObservableImmutableList<FeesStructureEntryModel>(fs.SaleItems));
                Document = DocumentHelper.GenerateDocument(temp);
            },
               o => CanGenerate());
        }

        private bool CanGenerate()
        {
            if (currentPayment == null)
                return false;
            currentStudent.CheckErrors();
            return !currentStudent.HasErrors;
        }

        private async void RefreshRecentPayments()
        {
            recentPayments.Clear();
            RecentPayments = await DataAccess.GetRecentPaymentsAsync(currentStudent.StudentID);
        }

        public FeePaymentModel CurrentPayment
        {
            get { return currentPayment; }

            set
            {
                if (value != currentPayment)
                {
                    currentPayment = value;
                    NotifyPropertyChanged("CurrentPayment");
                }
            }
        }

        public StudentBaseModel CurrentStudent
        {
            get { return currentStudent; }

            set
            {
                if (value != currentStudent)
                {
                    currentStudent = value;
                    NotifyPropertyChanged("CurrentStudent");
                }
            }
        }

        public ObservableCollection<FeePaymentModel> RecentPayments
        {
            get { return recentPayments; }

            private set
            {
                if (value != recentPayments)
                {
                    recentPayments = value;
                    NotifyPropertyChanged("RecentPayments");
                }
            }
        }


        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        
        public override void Reset()
        {

        }

        public FixedDocument Document
        {
            get { return this.fd; }

            private set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }
    }
}