using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLibrary.Control_Parts
{
    /// <summary>
    /// Логика взаимодействия для NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty TextBorderBrushProperty = 
            DependencyProperty.Register(
                "TextBorderBrush", 
                typeof(Brush), 
                typeof(NumericUpDownControl));
        public Brush TextBorderBrush
        {
            get { return (Brush)GetValue(TextBorderBrushProperty); }
            set
            {
                SetValue(TextBorderBrushProperty, value);
            }
        }


        public static readonly DependencyProperty TextBackgroundBrushProperty =
            DependencyProperty.Register(
                "TextBackgroundBrush",
                typeof(Brush),
                typeof(NumericUpDownControl));
        public Brush TextBackgroundBrush
        {
            get { return (Brush)GetValue(TextBackgroundBrushProperty); }
            set
            {
                SetValue(TextBackgroundBrushProperty, value);
            }
        }


        public static readonly DependencyProperty TextForegroundBrushProperty = 
            DependencyProperty.Register(
                "TextForegroundBrush", 
                typeof(Brush), 
                typeof(NumericUpDownControl));

        public Brush TextForegroundBrush
        {
            get { return (Brush)GetValue(TextForegroundBrushProperty); }
            set
            {
                SetValue(TextForegroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register(
                "Value", 
                typeof(decimal), 
                typeof(NumericUpDownControl));

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                "MinValue",
                typeof(decimal),
                typeof(NumericUpDownControl));

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                "MaxValue",
                typeof(decimal),
                typeof(NumericUpDownControl));

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                "Step",
                typeof(decimal),
                typeof(NumericUpDownControl));

        public decimal Step
        {
            get { return /*((decimal)GetValue(StepProperty)).Equals(0) ? 1 : */(decimal)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty NumberTypeProperty =
            DependencyProperty.Register(
                "NumberType",
                typeof(DataTypes.NumberType),
                typeof(NumericUpDownControl));
        public DataTypes.NumberType NumberType
        {
            get { return (DataTypes.NumberType)GetValue(NumberTypeProperty); }
            set { SetValue(NumberTypeProperty, value); }
        }

        public static readonly DependencyProperty TextFontSizeProperty = 
            DependencyProperty.Register(
                "TextFontSize", 
                typeof(double), 
                typeof(NumericUpDownControl));
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set
            {
                SetValue(TextFontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty ReadOnlyProperty = 
            DependencyProperty.Register(
                "ReadOnly", 
                typeof(bool), 
                typeof(NumericUpDownControl));
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set
            {
                SetValue(ReadOnlyProperty, value);
            }
        }

        public NumericUpDownControl()
        {
            NumberType = DataTypes.NumberType.Integer;

            InitializeComponent();
            MinValue = 0;
            MaxValue = 100;
            Step = 1;
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void ButtonInc_Click(object sender, RoutedEventArgs e)
        {
            if (!ReadOnly)
            {
                if (Value + Step <= MaxValue)
                {
                    Value += Step;
                }
            }
        }

        private void ButtonDec_Click(object sender, RoutedEventArgs e)
        {
            if (!ReadOnly)
            {
                if (Value - Step >= MinValue)
                {
                    Value -= Step;
                }
            }
        }
    }
}
