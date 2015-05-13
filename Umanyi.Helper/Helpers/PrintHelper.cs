using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Helper
{
    public static class PrintHelper
    {
        public static void PrintVisual(Visual visual)
        {
            if (visual == null)
                return;
            string s = GetXps(visual);
            PrintXPS(s);
        }

        private static string GetXps(Visual visual)
        {
            string s = Path.GetTempFileName();
            Package package = Package.Open(s, FileMode.Create);
            XpsDocument doc = new XpsDocument(package, CompressionOption.NotCompressed);
            XpsDocumentWriter xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
            VisualsToXpsDocument vToXpsD = (VisualsToXpsDocument)xpsdw.CreateVisualsCollator();
            vToXpsD.WriteAsync(visual);
            vToXpsD.EndBatchWrite();
            doc.Close();
            package.Close();
            return s;
        }

        public static void PrintXPSDirect(string filePath)
        {
            try
            {
                LocalPrintServer localPrintServer = new LocalPrintServer();
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                if (!File.Exists(filePath))
                    throw new ArgumentException("The file specified does not exist.");
                FileInfo f = new FileInfo(filePath);
                try
                {
                    PrintSystemJobInfo xpsPrintJob = defaultPrintQueue.AddJob(f.Name, filePath, false);
                }
                catch (PrintJobException e)
                {
                    Log.E(e.ToString(), null);
                }
                catch (Exception e)
                {
                    Log.E(e.ToString(), null);
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
            }
        }
        public static void PrintXPS(string filePath)
        {
            try
            {
                PrintDialog pDialog = new PrintDialog();
                pDialog.PageRangeSelection = PageRangeSelection.AllPages;
                pDialog.UserPageRangeEnabled = true;

                Nullable<Boolean> print = pDialog.ShowDialog();
                if (print == true)
                {
                    XpsDocument xpsDocument = new XpsDocument(filePath, FileAccess.ReadWrite);
                    FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
                    pDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Timetable Print");
                }
            }
            catch (Exception e)
            {
                Log.E(e.ToString(), null);
            }
        }
    }


}

