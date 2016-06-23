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
            AddAISRevenueAccounts(si.RevenueEntries,si.StartTime,si.EndTime);
            AddAISExpenseAccounts(si);
            AddAISOtherItems(si);
        }

        private static int CalculatePagesAccountsIncomeStatement(IncomeStatementModel si)
        {
            int pages = 0;
            int revPages = (si.RevenueEntries.Count % 35) != 0 ?
                (si.RevenueEntries.Count / 35) + 1 : (si.RevenueEntries.Count / 35);
            
                int lastRevYpos = 195+(si.RevenueEntries.Count-(revPages - 1) * 35)*25;
                int prevPageEnt = 0;
                if (lastRevYpos < 1000)
                {
                     prevPageEnt = ((1070 - (int)lastRevYpos) / 25);
                }

                int expPages = ((si.ExpenseEntries.Count - prevPageEnt) % 35) != 0 ?
                    ((si.ExpenseEntries.Count - prevPageEnt) / 35) + 1 : ((si.ExpenseEntries.Count - prevPageEnt) / 35);
                if (prevPageEnt >= si.ExpenseEntries.Count)
                    expPages = 0;

                if (expPages < 0)
                    expPages = 0;
                int lastExpYpos = 0;
                if (expPages==0)
                    lastExpYpos = lastRevYpos+si.ExpenseEntries.Count * 25;
                else
                 lastExpYpos = 195 + (si.ExpenseEntries.Count - prevPageEnt - (expPages - 1) * 35) * 25;
                pages = revPages + expPages;
            if (lastExpYpos<800)
            return pages;
            return pages + 1;
        }

        private static void AddAISPeriod(DateTime start, DateTime end, int pageNo)
        {
            AddText("PERIOD " + start.ToString("dd-MMM-yyyy") + " to " + end.ToString("dd-MMM-yyyy"), "Arial", 14, true, 0, Colors.Black, 290, 115, pageNo);
        }
        private static void AddAISExpenseAccounts(IncomeStatementModel si)
        {
            int expStartIndex = 0;
            int expEndIndex = 35;
            if (expStartIndex >= si.ExpenseEntries.Count)
                return;
            if (expEndIndex >= si.ExpenseEntries.Count)
                expEndIndex = si.ExpenseEntries.Count - 1;

            int revPages = (si.RevenueEntries.Count % 35) != 0 ?
                (si.RevenueEntries.Count / 35) + 1 : (si.RevenueEntries.Count / 35);

            int lastRevYpos = 195 + (si.RevenueEntries.Count - (revPages - 1) * 35) * 25 + 60;
            int prevPageEnt = 0;
            if (lastRevYpos < 1000)
            {
                prevPageEnt = ((1070 - (int)lastRevYpos) / 25);
                expEndIndex = prevPageEnt;
                if (expEndIndex > si.ExpenseEntries.Count)
                    expEndIndex = si.ExpenseEntries.Count;
                AddAISSubtitle("EXPENSES", lastRevYpos - 30, revPages - 1);
                for (int i = expStartIndex; i < expEndIndex; i++)
                {
                    AddAISExpenseAccount(si.ExpenseEntries[i].Name, si.ExpenseEntries[i].Balance, lastRevYpos + (i * 25), revPages - 1);
                }
                expStartIndex = expEndIndex;
                expEndIndex = expStartIndex + 35;
            }
            else
                AddAISSubtitle("EXPENSES", 160, revPages);

            if (expStartIndex >= si.ExpenseEntries.Count)
                return;
            if (expEndIndex >= si.ExpenseEntries.Count)
                expEndIndex = si.ExpenseEntries.Count - 1;


            int expPages = ((si.ExpenseEntries.Count - prevPageEnt) % 35) != 0 ?
                ((si.ExpenseEntries.Count - prevPageEnt) / 35) + 1 : ((si.ExpenseEntries.Count - prevPageEnt) / 35);

            int lastYPos = 0;
            int lastPage = 0;
            for (int pageNo = 0; pageNo < expPages; pageNo++)
            {
                lastPage = revPages + pageNo;
                AddAISPeriod(si.StartTime, si.EndTime, lastPage);

                if (expStartIndex >= si.ExpenseEntries.Count)
                    return;
                if (expEndIndex >= si.ExpenseEntries.Count)
                    expEndIndex = si.ExpenseEntries.Count - 1;

                for (int i = expStartIndex; i < expEndIndex; i++)
                {
                    lastYPos = 195 + ((i - prevPageEnt - 35 * pageNo) * 25);
                    AddAISExpenseAccount(si.ExpenseEntries[i].Name, si.ExpenseEntries[i].Balance, lastYPos, lastPage);
                }
                expStartIndex = expEndIndex;
                expEndIndex = expStartIndex + 35;
            }
            decimal d = 0;
            foreach (var y in si.ExpenseEntries)
                d += y.Balance;
            AddAISTotals(d, lastYPos, lastPage);
        }

        private static void AddAISSubtitle(string text,double yPos,int pageNo)
        {
            AddText(text, "Segoe UI", 14, true, 0, Colors.Black, 30, yPos, pageNo);
        }
        private static void AddAISRevenueAccounts(IList<AccountModel>accounts,DateTime startTime, DateTime endTime)    
         {
             int revPages = (accounts.Count % 35) != 0 ?
                (accounts.Count / 35) + 1 : (accounts.Count / 35);


             double lastRevYpos = 0;
             int lastRevPage = 0;
             AddAISSubtitle("REVENUES",160,0);
             for (int pageNo = 0; pageNo < revPages; pageNo++)
             {
                 AddAISPeriod(startTime, endTime, pageNo);
                 int startIndex = pageNo * 35;
                 int endIndex = startIndex + 35 - 1;
                 if (startIndex >= accounts.Count)
                     return;
                 if (endIndex >= accounts.Count)
                     endIndex = accounts.Count - 1;

                 for (int i = startIndex; i <= endIndex; i++)
                 {
                     lastRevYpos = 195 + ((i - 35 * pageNo) * 25);

                     AddAISRevenueAccount(accounts[i].Name, accounts[i].Balance, lastRevYpos, pageNo);
                     
                     lastRevPage = pageNo;
                 }
             }
            decimal d=0;
            foreach(var y in accounts)
                d+=y.Balance;
            AddAISTotals(d, lastRevYpos, lastRevPage);
         }


        private static void AddAISRevenueAccount(string name,decimal amount,double yPos, int pageNo)
        {
            AddText(name, "Arial", 14, false, 0, Colors.Black, 52, yPos, pageNo);
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 350, yPos, pageNo);
        }

        private static void AddAISExpenseAccount(string name, decimal amount, double yPos, int pageNo)
        {
            AddText(name, "Arial", 14, false, 0, Colors.Black, 52, yPos, pageNo);
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 350, yPos, pageNo);
        }

        private static void AddAISTotals(decimal amount, double ypos, int pageNo)
        {
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 590, ypos+25, pageNo);
        }


        private static void AddAISOtherItems(IncomeStatementModel si)
        {
            int pages = 0;
            int ypos=0;
            int revPages = (si.RevenueEntries.Count % 35) != 0 ?
                (si.RevenueEntries.Count / 35) + 1 : (si.RevenueEntries.Count / 35);

            int lastRevYpos = 195 + (si.RevenueEntries.Count - (revPages - 1) * 35) * 25;
            int prevPageEnt = 0;
            if (lastRevYpos < 1000)
            {
                prevPageEnt = ((1070 - (int)lastRevYpos) / 25);
            }

            int expPages = ((si.ExpenseEntries.Count - prevPageEnt) % 35) != 0 ?
                ((si.ExpenseEntries.Count - prevPageEnt) / 35) + 1 : ((si.ExpenseEntries.Count - prevPageEnt) / 35);
            if (prevPageEnt >= si.ExpenseEntries.Count)
                expPages = 0;

            if (expPages < 0)
                expPages = 0;
            int lastExpYpos = 0;
            if (expPages == 0)
                lastExpYpos = lastRevYpos + si.ExpenseEntries.Count * 25;
            else
                lastExpYpos = 195 + (si.ExpenseEntries.Count - prevPageEnt - (expPages - 1) * 35) * 25;
            pages = revPages + expPages;
            ypos = lastExpYpos+120;
            if (lastExpYpos > 800)
            {
                pages++;
                ypos=195;
            }
            decimal rev=0;
            decimal exp=0;
            foreach(var t in si.RevenueEntries)
            rev+=t.Balance;

            foreach(var t in si.ExpenseEntries)
           exp+=t.Balance;



            AddAISSubtitle("OPERATING INCOME", ypos, pages - 1);
            AddAISTotals(rev - exp, ypos-25, pages - 1);
            AddAISSubtitle("OTHER GAINS AND LOSSES", ypos+60, pages - 1);
            AddAISSubtitle("         -", ypos + 100, pages - 1);
            AddAISSubtitle("INCOME BEFORE TAXES", ypos + 140, pages - 1);
            AddAISTotals(rev - exp, ypos+115, pages - 1);
            AddAISSubtitle("INCOME TAX EXPENSES", ypos + 180, pages - 1);
            AddAISTotals(0, ypos + 155, pages - 1);
            AddAISSubtitle("NET INCOME", ypos + 240, pages - 1);
            AddAISTotals(rev - exp, ypos + 215, pages - 1);

        }

        
        #endregion

        #region BalanceSheet
        private static void GenerateAccountsBalanceSheet()
        {
            
        }
        #endregion

        #region Statement of Cashflows Statement
        private static int CalculatePagesAccountsSTOC( STCashFlowsModel si)
        {
            int pages = 0;
            int revPages = (si.OperatingActivitiesEntries.Count % 35) != 0 ?
                (si.OperatingActivitiesEntries.Count / 35) + 1 : (si.OperatingActivitiesEntries.Count / 35);

            int lastRevYpos = 195 + (si.OperatingActivitiesEntries.Count - (revPages - 1) * 35) * 25;
            pages += revPages;
            if (lastRevYpos < 800)            
                return pages;
            return pages + 1;
        }
        private static void GenerateAccountsSTofCashFlows()
        {
            STCashFlowsModel si = myWorkObject as STCashFlowsModel;
            AddASTOCActivityEntries(si);
            AddASTOCOtherItems(si);
        }

        private static void AddASTOCActivityEntries(STCashFlowsModel stoc)
        {
            int revPages = (stoc.OperatingActivitiesEntries.Count % 35) != 0 ?
                (stoc.OperatingActivitiesEntries.Count / 35) + 1 : (stoc.OperatingActivitiesEntries.Count / 35);


            double lastRevYpos = 0;
            int lastRevPage = 0;
            AddAISSubtitle("CASH FROM OPERATING ACTIVITIES", 160, 0);
            for (int pageNo = 0; pageNo < revPages; pageNo++)
            {
                AddAISPeriod(stoc.StartTime, stoc.EndTime, pageNo);
                int startIndex = pageNo * 35;
                int endIndex = startIndex + 35 - 1;
                if (startIndex >= stoc.OperatingActivitiesEntries.Count)
                    return;
                if (endIndex >= stoc.OperatingActivitiesEntries.Count)
                    endIndex = stoc.OperatingActivitiesEntries.Count - 1;

                for (int i = startIndex; i <= endIndex; i++)
                {
                    lastRevYpos = 195 + ((i - 35 * pageNo) * 25);

                    AddAISRevenueAccount(stoc.OperatingActivitiesEntries[i].Name, stoc.OperatingActivitiesEntries[i].Balance, lastRevYpos, pageNo);

                    lastRevPage = pageNo;
                }
            }
            decimal d = 0;
            foreach (var y in stoc.OperatingActivitiesEntries)
                d += y.Balance;
            AddASTOCSubtitle("TOTAL INCOME FROM OPERATING ACTIVITIES", lastRevYpos + 40, lastRevPage);
            AddASTOCTotals(d, lastRevYpos+40, lastRevPage);
        }

        private static void AddASTOCSubtitle(string text, double yPos, int pageNo)
        {
            AddText(text, "Arial", 14, true, 0, Colors.Black, 20, yPos, pageNo);
        }
        private static void AddASTOCTotals(decimal amount, double yPos, int pageNo)
        {            
            AddText(amount.ToString("N2"), "Arial", 14, true, 0, Colors.Black, 635, yPos, pageNo); 
        }

        private static void AddASTOCOtherItems(STCashFlowsModel si)
        {
            int pages = 0;
            int ypos = 0;
            int revPages = (si.OperatingActivitiesEntries.Count % 35) != 0 ?
                (si.OperatingActivitiesEntries.Count / 35) + 1 : (si.OperatingActivitiesEntries.Count / 35);

            int lastRevYpos = 195 + (si.OperatingActivitiesEntries.Count - (revPages - 1) * 35) * 25;
            
           

            pages = revPages;
            ypos = lastRevYpos + 120;
            if (lastRevYpos > 800)
            {
                pages++;
                ypos = 195;
            }

            AddASTOCSubtitle("CASH FROM INVESTING ACTIVIYIES", ypos, pages - 1);
            AddASTOCSubtitle("         -", ypos + 35, pages - 1);
            AddASTOCSubtitle("CASH FROM FINANCING ACTIVIYIES", ypos + 85, pages - 1);
            AddASTOCSubtitle("         -", ypos + 110, pages - 1);
            AddASTOCSubtitle("STARTING CASH BALANCE", ypos + 170, pages - 1);
            AddASTOCTotals(si.StartBalance, ypos + 170, pages - 1);
            AddASTOCSubtitle("CURRENT CASH BALANCE", ypos + 210, pages - 1);
            AddASTOCTotals(si.EndBalance, ypos + 210, pages - 1);
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
