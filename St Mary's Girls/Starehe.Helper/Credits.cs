using System.Collections.ObjectModel;

namespace Helper
{
    public static class Credits
    {
        static ObservableCollection<string> credits;
        static Credits()
        {
            credits = new ObservableCollection<string>();
            credits.Add("Raphael M.");
            credits.Add("Moses Ngovi,Shishi Ltd.");
            credits.Add("Meshack Mwongela");
            credits.Add("Mykola Dobrochynskyy: EmailFormatValidator");
            credits.Add("MahApps.Metro :UI");
            credits.Add("First Floor Software: UI");
            credits.Add("http://www.rgbstock.com: Images");
            credits.Add("http://metrowpf.codeplex.com");
            credits.Add("De TorstenMandelkow: MetroChart");
            credits.Add("MSDN Community");
            credits.Add("Muljadi Budiman: ObservableImmutableList");
        }
        public static ObservableCollection<string> TheCredits
        { get { return credits; } }
    }
}
