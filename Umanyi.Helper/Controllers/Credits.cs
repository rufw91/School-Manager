using System.Collections.ObjectModel;

namespace UmanyiSMS.Lib
{
    public static class Credits
    {
        static ObservableCollection<string> credits;
        static Credits()
        {
            credits = new ObservableCollection<string>();
            
            credits.Add("Jones M, Umanyi Ltd.");            
            credits.Add("Mykola Dobrochynskyy: EmailFormatValidator");
            credits.Add("MahApps.Metro :UI");
            credits.Add("Meshack Mwongela");
            credits.Add("First Floor Software: UI");
            credits.Add("http://www.rgbstock.com: Images");
            credits.Add("http://metrowpf.codeplex.com");
            credits.Add("De TorstenMandelkow: MetroChart");
            credits.Add("MSDN Community");
            credits.Add("Muljadi Budiman: ObservableImmutableList");
            credits.Add("Davies, Daveric Systems Ltd.");
        }
        public static ObservableCollection<string> TheCredits
        { get { return credits; } }
    }
}
