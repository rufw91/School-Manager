using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region General Ledger
        private static int CalculatePagesAccountsGenLedger(ObservableCollection<AccountModel> counts)
        {
            return 1;
        }
        
        private static void AddAGLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 650, 65, pageNo);
        }
       
        private static void AddAGLTotal(decimal meanScore, int pageNo)
        {
            AddText(meanScore.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 670, 1050, pageNo);
        }
        private static void AddAGLAccount(string meanGrade, int pageNo)
        {
            AddText(meanGrade, "Arial", 14, true, 0, Colors.Black, 330, 115, pageNo);
        }
        private static void AddAGLEntry(TransactionModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 190 + pageRelativeIndex * 25;

            AddText(item.TransactionDateTime.ToString("dd-MM-yyyy"), "Arial", 14, false, 0, Colors.Black, 30, yPos, pageNo);
            AddText(item.TransactionID, "Arial", fontsize, false, 0, Colors.Black, 125, yPos, pageNo);
            AddText(item.TransactionAmt.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, item.TransactionType == TransactionTypes.Debit ? 485 : 585, yPos, pageNo);
            
        }
        private static void AddAGLEntries(ObservableCollection<TransactionModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddAGLEntry(psi[i], i, pageNo);
        }

        private static void GenerateAccountsGeneralLedger()
        {
            GeneralLedgerModel si = myWorkObject as GeneralLedgerModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAGLDate(si.Date, pageNo);
                AddAGLAccount(si.AccountName, pageNo);
                AddAGLTotal(si.Total, pageNo);
                AddAGLEntries(si.Entries, pageNo);
            }
        }
        #endregion

        #region Income Statement
        private static void GenerateAccountsIncomeStatement()
        {

            IncomeStatementModel si = myWorkObject as IncomeStatementModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAISPeriod(si.StartTime,si.EndTime, pageNo);
                for (int i = 0; i < 3; i++)
                {
                    AddAISRevenueAccount(si.RevenueEntries[i].TransactionAmt,195+(i*25), pageNo);
                }
                AddAISTotalRevenue(si.TotalRevenue, pageNo);
                for (int i = 0; i < 6; i++)
                {
                    AddAISExpenseAccount(si.ExpenseEntries[i].TransactionAmt,325+(i*25), pageNo);
                }
                AddAISTotalExpenses(si.TotalExpense, pageNo);
                AddAISNetIncome(si.NetIncome, pageNo);
            }
        }

        private static void AddAISPeriod(DateTime start, DateTime end, int pageNo)
        {
            AddText("PERIOD " + start.ToString("dd-MMM-yyyy") + " to " + end.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 290, 115, pageNo);
        }

        private static void AddAISRevenueAccount(decimal amount,double yPos, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 350, yPos, pageNo);
        }

        private static void AddAISExpenseAccount(decimal amount,double yPos, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 350, yPos, pageNo);
        }

        private static void AddAISTotalRevenue(decimal amount, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 590, 260, pageNo);
        }

        private static void AddAISTotalExpenses(decimal amount, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 590, 470, pageNo);
        }

        private static void AddAISNetIncome(decimal amount, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 590, 690, pageNo);
        }

        
        #endregion

        #region BalanceSheet
        private static void GenerateAccountsBalanceSheet()
        {
            
        }
        #endregion

        #region Statement of Cashflows Statement
        private static void GenerateAccountsSTofCashFlows()
        {
            STCashFlowsModel si = myWorkObject as STCashFlowsModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddAISPeriod(si.StartTime, si.EndTime, pageNo);
                for (int i = 0; i < 8; i++)
                {
                    AddASTCActivityEntry(si.OperatingActivitiesEntries[i].TransactionAmt, 185 + (i * 25), pageNo);
                }
                AddASTCActivityTotal(si.OperatingActivitiesTotal, 400d, pageNo);

                AddASTCActivityTotal(si.StartBalance, 700d, pageNo);
                AddASTCActivityTotal(si.EndBalance, 740d, pageNo);
            }
        }

        private static void AddASTCActivityEntry(decimal amount, double yPos, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 450, yPos, pageNo); 
        }

        private static void AddASTCActivityTotal(decimal amount, double yPos, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 635, yPos, pageNo); 
        }
        #endregion

        #region Trial Balance
        private static void GenerateTrialBalance()
        {
            TrialBalanceModel si = myWorkObject as TrialBalanceModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddATBPeriod(si.StartTime, si.EndTime, pageNo);
                for (int i = 0; i < si.Accounts.Count; i++)
                {
                    AddATBAccount(si.Accounts[i], 185 + (i * 25), pageNo);
                }
                AddATBCreditTotals(si.CreditTotals,  pageNo);
                AddATBDebitTotals(si.CreditTotals, pageNo);
            }
        }

        private static void AddATBPeriod(DateTime startTime, DateTime endTime, int pageNo)
        {
            AddText(startTime.ToString("dd MMM yyyy")+" to "+endTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 450, 0, pageNo);
        }

        private static void AddATBAccount(TransactionModel transaction, double yPos, int pageNo)
        {
            AddText(transaction.TransactionID, "Arial", 14, true, 0, Colors.Black, 635, yPos, pageNo);
            if (transaction.TransactionType== TransactionTypes.Credit)
            AddText(transaction.TransactionID, "Arial", 14, true, 0, Colors.Black, 635, yPos, pageNo);
        }

        private static void AddATBCreditTotals(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 635, 0, pageNo);
        }

        private static void AddATBDebitTotals(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 635, 0, pageNo);
        }

        #endregion
    }
}
