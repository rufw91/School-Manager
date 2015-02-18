using Helper;
using Helper.Models;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace Starehe.Views
{
    
    public partial class StaffList : UserControl
    {
        public static readonly DependencyProperty DisplayStaffDetailCommandProperty =
            DependencyProperty.Register("DisplayStaffDetailCommand", typeof(ICommand), typeof(StaffList),
            new PropertyMetadata(null));
        public StaffList()
        {
            InitializeComponent();

            Binding myBinding = new Binding("DataContext.DisplayStaffDetailCommand");
            myBinding.RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor, typeof(MainWindow), 1);

            this.SetBinding(StaffList.DisplayStaffDetailCommandProperty, myBinding);
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentStaff = (StaffModel)(sender as ListBoxItem).Content;
            DisplayStaffDetailCommand.Execute(CurrentStaff);
        }

        public ICommand DisplayStaffDetailCommand
        {
            get { return (ICommand)this.GetValue(DisplayStaffDetailCommandProperty); }
            set { this.SetValue(DisplayStaffDetailCommandProperty, value); }
        }

        private StaffModel CurrentStaff
        {
            get;
            set;
        }
    }
}