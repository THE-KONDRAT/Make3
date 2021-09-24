using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class NewProjectWindow : Window, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        private System.Globalization.NumberFormatInfo ni = System.Globalization.NumberFormatInfo.InvariantInfo;
        ProjectClassLib.Project projectObj;

        #region Events
        public delegate bool SaveProject(ProjectClassLib.Project project);
        public event SaveProject OnSaveProject;

        public delegate bool DeleteProject(ulong id);
        public event DeleteProject OnDeleteProject;
        #endregion

        #region Project properties
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


        private string projectName;
        public string ProjectName
        {
            get { return projectObj == null ? null : projectObj.Name; }
            set
            {
                //ValidateName();
                projectName = projectObj == null ? null : value;
                if (projectObj != null) projectObj.Name = projectName;
                OnPropertyChanged("ProjectName");
            }
        }

        private string projectPath;
        public string ProjectPath
        {
            get { return projectObj == null ? null : projectObj.FullPath; }
            set
            {
                //ValidateName();
                projectPath = projectObj == null ? null : value;
                if (projectObj != null) projectObj.FullPath = projectPath;
                OnPropertyChanged("ProjectPath");
            }
        }

        private string projectFullPath;
        public string ProjectFullPath
        {
            get
            {
                //projectFullPath = FileOperations.FileStructure.GetProjectFullPath(ProjectPath, ProjectName);
                projectFullPath = FileOperations.FileStructure.GetProjectFullPath(txtProjectPath.Text, txtProjectName.Text);
                return projectFullPath;
            }
            set
            {
                projectFullPath = value;
                OnPropertyChanged("ProjectFullPath");
            }
        }

        #region Hologram dimensions
        private decimal hologramWidth;
        public string HologramWidth
        {
            get { return projectObj == null ? null : Convert.ToString(projectObj.HologramDimension.Width, ni); }
            set
            {
                bool a = decimal.TryParse(value, System.Globalization.NumberStyles.Float, ni, out hologramHeight);
                hologramWidth = Convert.ToDecimal(projectObj == null ? "-1" : value, ni);
                decimal height = projectObj == null ? -1 : projectObj.HologramDimension.Height;
                projectObj.HologramDimension = new DataContainers.LinearDimension<decimal>(Convert.ToDecimal(hologramWidth, ni), height);
                //SetValue(layerEnabled, value);
                OnPropertyChanged("HologramWidth");
                //OnPropertyChanged("MaskResolution");
                OnPropertyChanged("MaskWidth");
            }
        }

        private decimal hologramHeight;
        public string HologramHeight
        {
            get { return projectObj == null ? null : Convert.ToString(projectObj.HologramDimension.Height, ni); }
            set
            {
                bool a = decimal.TryParse(value, System.Globalization.NumberStyles.Float, ni, out hologramHeight);
                hologramHeight = Convert.ToDecimal(projectObj == null ? "-1" : value, ni);
                decimal width = projectObj == null ? -1 : projectObj.HologramDimension.Width;
                projectObj.HologramDimension = new DataContainers.LinearDimension<decimal>(width, Convert.ToDecimal(hologramHeight, ni));
                //SetValue(layerEnabled, value);
                OnPropertyChanged("HologramHeight");
                //OnPropertyChanged("MaskResolution");
                OnPropertyChanged("MaskHeight");
            }
        }
        #endregion

        #region Mask size recommended
        private string maskWidth;
        public string MaskWidth
        {
            get { return projectObj == null ? null : Convert.ToString(ProjectClassLib.Project.GetMaskSizeRecommended(projectObj.HologramDimension, projectObj.UnitSize).X, ni); }
            set
            {
                maskWidth = projectObj == null ? null : value;
                //decimal height = projectObj == null ? -1 : projectObj.HologramDimension.Height;
                OnPropertyChanged("MaskWidth");
            }
        }

        private string maskHeight;
        public string MaskHeight
        {
            get { return projectObj == null ? null : Convert.ToString(ProjectClassLib.Project.GetMaskSizeRecommended(projectObj.HologramDimension, projectObj.UnitSize).Y, ni); }
            set
            {
                maskHeight = projectObj == null ? null : value;
                //decimal width = projectObj == null ? -1 : projectObj.HologramDimension.Width;
                OnPropertyChanged("MaskHeight");
            }
        }

        private string maskResolution;
        public DataContainers.Resolution MaskResolution
        {
            get { return new DataContainers.Resolution(Convert.ToUInt32(MaskWidth, ni), Convert.ToUInt32(MaskHeight, ni)); }//$"{MaskWidth}x{MaskHeight} pix"/*projectObj == null ? null : maskWidth*//*Convert.ToString(projectObj.HologramDimension.Width, System.Globalization.NumberFormatInfo.InvariantInfo)*/; }
            set
            {
                //DataContainers.Resolution<decimal> res = ParseMaskResolution(value);
                MaskWidth = value.X.ToString();//res.X.ToString();
                MaskHeight = value.Y.ToString();//res.Y.ToString();
                //maskWidth = projectObj == null ? null : value;
                //decimal height = projectObj == null ? -1 : projectObj.HologramDimension.Height;
                //projectObj.HologramDimension = new DataContainers.LinearDimension<decimal>(Convert.ToDecimal(maskWidth, System.Globalization.NumberFormatInfo.InvariantInfo), height);
                //SetValue(layerEnabled, value);
                OnPropertyChanged("MaskResolution");
            }
        }
        private DataContainers.Resolution<decimal> ParseMaskResolution(string s)
        {
            decimal width = 0, height = 0;
            s = "1920.4x2029 pix";
            Regex r = new Regex(@"(\d+.?\d+)(x{1}\d+.?\d+)");
            Match match = r.Match(s);
            for (int i = 0; i < match.Groups.Count; i++)
            {
                if (i == 0)
                {
                    width = Convert.ToDecimal(match.Groups[i].Value.Replace("x", ""), System.Globalization.NumberFormatInfo.InvariantInfo);
                }
                else if (i == 1)
                {
                    width = Convert.ToDecimal(match.Groups[i].Value.Replace("x", ""), System.Globalization.NumberFormatInfo.InvariantInfo);
                }
            }
            /*while (match.Success)
            {
                string sMatch = match.Groups[0].Value;
                result = sMatch;
                match = match.NextMatch();
            }*/

            DataContainers.Resolution<decimal> result = new DataContainers.Resolution<decimal>(width, height);
            return result;
        }
        #endregion

        #region Unit size
        private decimal unitSize;
        public string UnitSize
        {
            get { return projectObj == null ? null : Convert.ToString(projectObj.UnitSize, ni); }
            set
            {
                unitSize = Convert.ToDecimal(projectObj == null ? "-1" : value, ni);
                projectObj.UnitSize = Convert.ToDecimal(unitSize, ni);
                //SetValue(layerEnabled, value);
                OnPropertyChanged("UnitSize");
                //OnPropertyChanged("MaskResolution");
                OnPropertyChanged("MaskWidth");
                OnPropertyChanged("MaskHeight");
            }
        }
        #endregion

        #region Gray range
        #region Hologram dimensions
        private uint grayLow;
        public double GrayLow
        {
            get { return projectObj == null ? 256 : Convert.ToDouble(projectObj.GreyRange.Low, ni); }
            set
            {
                grayLow = Convert.ToUInt32(projectObj == null ? 256.0 : value, ni);
                uint high = projectObj == null ? 256 : projectObj.GreyRange.High;
                projectObj.GreyRange = new Ranges.GreyRange(grayLow, high);
                //SetValue(layerEnabled, value);
                OnPropertyChanged("GrayLow");
            }
        }

        private uint grayHigh;
        public double GrayHigh
        {
            get { return projectObj == null ? 256 : Convert.ToDouble(projectObj.GreyRange.High, ni); }
            set
            {
                grayHigh = Convert.ToUInt32(projectObj == null ? 256.0 : value, ni);
                uint low = projectObj == null ? 256 : projectObj.GreyRange.Low;
                projectObj.GreyRange = new Ranges.GreyRange(low, grayHigh);
                //SetValue(layerEnabled, value);
                OnPropertyChanged("GrayHigh");
            }
        }
        #endregion
        #endregion
        #endregion

        public NewProjectWindow()
        {
            InitializeComponent();
            DataContext = this;
            txtMaskMultiplication.Text = ProjectClassLib.Project.ResolutionMultiplicationSymbol;
            txtMaskUnit.Text = ProjectClassLib.Project.ResolutionUnit;
            ProjectClassLib.Project project = new ProjectClassLib.Project();
            dc1.NumberType = DataTypes.NumberType.Integer;
            dc2.NumberType = DataTypes.NumberType.Fractional;
            project.Name = "Test proj";
            project.FullPath = "E:\\";
            project.HologramDimension = new DataContainers.LinearDimension<decimal>(640, 1234.5m);
            project.UnitSize = 6.25m;
            project.GreyRange = new Ranges.GreyRange(20, 155);

            Binding b = new Binding();
            b.Source = this;
            b.Path = new PropertyPath("ProjectName");
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //BindingOperations.SetBinding(txtProjectPath, txtProjectPath.Text, b);
            if (project != null)
            {
                LoadControlByProject(project);
            }
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

        public void LoadProjectTemplate()
        {
            ProjectClassLib.Project newProject = new ProjectClassLib.Project();
            LoadControlByProject(newProject);
        }

        public void LoadControlByProject(ProjectClassLib.Project project)
        {


            projectObj = project;

            ProjectName = project.Name;
            ProjectPath = project.FullPath;

            hologramWidth = projectObj.HologramDimension == null ? -1 : projectObj.HologramDimension.Width;
            hologramHeight = projectObj.HologramDimension == null ? -1 : projectObj.HologramDimension.Height;

            decimal frameWidth = projectObj.FrameDimension == null ? -1 : projectObj.FrameDimension.Width;
            decimal frameResX = projectObj.FrameDimension == null ? -1 : (decimal)projectObj.FrameResolution.X;
            decimal frameHeight = projectObj.FrameDimension == null ? -1 : projectObj.FrameDimension.Height;
            decimal frameRexY = projectObj.FrameResolution == null ? -1 : (decimal)projectObj.FrameResolution.Y;
            unitSize = projectObj.UnitSize;

            uint grayLow = projectObj.GreyRange.Low;
            uint grayHigh = projectObj.GreyRange.High;

            //maskResolution = project.
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool success = (OnSaveProject?.Invoke(projectObj)).GetValueOrDefault(false);
            if (success)
            {
                this.Close();
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Спросить
            bool success = (OnDeleteProject?.Invoke(projectObj.id)).GetValueOrDefault(false);
            if (success)
            {
                LoadProjectTemplate();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (projectObj != null)
            {
                ControlLibrary.DialogService.FileDialogService FDS = new ControlLibrary.DialogService.FileDialogService();
                if (FDS.OpenFileDialog())
                {
                    ProjectPath = FDS.FilePath;
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (projectObj != null)
            {
                ProjectPath = null;
                /*projectObj.FullPath = null;
                //OnPropertyChanged("ProjectPath");*/
            }
        }


        private void txtProjectName_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPropertyChanged("ProjectFullPath");
        }

        private void txtProjectPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPropertyChanged("ProjectFullPath");
        }

        private void ValidateNamePath(object sender, string name, string path)
        {
            TextBox tb = (TextBox)sender;

            //tb.
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
