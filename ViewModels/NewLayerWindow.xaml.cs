using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ViewModels
{
    /// <summary>
    /// Логика взаимодействия для NewProjectWindow.xaml
    /// </summary>
    public partial class NewLayerWindow : Window, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        private System.Globalization.NumberFormatInfo ni = System.Globalization.NumberFormatInfo.InvariantInfo;
        private Layers.Layer layerObj;
        private ProjectClassLib.Project projectObj;

        #region Events
        public delegate bool CreateLayer(Layers.Layer layer);
        public event CreateLayer OnCreateLayer;

        public delegate bool DeleteProject(ulong id);
        public event DeleteProject OnDeleteProject;
        #endregion

        #region Layer properties
        /*public static readonly DependencyProperty HologramWidthDP = DependencyProperty.Register(
            "HologramWidth", typeof(string), typeof(NewProjectWindow));

        public string HologramWidth
        {
            get { return (string)GetValue(HologramWidthDP); }
            set
            {
                SetValue(HologramWidthDP, value);
                OnPropertyChanged("HologramWidth");
            }
        }*/

        ControlLibrary.ValidationRules.StringWrapper sw = new ControlLibrary.ValidationRules.StringWrapper();


        private string layerName;
        public string LayerName
        {
            get { return layerObj == null ? null : layerObj.Name; }
            set
            {
                //ValidateName();
                layerName = layerObj == null ? null : value;
                if (layerObj != null) layerObj.Name = layerName;
                OnPropertyChanged("LayerName");
            }
        }

        private string technologyName;
        public string TechnologyName
        {
            get
            {
                return technologyName;
            }
            set
            {
                technologyName = value;
                OnPropertyChanged("TechnologyName");
            }
        }

        public ColorProfile.ColorProfileVM cpVM;
        #endregion

        public NewLayerWindow()
        {
            TechnologyName = " ";
            cpVM = new ColorProfile.ColorProfileVM();
            InitializeComponent();
            Init_Control(technologyName);
        }

        public NewLayerWindow(string techName, ProjectClassLib.Project project)
        {
            TechnologyName = techName;
            cpVM = new ColorProfile.ColorProfileVM();
            InitializeComponent();
            lpcProps.VM = cpVM;
            cpVM.MainArcWidth = 10;
            projectObj = project;
            Init_Control(techName);
        }

        private void Init_Control(string techName)
        {
            DataContext = this;
            //txtMaskMultiplication.Text = ProjectClassLib.Project.ResolutionMultiplicationSymbol;
            //txtMaskUnit.Text = ProjectClassLib.Project.ResolutionUnit;
            //Layers.Layer layer = Layers.Layer.NewLayer(technologyName);

            CreateLayerByTemplate(techName, null);

            //dc1.NumberType = DataTypes.NumberType.Integer;
            //dc2.NumberType = DataTypes.NumberType.Fractional;

            Binding b = new Binding();
            b.Source = this;
            b.Path = new PropertyPath("ProjectName");
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(txtProjectPath, txtProjectPath.Text, b);
            /*if (layerObj != null)
            {
                LoadControlByLayer(layerObj);
            }*/
            //OnPropertyChanged("UnitSize");
            //maskWidth = "4800";
            //maskHeight = "2387";
            Binding b1 = new Binding();
            b1.Source = this;
            b1.Path = new PropertyPath("HologramWidth");
            b1.Mode = BindingMode.TwoWay;
            b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(dc1, dc1.Val, b1);
        }

        public void CreateLayerByTemplate(string techName, string templateName)
        {
            Layers.Layer newLayer = Layers.Layer.NewLayer(technologyName);
            if (newLayer == null)
            {
                //close
                return;
            }

            if (!string.IsNullOrWhiteSpace(newLayer.TechnologyName))
            {
                TechnologyName = newLayer.TechnologyName;
            }

            if (string.IsNullOrWhiteSpace(templateName))
            {
                //use default template
                newLayer.Enabled = true;
                newLayer.PreviewResolution = new DataContainers.Resolution(100, 100);

                #region debug
                newLayer.ImagePath = @"E:\New project 1\Layers\Layer 1\SourceImages\Star3.bmp";
                newLayer.MaskPath = @"E:\New project 1\Layers\Layer 1\Mask\Holog-30x30_Mask_Face.bmp";

                newLayer.Image = ImageProcessing.ImageProcessing.LoadMatFromFile(newLayer.ImagePath);
                newLayer.Mask = ImageProcessing.ImageProcessing.LoadMatFromFile(newLayer.MaskPath);

                #endregion

                #region Order, name, ID
                newLayer.Order = (int)ProjectClassLib.ProjectLogic.GetNewLayerOrder(projectObj);
                newLayer.Name = $"Layer {newLayer.Order + 1} - {newLayer.TechnologyName}";
                newLayer.Id = ProjectClassLib.ProjectLogic.GetNewLayerID(projectObj);
                #endregion

                PropertyInfo anglesInfo = newLayer.GetType().GetProperty("AnglesPath");
                if (anglesInfo != null)
                {
                    string anglesPath = @"E:\New project 1\Layers\Layer 1\AnglesFile\ang.ini";
                    anglesInfo.SetValue(newLayer, anglesPath);
                }

                /*PropertyInfo stepInfo = newLayer.GetType().GetProperty("Step");
                if (stepInfo != null)
                {
                    decimal step = 0.625m;
                    stepInfo.SetValue(newLayer, step);
                }

                PropertyInfo radiusInfo = newLayer.GetType().GetProperty("ArcRadius");
                if (radiusInfo != null)
                {
                    DataContainers.Resolution<decimal> arcRadius = new DataContainers.Resolution<decimal>(20, 0.2m);
                    radiusInfo.SetValue(newLayer, arcRadius);
                }*/
                
            }
            else
            {
                //use template
            }
            layerObj = newLayer;
            LoadControlByLayer(newLayer);
        }

        private void LoadControlByLayer(Layers.Layer layer)
        {
            //Bindings
            lpcProps.Empty = false;
            LayerProperteisControlOperations.BindLayerObjToPropertyControl(layer, lpcProps);
        }

        private void UpdateByTechnologyName()
        {

        }

        #region Buttons
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            bool success = (OnCreateLayer?.Invoke(layerObj)).GetValueOrDefault(false);
            if (success)
            {
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /*
        // команда открытия файла
        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (dialogService.OpenFileDialog() == true)
                          {
                              var phones = fileService.Open(dialogService.FilePath);
                              Phones.Clear();
                              foreach (var p in phones)
                                  Phones.Add(p);
                              dialogService.ShowMessage("Файл открыт");
                          }
                      }
                      catch (Exception ex)
                      {
                          dialogService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }
        */
    }
}
