using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLibrary.Control_Parts
{
    /// <summary>
    /// Логика взаимодействия для RangeSlider.xaml
    /// </summary>
    public partial class RangeSlider : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        #region properties
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(RangeSlider));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(RangeSlider));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private SolidColorBrush rangeColor;
        public SolidColorBrush RangeColor
        {
            get { return rangeColor; }
            set
            {
                rangeColor = value;
                OnPropertyChanged("RangeColor");
            }
        }

        private double rangeFontSize;
        public double RangeFontSize
        {
            get { return rangeFontSize.Equals(0)? 1 : rangeFontSize; }
            set
            {
                rangeFontSize = value;
                OnPropertyChanged("RangeFontSize");
            }
        }


        public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register(
            "LowerValue", typeof(double), typeof(RangeSlider));

        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty = DependencyProperty.Register(
            "UpperValue", typeof(double), typeof(RangeSlider));

        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        private SolidColorBrush valuesColor;
        public SolidColorBrush ValuesColor
        {
            get { return valuesColor; }
            set
            {
                valuesColor = value;
                OnPropertyChanged("ValuesColor");
            }
        }

        private double valuesFontSize;
        public double ValuesFontSize
        {
            get { return valuesFontSize.Equals(0) ? 1 : valuesFontSize; }
            set
            {
                valuesFontSize = value;
                OnPropertyChanged("ValuesFontSize");
            }
        }
        #endregion

        public RangeSlider()
        {
            InitializeComponent();
            this.Loaded += RangeSlider_Loaded;
        }

        void RangeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            //LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            //UpperSlider.ValueChanged += UpperSlider_ValueChanged;
        }


        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value);
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSlider.Value = Math.Min(UpperSlider.Value, LowerSlider.Value);
        }


        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
