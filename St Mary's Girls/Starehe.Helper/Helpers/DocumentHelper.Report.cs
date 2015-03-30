using Helper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Helper
{
    public static partial class DocumentHelper
    {
        private static void AddRPTitle(string title, int pageNo)
        {
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
            double fontsize = 14;
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 355 + pageRelativeIndex * 25;

            AddText(entry[0].ToString(), "Arial", 14, false, 0, Colors.Black, 45, yPos, pageNo);
            AddText(entry[1].ToString(), "Arial", fontsize, false, 0, Colors.Black, 285, yPos, pageNo);
            AddText(entry[2].ToString(), "Arial", fontsize, false, 0, Colors.Black, 475, yPos, pageNo);
            AddText(entry[3].ToString(), "Arial", fontsize, false, 0, Colors.Black, 630, yPos, pageNo);
        }

        private static void GenerateReport()
        {
            AcademicReportModel si = myWorkObject as AcademicReportModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddRPTitle(si.Title, pageNo);
                AddRPItems(si.Entries, pageNo);
            }
        }
    }
}
