
namespace Helper.Models
{
    public class ColumnModel
    {
        public ColumnModel()
        {
            Name = "";
            IsSelected = true;
            Width = 0;
        }

        public ColumnModel(bool isSelected,string name, double width)
        {
            Name = name;
            IsSelected = isSelected;
            Width = width;
        }
        
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public double Width { get; set; }
    }
}
