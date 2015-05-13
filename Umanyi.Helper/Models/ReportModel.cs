using System.Collections.ObjectModel;
using System.Data;

namespace Helper.Models
{
    public class ReportModel:ModelBase
    {
        public ReportModel()
        {
            Title = "";
            Entries = new DataTable();
        }
        public string Title
        { get; set; }
        public DataTable Entries
        { get; set; }

        public ObservableCollection<ColumnModel> Columns
        { get; set; }
        public override void Reset()
        {
        }
    }
}
