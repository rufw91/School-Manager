using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Helper.Controls
{
    [TemplatePart(Name = "PART_HourButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_MinuteButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_SecondButton", Type = typeof(RepeatButton))]

    public class TimePicker : UserControl
    {
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan?),
            typeof(TimePicker), new PropertyMetadata(new TimeSpan(0, 0, 0), new PropertyChangedCallback(SelectedTimeChangedCallBack)));
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent(
        "SelectedTimeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimePicker));

        RepeatButton hourButton;
        RepeatButton minuteButton;
        RepeatButton secondButton;
        public TimePicker()
        {
        }

        private static void SelectedTimeChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePicker sh = (TimePicker)d;
            sh.OnSelectedTimeChanged(sh, new RoutedEventArgs(TimePicker.SelectedTimeChangedEvent, sh));
        }

        public event RoutedEventHandler SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        protected void OnSelectedTimeChanged(object sender, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        private void HourChanged(object sender, RoutedEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Add(new TimeSpan(1, 0, 0));
        }

        private void MinuteChanged(object sender, RoutedEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Add(new TimeSpan(0, 1, 0));
        }

        private void SecondChanged(object sender, RoutedEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Add(new TimeSpan(0, 0, 1));
        }

        private void ResetHour(object sender, MouseButtonEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Subtract(new TimeSpan(SelectedTime.Value.Hours, 0, 0));
        }

        private void ResetMinute(object sender, MouseButtonEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Subtract(new TimeSpan(0, SelectedTime.Value.Minutes, 0));
        }

        private void ResetSecond(object sender, MouseButtonEventArgs e)
        {
            if (!SelectedTime.HasValue)
                SelectedTime = new TimeSpan(0, 0, 0);
            SelectedTime = SelectedTime.Value.Subtract(new TimeSpan(0, 0, SelectedTime.Value.Seconds));
        }

        private RepeatButton HourButton
        {
            get
            {
                return hourButton;
            }

            set
            {
                if (hourButton != null)
                {
                    hourButton.Click -= new RoutedEventHandler(HourChanged);
                    hourButton.MouseRightButtonUp -= new MouseButtonEventHandler(ResetHour);
                }
                hourButton = value;

                if (hourButton != null)
                {
                    hourButton.Click += new RoutedEventHandler(HourChanged);
                    hourButton.MouseRightButtonUp += new MouseButtonEventHandler(ResetHour);
                }
            }
        }

        private RepeatButton MinuteButton
        {
            get
            {
                return minuteButton;
            }

            set
            {
                if (minuteButton != null)
                {
                    minuteButton.Click -= new RoutedEventHandler(MinuteChanged);
                    minuteButton.MouseRightButtonUp -= new MouseButtonEventHandler(ResetMinute);
                }
                minuteButton = value;

                if (minuteButton != null)
                {
                    minuteButton.Click += new RoutedEventHandler(MinuteChanged);
                    minuteButton.MouseRightButtonUp += new MouseButtonEventHandler(ResetMinute);
                }
            }
        }

        private RepeatButton SecondButton
        {
            get
            {
                return secondButton;
            }

            set
            {
                if (secondButton != null)
                {
                    secondButton.Click -= new RoutedEventHandler(SecondChanged);
                    secondButton.MouseRightButtonUp -= new MouseButtonEventHandler(ResetSecond);
                }
                secondButton = value;

                if (secondButton != null)
                {
                    secondButton.Click += new RoutedEventHandler(SecondChanged);
                    secondButton.MouseRightButtonUp += new MouseButtonEventHandler(ResetSecond);
                }
            }
        }

        public TimeSpan? SelectedTime
        {
            get { return (TimeSpan?)this.GetValue(SelectedTimeProperty); }
            set
            {
                this.SetValue(SelectedTimeProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            HourButton = GetTemplateChild("PART_HourButton") as RepeatButton;
            MinuteButton = GetTemplateChild("PART_MinuteButton") as RepeatButton;
            SecondButton = GetTemplateChild("PART_SecondButton") as RepeatButton;
        }
    }
}
