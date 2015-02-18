using System;
using System.Windows;
using System.Windows.Input;

namespace Helper.Controls
{
    public class CustomWindow: Window
    {
        public static readonly DependencyProperty ShowRestoreProperty = DependencyProperty.Register("ShowRestore", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register("ShowMaximize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register("ShowMinimize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true));
        public CustomWindow()
        {
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
        }
                 
        protected override void OnStateChanged(EventArgs e)
        {
            ShowRestore = this.WindowState == WindowState.Maximized;
            ShowMaximize = WindowState != WindowState.Maximized;
            base.OnStateChanged(e);
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (ResizeMode == System.Windows.ResizeMode.CanMinimize)
                ShowMaximize = false;

            if (ResizeMode == System.Windows.ResizeMode.NoResize)
            {
                ShowMinimize = false;
                ShowMaximize = false;
            }
            base.OnInitialized(e);
        }
        

        public bool ShowRestore
        {
            get { return (bool)GetValue(ShowRestoreProperty); }
            set { SetValue(ShowRestoreProperty, value); }
        }

        public bool ShowMaximize
        {
            get { return (bool)GetValue(ShowMaximizeProperty); }
            set { SetValue(ShowMaximizeProperty, value); }
        }

        public bool ShowMinimize
        {
            get { return (bool)GetValue(ShowMinimizeProperty); }
            set { SetValue(ShowMinimizeProperty, value); }
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
    }
}
