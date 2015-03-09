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
        #region Class Mark List

        private static void AddCMLClass(string nameOfClass, int pageNo)
        {
            AddText(nameOfClass, "Arial", 14, true, 0, Colors.Black, 70, 70, pageNo);
        }
        private static void AddCMLSubjects(DataTable entries, int pageNo)
        {
            double xPos = 255;
            double count = 0;

            for (int i = 2; i < entries.Columns.Count - 3; i++)
            {
                AddText(entries.Columns[i].ColumnName.ToUpper().Substring(0, 3), "Times New Roman", 14, true, 0, Colors.Black, xPos + count * 35, 110, pageNo);
                count++;
            }
        }
        private static void AddCMLStudent(DataRow item, int itemIndex, int pageNo)
        {
            int pageRelativeIndex = itemIndex - itemsPerPage * pageNo;
            double yPos = 140 + pageRelativeIndex * 26;
            double xPos = 255;
            double count = 0;
            AddText(item["Student ID"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 16, yPos, pageNo);
            AddText(item["Name"].ToString().Length > 25 ? item["Name"].ToString().Substring(0, 25) : item["Name"].ToString()
                , "Times New Roman", 12, false, 0, Colors.Black, 50, yPos, pageNo);
            for (int i = 2; i < item.ItemArray.Length - 3; i++)
            {
                AddText(item[i].ToString(), "Times New Roman", 12, false, 0, Colors.Black, xPos + count * 35, yPos, pageNo);
                count++;
            }
            AddText(item["MeanGrade"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 676, yPos, pageNo);
            AddText(item["Total"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 711, yPos, pageNo);
            AddText(item["Position"].ToString(), "Times New Roman", 12, false, 0, Colors.Black, 751, yPos, pageNo);
        }
        private static void AddCMLStudents(DataTable psi, int pageNo)
        {
            int startIndex = pageNo * itemsPerPage;
            int endIndex = startIndex + itemsPerPage - 1;
            if (startIndex >= psi.Rows.Count)
                return;
            if (endIndex >= psi.Rows.Count)
                endIndex = psi.Rows.Count - 1;

            for (int i = startIndex; i <= endIndex; i++)
                AddCMLStudent(psi.Rows[i], i, pageNo);
        }

        private static void GenerateClassMarkList()
        {
            ClassExamResultModel si = myWorkObject as ClassExamResultModel;

            int pageNo;
            for (pageNo = 0; pageNo < noOfPages; pageNo++)
            {
                AddCMLClass(si.NameOfClass, pageNo);
                AddCMLSubjects(si.Entries, pageNo);
                AddCMLStudents(si.Entries, pageNo);
            }
        }
        #endregion
    }
}
