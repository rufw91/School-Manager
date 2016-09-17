using Helper.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Helper
{
    public static class XPSHelper
    {
        public static string CreatePDFromFixedDocument(FixedDocument document)
        {
            try
            {
                string file = Path.GetTempFileName() + ".xps";
                using (FileStream ms = File.Open(file, FileMode.Create))
                {
                    Package p = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
                    XpsDocument doc = new XpsDocument(p, CompressionOption.NotCompressed);

                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(document.DocumentPaginator);
                    p.Flush(); ms.Close();
                }


                if (!string.IsNullOrWhiteSpace(file))
                {
                    string pdfPath = Path.GetTempFileName() + ".pdf";
                    Log.E(pdfPath,null);
                    try
                    {
                        string path = new FileInfo(Application.ResourceAssembly.Location).DirectoryName + "\\gxps-9.15-win32.exe";
                        Process.Start(path, "-sDEVICE=pdfwrite -sOutputFile=" +
                  pdfPath +
                  " -dNOPAUSE " +
                  file).WaitForExit();
                        return pdfPath;
                    }
                    catch (Exception e) { Log.E(e.ToString(), null); }
                }

            }
            catch (Exception e) { Log.E(e.ToString(), null); }
            return "";
        }

        public static string CreatePDFFromDataGrid(DataGrid dataGrid)
        {
            try
            {
                var d = DataGridToDataTable.Convert(dataGrid);
                string file= GetXpsDocumentFromTable(d, dataGrid);
                
                if (!string.IsNullOrWhiteSpace(file))
                {
                    string pdfPath = Path.GetTempFileName() + ".pdf";
                    try
                    {
                        string path = new FileInfo(Application.ResourceAssembly.Location).DirectoryName+"\\gxps-9.15-win32.exe";
                        Process.Start(path, "-sDEVICE=pdfwrite -sOutputFile=" +
                  pdfPath +
                  " -dNOPAUSE " +
                  file).WaitForExit();
                        return pdfPath;
                    }
                    catch (Exception e) { Log.E(e.ToString(), null); }
                }

            }
            catch (Exception e) { Log.E(e.ToString(), null); }
            return "";
        }

        private static string GetXpsDocumentFromTable(DataTable dataTable, DataGrid dataGrid)
        {
            FlowDocument flowDoc = new FlowDocument();
            flowDoc.ColumnWidth = 999999;
            Table table1 = new Table();
            flowDoc.Blocks.Add(table1);
            table1.FontSize = 11;
            table1.CellSpacing = 10;
            table1.Background = Brushes.White;

            int numberOfColumns = dataTable.Columns.Count;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table1.Columns.Add(new TableColumn());
                
                if (x % 2 == 0)
                    table1.Columns[x].Background = Brushes.White;
                else
                    table1.Columns[x].Background = Brushes.WhiteSmoke;
            }
            table1.RowGroups.Add(new TableRowGroup());
            table1.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = table1.RowGroups[0].Rows[0];


            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Bold;

            foreach (DataColumn dc in dataTable.Columns)
            {
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(dc.ColumnName))));
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                table1.RowGroups[0].Rows.Add(new TableRow());
                currentRow = table1.RowGroups[0].Rows[i + 1];
                currentRow.FontSize = 11;
                currentRow.FontWeight = FontWeights.Normal;
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(dataTable.Rows[i][j].ToString()))));
                }
            }
            string file = Path.GetTempFileName() + ".xps";
            using (FileStream ms = File.Open(file, FileMode.Create))
            {
                Package p = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
                using (XpsDocument doc = new XpsDocument(p, CompressionOption.NotCompressed))
                {
                    var paginator = ((IDocumentPaginatorSource)flowDoc).DocumentPaginator;
                    paginator.PageSize = new Size(1122.52, 793.70);

                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(paginator);
                    p.Flush(); ms.Close();
                }
            }
            return file;
        }


    }
}
