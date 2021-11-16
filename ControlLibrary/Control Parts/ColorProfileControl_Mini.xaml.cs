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
    /// Логика взаимодействия для ColorProfileControl_Mini.xaml
    /// </summary>
    public partial class ColorProfileControl_Mini : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        /*
        //VM
        private ColorProfile.ColorProfileVM vm;
        public ColorProfile.ColorProfileVM VM
        {
            get { return vm; }
            set
            {
                vm = value;
                OnPropertyChanged("VM");
            }
        }

        /*public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register(
                "VM",
                typeof(ColorProfile.ColorProfileVM),
                typeof(ColorProfileControl_Mini));
        public ColorProfile.ColorProfileVM VM
        {
            get { return (ColorProfile.ColorProfileVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }*/

        //Physical dimension unit
        /*public static readonly DependencyProperty ArcWidthLinearUnitProperty =
            DependencyProperty.Register(
                "ArcWidthLinearUnit",
                typeof(DataTypes.LinearUnit),
                typeof(ColorProfileControl_Mini));
        public DataTypes.LinearUnit ArcWidthLinearUnit
        {
            get { return (DataTypes.LinearUnit)GetValue(ArcWidthLinearUnitProperty); }
            set { SetValue(ArcWidthLinearUnitProperty, value); }
        }

        public static readonly DependencyProperty ArcWidthDimnsionUnitProperty =
            DependencyProperty.Register(
                "ArcWidthDimnsionUnit",
                typeof(DataTypes.ImageDimnsionUnit),
                typeof(ColorProfileControl_Mini));
        public DataTypes.ImageDimnsionUnit ArcWidthDimnsionUnit
        {
            get { return (DataTypes.ImageDimnsionUnit)GetValue(ArcWidthDimnsionUnitProperty); }
            set { SetValue(ArcWidthDimnsionUnitProperty, value); }
        }*/

        public ColorProfileControl_Mini()
        {
            InitializeComponent();

            //VM = new ColorProfile.ColorProfileVM();
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                /*if (property.Equals("LayerEnabled"))
                {
                    ChangeEnabled(LayerEnabled);
                }*/
            }
        }
    }
}
