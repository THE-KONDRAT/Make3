using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для LayerPropertiesControl.xaml
    /// </summary>
    public partial class LayerPropertiesControl : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

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
        /*
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register(
                "VM",
                typeof(ColorProfile.ColorProfileVM),
                typeof(LayerPropertiesControl));
        public ColorProfile.ColorProfileVM VM
        {
            get { return (ColorProfile.ColorProfileVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }
        */
        #region Events
        public delegate void PreviewClicked();
        public event PreviewClicked OnPreviewClicked;
        #endregion

        /*public static readonly DependencyProperty UnitSizeProperty = DependencyProperty.Register(
            "UnitSize", typeof(decimal), typeof(LayerPropertiesControl));

        public decimal UnitSize
        {
            get { return (decimal)GetValue(UnitSizeProperty); }
            set
            {
                SetValue(UnitSizeProperty, value);
                OnPropertyChanged("UnitSize");
            }
        }*/

        /*public static readonly DependencyProperty LayerPreviewProperty = DependencyProperty.Register(
            "LayerPreview", typeof(ImageSource), typeof(LayerPropertiesControl));
        public ImageSource LayerPreview
        {
            get { return (ImageSource)GetValue(LayerPreviewProperty); }
            set
            {
                SetValue(LayerPreviewProperty, value);
                OnPropertyChanged("LayerPreview");
            }
        }

        public static readonly DependencyProperty LayerMaskProperty = DependencyProperty.Register(
            "LayerMask", typeof(ImageSource), typeof(LayerPropertiesControl));
        public ImageSource LayerMask
        {
            get { return (ImageSource)GetValue(LayerMaskProperty); }
            set
            {
                SetValue(LayerMaskProperty, value);
                OnPropertyChanged("LayerMask");
            }
        }*/

        public static readonly DependencyProperty PreviewCommandProperty =
        DependencyProperty.Register(
        "PreviewCommand",
        typeof(ICommand),
        typeof(LayerPropertiesControl));

        public ICommand PreviewCommand
        {
            get
            {
                return (ICommand)GetValue(PreviewCommandProperty);
            }

            set
            {
                SetValue(PreviewCommandProperty, value);
            }
        }


        public LayerPropertiesControl()
        {
            //VM = new ColorProfile.ColorProfileVM();
            InitializeComponent();
            //LayerName = "testL";

            //var a = btnPreview.Command;
        }

        #region Image Path
        private UI_Helper.RelayCommand openCommand;
        public UI_Helper.RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      MessageBox.Show("Open_nnn");
                      //OnClearClicked?.Invoke();
                  }
                  ));
            }
        }

        private UI_Helper.RelayCommand clearCommand;
        public UI_Helper.RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                  (clearCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      MessageBox.Show("Clearr");
                      //OnClearClicked?.Invoke();
                  }
                  ));
            }
        }

        #endregion

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
