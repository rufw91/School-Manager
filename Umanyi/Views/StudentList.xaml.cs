
using Helper.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace UmanyiSMS.Views
{
    public partial class StudentList : UserControl
    {
        public static readonly DependencyProperty DisplayStudentDetailCommandProperty =
            DependencyProperty.Register("DisplayStudentDetailCommand", typeof(ICommand), typeof(StudentList),
            new PropertyMetadata(null));
        public StudentList()
        {
            InitializeComponent();
            
            Binding myBinding = new Binding("DataContext.DisplayStudentDetailCommand");
            myBinding.RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor, typeof(MainWindow), 1);
            
            this.SetBinding(DisplayStudentDetailCommandProperty, myBinding);
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentStudent = (StudentModel)(sender as ListBoxItem).Content;
            DisplayStudentDetailCommand.Execute(CurrentStudent);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (lbx.SelectedItem is StudentModel)
            {
                CurrentStudent = (lbx.SelectedItem as StudentModel);
                DisplayStudentDetailCommand.Execute(CurrentStudent);
            }
        }

        public ICommand DisplayStudentDetailCommand
        {
            get { return (ICommand)this.GetValue(DisplayStudentDetailCommandProperty); }
            set { this.SetValue(DisplayStudentDetailCommandProperty, value); }            
        }

        private StudentModel CurrentStudent
        {
            get;
            set;
        }
    }
}
