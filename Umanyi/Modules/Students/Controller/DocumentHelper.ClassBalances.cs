using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        #region Balances

        private static void AddBLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 100, 85, pageNo);
        }
        private static void AddBLDate(DateTime dateTime, int pageNo)
        {
            AddText(dateTime.ToString("dd MMM yyyy"), "Arial", 14, true, 0, Colors.Black, 600, 85, pageNo);
        }

        private static void AddBLTotal(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 16, true, 0, Colors.Black, 80, 1050, pageNo);
        }

        private static void AddBLTotalUnpaid(decimal total, int pageNo)
        {
            AddText(total.ToString("N2"), "Arial", 16, true, 0, Colors.Black, 310, 1050, pageNo);
        }

        private static void AddBLStudentBalance(StudentFeesDefaultModel item, int itemIndex, int pageNo)
        {
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 165 + pageRelativeIndex * 25;

            AddText(item.StudentID.ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(item.NameOfStudent, "Arial", fontsize, false, 0, Colors.Black, 175, yPos, pageNo);
            AddText(item.Balance.ToString("N2"), "Arial", fontsize, false, 0, Colors.Black, 445, yPos, pageNo);
            AddText(item.GuardianPhoneNo, "Arial", fontsize, false, 0, Colors.Black, 580, yPos, pageNo);
        }
        private static void AddBLStudentBalances(ObservableCollection<StudentFeesDefaultModel> psi, int pageNo)
        {

            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Count)
                return;
            if (endIndex >= psi.Count)
                endIndex = psi.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddBLStudentBalance(psi[i], i, pageNo);
        }

        private static void GenerateBalanceList()
        {
            ClassBalancesListModel si = myWorkObject as ClassBalancesListModel;
            
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddBLClass(si.NameOfClass, pageNo);
                AddBLDate(si.Date, pageNo);
                if (pageNo == 0)
                {
                    AddBLTotal(si.Total, pageNo);
                    AddBLTotalUnpaid(si.TotalUnpaid, pageNo);
                }
                AddBLStudentBalances(si.Entries, pageNo);
            }
        }
        #endregion
    }
}
