using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace UmanyiSMS.Lib.Controllers
{
    public abstract class DocumentHelperBase
    {
        protected static FixedDocument Document { get; set; }
        protected static int NoOfPages { get; set; }
        protected static int ItemsPerPage { get; set; }
        protected static object MyWorkObject { get; set; }
        protected static string ResourceString { get; set; }

        public DocumentHelperBase(object workObject)
        {
            if (workObject == null)
                throw new ArgumentNullException("workObject", "workObject cannot be null.");
            InitVars(workObject);
            AddPagesToDocument();
            AddDataToDocument();
        }
        protected void AddPagesToDocument()
        {
            try
            {
                FixedPage p;
                for (int i = 0; i < NoOfPages; i++)
                {
                    PageContent pageContent = new PageContent();
                    p = GetPage(ResourceString);
                    ((IAddChild)pageContent).AddChild(p);
                    Document.Pages.Add(pageContent);
                }
            }
            catch { throw new InvalidOperationException(); }
        }

        protected FixedPage GetPage(string resString)
        { 
            StringReader stringReader = new StringReader(resString);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            FixedPage page = (FixedPage)XamlReader.Load(xmlReader);
            return page;
        }

        protected string GetResourceString(Uri assemblyLocation)
        {
            string resString = "";
            StreamReader tr = new StreamReader(Application.GetResourceStream(assemblyLocation).Stream);
            resString = tr.ReadToEnd();
            return resString;
        }

        protected void InitVars(object workObject)
        {
            MyWorkObject = workObject;
            NoOfPages = GetNoOfPages();
            ResourceString = GetResString();
            ItemsPerPage = GetItemsPerPage();
            Document = new FixedDocument();
        }

        protected abstract void AddDataToDocument();

        protected abstract string GetResString();

        protected abstract int GetNoOfPages();

        protected abstract int GetItemsPerPage();

        protected void AddText(string text, string fontFamily, double fontSize, bool isBold, double rotateAngle,
            Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily(fontFamily);
            text1.FontSize = fontSize;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);

            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }

        protected void AddText( string text, string fontFamily, double fontSize, bool isBold, double rotateAngle,
            Color fontColor, double left, double top,double maxWidth, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily(fontFamily);
            text1.FontSize = fontSize;
            text1.MaxWidth = maxWidth;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);

            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }


        protected void AddTextWithWrap(string text, string fontFamily, double width, double height, double fontSize, bool isBold, double rotateAngle,
           Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily(fontFamily);
            text1.FontSize = fontSize;
            text1.TextWrapping = TextWrapping.Wrap;
            text1.Width = width;
            text1.Height = height;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);

            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }

        protected void AddText(string text, double fontSize, bool isBold, double rotateAngle,
            Color fontColor, double left, double top, int pageNo)
        {

            TextBlock text1 = new TextBlock();
            text1.Text = text;
            text1.FontFamily = new FontFamily("Times New Roman");
            text1.FontSize = fontSize;
            text1.Foreground = new SolidColorBrush(fontColor);
            if (isBold)
                text1.FontWeight = System.Windows.FontWeights.Bold;
            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);
            

            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }
        protected void AddImage( byte[] image, double width, double height, double left, double top, double rotateAngle, int pageNo)
        {
            Image text1 = new Image();
            text1.Stretch = Stretch.Fill;

            if (rotateAngle != 0)
            {
                RotateTransform rotateTransform1 =
                    new RotateTransform(rotateAngle);
                text1.RenderTransform = rotateTransform1;
            }

            text1.HorizontalAlignment = (left == -1) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
            text1.VerticalAlignment = VerticalAlignment.Top;
            text1.Margin = new Thickness((left == -1) ? 0 : left, top, 0, 0);
            text1.Width = width;
            text1.Height = height;
            if (image!=null&&image.Length>10)
            text1.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
            Grid g = Document.Pages[pageNo].Child.Children[0] as Grid;
            g.Children.Add(text1);
        }
        protected void AddText(FixedDocument doc, string text, double left, double top, int pageNo)
        {
            AddText(text, 12, false, 0, Colors.Black, left, top, pageNo);
        }
        
    }

}
