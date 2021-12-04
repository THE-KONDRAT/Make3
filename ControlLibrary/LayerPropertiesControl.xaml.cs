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
        private bool empty;
        public bool Empty
        {
            get { return empty; }
            set
            {
                empty = value;
                OnPropertyChanged("Empty");
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

    public class LayerDataTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary resourceDictionary { get; set; }
        
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            DataTemplate dt = element.FindResource("Empty") as DataTemplate;
            if (element != null)
            {
                if (item != null)
                {
                    var n = item.GetType().Name;

                    try
                    {
                        dt = element.FindResource(item.GetType().Name) as DataTemplate;

                    }
                    catch (Exception e)
                    {

                    }
                }
                return dt;
                
                    
            }
            return null;
            resourceDictionary = new ResourceDictionary();
            /*string a = "../ControlLibrary;component/Layer Controls/SharedResourceDictionary.xaml";
            Uri rel = new Uri(a, UriKind.RelativeOrAbsolute);
            var a = 
            resourceDictionary.Source = new Uri(rel.AbsoluteUri, UriKind.RelativeOrAbsolute);*/

            /*Uri abs = new Uri(@"E:\Make3\ControlLibrary\Layer Controls\LayersResourceDictionary.xaml", UriKind.RelativeOrAbsolute);

            DataTemplate dt = null;
            if (item == null)
            {
                dt = (DataTemplate)resourceDictionary["Empty"];
            }
            else
            {
                dt = (DataTemplate)resourceDictionary[item.GetType().Name];
            }

            if (resourceDictionary != null)
            {
                 return dt;
            }
            else
            {
                return null;
            }*/

        }

    }

    public class UITemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary resourceDictionary { get; set; }

        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            DataTemplate dt = element.FindResource("UIEmpty") as DataTemplate;
            if (element != null)
            {
                if (item != null)
                {
                    //This property exists only in Layer-based classes
                    System.Reflection.PropertyInfo tn = item.GetType().GetProperty("TechnologyName");
                    try
                    {

                        if (tn != null)
                        {
                            dt = element.FindResource("LayerUI") as DataTemplate;
                        }
                        //If we want to exclude "Empty" DataTemplate
                        /*var layerTemplate = element.FindResource(item.GetType().Name) as DataTemplate;
                        if (layerTemplate != null)
                        {
                            dt = element.FindResource("LayerUI") as DataTemplate;
                        }*/

                    }
                    catch (Exception e)
                    {

                    }
                }
                return dt;


            }
            return null;
            resourceDictionary = new ResourceDictionary();

        }

    }
}
