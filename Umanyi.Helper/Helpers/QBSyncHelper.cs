using Helper.Models;
using Helper.Presentation;
using Interop.QBFC13;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace Helper
{
    public class QBSyncHelper : IDisposable
    {
        private enum SyncType
        {
            Student,
            Supplier,
            NIItem,
            Purchase,
            FeesPayment,
            FeesItem,
            StudentBill,
            SupplierPayment,
            InventoryItem,
            Donation,
            Pledge,
            PayrollItem,
            Payslip,
            Donor,
            Project
        }

        private class SaleQBModel
        {
            public string SaleID
            {
                get;
                set;
            }

            public string NameOfStudent
            {
                get;
                set;
            }

            public DateTime InvoiceDate
            {
                get;
                set;
            }

            public double Amount
            {
                get;
                set;
            }

            public string InvoiceItem
            {
                get;
                set;
            }

            public SaleQBModel()
            {
                this.SaleID = "";
                this.NameOfStudent = "";
                this.InvoiceDate = DateTime.Now;
                this.Amount = 0.0;
                this.InvoiceItem = "";
            }
        }

        private class FeesPaymentQBModel : FeePaymentModel
        {
            public FeesPaymentQBModel()
            {
                this.CheckErrors();
                base.NameOfStudent = "";
            }

            public override bool CheckErrors()
            {
                base.ClearAllErrors();
                return true;
            }
        }

        private class QBItemModel
        {
            public double Quantity
            {
                get;
                set;
            }

            public double Price
            {
                get;
                set;
            }

            public string FullName
            {
                get;
                set;
            }
        }

        private class QBSyncModel
        {
            public List<int> PaymentIDs
            {
                get;
                set;
            }

            public List<int> PayoutIDs
            {
                get;
                set;
            }

            public List<int> PayslipIDs
            {
                get;
                set;
            }

            public QBSyncModel()
            {
                this.PaymentIDs = new List<int>();
                this.PayoutIDs = new List<int>();
                this.PayslipIDs = new List<int>();
            }
        }

        private class PurchaseQBModel : PurchaseModel
        {
            public string NameOfSupplier
            {
                get;
                set;
            }

            public PurchaseQBModel()
            {
                this.NameOfSupplier = "";
            }
        }

        private class StudentQBModel : StudentBaseModel
        {
            public string PhoneNo
            {
                get;
                set;
            }

            public StudentQBModel()
            {
                this.PhoneNo = "";
            }
        }

        private class SupplierQBModel : SupplierBaseModel
        {
            public string PhoneNo
            {
                get;
                set;
            }

            public SupplierQBModel()
            {
                this.PhoneNo = "";
            }
        }

        private double contingency = 0.0;

        private bool saveContingency = false;

        private QBSessionManager session = null;

        public QBSyncHelper()
        {
            try
            {
                this.session = this.CreateNewSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Dispose(bool freeAll)
        {
            if (this.session != null)
            {
                try
                {
                    this.session.ClearErrorRecovery();
                    this.session.EndSession();
                    this.session.CloseConnection();
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Sync(IProgress<SyncOperationProgress> progressReporter, bool saveContingency, double contingency)
        {
            this.saveContingency = saveContingency;
            this.contingency = contingency;
            try
            {
                await this.SyncStudents(progressReporter);
                await this.SyncDonors(progressReporter);
                await this.SyncFeesItems(progressReporter);
                await this.SyncStudentBills(progressReporter);
                await this.SyncFeesPayments(progressReporter);
                await this.SyncNIItems(progressReporter);
                await this.SyncSuppliers(progressReporter);
                await this.SyncPayrollItems(progressReporter);
                await this.SyncPayroll(progressReporter);
                await this.SyncSupplierPayments(progressReporter);
                await this.SyncProjects(progressReporter);
                await this.SyncPledges(progressReporter);
                await this.SyncDonations(progressReporter);
            }
            catch (Exception ex)
            {
                Log.E(ex.Message + "\r\n" + ex.StackTrace, this);
            }
        }

        private async Task SyncStudents(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllCustomers(this.session);
        }

        private async Task SyncFeesPayments(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllFeesPayments(this.session);
        }

        private async Task SyncSuppliers(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllVendors(this.session);
        }

        private Task SyncSupplierPayments(IProgress<SyncOperationProgress> progressReporter)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
            });
        }

        private async Task SyncFeesItems(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllFeesItems(this.session);
        }

        private async Task SyncNIItems(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllNIItems(this.session);
        }

        private async Task SyncPayrollItems(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllPayrollItems(this.session);
        }

        private async Task SyncPayroll(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckPayroll(this.session);
        }

        private async Task SyncDonors(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckDonors(this.session);
        }

        private async Task SyncStudentBills(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllStudentBills(this.session);
        }

        private async Task SyncProjects(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllProjects(this.session);
        }

        private async Task SyncPledges(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllPledges(this.session);
        }

        private async Task SyncDonations(IProgress<SyncOperationProgress> progressReporter)
        {
            await this.CheckAllDonations(this.session);
        }

        private QBSessionManager CreateNewSession()
        {
            QBSessionManager qBSessionManager = null;// (QBSessionManager)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("22E885D7-FB0B-49E3-B905-CCA6BD526B52")));
            qBSessionManager.OpenConnection("", "Umanyi SMS");
            qBSessionManager.BeginSession("", ENOpenMode.omDontCare);
            string value = "{E74068B5-0D6D-454d-B0FD-BDDF95CE6778}";
            qBSessionManager.ErrorRecoveryID.SetValue(value);
            qBSessionManager.EnableErrorRecovery = true;
            qBSessionManager.SaveAllMsgSetRequestInfo = true;
            if (qBSessionManager.IsErrorRecoveryInfo())
            {
                IMsgSetResponse msgSetResponse = qBSessionManager.GetErrorRecoveryStatus();
                if (msgSetResponse.Attributes.MessageSetStatusCode.Equals("600"))
                {
                    MessageBox.Show("The oldMessageSetID does not match any stored IDs, and no newMessageSetID is provided.");
                }
                else if (msgSetResponse.Attributes.MessageSetStatusCode.Equals("9001"))
                {
                    MessageBox.Show("Invalid checksum. The newMessageSetID specified, matches the currently stored ID, but checksum fails.");
                }
                else if (msgSetResponse.Attributes.MessageSetStatusCode.Equals("9002"))
                {
                    MessageBox.Show("No stored response was found.");
                }
                else if (msgSetResponse.Attributes.MessageSetStatusCode.Equals("9004"))
                {
                    MessageBox.Show("Invalid MessageSetID, greater than 24 character was given.");
                }
                else if (msgSetResponse.Attributes.MessageSetStatusCode.Equals("9005"))
                {
                    MessageBox.Show("Unable to store response.");
                }
                else
                {
                    IResponse at = msgSetResponse.ResponseList.GetAt(0);
                    int statusCode = at.StatusCode;
                    if (statusCode != 0)
                    {
                        if (statusCode > 0)
                        {
                            MessageBox.Show("There was a warning but last request was processed successfully!");
                        }
                        else
                        {
                            MessageBox.Show("It seems that there was an error in processing last request");
                            IMsgSetRequest savedMsgSetRequest = qBSessionManager.GetSavedMsgSetRequest();
                            msgSetResponse = qBSessionManager.DoRequests(savedMsgSetRequest);
                            IResponse at2 = msgSetResponse.ResponseList.GetAt(0);
                            int statusCode2 = at2.StatusCode;
                            if (statusCode2 == 0)
                            {
                            }
                        }
                    }
                }
                qBSessionManager.ClearErrorRecovery();
            }
            qBSessionManager.ClearErrorRecovery();
            return qBSessionManager;
        }

        private double QBFCLatestVersion(QBSessionManager sessionManager)
        {
            IMsgSetRequest msgSetRequest = sessionManager.CreateMsgSetRequest("US", 1, 0);
            sessionManager.ClearErrorRecovery();
            msgSetRequest.AppendHostQueryRq();
            IMsgSetResponse msgSetResponse = sessionManager.DoRequests(msgSetRequest);
            IResponse at = msgSetResponse.ResponseList.GetAt(0);
            IHostRet hostRet = at.Detail as IHostRet;
            IBSTRList supportedQBXMLVersionList = hostRet.SupportedQBXMLVersionList;
            double num = 0.0;
            for (int i = 0; i <= supportedQBXMLVersionList.Count - 1; i++)
            {
                string at2 = supportedQBXMLVersionList.GetAt(i);
                double num2 = Convert.ToDouble(at2);
                if (num2 > num)
                {
                    num = num2;
                }
            }
            return num;
        }

        private IMsgSetRequest GetLatestMsgSetRequest(QBSessionManager sessionManager)
        {
            double num = this.QBFCLatestVersion(sessionManager);
            short qbXMLMajorVersion;
            short qbXMLMinorVersion;
            if (num >= 6.0)
            {
                qbXMLMajorVersion = 6;
                qbXMLMinorVersion = 0;
            }
            else if (num >= 5.0)
            {
                qbXMLMajorVersion = 5;
                qbXMLMinorVersion = 0;
            }
            else if (num >= 4.0)
            {
                qbXMLMajorVersion = 4;
                qbXMLMinorVersion = 0;
            }
            else if (num >= 3.0)
            {
                qbXMLMajorVersion = 3;
                qbXMLMinorVersion = 0;
            }
            else if (num >= 2.0)
            {
                qbXMLMajorVersion = 2;
                qbXMLMinorVersion = 0;
            }
            else if (num >= 1.1)
            {
                qbXMLMajorVersion = 1;
                qbXMLMinorVersion = 1;
            }
            else
            {
                qbXMLMajorVersion = 1;
                qbXMLMinorVersion = 0;
                MessageBox.Show("It seems that you are running an old version of Quickbooks. We recommend that you upgrade to a new version.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            IMsgSetRequest msgSetRequest = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVersion, qbXMLMinorVersion);
            msgSetRequest.Attributes.OnError = ENRqOnError.roeContinue;
            return msgSetRequest;
        }

        private void AddSyncedItem(string refNo, string type)
        {
            string commandText = "INSERT INTO [Institution].[QBSyncDetail] (SyncType,RefNo) VALUES(@typ,@refNo)";
            DataAccessHelper.ExecuteNonQueryWithParameters(commandText, new ObservableCollection<SqlParameter>
            {
                new SqlParameter("@typ", type),
                new SqlParameter("@refNo", refNo)
            });
        }

        public void AddNewDonors(IEnumerable<DonorModel> students, QBSessionManager sessionManager)
        {
            foreach (DonorModel current in students)
            {
                this.AddNewDonor(sessionManager, current.DonorID.ToString(), string.Concat(new object[]
                {
                    "DONOR-",
                    current.DonorID,
                    "-",
                    current.NameOfDonor
                }), current.PhoneNo);
            }
        }

        public void AddNewDonations(IEnumerable<DonationModel> students, QBSessionManager sessionManager)
        {
            foreach (DonationModel current in students)
            {
                this.AddNewDonation(sessionManager, current);
            }
        }

        public void AddNewProjects(IEnumerable<ProjectModel> students, QBSessionManager sessionManager)
        {
            foreach (ProjectModel current in students)
            {
                this.AddNewProject(sessionManager, current);
            }
        }

        public void AddNewPledges(IEnumerable<DonationModel> students, QBSessionManager sessionManager)
        {
            foreach (DonationModel current in students)
            {
                this.AddNewPledge(sessionManager, current);
            }
        }

        private void AddNewProject(QBSessionManager sessionManager, ProjectModel project)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IItemServiceAdd itemServiceAdd = latestMsgSetRequest.AppendItemServiceAddRq();
            itemServiceAdd.Name.SetValue(string.Concat(new object[]
            {
                "PROJECT-",
                project.Name,
                "-",
                project.ProjectID
            }));
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue("Contract Services");
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue((double)project.Budget);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            IMsgSetRequest latestMsgSetRequest2 = this.GetLatestMsgSetRequest(sessionManager);
            IBillAdd billAdd = latestMsgSetRequest2.AppendBillAddRq();
            billAdd.VendorRef.FullName.SetValue("0-PROJECTS");
            billAdd.RefNumber.SetValue(string.Concat(new object[]
            {
                "PROJECT-",
                project.Name,
                "-",
                project.ProjectID
            }));
            billAdd.TxnDate.SetValue(project.StartDate);
            IItemLineAdd itemLineAdd = billAdd.ORItemLineAddList.Append().ItemLineAdd;
            itemLineAdd.ItemRef.FullName.SetValue(string.Concat(new object[]
            {
                "PROJECT-",
                project.Name,
                "-",
                project.ProjectID
            }));
            itemLineAdd.Quantity.SetValue(1.0);
            itemLineAdd.Cost.SetValue(this.saveContingency ? double.Parse(((double)project.Budget * (1.0 + this.contingency)).ToString("N2")) : ((double)project.Budget));
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest2);
            this.AddSyncedItem(project.ProjectID.ToString(), QBSyncHelper.SyncType.Project.ToString());
        }

        private void AddNewDonor(QBSessionManager sessionManager, string donorID, string nameOfCustomer, string phoneNumber)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            ICustomerAdd customerAdd = latestMsgSetRequest.AppendCustomerAddRq();
            customerAdd.Name.SetValue(nameOfCustomer);
            customerAdd.Phone.SetValue(phoneNumber);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(donorID, QBSyncHelper.SyncType.Donor.ToString());
        }

        private void AddNewCustomer(QBSessionManager sessionManager, string customerID, string nameOfCustomer, string phoneNumber)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            ICustomerAdd customerAdd = latestMsgSetRequest.AppendCustomerAddRq();
            customerAdd.Name.SetValue(nameOfCustomer);
            customerAdd.Phone.SetValue(phoneNumber);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(customerID, QBSyncHelper.SyncType.Donor.ToString());
        }

        private void AddNewSupplier(QBSessionManager sessionManager, string supplierID, string nameOfCustomer, string phoneNumber)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IVendorAdd vendorAdd = latestMsgSetRequest.AppendVendorAddRq();
            vendorAdd.Name.SetValue(nameOfCustomer);
            vendorAdd.Phone.SetValue(phoneNumber);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(supplierID, QBSyncHelper.SyncType.Supplier.ToString());
        }

        private void AddNewCustomers(IEnumerable<QBSyncHelper.StudentQBModel> students, QBSessionManager sessionManager)
        {
            foreach (QBSyncHelper.StudentQBModel current in students)
            {
                this.AddNewCustomer(sessionManager, current.StudentID.ToString(), string.Concat(new object[]
                {
                    "STUDENT-",
                    current.StudentID,
                    "-",
                    current.NameOfStudent
                }), current.PhoneNo);
            }
        }

        private void AddNewSuppliers(IEnumerable<QBSyncHelper.SupplierQBModel> students, QBSessionManager sessionManager)
        {
            foreach (QBSyncHelper.SupplierQBModel current in students)
            {
                this.AddNewSupplier(sessionManager, current.SupplierID.ToString(), current.SupplierID + "-" + current.NameOfSupplier, current.PhoneNo);
            }
        }

        private void AddNewPurchases(IEnumerable<QBSyncHelper.PurchaseQBModel> students, QBSessionManager sessionManager)
        {
            foreach (QBSyncHelper.PurchaseQBModel current in students)
            {
                this.AddNewPurchase(sessionManager, current);
            }
        }

        private void AddNewNIItems(IEnumerable<ItemBaseModel> students, QBSessionManager sessionManager)
        {
            foreach (ItemBaseModel current in students)
            {
                this.AddNewNIItem(sessionManager, current.ItemID.ToString(), current.ItemID + "-" + current.Description, 1.0);
            }
        }

        private void AddNewFeesItems(IEnumerable<string> students, QBSessionManager sessionManager)
        {
            foreach (string current in students)
            {
                this.AddNewFeesItem(sessionManager, current);
            }
        }

        private void AddNewFeesPayments(IEnumerable<QBSyncHelper.FeesPaymentQBModel> students, QBSessionManager sessionManager)
        {
            foreach (QBSyncHelper.FeesPaymentQBModel current in students)
            {
                this.AddNewFeesPayment(sessionManager, current);
            }
        }

        private void AddNewPayrolls(IEnumerable<PayslipModel> students, QBSessionManager sessionManager)
        {
            foreach (PayslipModel current in students)
            {
                this.AddNewPayroll(sessionManager, current);
            }
        }

        private void AddNewFeesItem(QBSessionManager sessionManager, string name)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IItemServiceAdd itemServiceAdd = latestMsgSetRequest.AppendItemServiceAddRq();
            itemServiceAdd.Name.SetValue(name);
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue("School Fees Paid");
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(0.0);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
        }

        private void AddNewProjectTask(QBSessionManager sessionManager, int projectID, string nameOfProject, ProjectTaskModel task)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IItemServiceAdd itemServiceAdd = latestMsgSetRequest.AppendItemServiceAddRq();
            itemServiceAdd.Name.SetValue(string.Concat(new object[]
            {
                nameOfProject,
                "-",
                projectID,
                "-",
                task.NameOfTask
            }));
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue("Contract Services");
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue((double)task.Allocation);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
        }

        private void AddNewPayrollItem(QBSessionManager sessionManager, string name)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IItemServiceAdd itemServiceAdd = latestMsgSetRequest.AppendItemServiceAddRq();
            itemServiceAdd.Name.SetValue(name);
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue("Payroll Expenses");
            itemServiceAdd.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(0.0);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
        }

        private void AddNewPayrollItems(IEnumerable<string> students, QBSessionManager sessionManager)
        {
            foreach (string current in students)
            {
                this.AddNewPayrollItem(sessionManager, current);
            }
        }

        private void AddNewNIItem(QBSessionManager sessionManager, string itemID, string name, double price)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IItemNonInventoryAdd itemNonInventoryAdd = latestMsgSetRequest.AppendItemNonInventoryAddRq();
            itemNonInventoryAdd.Name.SetValue(name);
            itemNonInventoryAdd.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue("Cost of Goods Sold");
            itemNonInventoryAdd.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(price);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(itemID, QBSyncHelper.SyncType.NIItem.ToString());
        }

        private void AddNewPurchase(QBSessionManager sessionManager, QBSyncHelper.PurchaseQBModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IBillAdd billAdd = latestMsgSetRequest.AppendBillAddRq();
            billAdd.VendorRef.FullName.SetValue(purchase.NameOfSupplier);
            billAdd.RefNumber.SetValue(purchase.RefNo);
            billAdd.TxnDate.SetValue(purchase.OrderDate);
            foreach (ItemPurchaseModel current in purchase.Items)
            {
                IItemLineAdd itemLineAdd = billAdd.ORItemLineAddList.Append().ItemLineAdd;
                itemLineAdd.ItemRef.FullName.SetValue(current.Description);
                itemLineAdd.Quantity.SetValue((double)current.Quantity);
                itemLineAdd.Cost.SetValue((double)current.Cost);
            }
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.PurchaseID.ToString(), QBSyncHelper.SyncType.Purchase.ToString());
        }

        private void AddNewPayroll(QBSessionManager sessionManager, PayslipModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IBillAdd billAdd = latestMsgSetRequest.AppendBillAddRq();
            billAdd.VendorRef.FullName.SetValue("0-EMPLOYEES");
            billAdd.RefNumber.SetValue(purchase.Name);
            billAdd.TxnDate.SetValue(purchase.DatePaid);
            IItemLineAdd itemLineAdd = billAdd.ORItemLineAddList.Append().ItemLineAdd;
            itemLineAdd.ItemRef.FullName.SetValue("EMPLOYEE PAYMENT");
            itemLineAdd.Quantity.SetValue(1.0);
            itemLineAdd.Cost.SetValue((double)purchase.AmountPaid);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.PayslipID.ToString(), QBSyncHelper.SyncType.Payslip.ToString());
        }

        private void AddNewDonation(QBSessionManager sessionManager, DonationModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            ISalesReceiptAdd salesReceiptAdd = latestMsgSetRequest.AppendSalesReceiptAddRq();
            salesReceiptAdd.CustomerRef.FullName.SetValue(string.Concat(new object[]
            {
                "DONOR-",
                purchase.DonorID,
                "-",
                purchase.NameOfDonor
            }));
            salesReceiptAdd.RefNumber.SetValue(purchase.DonationID.ToString());
            salesReceiptAdd.TxnDate.SetValue(purchase.DateDonated);
            ISalesReceiptLineAdd salesReceiptLineAdd = salesReceiptAdd.ORSalesReceiptLineAddList.Append().SalesReceiptLineAdd;
            salesReceiptLineAdd.ItemRef.FullName.SetValue("DONATION");
            double num = double.Parse(((double)purchase.Amount * (1.0 - this.contingency)).ToString("N2"));
            salesReceiptLineAdd.ORRatePriceLevel.Rate.SetValue(this.saveContingency ? num : ((double)purchase.Amount));
            salesReceiptLineAdd.Quantity.SetValue(1.0);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.DonationID.ToString(), QBSyncHelper.SyncType.Donation.ToString());
        }

        private void AddNewPledge(QBSessionManager sessionManager, DonationModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IInvoiceAdd invoiceAdd = latestMsgSetRequest.AppendInvoiceAddRq();
            invoiceAdd.CustomerRef.FullName.SetValue(string.Concat(new object[]
            {
                "DONOR-",
                purchase.DonorID,
                "-",
                purchase.NameOfDonor
            }));
            invoiceAdd.RefNumber.SetValue(purchase.DonationID.ToString());
            invoiceAdd.TxnDate.SetValue(purchase.DateDonated);
            IInvoiceLineAdd invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
            invoiceLineAdd.ItemRef.FullName.SetValue("DONATION");
            invoiceLineAdd.ORRatePriceLevel.Rate.SetValue(1.0);
            double num = double.Parse(((double)purchase.Amount * (1.0 - this.contingency)).ToString("N2"));
            invoiceLineAdd.ORRatePriceLevel.Rate.SetValue(this.saveContingency ? num : ((double)purchase.Amount));
            invoiceLineAdd.Quantity.SetValue(1.0);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.DonationID.ToString(), QBSyncHelper.SyncType.Pledge.ToString());
        }

        private void AddNewFeesPayment(QBSessionManager sessionManager, QBSyncHelper.FeesPaymentQBModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IReceivePaymentAdd receivePaymentAdd = latestMsgSetRequest.AppendReceivePaymentAddRq();
            receivePaymentAdd.CustomerRef.FullName.SetValue(purchase.NameOfStudent);
            receivePaymentAdd.RefNumber.SetValue(purchase.FeePaymentID.ToString());
            receivePaymentAdd.TxnDate.SetValue(purchase.DatePaid);
            receivePaymentAdd.TotalAmount.SetValue(this.saveContingency ? double.Parse(((double)purchase.AmountPaid * (1.0 + this.contingency)).ToString("N2")) : ((double)purchase.AmountPaid));
            receivePaymentAdd.ORApplyPayment.IsAutoApply.SetValue(true);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.FeePaymentID.ToString(), QBSyncHelper.SyncType.FeesPayment.ToString());
        }

        private void AddNewStudentBills(IEnumerable<QBSyncHelper.SaleQBModel> students, QBSessionManager sessionManager)
        {
            foreach (QBSyncHelper.SaleQBModel current in students)
            {
                this.AddNewStudentBill(sessionManager, current);
            }
        }

        private void AddNewStudentBill(QBSessionManager sessionManager, QBSyncHelper.SaleQBModel purchase)
        {
            IMsgSetRequest latestMsgSetRequest = this.GetLatestMsgSetRequest(sessionManager);
            IInvoiceAdd invoiceAdd = latestMsgSetRequest.AppendInvoiceAddRq();
            invoiceAdd.CustomerRef.FullName.SetValue(purchase.NameOfStudent);
            invoiceAdd.RefNumber.SetValue(purchase.SaleID.ToString());
            invoiceAdd.TxnDate.SetValue(purchase.InvoiceDate);
            IInvoiceLineAdd invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
            invoiceLineAdd.ItemRef.FullName.SetValue(purchase.InvoiceItem);
            invoiceLineAdd.ORRatePriceLevel.Rate.SetValue(1.0);
            invoiceLineAdd.Quantity.SetValue(purchase.Amount);
            invoiceLineAdd.Amount.SetValue(this.saveContingency ? double.Parse((purchase.Amount * (1.0 + this.contingency)).ToString("N2")) : purchase.Amount);
            sessionManager.ClearErrorRecovery();
            sessionManager.DoRequests(latestMsgSetRequest);
            this.AddSyncedItem(purchase.SaleID, QBSyncHelper.SyncType.StudentBill.ToString());
        }

        private Task CheckAllProjects(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<ProjectModel> allUnSyncedProjects = this.GetAllUnSyncedProjects();
                this.AddNewProjects(allUnSyncedProjects, sessionManager);
            });
        }

        private Task CheckAllDonations(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<DonationModel> allUnSyncedDonations = this.GetAllUnSyncedDonations();
                this.AddNewDonations(allUnSyncedDonations, sessionManager);
            });
        }

        private Task CheckAllPledges(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<DonationModel> allUnSyncedPledges = this.GetAllUnSyncedPledges();
                this.AddNewPledges(allUnSyncedPledges, sessionManager);
            });
        }

        private Task CheckDonors(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<DonorModel> allUnSyncedDonors = this.GetAllUnSyncedDonors();
                this.AddNewDonors(allUnSyncedDonors, sessionManager);
            });
        }

        private Task CheckAllCustomers(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<QBSyncHelper.StudentQBModel> allUnSyncedCustomers = this.GetAllUnSyncedCustomers();
                this.AddNewCustomers(allUnSyncedCustomers, sessionManager);
            });
        }

        private Task CheckAllVendors(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<QBSyncHelper.SupplierQBModel> allUnSyncedSuppliers = this.GetAllUnSyncedSuppliers();
                this.AddNewSuppliers(allUnSyncedSuppliers, sessionManager);
            });
        }

        private Task CheckAllNIItems(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<ItemBaseModel> allUnSyncedNIItems = this.GetAllUnSyncedNIItems();
                this.AddNewNIItems(allUnSyncedNIItems, sessionManager);
            });
        }

        private Task CheckAllFeesItems(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                string item = "FEES FOR TERM 1 " + DateTime.Now.Year;
                string item2 = "FEES FOR TERM 2 " + DateTime.Now.Year;
                string item3 = "FEES FOR TERM 3 " + DateTime.Now.Year;
                List<string> students = new List<string>
                {
                    item,
                    item2,
                    item3
                };
                this.AddNewFeesItems(students, sessionManager);
            });
        }

        private Task CheckAllPayrollItems(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                this.AddNewPayrollItem(sessionManager, "EMPLOYEE PAYMENT");
            });
        }

        private Task CheckPayroll(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<PayslipModel> allUnSyncedPayroll = this.GetAllUnSyncedPayroll();
                this.AddNewPayrolls(allUnSyncedPayroll, sessionManager);
            });
        }

        private Task CheckAllPurchases(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<QBSyncHelper.PurchaseQBModel> allUnSyncedPurchases = this.GetAllUnSyncedPurchases();
                this.AddNewPurchases(allUnSyncedPurchases, sessionManager);
            });
        }

        private Task CheckAllFeesPayments(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<QBSyncHelper.FeesPaymentQBModel> allUnSyncedFeesPayments = this.GetAllUnSyncedFeesPayments();
                this.AddNewFeesPayments(allUnSyncedFeesPayments, sessionManager);
            });
        }

        private Task CheckAllStudentBills(QBSessionManager sessionManager)
        {
            return
#if NET4
 Task.Factory.StartNew
#else
                Task.Run
#endif
(delegate
            {
                List<QBSyncHelper.SaleQBModel> allUnSyncedStudentBills = this.GetAllUnSyncedStudentBills();
                this.AddNewStudentBills(allUnSyncedStudentBills, sessionManager);
            });
        }

        private List<ProjectModel> GetAllUnSyncedProjects()
        {
            List<ProjectModel> list = new List<ProjectModel>();
            string commandText = "SELECT p.ProjectID,p.NameOfProject,p.StartDateTime,p.EndDateTime,p.Budget,p.Description FROM [Institution].[ProjectHeader]p RIGHT OUTER JOIN (SELECT ProjectID FROM [Institution].[ProjectHeader] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Project')x ON (x.ProjectID = p.ProjectID) ";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new ProjectModel
                {
                    ProjectID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString(),
                    StartDate = DateTime.Parse(dataRow[2].ToString()),
                    EndDate = DateTime.Parse(dataRow[3].ToString()),
                    Budget = decimal.Parse(dataRow[4].ToString()),
                    Description = dataRow[5].ToString()
                });
            }
            return list;
        }

        private List<DonationModel> GetAllUnSyncedDonations()
        {
            List<DonationModel> list = new List<DonationModel>();
            string commandText = "SELECT s.DonorID,d.NameOfDonor,s.AmountDonated,s.DateDonated,s.DonateTo,s.DonationID FROM [Institution].[Donation]s RIGHT OUTER JOIN (SELECT DonationID FROM [Institution].[Donation] WHERE [Type]=1 EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Donation')x ON (x.DonationID = s.DonationID) LEFT OUTER JOIN [Institution].[Donor] d ON (d.DonorID = s.DonorID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new DonationModel
                {
                    DonorID = int.Parse(dataRow[0].ToString()),
                    NameOfDonor = dataRow[1].ToString(),
                    Amount = decimal.Parse(dataRow[2].ToString()),
                    DateDonated = DateTime.Parse(dataRow[3].ToString()),
                    DonateTo = (DonateTo)Enum.Parse(typeof(DonateTo), dataRow[4].ToString()),
                    DonationID = int.Parse(dataRow[5].ToString())
                });
            }
            return list;
        }

        private List<DonationModel> GetAllUnSyncedPledges()
        {
            List<DonationModel> list = new List<DonationModel>();
            string commandText = "SELECT s.DonorID,d.NameOfDonor,s.AmountDonated,s.DateDonated,s.DonateTo,s.DonationID FROM [Institution].[Donation]s RIGHT OUTER JOIN (SELECT DonationID FROM [Institution].[Donation]  WHERE [Type]=2 EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Pledge')x ON (x.DonationID = s.DonationID) LEFT OUTER JOIN [Institution].[Donor] d ON (d.DonorID = s.DonorID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new DonationModel
                {
                    DonorID = int.Parse(dataRow[0].ToString()),
                    NameOfDonor = dataRow[1].ToString(),
                    Amount = decimal.Parse(dataRow[2].ToString()),
                    DateDonated = DateTime.Parse(dataRow[3].ToString()),
                    DonateTo = (DonateTo)Enum.Parse(typeof(DonateTo), dataRow[4].ToString()),
                    DonationID = int.Parse(dataRow[5].ToString())
                });
            }
            return list;
        }

        private List<DonorModel> GetAllUnSyncedDonors()
        {
            List<DonorModel> list = new List<DonorModel>();
            string commandText = "SELECT x.DonorID,s.NameOfDonor,s.PhoneNo FROM [Institution].[Donor]s RIGHT OUTER JOIN (SELECT DonorID FROM [Institution].[Donor] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Donor')x ON (x.DonorID = s.DonorID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new DonorModel
                {
                    DonorID = int.Parse(dataRow[0].ToString()),
                    NameOfDonor = dataRow[1].ToString(),
                    PhoneNo = dataRow[2].ToString()
                });
            }
            return list;
        }

        private List<QBSyncHelper.StudentQBModel> GetAllUnSyncedCustomers()
        {
            List<QBSyncHelper.StudentQBModel> list = new List<QBSyncHelper.StudentQBModel>();
            string commandText = "SELECT x.StudentID,s.NameOfStudent,s.GuardianPhoneNo FROM [Institution].[Student]s RIGHT OUTER JOIN (SELECT StudentID FROM [Institution].[Student] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Student')x ON (x.StudentID = s.StudentID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new QBSyncHelper.StudentQBModel
                {
                    StudentID = int.Parse(dataRow[0].ToString()),
                    NameOfStudent = dataRow[1].ToString(),
                    PhoneNo = dataRow[2].ToString()
                });
            }
            return list;
        }

        private List<QBSyncHelper.SupplierQBModel> GetAllUnSyncedSuppliers()
        {
            List<QBSyncHelper.SupplierQBModel> list = new List<QBSyncHelper.SupplierQBModel>();
            string commandText = "SELECT x.SupplierID,s.NameOfSupplier,s.PhoneNo FROM [Sales].[Supplier]s RIGHT OUTER JOIN (SELECT SupplierID FROM [Sales].[Supplier] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Supplier')x ON (x.SupplierID = s.SupplierID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            list.Add(new QBSyncHelper.SupplierQBModel
            {
                SupplierID = 0,
                NameOfSupplier = "EMPLOYEES",
                PhoneNo = "X"
            });
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new QBSyncHelper.SupplierQBModel
                {
                    SupplierID = int.Parse(dataRow[0].ToString()),
                    NameOfSupplier = dataRow[1].ToString(),
                    PhoneNo = dataRow[2].ToString()
                });
            }
            return list;
        }

        private List<ItemBaseModel> GetAllUnSyncedNIItems()
        {
            List<ItemBaseModel> list = new List<ItemBaseModel>();
            string commandText = "SELECT x.ItemID,s.Description FROM [Sales].[Item]s RIGHT OUTER JOIN (SELECT ItemID FROM [Sales].[Item] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='NIItem')x ON (x.ItemID = s.ItemID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new ItemBaseModel
                {
                    ItemID = (long)int.Parse(dataRow[0].ToString()),
                    Description = dataRow[1].ToString()
                });
            }
            return list;
        }

        private List<QBSyncHelper.PurchaseQBModel> GetAllUnSyncedPurchases()
        {
            List<QBSyncHelper.PurchaseQBModel> list = new List<QBSyncHelper.PurchaseQBModel>();
            string commandText = "SELECT x.ItemReceiptID,sup.NameOfSupplier,s.TotalAmt,s.RefNo,s.OrderDate FROM [Sales].[ItemReceiptHeader]s RIGHT OUTER JOIN (SELECT ItemReceiptID FROM [Sales].[ItemReceiptHeader] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Purchase')x ON (x.ItemID = s.ItemID) LEFT OUTER JOIN [Sales].[Supplier] sup ON (sup.SupplierID=s.SupplierID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                QBSyncHelper.PurchaseQBModel purchaseQBModel = new QBSyncHelper.PurchaseQBModel();
                purchaseQBModel.PurchaseID = int.Parse(dataRow[0].ToString());
                purchaseQBModel.NameOfSupplier = dataRow[1].ToString();
                purchaseQBModel.OrderTotal = decimal.Parse(dataRow[2].ToString());
                purchaseQBModel.RefNo = dataRow[3].ToString();
                purchaseQBModel.OrderDate = DateTime.Parse(dataRow[4].ToString());
                purchaseQBModel.Items = DataAccess.GetItemsReceiptItems(purchaseQBModel.PurchaseID);
                list.Add(purchaseQBModel);
            }
            return list;
        }

        private List<QBSyncHelper.FeesPaymentQBModel> GetAllUnSyncedFeesPayments()
        {
            List<QBSyncHelper.FeesPaymentQBModel> list = new List<QBSyncHelper.FeesPaymentQBModel>();
            string commandText = "SELECT x.FeesPaymentID,CONVERT(varchar(50),s.StudentID)+'-'+s.NameOfStudent ,p.DatePaid, p.AmountPaid FROM [Institution].[FeesPayment]p RIGHT OUTER JOIN (SELECT FeesPaymentID FROM [Institution].[FeesPayment] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='FeesPayment')x ON (x.FeesPaymentID = p.FeesPaymentID) LEFT OUTER JOIN [Institution].[Student] s on (s.StudentID = p.StudentID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new QBSyncHelper.FeesPaymentQBModel
                {
                    FeePaymentID = int.Parse(dataRow[0].ToString()),
                    NameOfStudent = "STUDENT-" + dataRow[1].ToString(),
                    DatePaid = DateTime.Parse(dataRow[2].ToString()),
                    AmountPaid = decimal.Parse(dataRow[3].ToString())
                });
            }
            return list;
        }

        private List<PayslipModel> GetAllUnSyncedPayroll()
        {
            List<PayslipModel> list = new List<PayslipModel>();
            string commandText = "SELECT x.PayslipID,p.Designation+'-'+CONVERT(varchar(50),s.StaffID)+'-'+s.[Name] ,p.DatePaid, p.AmountPaid FROM [Institution].[PayslipHeader]p RIGHT OUTER JOIN (SELECT PayslipID FROM [Institution].[PayslipHeader] EXCEPT SELECT CONVERT(int,RefNo) FROM [Institution].[QBSyncDetail] WHERE SyncType='Payslip')x ON (x.PayslipID = p.PayslipID) LEFT OUTER JOIN [Institution].[Staff] s on (s.StaffID = p.StaffID)";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new PayslipModel
                {
                    PayslipID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString(),
                    DatePaid = DateTime.Parse(dataRow[2].ToString()),
                    AmountPaid = decimal.Parse(dataRow[3].ToString())
                });
            }
            return list;
        }

        private List<QBSyncHelper.SaleQBModel> GetAllUnSyncedStudentBills()
        {
            List<QBSyncHelper.SaleQBModel> list = new List<QBSyncHelper.SaleQBModel>();
            string commandText = "SELECT x.SaleID,p.CustomerID+'-'+s.NameOfStudent ,p.OrderDate, p.TotalAmt FROM [Sales].[SaleHeader]p RIGHT OUTER JOIN (SELECT SaleID FROM [Sales].[SaleHeader] EXCEPT SELECT RefNo FROM [Institution].[QBSyncDetail] WHERE SyncType='StudentBill')x ON (x.SaleID = p.SaleID) LEFT OUTER JOIN [Institution].[Student] s on (s.StudentID = CONVERT(int,p.CustomerID))";
            DataTable dataTable = DataAccessHelper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                QBSyncHelper.SaleQBModel saleQBModel = new QBSyncHelper.SaleQBModel();
                saleQBModel.SaleID = dataRow[0].ToString();
                saleQBModel.NameOfStudent = "STUDENT-" + dataRow[1].ToString();
                saleQBModel.InvoiceDate = DateTime.Parse(dataRow[2].ToString());
                saleQBModel.Amount = double.Parse(dataRow[3].ToString());
                saleQBModel.InvoiceItem = string.Concat(new object[]
                {
                    "FEES FOR TERM ",
                    DataAccess.GetTerm(saleQBModel.InvoiceDate),
                    " ",
                    saleQBModel.InvoiceDate.Year
                });
                list.Add(saleQBModel);
            }
            return list;
        }
    }
}
