using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLibrary.Control_Parts
{
    /// <summary>
    /// Логика взаимодействия для DimensionControl.xaml
    /// </summary>
    public partial class DimensionControl : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        /*private DataTypes.NumberType numberType;
        public DataTypes.NumberType NumberType
        {
            get
            {
                return numberType;
            }
            set
            {
                numberType = value;
                OnPropertyChanged("NumberType");
            }
        }*/

        public static readonly DependencyProperty NumberTypeProperty =
            DependencyProperty.Register(
                "NumberType",
                typeof(DataTypes.NumberType),
                typeof(DimensionControl));
        public DataTypes.NumberType NumberType
        {
            get { return (DataTypes.NumberType)GetValue(NumberTypeProperty); }
            set { SetValue(NumberTypeProperty, value); }
        }

        /*
        public Type NumberType
        {
            get
            {
                return ValRule.NumberType;
            }
            set
            {
                ValRule.NumberType = value;
            }
        }
        */
        #region Value (textBox)
        //dynamic numValue;

        public static readonly DependencyProperty Val_DP = DependencyProperty.Register("Val", typeof(decimal), typeof(DimensionControl));

        public decimal Val
        {
            get { return (decimal)GetValue(Val_DP); }
            set
            {
                SetValue(Val_DP, value);
            }
        }

        private decimal minValue;
        public decimal MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        private decimal maxValue;
        public decimal MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        private decimal step;
        public decimal Step
        {
            get { return step; }
            set
            {
                step = value;
                OnPropertyChanged("Step");
            }
        }

        private SolidColorBrush valColor;
        public SolidColorBrush ValColor
        {
            get { return valColor; }
            set
            {
                valColor = value;
                OnPropertyChanged("ValColor");
            }
        }

        private SolidColorBrush valBackColor;
        public SolidColorBrush ValBackColor
        {
            get { return valBackColor; }
            set
            {
                valBackColor = value;
                OnPropertyChanged("ValBackColor");
            }
        }

        private SolidColorBrush valBorderColor;
        public SolidColorBrush ValBorderColor
        {
            get { return valBorderColor; }
            set
            {
                valBorderColor = value;
                OnPropertyChanged("ValBorderColor");
            }
        }

        private double valFontSize;
        public double ValFontSize
        {
            get { return valFontSize.Equals(0) ? 1 : valFontSize; }
            set
            {
                valFontSize = value;
                OnPropertyChanged("ValFontSize");
            }
        }

        private bool valReadOnly;
        public bool ValReadOnly
        {
            get { return valReadOnly; }
            set
            {
                valReadOnly = value;
                OnPropertyChanged("ValReadOnly");
            }
        }
        #endregion

        #region Property name (label)
        private string propertyName;
        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private SolidColorBrush propertyNameColor;
        public SolidColorBrush PropertyNameColor
        {
            get { return propertyNameColor; }
            set
            {
                propertyNameColor = value;
                OnPropertyChanged("PropertyNameColor");
            }
        }

        private double propertyNameFontSize;
        public double PropertyNameFontSize
        {
            get { return propertyNameFontSize.Equals(0) ? 1 : propertyNameFontSize; }
            set
            {
                propertyNameFontSize = value;
                OnPropertyChanged("PropertyNameFontSize");
            }
        }
        #endregion

        #region Property unit (label)
        private string propertyUnit;
        public string PropertyUnit
        {
            get { return propertyUnit; }
            set
            {
                propertyUnit = value;
                OnPropertyChanged("PropertyUnit");
            }
        }

        private SolidColorBrush propertyUnitColor;
        public SolidColorBrush PropertyUnitColor
        {
            get { return propertyUnitColor; }
            set
            {
                propertyUnitColor = value;
                OnPropertyChanged("PropertyUnitColor");
            }
        }

        private double propertyUnitFontSize;
        public double PropertyUnitFontSize
        {
            get { return propertyUnitFontSize.Equals(0) ? 1 : propertyUnitFontSize; }
            set
            {
                propertyUnitFontSize = value;
                OnPropertyChanged("PropertyUnitFontSize");
            }
        }
        #endregion

        public DimensionControl()
        {
            //NumberType = DataTypes.NumberType.Integer;
            InitializeComponent();
            MinValue = 0;
            MaxValue = 100;
            Step = 1;
            //this.NumberType = typeof(decimal);
            //FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            //DependencyProperty.Register(
            //Val_DP = new DependencyProperty("Val", typeof(string), typeof(DimensionControl), metadata, CoerceValueCallback());
            //DataContext = this;
            /*txtVal.DataContext = this;
            lblPropName.DataContext = this;
            lblPropUnit.DataContext = this;*/
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