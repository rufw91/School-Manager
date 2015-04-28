using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StockTakeResultsVM: ViewModelBase
    {
        ObservableCollection<StockTakingBaseModel> allStocktakings;
        StockTakingResultsModel currentStocktaking;
        public StockTakeResultsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            IsBusy = true;
            Title = "Stock Taking Results";
            CurrentStocktaking = new StockTakingResultsModel();
            AllStocktakings = await DataAccess.GetAllStockTakings();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o => 
            {
                IsBusy = true;
                CurrentStocktaking.Items.Clear();
                StockTakingResultsModel c = await DataAccess.GetStockTakingResults(currentStocktaking.StockTakingID);
                CurrentStocktaking.DateTaken = c.DateTaken;
                foreach (var x in c.Items)
                    CurrentStocktaking.Items.Add(x);
                IsBusy = false;
            },
            o => !IsBusy&&currentStocktaking.StockTakingID > 0);
        }

        public override void Reset()
        {
        }

        public ObservableCollection<StockTakingBaseModel> AllStocktakings
        {
            get { return this.allStocktakings; }

            private set
            {
                if (value != this.allStocktakings)
                {
                    this.allStocktakings = value;
                    NotifyPropertyChanged("AllStocktakings");
                }
            }
        }

        public StockTakingResultsModel CurrentStocktaking
        {
            get { return this.currentStocktaking; }

            private set
            {
                if (value != this.currentStocktaking)
                {
                    this.currentStocktaking = value;
                    NotifyPropertyChanged("CurrentStocktaking");
                }
            }
        }

        public ICommand RefreshCommand
        { get; private set; }
    }
}
