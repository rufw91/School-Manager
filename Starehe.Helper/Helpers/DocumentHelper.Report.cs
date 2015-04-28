using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        static DataColumnCollection columns;
        static ObservableCollection<ColumnModel> columnsW = new ObservableCollection<ColumnModel>();
        private static void AddRPTitle(string title, int pageNo)
        {
            AddText(title.ToUpper(),"Segoe UI Light" ,20, false, 0, Colors.Black, -1, 60, pageNo);
        }

        private static void AddRPPageNo(int pageNo,int totalPages)
        {
            AddText("Page " + (pageNo+1).ToString() + " of " + totalPages, "Times New Roman", 11, true, 0, Colors.Black, 700, 20, pageNo);
        }

        private static void AddRPItems(DataTable entries, int pageNo)
        {            
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= entries.Rows.Count)
                return;
            if (endIndex >= entries.Rows.Count)
                endIndex = entries.Rows.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddRPItem(entries.Rows[i], i, pageNo);
        }

        private static void AddRPItem(DataRow entry, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 150 + pageRelativeIndex * 25;
            double xPos = 10;
            double currZero = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                xPos = i != 0 ? ((773.76d / columns.Count) * columnsW[i].Width) + currZero : 10;
                AddText(entry[i].ToString(), "Arial", 10, false, 0, Colors.Black, xPos, yPos, pageNo);
                currZero += i != 0 ? (773.76d / columns.Count) : 10;
               // Log.I(xPos + " ,Column [" + i + "], Column Width " + columnsW[i].Width + " xxy " + (1102d / columns.Count), null);
            }
        }

        private static void AddRPColumns(ObservableCollection<ColumnModel> columns,  int pageNo)
        {
            double xPos = 10;
            double currZero = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                xPos = i != 0 ? ((773.76d / columns.Count) * columnsW[i].Width) + currZero : 10;
                AddText(columns[i].FriendlyName, "Arial", 10, true, 0, Colors.Black, xPos, 108, pageNo);
                currZero += i != 0 ? (773.76d / columns.Count) : 10;
            }
        }

        private static void GenerateReport()
        {
            ReportModel si = myWorkObject as ReportModel;
            columns = si.Entries.Columns;
            columnsW = si.Columns;

            var f = columns == null;
            var ft = columnsW == null;
            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddRPPageNo(pageNo, noOfPages);
                AddRPTitle(si.Title, pageNo);
                AddRPColumns(si.Columns, pageNo);
                AddRPItems(si.Entries, pageNo);
            }
        }


    }
}
