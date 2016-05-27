using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helper.Controls
{
    public class MyImage:Image
    {
        public static readonly RoutedEvent ImageSourceChangedEvent = EventManager.RegisterRoutedEvent("ImageSourceChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyImage));
        public event RoutedEventHandler ImageSourceChanged
        {
            add
            {
                AddHandler(ImageSourceChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ImageSourceChangedEvent, value);
            }
            
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MyImage), new PropertyMetadata((ImageSource)null, OnImageSourceChanged));

        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyImage myImage = d as MyImage;
            if (myImage != null)
                myImage.OnImageSourceChanged((ImageSource)e.OldValue, (ImageSource)e.NewValue);
        }

        protected virtual void OnImageSourceChanged(ImageSource oldValue, ImageSource newValue)
        {
            this.Source = newValue;
            RaiseRoutedEvent(MyImage.ImageSourceChangedEvent);
        }

        private void RaiseRoutedEvent(RoutedEvent routedEvent)
        {
            RoutedEventArgs args = new RoutedEventArgs(routedEvent, this);
            RaiseEvent(args);
        }
    }
}
