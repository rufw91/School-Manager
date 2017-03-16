
namespace UmanyiSMS.Modules.Purchases.Models
{
    public class ItemStockTakingResultsModel: ItemBaseModel
    {
        decimal counted;
        decimal expected;
        decimal varianceQty;
        decimal variancePc;
        public ItemStockTakingResultsModel()
        {
            Counted = 0;
            Expected = 0;
            VarianceQty = 0;
            VariancePc = 0;
        }
        public override void Reset()
        {
            base.Reset();
            Counted = 0;
            Expected = 0;
            VarianceQty = 0;
            VariancePc = 0;
        }

        public decimal Counted
        {
            get { return this.counted; }

            set
            {
                if (value != this.counted)
                {
                    this.counted = value;
                    NotifyPropertyChanged("Counted");
                }
            }
        }

        public decimal Expected
        {
            get { return this.expected; }

            set
            {
                if (value != this.expected)
                {
                    this.expected = value;
                    NotifyPropertyChanged("Expected");
                }
            }
        }

        public decimal VarianceQty
        {
            get { return this.varianceQty; }

            set
            {
                if (value != this.varianceQty)
                {
                    this.varianceQty = value;
                    NotifyPropertyChanged("VarianceQty");
                }
            }
        }

        public decimal VariancePc
        {
            get { return this.variancePc; }

            set
            {
                if (value != this.variancePc)
                {
                    this.variancePc = value;
                    NotifyPropertyChanged("VariancePc");
                }
            }
        }
    }
}
