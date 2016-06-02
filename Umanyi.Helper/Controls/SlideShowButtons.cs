using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Helper.Controls
{
    public class SlideShowButtons:Control
    {
        public static readonly DependencyProperty HasAudioOrVideoProperty = DependencyProperty.Register("HasAudioOrVideo", typeof(bool), typeof(SlideShowButtons), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMutedProperty = DependencyProperty.Register("IsMuted", typeof(bool), typeof(SlideShowButtons), new PropertyMetadata(false));
        public static readonly DependencyProperty PauseCommandProperty = DependencyProperty.Register("PauseCommand", typeof(ICommand), typeof(SlideShowButtons), new PropertyMetadata(null));
        public static readonly DependencyProperty ResumeCommandProperty = DependencyProperty.Register("ResumeCommand", typeof(ICommand), typeof(SlideShowButtons), new PropertyMetadata(null));
        public static readonly DependencyProperty MuteCommandProperty = DependencyProperty.Register("MuteCommand", typeof(ICommand), typeof(SlideShowButtons), new PropertyMetadata(null));
        public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register("NextCommand", typeof(ICommand), typeof(SlideShowButtons), new PropertyMetadata(null));
       
        public static readonly DependencyProperty UnmuteCommandProperty = DependencyProperty.Register("UnmuteCommand", typeof(ICommand), typeof(SlideShowButtons), new PropertyMetadata(null));
       
        public SlideShowButtons()
        {
        }

        public ICommand PauseCommand
        {
            get { return (ICommand)GetValue(PauseCommandProperty); }
            set { SetValue(PauseCommandProperty, value); }
        }

        public ICommand ResumeCommand
        {
            get { return (ICommand)GetValue(ResumeCommandProperty); }
            set { SetValue(ResumeCommandProperty, value); }
        }

        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }

        public ICommand MuteCommand
        {
            get { return (ICommand)GetValue(MuteCommandProperty); }
            set { SetValue(MuteCommandProperty, value); }
        }

        public ICommand UnmuteCommand
        {
            get { return (ICommand)GetValue(UnmuteCommandProperty); }
            set { SetValue(UnmuteCommandProperty, value); }
        }
        
        public bool HasAudioOrVideo
        {
            get { return (bool)GetValue(HasAudioOrVideoProperty); }
            set { SetValue(HasAudioOrVideoProperty, value); }
        }

        public bool IsMuted
        {
            get { return (bool)GetValue(IsMutedProperty); }
            set { SetValue(IsMutedProperty, value); }
        }

        
    }
}
