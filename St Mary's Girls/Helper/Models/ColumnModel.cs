
using System;
namespace Helper.Models
{
    public class ColumnModel
    {
        private string name;
        private bool isSelected;
        private double width;
        private string friendlyName;
        public ColumnModel()
        {
            Name = "";
            FriendlyName = "";
            IsSelected = false;
            Width = 0;
        }

        public ColumnModel(bool isSelected,string name,string friendlyName, double width)
        {
            Name = name;
            IsSelected = isSelected;
            FriendlyName = friendlyName;
            Width = width;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public double Width
        {
            get { return width; }
            set {
                if ((value < 0d) || (value > 1d))
                    throw new ArgumentOutOfRangeException("Width Value should be non negative value greater than zero and less than 1");
                width = value; }
        }
    }
}
