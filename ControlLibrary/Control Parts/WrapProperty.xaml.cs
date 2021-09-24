using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для WrapProperty.xaml
    /// </summary>
    public partial class WrapProperty : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameDP); }
            set
            {
                SetValue(PropertyNameDP, value);
                OnPropertyChanged("PropertyName");
            }
        }

        public static readonly DependencyProperty PropertyNameDP = DependencyProperty.Register(
            "PropertyName", typeof(string), typeof(WrapProperty));

        public static DependencyProperty PropertiesDP { get; set; }

        public ObservableCollection<Control> Properties
        {
            get { return (ObservableCollection<Control>)GetValue(PropertiesDP); }
            set { SetValue(PropertiesDP, value); }
        }
        public WrapProperty()
        {
            InitializeComponent();
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
