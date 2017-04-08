using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetupUI
{
    public partial class SetupUIView : CustomWindow
    {
        Stack<UserControl> pages = new Stack<UserControl>();

        public UserControl CurrentPage { get; set; }

        public SetupUIView()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
              {
                  ShowPage(new Page1());
              };
            this.Closing += (o, e) =>
              {

                  if (DataContext != null && (DataContext as SetupUIViewModel).State ==
                  (SetupUIViewModel.InstallState.Applying | SetupUIViewModel.InstallState.Initializing))
                  {
                      e.Cancel = true;
                      (DataContext as SetupUIViewModel).CancelCommand.Execute(null);
                  }
                  else
                      Dispatcher.InvokeShutdown();

              };
            this.DataContextChanged += (o, e) =>
              {
                  if (DataContext != null)
                  {
                      (DataContext as SetupUIViewModel).OpenPage2Action = () =>
                      {
                          ShowPage(new Page2());
                      };
                      (DataContext as SetupUIViewModel).OpenPage3Action = () =>
                      {
                          ShowPage(new Page3());
                      };
                      (DataContext as SetupUIViewModel).OpenPage4Action = () =>
                      {
                          ShowPage(new Page4());
                      };
                      (DataContext as SetupUIViewModel).OpenPage5Action = () =>
                      {
                          ShowPage(new Page5());
                      };
                      (DataContext as SetupUIViewModel).OpenPage6Action = () =>
                      {
                          ShowPage(new Page6());
                      };
                      (DataContext as SetupUIViewModel).OpenPage7Action = () =>
                      {
                          ShowPage(new Page7());
                      };
                      (DataContext as SetupUIViewModel).OpenPage8Action = () =>
                      {
                          ShowPage(new Page8());
                      };
                      (DataContext as SetupUIViewModel).OpenPage9Action = () =>
                      {
                          ShowPage(new Page9());
                      }; 
                  }
              };
        }

        public static ImageSource GetIcon()
        {
            NativeMethods.SHSTOCKICONINFO iconResult = new NativeMethods.SHSTOCKICONINFO();
            iconResult.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(iconResult);

            NativeMethods.SHGetStockIconInfo(
                NativeMethods.SHSTOCKICONID.SIID_SHIELD,
                NativeMethods.SHGSI.SHGSI_ICON | NativeMethods.SHGSI.SHGSI_SMALLICON,
                ref iconResult);
            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                        iconResult.hIcon,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
            NativeMethods.DestroyIcon(iconResult.hIcon);
            return img;
        }

        private void SetUacShield()
        {
            
        }
                
        public void ShowPage(UserControl newPage)
        {
            pages.Push(newPage);

         ShowNewPage();
        }

        void ShowNewPage()
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                if (contentPresenter.Content != null)
                {
                    UserControl oldPage = contentPresenter.Content as UserControl;

                    if (oldPage != null)
                    {
                        oldPage.Loaded -= newPage_Loaded;

                        UnloadPage(oldPage);
                    }
                }
                else
                {
                    ShowNextPage();
                }

            });
            
        }

        void ShowNextPage()
        {
            UserControl newPage = pages.Pop();

            newPage.Loaded += newPage_Loaded;

            contentPresenter.Content = newPage;
        }

        void UnloadPage(UserControl page)
        {
            Storyboard hidePage = (Resources["SlideOut"] as Storyboard).Clone();

            hidePage.Completed += hidePage_Completed;

            hidePage.Begin(contentPresenter);
        }

        void newPage_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard showNewPage = Resources["SlideIn"] as Storyboard;

            showNewPage.Begin(contentPresenter);

            CurrentPage = sender as UserControl;
        }

        void hidePage_Completed(object sender, EventArgs e)
        {
            contentPresenter.Content = null;

            ShowNextPage();
        }

    }
}
