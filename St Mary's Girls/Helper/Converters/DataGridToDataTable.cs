using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Helper.Converters
{
    public static class DataGridToDataTable
    {
        public static DataTable Convert(DataGrid dataGrid)
        {
            DataTable dataTable = new DataTable();

            foreach (DataGridColumn column in dataGrid.Columns)
            {
                dataTable.Columns.Add(new DataColumn(column.Header.ToString(), typeof(string)));
            }
            int colCount = dataTable.Columns.Count;
            if (colCount == 0)
                return dataTable;
            DataRow dtr;
            dataGrid.UpdateLayout();
            for (int index = 0; index < dataGrid.Items.Count; index++)
            {
                dtr = dataTable.NewRow();                
                dataGrid.ScrollIntoView(dataGrid.Items[index]);
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                for (int i = 0; i < colCount; i++)
                {
                    DataGridCellsPresenter presenter = VisualTreeHelperEx.FindVisualChild<DataGridCellsPresenter>(row);
                    dataGrid.ScrollIntoView(row, dataGrid.Columns[i]);
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(i);
                    dtr[i] = (cell.Content as TextBlock).Text;
                }

                dataTable.Rows.Add(dtr);
            }
            return dataTable;
        }
    }


    
    
}
