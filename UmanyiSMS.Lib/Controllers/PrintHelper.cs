using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Management;
using System.Printing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using UmanyiSMS.Lib.Models;

namespace UmanyiSMS.Lib.Controllers
{
    public static class PrintHelper
    {
        public enum PrinterLocation
        {
            Local, Network
        }

        public static void PrintRaw(string printerName, byte[] document)
        {
            NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;

            documentInfo = new NativeMethods.DOC_INFO_1();
            documentInfo.pDataType = "RAW";
            documentInfo.pDocName = "Document";

            printerHandle = new IntPtr(0);

            if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
            {
                if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
                {
                    int bytesWritten;
                    byte[] managedData;
                    IntPtr unmanagedData;

                    managedData = document;
                    unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                    Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);

                    if (NativeMethods.StartPagePrinter(printerHandle))
                    {
                        NativeMethods.WritePrinter(
                            printerHandle,
                            unmanagedData,
                            managedData.Length,
                            out bytesWritten);
                        NativeMethods.EndPagePrinter(printerHandle);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }

                    Marshal.FreeCoTaskMem(unmanagedData);

                    NativeMethods.EndDocPrinter(printerHandle);
                }
                else
                {
                    throw new Win32Exception();
                }

                NativeMethods.ClosePrinter(printerHandle);
            }
            else
            {
                throw new Win32Exception();
            }

        }

        public static void PrintRaw(byte[] document)
        {
            string name = GetDefaultPrinterName().Result;
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No default printer set.");
            PrintRaw(name, document);
        }

        public static void PrintXPS(FixedDocument document, bool showDialog)
        {
            if (document == null)
                throw new ArgumentNullException("Document cannot be null.");
            PrintXPS(document.DocumentPaginator, showDialog);
            return;
        }

        public static void PrintXPS(DocumentPaginator document, bool showDialog)
        {
            string printerName = GetDefaultPrinterName().Result;
            if (string.IsNullOrWhiteSpace(printerName))
                throw new InvalidOperationException("No default printer set.");
            PrintXPS(printerName, document, showDialog);
            return;
        }

        public static void PrintXPS(string printerName, FixedDocument document, bool showDialog)
        {
            if (document == null)
                throw new ArgumentNullException("Document cannot be null.");
            PrintXPS(printerName, document.DocumentPaginator, showDialog);
            return;
        }

        public static void PrintXPS(string printerName, DocumentPaginator document, bool showDialog)
        {
            if (document == null)
                throw new ArgumentNullException("Document cannot be null.");
            SetDefault(printerName);
            PrintDialog printDialog = new PrintDialog();
            PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            if (defaultPrintQueue == null)
                throw new InvalidOperationException("No printer selected.");

            if (!defaultPrintQueue.IsXpsDevice)
                throw new InvalidOperationException("Printer does not support XPS format.");
            printDialog.PrintQueue = defaultPrintQueue;
            printDialog.UserPageRangeEnabled = true;
            PrintTicket defaultPrintTicket = defaultPrintQueue.DefaultPrintTicket;
            defaultPrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            printDialog.PrintTicket = defaultPrintTicket;

            if (printDialog.ShowDialog() == true)
            {
                var paginator = document;
                if (printDialog.PageRangeSelection == PageRangeSelection.UserPages)
                {
                    paginator = new PageRangeDocumentPaginator(
                                    paginator,
                                     printDialog.PageRange);
                    printDialog.PrintDocument(paginator, "Umanyi POS Document");
                }
                else if (printDialog.PageRangeSelection == PageRangeSelection.AllPages)
                    printDialog.PrintDocument(paginator, "Umanyi POS Document");

            }
            return;
        }

        public static void PrintXPS(FixedDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("Document cannot be null.");
            PrintXPS(document.DocumentPaginator);
        }

        public static void PrintXPS(DocumentPaginator document)
        {
            string name = GetDefaultPrinterName().Result;
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("No default printer set.");
            PrintXPS(name, document, true);
        }

        public static Task<bool> PrintTestPage(string nameOfPrinter)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    var q = new ManagementObjectSearcher("SELECT * from Win32_Printer where name ='" + nameOfPrinter + "'").Get();

                    foreach (ManagementObject p in q)
                    {
                        var v = p.InvokeMethod("PrintTestPage", null);
                        break;
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        public static Task<bool> SetDefault(string nameOfPrinter)
        {
            return Task.Run<bool>(() =>
            {
                try
                {
                    var q = new ManagementObjectSearcher("SELECT * from Win32_Printer where name ='" + nameOfPrinter + "'").Get();

                    foreach (ManagementObject p in q)
                    {
                        p.InvokeMethod("SetDefaultPrinter", null);
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        private static Task<string> GetDefaultPrinterName()
        {
            return Task.Run<string>(() =>
            {
                string s = "";
                try
                {
                    var q = new ManagementObjectSearcher("SELECT * from Win32_Printer where default =True").Get();

                    if (q.Count == 0)
                        return "";
                    foreach (ManagementObject p in q)
                    {
                        s = p["Name"].ToString();
                        break;
                    }
                    return s;
                }
                catch { return ""; }
            });
        }

        public static Task<ObservableCollection<PrinterHolderModel>> GetAllPrinters()
        {
            return Task.Run<ObservableCollection<PrinterHolderModel>>(() =>
            {
                ObservableCollection<PrinterHolderModel> temp = new ObservableCollection<PrinterHolderModel>();

                var q = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                foreach (var p in q.Get())
                {
                    PrinterHolderModel gt = new PrinterHolderModel();

                    gt.Name = p.GetPropertyValue("Name").ToString();
                    if (gt.Name.ToLowerInvariant().Contains("microsoft xps document writer") || gt.Name.Equals("Fax"))
                        continue;
                    gt.Description = p.GetPropertyValue("Caption").ToString();
                    gt.Status = int.Parse(p.GetPropertyValue("PrinterStatus").ToString());
                    gt.IsDefault = bool.Parse(p.GetPropertyValue("Default").ToString());
                    gt.Location = bool.Parse(p.GetPropertyValue("Network").ToString()) ? PrinterLocation.Network : PrinterLocation.Local;
                    temp.Add(gt);
                }

                return temp;
            });
        }

        private class PageRangeDocumentPaginator : DocumentPaginator
        {
            private int _startIndex;
            private int _endIndex;
            private DocumentPaginator _paginator;
            public PageRangeDocumentPaginator(
              DocumentPaginator paginator,
              PageRange pageRange)
            {
                if (pageRange.PageFrom > pageRange.PageTo)
                {
                    _startIndex = pageRange.PageTo - 1;
                    _endIndex = pageRange.PageFrom - 1;
                }
                else if (pageRange.PageFrom < pageRange.PageTo)
                {
                    _startIndex = pageRange.PageFrom - 1;
                    _endIndex = pageRange.PageTo - 1;
                }
                _paginator = paginator;
                if (_startIndex >= _paginator.PageCount)
                    throw new ArgumentOutOfRangeException("Page range is not valid.");
                _endIndex = Math.Min(_endIndex, _paginator.PageCount - 1);
            }

            public override DocumentPage GetPage(int pageNumber)
            {
                var page = _paginator.GetPage(_startIndex == _endIndex ? pageNumber : pageNumber + _startIndex);

                var cv = new ContainerVisual();
                if (page.Visual is FixedPage)
                {
                    foreach (UIElement child in ((FixedPage)page.Visual).Children)
                    {
                        var childClone = (UIElement)child.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(child, null);

                        var parentField = childClone.GetType().GetField("_parent",
                                                                       BindingFlags.Instance | BindingFlags.NonPublic);
                        if (parentField != null)
                        {
                            parentField.SetValue(childClone, null);
                            cv.Children.Add(childClone);
                        }
                    }

                    return new DocumentPage(cv, page.Size, page.BleedBox, page.ContentBox);
                }

                return page;
            }

            public override bool IsPageCountValid
            {
                get { return true; }
            }

            public override int PageCount
            {
                get
                {
                    if (_startIndex < 0)
                        return 1;
                    if (_startIndex > _paginator.PageCount - 1)
                        return 0;
                    if (_startIndex > _endIndex)
                        return 0;

                    return _endIndex - _startIndex + 1;

                }
            }

            public override Size PageSize
            {
                get { return _paginator.PageSize; }
                set { _paginator.PageSize = value; }
            }

            public override IDocumentPaginatorSource Source
            {
                get { return _paginator.Source; }
            }
        }

        internal static class NativeMethods
        {
            /// <summary>
            /// An enumeration of GetDeviceCaps parameters.
            /// </summary>
            internal enum DeviceCap : int
            {
                /// <summary>
                /// Device driver version
                /// </summary>
                DRIVERVERSION = 0,

                /// <summary>
                /// Device classification
                /// </summary>
                TECHNOLOGY = 2,

                /// <summary>
                /// Horizontal size in millimeters
                /// </summary>
                HORZSIZE = 4,

                /// <summary>
                /// Vertical size in millimeters
                /// </summary>
                VERTSIZE = 6,

                /// <summary>
                /// Horizontal width in pixels
                /// </summary>
                HORZRES = 8,

                /// <summary>
                /// Vertical height in pixels
                /// </summary>
                VERTRES = 10,

                /// <summary>
                /// Number of bits per pixel
                /// </summary>
                BITSPIXEL = 12,

                /// <summary>
                /// Number of planes
                /// </summary>
                PLANES = 14,

                /// <summary>
                /// Number of brushes the device has
                /// </summary>
                NUMBRUSHES = 16,

                /// <summary>
                /// Number of pens the device has
                /// </summary>
                NUMPENS = 18,

                /// <summary>
                /// Number of markers the device has
                /// </summary>
                NUMMARKERS = 20,

                /// <summary>
                /// Number of fonts the device has
                /// </summary>
                NUMFONTS = 22,

                /// <summary>
                /// Number of colors the device supports
                /// </summary>
                NUMCOLORS = 24,

                /// <summary>
                /// Size required for device descriptor
                /// </summary>
                PDEVICESIZE = 26,

                /// <summary>
                /// Curve capabilities
                /// </summary>
                CURVECAPS = 28,

                /// <summary>
                /// Line capabilities
                /// </summary>
                LINECAPS = 30,

                /// <summary>
                /// Polygonal capabilities
                /// </summary>
                POLYGONALCAPS = 32,

                /// <summary>
                /// Text capabilities
                /// </summary>
                TEXTCAPS = 34,

                /// <summary>
                /// Clipping capabilities
                /// </summary>
                CLIPCAPS = 36,

                /// <summary>
                /// Bitblt capabilities
                /// </summary>
                RASTERCAPS = 38,

                /// <summary>
                /// Length of the X leg
                /// </summary>
                ASPECTX = 40,

                /// <summary>
                /// Length of the Y leg
                /// </summary>
                ASPECTY = 42,

                /// <summary>
                /// Length of the hypotenuse
                /// </summary>
                ASPECTXY = 44,

                /// <summary>
                /// Shading and Blending caps
                /// </summary>
                SHADEBLENDCAPS = 45,

                /// <summary>
                /// Logical pixels inch in X
                /// </summary>
                LOGPIXELSX = 88,

                /// <summary>
                /// Logical pixels inch in Y
                /// </summary>
                LOGPIXELSY = 90,

                /// <summary>
                /// Number of entries in physical palette
                /// </summary>
                SIZEPALETTE = 104,

                /// <summary>
                /// Number of reserved entries in palette
                /// </summary>
                NUMRESERVED = 106,

                /// <summary>
                /// Actual color resolution
                /// </summary>
                COLORRES = 108,

                /// <summary>
                /// Physical Width in device units
                /// </summary>
                PHYSICALWIDTH = 110,

                /// <summary>
                /// Physical Height in device units
                /// </summary>
                PHYSICALHEIGHT = 111,

                /// <summary>
                /// Physical Printable Area x margin
                /// </summary>
                PHYSICALOFFSETX = 112,

                /// <summary>
                /// Physical Printable Area y margin
                /// </summary>
                PHYSICALOFFSETY = 113,

                /// <summary>
                /// Scaling factor x
                /// </summary>
                SCALINGFACTORX = 114,

                /// <summary>
                /// Scaling factor y
                /// </summary>
                SCALINGFACTORY = 115,

                /// <summary>
                /// Current vertical refresh rate of the display device (for displays only) in Hz
                /// </summary>
                VREFRESH = 116,

                /// <summary>
                /// Horizontal width of entire desktop in pixels
                /// </summary>
                DESKTOPVERTRES = 117,

                /// <summary>
                /// Vertical height of entire desktop in pixels
                /// </summary>
                DESKTOPHORZRES = 118,

                /// <summary>
                /// Preferred blt alignment
                /// </summary>
                BLTALIGNMENT = 119
            }

            /// <summary>
            /// The CreateDC function creates a device context (DC) for a device 
            /// using the specified name.
            /// </summary>
            /// <param name="lpszDriver">Pointer to a null-terminated character
            /// string that specifies either DISPLAY or the name of a specific 
            /// display device or the name of a print provider, which is usually WINSPOOL.</param>
            /// <param name="lpszDevice">Pointer to a null-terminated character string 
            /// that specifies the name of the specific output device being used, 
            /// as shown by the Print Manager (for example, Epson FX-80). It is not 
            /// the printer model name. The lpszDevice parameter must be used.</param>
            /// <param name="lpszOutput">This parameter is ignored and should be set
            /// to NULL. It is provided only for compatibility with 16-bit Windows.</param>
            /// <param name="lpInitData">Pointer to a DEVMODE structure containing 
            /// device-specific initialization data for the device driver. The 
            /// DocumentProperties function retrieves this structure filled in for
            /// a specified device. The lpInitData parameter must be NULL if the
            /// device driver is to use the default initialization (if any) specified
            /// by the user.</param>
            /// <returns>If the function succeeds, the return value is the handle
            /// to a DC for the specified device. If the function fails, the 
            /// return value is NULL. The function will return NULL for a DEVMODE
            /// structure other than the current DEVMODE.</returns>
            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr CreateDC(
                string lpszDriver,
                string lpszDevice,
                string lpszOutput,
                IntPtr lpInitData);

            /// <summary>
            /// The DeleteDC function deletes the specified device context (DC).
            /// </summary>
            /// <param name="hdc">Handle to the device context.</param>
            /// <returns>If the function succeeds, the return value is nonzero. 
            /// If the function fails, the return value is zero.</returns>
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteDC(IntPtr hdc);

            [DllImport(
                "winspool.drv",
                EntryPoint = "OpenPrinterW",
                SetLastError = true,
                CharSet = CharSet.Unicode,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenPrinter(
                [MarshalAs(UnmanagedType.LPWStr)] string szPrinter,
                out IntPtr hPrinter,
                IntPtr pd);

            /// <summary>
            /// Closes the specified printer object.
            /// </summary>
            /// <param name="hPrinter">Handle to the printer object to be closed.
            /// This handle is returned by the OpenPrinter or AddPrinter function.</param>
            /// <returns>If the function succeeds, the return value is a nonzero value.
            /// If the function fails, the return value is zero</returns>
            [DllImport(
                "winspool.drv",
                EntryPoint = "ClosePrinter",
                SetLastError = true,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ClosePrinter(IntPtr hPrinter);

            /// <summary>
            /// The StartDoc function starts a print job.
            /// </summary>
            /// <param name="hdc">Handle to the device context for the print job.</param>
            /// <param name="lpdi">Pointer to a DOCINFO structure containing the name 
            /// of the document file and the name of the output file.</param>
            /// <returns>If the function succeeds, the return value is greater than
            /// zero. This value is the print job identifier for the document.</returns>
            [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int StartDoc(IntPtr hdc, DOCINFO lpdi);

            /// <summary>
            /// The EndDoc function ends a print job.
            /// </summary>
            /// <param name="hdc">Handle to the device context for the print job.</param>
            /// <returns>If the function succeeds, the return value is greater than zero.
            /// If the function fails, the return value is less than or equal
            /// to zero.</returns>
            [DllImport("gdi32.dll")]
            internal static extern int EndDoc(IntPtr hdc);

            /// <summary>
            /// The GetDeviceCaps function retrieves device-specific information 
            /// for the specified device.
            /// </summary>
            /// <param name="hdc">Handle to the DC.</param>
            /// <param name="capindex">Specifies the item to return.</param>
            /// <returns>The return value specifies the value of the desired item.</returns>
            [DllImport("gdi32.dll")]
            internal static extern int GetDeviceCaps(IntPtr hdc, DeviceCap capindex);

            /// <summary>
            /// The StartPage function prepares the printer driver to accept data.
            /// </summary>
            /// <param name="hdc">Handle to the device context for the print job.</param>
            /// <returns>If the function succeeds, the return value is greater than zero.
            /// If the function fails, the return value is less than or equal to zero.</returns>
            [DllImport("gdi32.dll")]
            internal static extern int StartPage(IntPtr hdc);

            /// <summary>
            /// The EndPage function notifies the device that the application has
            /// finished writing to a page. This function is typically used to 
            /// direct the device driver to advance to a new page.
            /// </summary>
            /// <param name="hdc">Handle to the device context for the print job.</param>
            /// <returns>If the function succeeds, the return value is greater than zero.
            /// If the function fails, the return value is less than or equal to zero.</returns>
            [DllImport("gdi32.dll")]
            internal static extern int EndPage(IntPtr hdc);

            /// <summary>
            /// The StartDocPrinter function notifies the print spooler
            /// that a document is to be spooled for printing.
            /// </summary>
            /// <param name="hPrinter">Handle to the printer. Use the OpenPrinter or
            /// AddPrinter function to retrieve a printer handle.</param>
            /// <param name="level">Specifies the version of the structure to 
            /// which pDocInfo points. On WIndows NT/2000/XP, the value must be 1.</param>
            /// <param name="di">Pointer to a structure that describes the document to print.</param>
            /// <returns>If the function succeeds, the return value identifies the print job.
            /// If the function fails, the return value is zero. </returns>
            [DllImport(
                "winspool.drv",
                EntryPoint = "StartDocPrinterW",
                SetLastError = true,
                CharSet = CharSet.Unicode,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool StartDocPrinter(
                IntPtr hPrinter,
                int level,
                [In, MarshalAs(UnmanagedType.LPStruct)] DOC_INFO_1 di);

            [DllImport(
                "winspool.drv",
                EntryPoint = "EndDocPrinter",
                SetLastError = true,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport(
                "winspool.drv",
                EntryPoint = "StartPagePrinter",
                SetLastError = true,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport(
                "winspool.drv",
                EntryPoint = "EndPagePrinter",
                SetLastError = true,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport(
                "winspool.drv",
                EntryPoint = "WritePrinter",
                SetLastError = true,
                ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool WritePrinter(
                IntPtr hPrinter,
                IntPtr pBytes,
                int dwCount,
                out int dwWritten);

            /// <summary>
            /// The DOCINFO structure contains the input and output file names and 
            /// other information used by the StartDoc function.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal class DOCINFO
            {
                /// <summary>
                /// The size, in bytes, of the structure.
                /// </summary>
                public int cbSize = 20;

                /// <summary>
                /// Pointer to a null-terminated string that specifies the name
                /// of the document.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpszDocName;

                /// <summary>
                /// Pointer to a null-terminated string that specifies the name of 
                /// an output file. If this pointer is NULL, the output will be 
                /// sent to the device identified by the device context handle that 
                /// was passed to the StartDoc function.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpszOutput;

                /// <summary>
                /// Pointer to a null-terminated string that specifies the type of 
                /// data used to record the print job. The legal values for this 
                /// member can be found by calling EnumPrintProcessorDatatypes and 
                /// can include such values as raw, emf, or XPS_PASS. This member 
                /// can be NULL. Note that the requested data type might be ignored.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string lpszDatatype;

                /// <summary>
                /// Specifies additional information about the print job. This 
                /// member must be zero or one of the following values.
                /// </summary>
                public int fwType;
            }

            /// <summary>
            /// The DOC_INFO_1 structure describes a document that will be printed.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal class DOC_INFO_1
            {
                /// <summary>
                /// Pointer to a null-terminated string that specifies the name of
                /// the document.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pDocName;

                /// <summary>
                /// Pointer to a null-terminated string that specifies the name of
                /// an output file. To print to a printer, set this to NULL.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pOutputFile;

                /// <summary>
                /// Pointer to a null-terminated string that identifies the type 
                /// of data used to record the document.
                /// </summary>
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pDataType;
            }
        }
    }
}
