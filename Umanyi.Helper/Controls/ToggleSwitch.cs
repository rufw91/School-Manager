using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UmanyiSMS.Lib.Controls
{
    /// <summary>
    /// Represents a switch that can be toggled between two states.
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
    [TemplateVisualState(Name = "Dragging", GroupName = "ToggleStates")]
    [TemplateVisualState(Name = "Off", GroupName = "ToggleStates")]
    [TemplateVisualState(Name = "On", GroupName = "ToggleStates")]
    [TemplateVisualState(Name = "OffContent", GroupName = "ContentStates")]
    [TemplateVisualState(Name = "OnContent", GroupName = "ContentStates")]
    public class ToggleSwitch : Control
    {
        /// <summary>
        /// Identifies the Header dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(default(object), OnHeaderChanged));

        /// <summary>
        /// Identifies the HeaderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ToggleSwitch), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IsOn dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOnProperty =
        DependencyProperty.Register("IsOn", typeof(bool), typeof(ToggleSwitch), new PropertyMetadata(default(bool), IsOnChanged));

        /// <summary>
        /// Identifies the OffContent dependency property.
        /// </summary>
        public static readonly DependencyProperty OffContentProperty =
        DependencyProperty.Register("OffContent", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(default(object)));

        /// <summary>
        /// Identifies the OffContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty OffContentTemplateProperty =
        DependencyProperty.Register("OffContentTemplate", typeof(DataTemplate), typeof(ToggleSwitch), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the OnContent dependency property.
        /// </summary>
        public static readonly DependencyProperty OnContentProperty =
        DependencyProperty.Register("OnContent", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(default(object)));

        /// <summary>
        /// Identifies the OnContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty OnContentTemplateProperty =
        DependencyProperty.Register("OnContentTemplate", typeof(DataTemplate), typeof(ToggleSwitch), new PropertyMetadata(null));

        private const string ThumbTemplatePartName = "PART_SwitchThumb";

        private Thumb thumbPart;

        /// <summary>
        /// Initializes a new instance of the ToggleSwitch class.
        /// </summary>
        public ToggleSwitch()
        {
        }

        /// <summary>
        /// Occurs when "On"/"Off" state changes for this <see cref="ToggleSwitch"/>.
        /// </summary>
        public event RoutedEventHandler Toggled;

        /// <summary>
        /// Gets or sets the header content.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that declares whether the state of the ToggleSwitch is On.
        /// </summary>
        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        /// <summary>
        /// Provides the object content that should be displayed using the OffContentTemplate when this ToggleSwitch has state of Off.
        /// </summary>
        public object OffContent
        {
            get { return (object)GetValue(OffContentProperty); }
            set { SetValue(OffContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the control's content while in Off state.
        /// </summary>
        public DataTemplate OffContentTemplate
        {
            get { return (DataTemplate)GetValue(OffContentTemplateProperty); }
            set { SetValue(OffContentTemplateProperty, value); }
        }

        /// <summary>
        /// Provides the object content that should be displayed using the OnContentTemplate when this ToggleSwitch has state of On.
        /// </summary>
        public object OnContent
        {
            get { return (object)GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the control's content while in On state.
        /// </summary>
        public DataTemplate OnContentTemplate
        {
            get { return (DataTemplate)GetValue(OnContentTemplateProperty); }
            set { SetValue(OnContentTemplateProperty, value); }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "Normal", true);
            this.UpdateVisualState();
            this.InitializeThumb();
        }

        /// <summary>
        /// Invoked when the content for Header changes.
        /// </summary>
        /// <param name="oldContent">The string or object content of the old content.</param>
        /// <param name="newContent">The string or object content of the new content.</param>
        protected virtual void OnHeaderChanged(object oldContent, object newContent)
        {
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Mouse.MouseEnter"/> attached event is raised on this element.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.UpdateCommonStates();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Mouse.MouseLeave"/> attached event is raised on this element.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.UpdateCommonStates();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Mouse.MouseMove"/> attached event is raised on this element.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.UpdateCommonStates();
        }

        /// <summary>
        /// Invoked before the Toggled event is raised.
        /// </summary>
        protected virtual void OnToggled()
        {
            this.UpdateToggleStates();
            if (this.Toggled != null)
            {
                this.Toggled(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Update the visual state of this <see cref="ToggleSwitch"/>.
        /// </summary>
        protected virtual void UpdateVisualState()
        {
            this.UpdateToggleStates();
            this.UpdateCommonStates();
            this.UpdateFocusStates();
        }

        /// <summary>
        /// Called when the <see cref="IsOn"/> property is changed.
        /// </summary>
        /// <param name="d">The System.Windows.DependencyObject on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void IsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleSwitch = d as ToggleSwitch;
            if (toggleSwitch != null)
            {
                toggleSwitch.OnToggled();
            }
        }

        /// <summary>
        /// Called when the <see cref="Header"/> property is changed.
        /// </summary>
        /// <param name="d">The System.Windows.DependencyObject on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggleSwitch = d as ToggleSwitch;
            if (toggleSwitch != null)
            {
                toggleSwitch.OnHeaderChanged(e.OldValue, e.NewValue);
            }
        }

        /// <summary>
        /// Initialize the thumb in the template.
        /// </summary>
        private void InitializeThumb()
        {
            try
            {
                this.thumbPart = this.GetTemplateChild(ThumbTemplatePartName) as Thumb;
            }
            catch (Exception)
            { }
            this.thumbPart.DragCompleted += OnThumbPartDragCompleted;
            thumbPart.DragStarted += OnThumbPartDragStarted;
        }

        /// <summary>
        /// Called when the thumb is drag finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnThumbPartDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (e.HorizontalChange >= 22)
            {
                this.IsOn = true;
            }
            else if (e.HorizontalChange <= -22)
            {
                this.IsOn = false;
            }
            else if (e.HorizontalChange == 0)
            {
                this.IsOn = !this.IsOn;
            }
            this.UpdateVisualState();
        }

        /// <summary>
        /// Called when the thumb is drag started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnThumbPartDragStarted(object sender, DragStartedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        /// <summary>
        /// Update the common visual state group of this <see cref="ToggleSwitch"/>.
        /// </summary>
        private void UpdateCommonStates()
        {
            if (this.IsEnabled)
            {
                if (this.IsMouseOver)
                {
                    VisualStateManager.GoToState(this, "MouseOver", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Normal", true);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "Disabled", true);
            }
        }

        /// <summary>
        /// Update the focus visual state group of this <see cref="ToggleSwitch"/>.
        /// </summary>
        private void UpdateFocusStates()
        {
            if (this.IsFocused)
            {
                VisualStateManager.GoToState(this, "Focused", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", true);
            }
        }

        /// <summary>
        /// Update the toggle visual state group of this <see cref="ToggleSwitch"/>.
        /// </summary>
        private void UpdateToggleStates()
        {
            if (this.IsOn)
            {
                VisualStateManager.GoToState(this, "On", true);
                VisualStateManager.GoToState(this, "OnContent", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Off", true);
                VisualStateManager.GoToState(this, "OffContent", true);
            }
        }
    }
}