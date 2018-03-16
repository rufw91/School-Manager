using System.Collections.ObjectModel;

namespace UmanyiSMS.Lib
{
    public static class Credits
    {
        static ObservableCollection<string> credits;
        static Credits()
        {
            credits = new ObservableCollection<string>();
            credits.Add("First Floor Software : UI");
            credits.Add("http://metrowpf.codeplex.com");
            credits.Add("MahApps.Metro :UI");
            credits.Add("Microsoft Corp. : SingleInstance.cs");
            credits.Add(".NET Foundation");
            credits.Add("MSDN Community");
            credits.Add("Apache Org : log4net");
            credits.Add("Mykola Dobrochynskyy : EmailFormatValidator");
            credits.Add("http://www.rgbstock.com : Images");
            credits.Add("De TorstenMandelkow : MetroChart");
            credits.Add("Muljadi Budiman : ObservableImmutableList");
            credits.Add("https://www.codeplex.com/site/users/view/uhimania : VirtualizingWrapPanel");
            credits.Add("http://joyfulwpf.blogspot.co.ke/2007/10/extented-visualtreehelper-class.html : VisualTreeHelperEx");           
            credits.Add("Thomas Barnekow : Open XML Packaging");
            


        }
        public static ObservableCollection<string> TheCredits
        { get { return credits; } }
    }
}
