using Helper;
using UmanyiSMS.ViewModels;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{

    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            HomePageVM hpvm=null;
            this.DataContextChanged += (o, e) =>
                {                    
                     hpvm = this.DataContext as HomePageVM;
                    if (hpvm != null)
                    {
                        hpvm.PauseAction = () =>
                            {
                                mediaE.Pause();
                            };

                        hpvm.PlayAction = () =>
                        {
                            if (mediaE.Source != null)
                                mediaE.Play();
                        };

                        if (hpvm.VideoSource != null && hpvm.ShowVideoFeed)
                        {
                            hpvm.LoadFeeds(null);
                            mediaE.Play();
                        }

                             
                    }
                };
            mediaE.MediaOpened += (o2, e2) =>
                                   hpvm.CurrentMediaDuration = mediaE.NaturalDuration.TimeSpan;

            
        }
    }
}