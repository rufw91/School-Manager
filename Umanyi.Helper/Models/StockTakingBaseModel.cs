using System;

namespace Helper.Models
{
    public class StockTakingBaseModel : ModelBase
    {
        int stockTakingID;
        DateTime? dateTaken;

        public StockTakingBaseModel()
        {
            StockTakingID = 0;
        }

        public int StockTakingID
        {
            get { return this.stockTakingID; }

            set
            {
                if (value != this.stockTakingID)
                {
                    this.stockTakingID = value;
                    NotifyPropertyChanged("StockTakingID");
                }
            }
        }

        public DateTime? DateTaken
        {
            get { return this.dateTaken; }

            set
            {
                if (value != this.dateTaken)
                {
                    this.dateTaken = value;
                    NotifyPropertyChanged("DateTaken");
                }
            }
        }

        public override void Reset()
        {
            DateTaken = null;
            StockTakingID = 0;
        }
    }
}
