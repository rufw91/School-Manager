using Helper.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Starehe.Views
{
    public partial class ShowErrors : CustomWindow
    {
        Dictionary<int, List<string>> _errors;

        public ShowErrors(Dictionary<int,List<string>> errors)
        {
            InitializeComponent();
            _errors = errors;
            InitVars();
            this.DataContext = this;
        }

        async void InitVars()
        {
            Processed = new ObservableCollection<string>();
            await Task.Run(() =>
            {
                string s;
                foreach(var v in _errors)
                {
                    s = "The Row at position " + v.Key + " has the following errors:";
                    foreach (string x in v.Value)
                        s += "\r\n-" + x;
                    Processed.Add(s);
                }
            });
        }

        public ObservableCollection<string> Processed
        {get; private set;}
    }
}
