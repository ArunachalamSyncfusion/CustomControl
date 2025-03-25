using CustomControl.ControlStyling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeLibrary;

namespace CustomControl.Control
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomControl:ContentView,IParentThemeElement
    {

        private const string OffStateName = "Off";

        private string currentState = OffStateName;

        private bool isHovered;

        private bool isPressed;

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty? IsOnProperty =
            BindableProperty.Create(nameof(IsOn), typeof(bool?), typeof(CustomControl), false, BindingMode.TwoWay, null, OnIsOnPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty CustomControlSettingsProperty =
                BindableProperty.Create(nameof(CustomControlSettings), typeof(CustomControlSettings), typeof(CustomControl), null, BindingMode.TwoWay, propertyChanged: OnCustomControlSettingsPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public bool? IsOn
        {
            get { return (bool?)this.GetValue(IsOnProperty); }
            set { this.SetValue(IsOnProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomControlSettings CustomControlSettings
        {
            get { return (CustomControlSettings)this.GetValue(CustomControlSettingsProperty); }
            set { this.SetValue(CustomControlSettingsProperty, value); }
        }

        private static void OnIsOnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomControl)?.ChangeVisualState();

        }

        private static void OnCustomControlSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomControl control && control.CustomControlSettings is not null)
            {
                control.OnDefaultCenterButtonSettingsChanged();
                control.CustomControlSettings.PropertyChanged -= control.OnDefaultCustomControlSettings_PropertyChanged;
                control.CustomControlSettings.PropertyChanged += control.OnDefaultCustomControlSettings_PropertyChanged;
            }

            if (oldValue != null)
            {
                if (oldValue is CustomControlSettings previousSetting)
                {
                    previousSetting.customControl = null;
                    previousSetting.BindingContext = null;
                    SetInheritedBindingContext(previousSetting, null);
                    previousSetting.Parent = null;
                }
            }
            if (newValue != null)
            {
                if (newValue is CustomControlSettings currentSetting && bindable is CustomControl customControl)
                {
                    currentSetting.Parent = currentSetting.CustomControl = customControl;
                    customControl.UpdateCurrentStyle();
                    SetInheritedBindingContext(customControl.CustomControlSettings, customControl.BindingContext);
                }
            }
        }

        void OnDefaultCenterButtonSettingsChanged()
        {
            UpdateCurrentStyle();
        }

        void OnDefaultCustomControlSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TrackBackground")
            {
                UpdateCurrentStyle();
            }
        }

        internal Border _box;

        /// <summary>
        /// 
        /// </summary>
        public CustomControl()
        {
            _box = new Border
            {

                WidthRequest = 100,
                HeightRequest = 100
            };
            ThemeElement.InitializeThemeResources(this, "CustomControlTheme");

            PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();

            // Attach event handlers
            pointerGestureRecognizer.PointerEntered += OnPointerEntered;
            pointerGestureRecognizer.PointerExited += OnPointerExited;
            pointerGestureRecognizer.PointerMoved += OnPointerMoved;
            pointerGestureRecognizer.PointerPressed += PointerGestureRecognizer_PointerPressed;
            pointerGestureRecognizer.PointerReleased += PointerGestureRecognizer_PointerReleased;

            GestureRecognizers.Add(pointerGestureRecognizer);

            Content = _box;
        }

        /// <summary>
        /// 
        /// </summary>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        private void UpdateCurrentStyle()
        {

            if (_box is not null && CustomControlSettings is not null)
            {
                _box.BackgroundColor = CustomControlSettings.TrackBackground;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ChangeVisualState()
        {
            if (IsOn == true)
            {
                if (isPressed && isHovered)
                {
                    currentState = "OnPressed";
                }
                else if (isHovered)
                {
                    currentState = "OnHovered";
                }
                else
                {
                    currentState = "On";
                }
            }
            else if (IsOn == false)
            {
                if (isPressed && isHovered)
                {
                    currentState = "OffPressed";
                }
                else if (isHovered)
                {
                    currentState = "OffHovered";
                }
                else
                {
                    currentState = "Off";
                }
            }
            else
            {
                if (isPressed && isHovered)
                {
                    currentState = "IndeterminatePressed";
                }
                else if (isHovered)
                {
                    currentState = "IndeterminateHovered";
                }
                else
                {
                    currentState = "Indeterminate";
                }
            }
            VisualStateManager.GoToState(this, currentState);
            UpdateCurrentStyle();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.CustomControlSettings != null)
            {
                CustomControl.SetInheritedBindingContext(this.CustomControlSettings, this.BindingContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateCurrentStyle();
        }

        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            isHovered = true;
            ChangeVisualState();
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            isHovered = false;
            ChangeVisualState();
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            ChangeVisualState();
        }

        private void PointerGestureRecognizer_PointerReleased(object? sender, PointerEventArgs e)
        {
            isPressed = false;
            ChangeVisualState();
        }

        private void PointerGestureRecognizer_PointerPressed(object? sender, PointerEventArgs e)
        {
            isPressed = true;
            ChangeVisualState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResourceDictionary GetThemeDictionary()
        {
            return new CustomControlStyling();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldTheme"></param>
        /// <param name="newTheme"></param>
        public void OnCommonThemeChanged(string oldTheme, string newTheme)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldTheme"></param>
        /// <param name="newTheme"></param>
        public void OnControlThemeChanged(string oldTheme, string newTheme)
        {
            
        }
    }
}
