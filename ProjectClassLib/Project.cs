using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DataContainers;
using Newtonsoft.Json;

namespace ProjectClassLib
{
    [Serializable]
    // Set this 'Customer' class as the root node of any XML file its serialized to.
    [XmlRootAttribute("Project", Namespace = "", IsNullable = false)]
    public class Project : INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        #region Project params
        public string Name { get; set; }
        public UInt64 id { get; set; }

        [XmlIgnoreAttribute()]
        private string openedFilePath;
        /*Path of the project file. Fills when project loads from file. If generated project path and this path becomes different
        if means we should check new location and move project there
        if this string is null or empty this means that the project was created but not saved
         */
        [JsonIgnore]
        public string OpenedFilePath
        {
            get { return openedFilePath; }
        }

        //Flag that represents if project was changed and not saved

        [XmlIgnoreAttribute()]
        private bool projectSaved;
        [JsonIgnore]
        public bool ProjectSaved
        {
            get { return projectSaved; }
            set
            {
                projectSaved = value;
                //propertychanged
            }
        }

        [XmlIgnoreAttribute()]
        DateTime creationDate;

        // Set this 'DateTimeValue' field to be an attribute of the root node.
        [XmlAttributeAttribute(DataType = "date")]
        public DateTime CreationDate
        {
            get { return creationDate; }
        }

        // Set this 'DateTimeValue' field to be an attribute of the root node.
        [XmlAttributeAttribute(DataType = "date")]
        public DateTime LastUpdatedDate { get; set; }

        /*[XmlIgnoreAttribute]
        /// <summary>
        /// Show index of selected layer (if selected, else = null)
        /// </summary>
        private int layerSelected;
        public int LayerSelected
        {
            get
            {
                if (Layers == null)
                {
                    layerSelected = -1;
                    OnPropertyChanged("LayerSelected");
                }
                else
                {
                    if (Layers.Count < 1)
                    {
                        layerSelected = -1;
                        OnPropertyChanged("LayerSelected");
                    }
                    else
                    {
                        if (layerSelected > Layers.Count - 1)
                        {
                            layerSelected = Layers.Count - 1;
                            OnPropertyChanged("LayerSelected");
                        }
                    }
                }

                return layerSelected;
            }

            set
            {
                int input = -1;
                if (Layers != null)
                {
                    if (Layers.Count >= 1)
                    {
                        if (layerSelected <= Layers.Count - 1)
                        {
                            input = value;
                        }
                    }
                }
                layerSelected = input;
                OnPropertyChanged("LayerSelected");
            }
        }*/
        #endregion

        #region User params
        /// <summary>
        /// Project ppf file path
        /// </summary>
        public string FullPath { get; set; }
        public string ResultFullPath { get; set; }

        /// <summary>
        ///Resolution of holoprint matrix
        ///Unit: pix
        /// </summary>
        public DataContainers.Resolution FrameResolution { get; set; }

        /// <summary>
        ///Dimension of matrix projection on hologram
        ///Unit: mm
        /// </summary>
        public DataContainers.LinearDimension<decimal> FrameDimension { get; set; }

        //add unit


        /// <summary>
        ///Dimension of hologram 
        ///Unit: mm
        /// </summary>
        public DataContainers.LinearDimension<decimal> HologramDimension { get; set; }

        //add unit

        

        public Ranges.GreyRange GreyRange { get; set; }

        /// <summary>
        /// Layers collection
        /// </summary>
        [XmlArray("Layers"), XmlArrayItem("Layer", typeof(Layers.Layer))]
        public List<Layers.Layer> Layers { get; set; }

        /// <summary>
        /// Size of holographic pixel
        /// Unit: um
        /// </summary>
        public decimal UnitSize { get; set; }

        /// <summary>
        /// Pixels in UnitSize (np: 32= square 32x32)
        /// Unit: pix
        /// </summary>
        public uint PPU { get; set; }
        #endregion

        #region Static params
        public static readonly string ResolutionMultiplicationSymbol = "x";
        public static readonly string ResolutionUnit = "pix";
        #endregion

        #region Methods
        /// <summary>
        /// Sets file path from opened project file
        /// </summary>
        /// <param name="filePath"></param>
        public void SetFilePath(string filePath)
        {
            openedFilePath = filePath;
        }

        public void SetCreationDate(DateTime d)
        {
            creationDate = d;
        }

        public Project Clone()
        {
            return (Project)this.MemberwiseClone();
        }
        #endregion

        #region Constructors
        public Project()
        {
            //layerSelected = -1;

        }
        #endregion

        #region Template
        public static Project GetProjectTemplate()
        {
            ProjectClassLib.Project project = new ProjectClassLib.Project();
            project.Name = "Test proj";

            #region template
            project.FrameResolution = new DataContainers.Resolution(1200, 820);
            project.HologramDimension = new DataContainers.LinearDimension<decimal>(640, 1234.5m);
            project.UnitSize = 6.25m;
            project.GreyRange = new Ranges.GreyRange(20, 155);
            //profileTemplate.
            #endregion

            return project;
        }

        /*
        public override Layer GetLayerTemplate(string name)
        {
            return new Layer();
        }*/
        /*private void LoadTrekoByDefaultTemplate()
        {
            //this = (Treko)GetLayerTemplate();
        }*/
        #endregion

        #region Destructors
        ~Project()
        {

        }
        #endregion

        /*public Project(ProjectModel projectModel)
        {
            id = projectModel.Id;
            _projectName = projectModel.ProjectName;
            _projectPath = projectModel.ProjectPath;
            _outPath = projectModel.OutPath;
            _previewPath = projectModel.PreviewPath;
            _createdDate = projectModel.CreatedDate;
            _lastUpdatedDate = projectModel.LastUpdatedDate;
            _hologramWidth = projectModel.HologramWidth;
            _hologramHeight = projectModel.HologramHeight;
            _frameWidth = projectModel.FrameWidth;
            _frameHeight = projectModel.FrameHeight;
            _frameResolutionX = projectModel.FrameResolutionX;
            _frameResolutionY = projectModel.FrameResolutionY;
            _grayRangeHigh = projectModel.GrayRangeHigh;
            _grayRangeLow = projectModel.GrayRangeLow;
            _unitSize = projectModel.UnitSize;

            #region new params
            _PPU = projectModel.PPU;
            #endregion

            foreach (LayerModel lM in projectModel.Layers)
            {
                Layer layer = new Layer(lM);
                _layers.Add(layer);
            }
        }*/

        #region static logic
        public static DataContainers.Resolution GetMaskSizeRecommended(DataContainers.LinearDimension<decimal> hologramDimension, decimal unitSize)
        {
            System.Globalization.NumberFormatInfo ni = System.Globalization.NumberFormatInfo.InvariantInfo;
            decimal x, y;
            // hologr dimension unit: mm, unitsize unit: um
            x = hologramDimension.Width * 1000 / unitSize;
            y = hologramDimension.Height * 1000 / unitSize;
            return new Resolution(Convert.ToUInt32(x, ni), Convert.ToUInt32(y, ni));
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
