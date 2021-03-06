﻿using System.Windows;
using System.Windows.Forms.Integration;

namespace UmanyiSMS.Controls
{
    public class PdfViewerHost : WindowsFormsHost
    {
        public static readonly DependencyProperty PdfPathProperty = DependencyProperty.Register(
            "PdfPath", typeof(string), typeof(PdfViewerHost), new PropertyMetadata(PdfPathPropertyChanged));

        private readonly PDFViewer wrappedControl;

        public PdfViewerHost()
        {
            wrappedControl = new PDFViewer();
            Child = wrappedControl;
        }

        public string PdfPath
        {
            get
            {
                return (string)GetValue(PdfPathProperty);
            }

            set
            {
                SetValue(PdfPathProperty, value);
            }
        }

        public void Print()
        {
            wrappedControl.Print();
        }

        private static void PdfPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PdfViewerHost host = (PdfViewerHost)d;
            host.wrappedControl.PdfFilePath = (string)e.NewValue;
        }
    }
}
