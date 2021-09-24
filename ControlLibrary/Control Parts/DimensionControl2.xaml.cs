using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlLibrary.Control_Parts
{
    /// <summary>
    /// Логика взаимодействия для DimensionControl2.xaml
    /// </summary>
    public partial class DimensionControl2 : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        #region Value (textBox)
        public static readonly DependencyProperty Val_DP = DependencyProperty.Register(
            "Val", typeof(string), typeof(DimensionControl2));

        public string Val
        {
            get { return (string)GetValue(Val_DP); }
            set
            {
                SetValue(Val_DP, value);
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
            get { return valFontSize; }
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
            get { return propertyUnitFontSize; }
            set
            {
                propertyUnitFontSize = value;
                OnPropertyChanged("PropertyUnitFontSize");
            }
        }
        #endregion

        public DimensionControl2()
        {
            InitializeComponent();
            //DataContext = this;
            txtVal.DataContext = this;
            lblPropUnit.DataContext = this;
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
