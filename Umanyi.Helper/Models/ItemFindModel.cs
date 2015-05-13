using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemFindModel:ItemModel
    {
        bool isSelected;
        public ItemFindModel()
        {
            IsSelected = false;
        }
        public ItemFindModel(ItemModel item)
            : base(item)
        {
            IsSelected = false;
        }
        public override void Reset()
        {
            base.Reset();
            IsSelected = false;
        }

        public bool IsSelected
        {
            get { return this.isSelected; }

            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
    }
}
