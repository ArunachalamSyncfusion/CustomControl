using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControl.Control
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomControlSettings : Element, INotifyPropertyChanged
    {
        internal CustomControl? customControl;

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty TrackBackgroundProperty =
BindableProperty.Create(nameof(TrackBackground), typeof(Color), typeof(CustomControlSettings), Colors.Red, propertyChanged: OnTrackBackgroundPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty =
           BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(CustomControlSettings), Colors.Green);

        /// <summary>
        /// 
        /// </summary>
        public Color TrackBackground
        {
            get => (Color)GetValue(TrackBackgroundProperty);
            set
            {
                SetValue(TrackBackgroundProperty, value);
                OnPropertyChanged(nameof(TrackBackground));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color StrokeColor
        {
            get => (Color)GetValue(StrokeColorProperty);
            set
            {
                SetValue(StrokeColorProperty, value);
                OnPropertyChanged(nameof(StrokeColor));
            }
        }

        internal CustomControl? CustomControl
        {
            get
            {
                return customControl;
            }
            set
            {
                if (value != null)
                {
                    customControl = value;
                }
            }
        }

        private static void OnTrackBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           if (bindable is CustomControlSettings settings && settings.CustomControl is not null)
            {
                settings.CustomControl._box.BackgroundColor = (Color)newValue;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
