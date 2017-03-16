using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UmanyiSMS.Controls
{
    public partial class PDFViewer : UserControl
    {
        private string pdfFilePath;

        public PDFViewer()
        {
            InitializeComponent();
            acrobatViewer.setShowToolbar(true);
            acrobatViewer.setShowScrollbars(true);
            acrobatViewer.setView("FitH");
        }

        public string PdfFilePath
        {
            get
            {
                return pdfFilePath;
            }

            set
            {
                if (pdfFilePath != value)
                {
                    pdfFilePath = value;
                    ChangeCurrentDisplayedPdf();
                }
            }
        }

        public void Print()
        {
            acrobatViewer.printWithDialog();
        }

        private void ChangeCurrentDisplayedPdf()
        {
            acrobatViewer.LoadFile(PdfFilePath);
            acrobatViewer.src = PdfFilePath;
            acrobatViewer.setViewScroll("FitH", 0);
        }
    }
}
