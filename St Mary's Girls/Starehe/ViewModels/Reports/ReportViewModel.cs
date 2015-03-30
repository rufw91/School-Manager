using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    public class ReportViewModel:ViewModelBase
    {
        private ObservableCollection<ColumnModel> columns;
        public ReportViewModel()
        {
            columns = new ObservableCollection<ColumnModel>();
        }

        public ObservableCollection<ColumnModel> Columns { get { return columns; } }

        protected override void InitVars()
        {
        }

        protected override void CreateCommands()
        {
        }

        public override void Reset()
        {
        }
    }
}
