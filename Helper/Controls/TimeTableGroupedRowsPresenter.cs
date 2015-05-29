using System.Windows;
using System.Windows.Controls;

namespace Helper.Controls
{
    public class TimeTableGroupedRowsPresenter : ItemsControl
    {
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day", typeof(string), typeof(TimeTableGroupedRowsPresenter), new PropertyMetadata(""));
        public TimeTableGroupedRowsPresenter()
        {

        }

        public string Day
        {
            get
            {
                return (string)GetValue(DayProperty);
            }

            set
            {
                SetValue(DayProperty, value);
            }
        }

        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            double h = CalculateHeight();
            if (h == 80.0)
                this.Visibility = System.Windows.Visibility.Collapsed;
            else this.Height = h;
        }

        public double CalculateHeight()
        {
            if (Items.Count == 1)
                return 80;
            double height = 0;            
            foreach (var i in Items)
                height += 50d;
            return height;
        }
    }
}
