using System;
using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace UmanyiSMS.Lib.Controllers
{
    public static class PrintHelper
    {
        public static void PrintVisual(Visual visual)
        {
            if (visual != null)
            {
                string xps = PrintHelper.GetXps(visual);
                PrintHelper.PrintXPS(xps);
            }
        }

        private static string GetXps(Visual visual)
        {
            string tempFileName = Path.GetTempFileName();
            Package package = Package.Open(tempFileName, FileMode.Create);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.NotCompressed);
            XpsDocumentWriter xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            VisualsToXpsDocument visualsToXpsDocument = (VisualsToXpsDocument)xpsDocumentWriter.CreateVisualsCollator();
            visualsToXpsDocument.WriteAsync(visual);
            visualsToXpsDocument.EndBatchWrite();
            xpsDocument.Close();
            package.Close();
            return tempFileName;
        }

        public static void PrintXPSDirect(string filePath)
        {
            try
            {
                LocalPrintServer localPrintServer = new LocalPrintServer();
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                if (!File.Exists(filePath))
                {
                    throw new ArgumentException("The file specified does not exist.");
                }
                FileInfo fileInfo = new FileInfo(filePath);
                try
                {
                    PrintSystemJobInfo printSystemJobInfo = defaultPrintQueue.AddJob(fileInfo.Name, filePath, false);
                }
                catch (PrintJobException ex)
                {
                    Log.E(ex.ToString(), null);
                }
                catch (Exception ex2)
                {
                    Log.E(ex2.ToString(), null);
                }
            }
            catch (Exception ex2)
            {
                Log.E(ex2.ToString(), null);
            }
        }

        public static void PrintXPS(string filePath)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.PageRangeSelection = PageRangeSelection.AllPages;
                printDialog.UserPageRangeEnabled = true;
                if (printDialog.ShowDialog() == true)
                {
                    XpsDocument xpsDocument = new XpsDocument(filePath, FileAccess.ReadWrite);
                    FixedDocumentSequence fixedDocumentSequence = xpsDocument.GetFixedDocumentSequence();
                    printDialog.PrintDocument(fixedDocumentSequence.DocumentPaginator, "Timetable Print");
                }
            }
            catch (Exception ex)
            {
                Log.E(ex.ToString(), null);
            }
        }
    }
}
