using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для LayerControl.xaml
    /// </summary>
    public partial class LayerControl : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate bool DeleteLayer(int layerId, ulong projectId);
        public event DeleteLayer OnDeleteLayer;

        private double controlAdpectRatio = 3.0d;

        private double currentHeight;

        public double CurrentHeight
        {
            get { return this.Width / controlAdpectRatio; }
            set
            {
                currentHeight = value;
                OnPropertyChanged("CurrentHeight");
            }
        }

        private ulong projectId;
        #region Layer params
        public static readonly DependencyProperty LayerIdProperty = DependencyProperty.Register(
            "LayerId", typeof(int), typeof(LayerControl));
        public int LayerId
        {
            get { return (int)GetValue(LayerIdProperty); }
            set
            {
                SetValue(LayerIdProperty, value);
                OnPropertyChanged("LayerId");
            }
        }

        /*private int layerId;
        public int LayerId
        {
            get { return layerId; }
        }*/

        public static readonly DependencyProperty LayerNameProperty = DependencyProperty.Register(
            "LayerName", typeof(string), typeof(LayerControl));
        public string LayerName
        {
            get { return (string)GetValue(LayerNameProperty); }
            set
            {
                SetValue(LayerNameProperty, value);
                OnPropertyChanged("LayerName");
            }
        }

        /*private string layerName;
        public string LayerName
        {
            get { return layerName; }
            set
            {
                layerName = layerObj == null ? "" : layerObj.Name;
                OnPropertyChanged("LayerName");
            }
        }*/

        public static readonly DependencyProperty LayerOrderProperty = DependencyProperty.Register(
            "LayerOrder", typeof(int), typeof(LayerControl));
        public int LayerOrder
        {
            get { return (int)GetValue(LayerOrderProperty); }
            set
            {
                SetValue(LayerOrderProperty, value);
                OnPropertyChanged("LayerOrder");
            }
        }

        /*private int order;
        public int Order
        {
            get { return order; }
            set
            {
                order = layerObj == null ? -1 : layerObj.Order;
                OnPropertyChanged("Order");
            }
        }*/

        private string opticalSchema;
        public string OpticalSchema
        {
            get { return opticalSchema; }
            set
            {
                opticalSchema = value;
                OnPropertyChanged("OpticalSchema");
            }
        }

        public static readonly DependencyProperty LayerEnabledProperty = DependencyProperty.Register(
            "LayerEnabled", typeof(bool), typeof(LayerControl), 
            new FrameworkPropertyMetadata(
                false, new PropertyChangedCallback(OnLayerEnabledPropertyChanged)
                )
            );
        public bool LayerEnabled
        {
            get { return (bool)GetValue(LayerEnabledProperty); }
            set
            {
                SetValue(LayerEnabledProperty, value);
                OnPropertyChanged("LayerEnabled");
            }
        }

        /*public static readonly DependencyProperty LayerPreviewProperty = DependencyProperty.Register(
            "LayerPreview", typeof(System), typeof(LayerControl),
            new FrameworkPropertyMetadata(
                false, new PropertyChangedCallback(OnLayerPreviewPropertyChanged)
                )
            );
        public Emgu.CV.Mat LayerPreview { get; set; }*/
        /*{
            get { return (Emgu.CV.Mat)GetValue(LayerPreviewProperty); }
            set
            {
                SetValue(LayerPreviewProperty, value);
                OnPropertyChanged("LayerPreview");
            }
        }*/

        /*public static readonly DependencyProperty LayerMaskProperty = DependencyProperty.Register(
            "LayerMask", typeof(Emgu.CV.Mat), typeof(LayerControl),
            new FrameworkPropertyMetadata(
                false, new PropertyChangedCallback(OnLayerMaskPropertyChanged)
                )
            );
        public Emgu.CV.Mat LayerMask
        {
            get { return (Emgu.CV.Mat)GetValue(LayerMaskProperty); }
            set
            {
                SetValue(LayerMaskProperty, value);
                OnPropertyChanged("LayerMask");
            }
        }
        */

        /*private bool layerEnabled;
        public bool LayerEnabled
        {
            get { return LayerObject == null ? false : LayerObject.Enabled; }
            set
            {
                layerEnabled = layerObj == null ? false : value;
                layerObj.Enabled = layerEnabled;
                //SetValue(layerEnabled, value);
                OnPropertyChanged("LayerEnabled");
                ChangeEnabled(value);
            }
        }*/
        #endregion

        public static readonly DependencyProperty LayerPreviewProperty = DependencyProperty.Register(
            "LayerPreview", typeof(ImageSource), typeof(LayerControl));
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
            "LayerMask", typeof(ImageSource), typeof(LayerControl));
        public ImageSource LayerMask
        {
            get { return (ImageSource)GetValue(LayerMaskProperty); }
            set
            {
                SetValue(LayerMaskProperty, value);
                OnPropertyChanged("LayerMask");
            }
        }
        

        public LayerControl()
        {
            InitializeComponent();
            //LayerName = "layername";
            //this.DataContext = this;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)

            {

                this.Width = 300; ;

                this.Height = 100; ;

            }
        }

        public LayerControl(bool layerEnabled, string opticalSchema, string technologyName, ulong projectID)
        {
            //LayerPreview = null;
            InitializeComponent();
            this.DataContext = this;
            LoadControlByLayer(layerEnabled, opticalSchema, technologyName, projectID);
        }

        ~LayerControl()
        {
            
        }

        #region Actions
        public void LoadControlByLayer(bool layerEnabled, string opticalSchema, string technologyName, ulong projectID)
        {
            projectId = projectID;
            //layerId = layer.id;
            #region images
            /*if (layer.Image != null)
            {

                LayerPreview = ImageProcessing.ImageProcessingUI.GetImageSourceFromMat(layer.Image);
                if (layer.Thumbnail != null)
                {

                }
                else
                {

                    //layer.Thumbnail = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>()
                }
            }
            else
            {
                LoadEmptyPreviewImage();
            }

            if (layer.Mask != null)
            {
                LayerMask = ImageProcessing.ImageProcessingUI.GetImageSourceFromMat(layer.Image);
                if (layer.MaskThumbnail != null)
                {

                }
                else
                {

                    //layer.MaskThumbnail = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>()
                }
            }
            else
            {
                LoadEmptyMaskImage();
            }*/
            #endregion
            //LayerName = layer.Name;
            ChangeEnabled(layerEnabled);
            //LayerEnabled = layer.Enabled;
            //Order = layer.Order;
            //ChangeEnabled(layerEnabled);
            OpticalSchema = opticalSchema;
            SetLayerColor(Color.FromRgb(255, 48, 255));
            //Binding b = new Binding();
            //b.Source = layer;
            //b.Path = new PropertyPath(layer.Enabled);
            //b.Mode = BindingMode.TwoWay;
            //b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(this, this.layerEnabled, b);
            //ChangeEnabled(layer.Enabled);
        }

        private void LoadEmptyPreviewImage()
        {

        }

        private void LoadEmptyMaskImage()
        {

        }

        private void SetLayerColor(Color technologyColor)
        {
            SolidColorBrush brush = new SolidColorBrush(technologyColor);
            TechnologyNameGrid.Background = brush;
        }

        private static void OnLayerEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.PropertyType.Equals(typeof(bool)))
            {
                ((LayerControl)sender).ChangeEnabled((bool)e.NewValue);
            }
            /*PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
            {
                h(sender, new PropertyChangedEventArgs("Second"));
            }*/
        }

        private void ChangeEnabled(bool value)
        {
            Color enColor = Color.FromRgb(48, 48, 48);
            Color disColor = Color.FromRgb(10, 10, 10);
            SolidColorBrush brush = new SolidColorBrush(enColor);
            if (!value)
            {
                brush = new SolidColorBrush(disColor);
            }

            EnabledGrid.Background = brush;
            EnabledPolygon.Fill = brush;
        }



        

        private void LayerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //float ar = this.des = "300"
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (OnDeleteLayer?.Invoke(LayerId, projectId) == true)
            {
                //~LayerControl();
                //this.Dispose;
            }
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                if (property.Equals("LayerEnabled"))
                {
                    ChangeEnabled(LayerEnabled);
                }
            }
        }
        #endregion
    }
}
