using Helper;

namespace UmanyiSMS.ViewModels
{
    public class MainWindowVM : ViewModelBase    
    {
        ViewModelBase source;
        public MainWindowVM() 
        {
            InitVars();
            CreateCommands();            
        }

        public ViewModelBase Source
        {
            get { return this.source; }

            set
            {
                if (value != this.source)
                {                   
                    this.source = value;                    
                    NotifyPropertyChanged("Source");
                }
            }
        }
        
        protected async override void InitVars()
        {
            Source = new HomePageVM();
            App.AppExamSettings.CopyFrom(await DataAccess.GetExamSettingsAsync());
        }

        protected override void CreateCommands()
        {
        }
        
      
        public override void Reset()
        {
            
        }

    }
}
