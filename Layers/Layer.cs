using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Layers
{
    [Serializable]

    [XmlInclude(typeof(Treko_base)),
        XmlInclude(typeof(Treko)),
        XmlInclude(typeof(Treko3D)),
        XmlInclude(typeof(TrekoCylinder)),
        XmlInclude(typeof(TrekoSwitch)),
    XmlType]

    //[KnownType("GetTypes")]

    public class Layer : IRenderableLayer, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnoreAttribute]
        public List<Exception> Exceptions { get; set; }

        #region Layer params
        [XmlIgnoreAttribute]
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [XmlIgnoreAttribute]
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string RelativePath { get; set; }

        [XmlIgnoreAttribute]
        private int order;
        public int Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        
        [XmlIgnoreAttribute]
        private ulong projectID;
        /// <summary>
        /// Foreign key
        /// </summary>
        public ulong ProjectID
        {
            get { return projectID; }
            set
            {
                projectID = value;
                OnPropertyChanged("ProjectID");
            }
        }

        public string technologyName;
        public string TechnologyName
        {
            get { return technologyName; }
            internal set
            {
                technologyName = value;
                OnPropertyChanged("TechnologyName");
            }
        }

        /// <summary>
        /// Name of optical schema
        /// values: 0; 0.1; 1.1
        /// </summary>
        [XmlIgnoreAttribute]
        public string OpticalSchema { get; internal set; }

        

        ///???
        double _layerLeft;
        double _layerTop;
        double _layerWidth;
        double _layerHeight;


        [XmlIgnoreAttribute]
        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
        #endregion

        #region User params
        private bool imageNeedSave = false;


        [XmlIgnoreAttribute]
        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        [XmlIgnoreAttribute]
        private Emgu.CV.Mat image;
        [XmlIgnoreAttribute]
        [JsonIgnore]
        public virtual Emgu.CV.Mat Image
        {
            get { return image; }
            set
            {
                image = value;

                if (Thumbnail == null)
                {
                    Thumbnail = new ImageProcessing.MatToSource(image);
                    //OnPropertyChanged("Thumbnail");
                }
                else
                {
                    //CreateThumb
                    Thumbnail.MatValue = image;
                }
                OnPropertyChanged("Image");
            }
        }


        private bool maskNeedSave = false;
        [XmlIgnoreAttribute]
        private string maskPath;
        public string MaskPath
        {
            get { return maskPath; }
            set
            {
                maskPath = value;
                OnPropertyChanged("MaskPath");
            }
        }

        [XmlIgnoreAttribute]
        private Emgu.CV.Mat mask;
        [XmlIgnoreAttribute]
        [JsonIgnore]
        public Emgu.CV.Mat Mask
        {
            get { return mask; }
            set
            {
                mask = value;

                if (MaskThumbnail == null)
                {
                    MaskThumbnail = new ImageProcessing.MatToSource(mask);
                    //OnPropertyChanged("MaskThumbnail");
                }
                else
                {
                    //CreateThumb
                    MaskThumbnail.MatValue = mask;
                }
                OnPropertyChanged("Mask");
            }
        }


        /// <summary>
        /// Coordinates of mask white area
        /// </summary>
        DataContainers.RectangleDec Mask_WhiteArea;
        //RectangleF Mask_WhiteArea;

        /// <summary>
        /// Mask mode: Stretch or crop
        /// </summary>
        public StretchCrop StretchCrop { get; set; }

        private ColorProfile.ColorProfile colorProfile;
        public virtual ColorProfile.ColorProfile ColorProfile
        {
            get { return colorProfile; }
            set
            {
                colorProfile = value;
                OnPropertyChanged("ColorProfiles");
            }
        }
        #endregion

        #region Thumbnails
        //public virtual OpenCV.Net.Mat Thumbnail { get; set; }
        [XmlIgnoreAttribute]
        private ImageProcessing.MatToSource thumbnail;
        [XmlIgnoreAttribute]
        public virtual ImageProcessing.MatToSource Thumbnail
        {
            get { return thumbnail; }
            set
            {
                thumbnail = value;
                OnPropertyChanged("Thumbnail");
            }
        }

        //public virtual Emgu.CV.Image<Rgb, Byte> Thumbnail { get; set; }

        [XmlIgnoreAttribute]
        private ImageProcessing.MatToSource maskThumbnail;
        [XmlIgnoreAttribute]
        public virtual ImageProcessing.MatToSource MaskThumbnail
        {
            get { return maskThumbnail; }
            set
            {
                maskThumbnail = value;
                OnPropertyChanged("MaskThumbnail");
            }
        }
        /*public void GBE()
        {
            var b = new Bitmap();
            var bmp = b.GetHbitmap();
        }*/
        #endregion

        #region system variables
        /// <summary>
        /// Path (directory) where we saving result (pictures and *.ini)
        /// </summary>
        private string resultDir;

        private int imageMaskRatio; //??

        private int mmToPixelsCoeff; //??
        //do not serialize
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }
        #endregion

        public DataContainers.Resolution PreviewResolution { get; set; }

        #region Basic methods
        public virtual Layer GetLayerTemplate()
        {

            return null;
        }

        public virtual Layer GetLayerTemplate(string name)
        {
            return new Layer();
        }

        public void SetResultDir(string folderpath)
        {
            Mask_WhiteArea = new DataContainers.RectangleDec(new DataContainers.PointDec(0, 1), 20, 10);
            resultDir = folderpath;
        }

        internal void CalculateImageMaskRatio(Emgu.CV.Mat image, Emgu.CV.Mat mask)
        {
            if (image != null && mask != null)
            {
                imageMaskRatio = mask.Width / mask.Height;
            }
        }
        #endregion

        #region Constructors
        public Layer()
        {
            Thumbnail = new ImageProcessing.MatToSource();
            maskThumbnail = new ImageProcessing.MatToSource();
        }

        public static Layers.Layer NewLayer(string technologyName)
        {
            Layers.Layer result = null;
            try
            {
                Type t = GetLayerTypeByTechnologyName(technologyName);
                if (t != null)
                {
                    ConstructorInfo ctor = t.GetConstructor(Type.EmptyTypes);

                    result = (Layers.Layer)ctor?.Invoke(null);
                    result = result.GetLayerTemplate();
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }
        #endregion



        #region Destructors
        ~Layer()
        {

        }
        #endregion


        #region Logic
        public static Regex RegexBaseLayers = new Regex("(_base)$");
        public static Regex RegexDigitStartLayers = new Regex("^Layer(\\d+.*)");
        public static Regex RegexDigitStartTechnologyName = new Regex("^(\\d+.*)");

        internal static string GetLayerTechnologyName(Layer layer)
        {
            return GetLayerTechnologyName(layer.GetType());
        }

        public static string GetLayerTechnologyName(Type layerType)
        {
            string tName = null;

            string lClassName = layerType.Name;

            //Filter "_base"
            string matchResult = null;
            Match match = RegexBaseLayers.Match(lClassName);
            while (match.Success)
            {
                string sMatch = match.Groups[0].Value;
                matchResult = sMatch;
                match = match.NextMatch();
            }

            if (!string.IsNullOrWhiteSpace(matchResult))
            {
                return tName;
            }

            #region starts from number
            matchResult = null;
            match = RegexDigitStartLayers.Match(lClassName);
            while (match.Success)
            {
                string sMatch = match.Groups[1].Value;
                matchResult = sMatch;
                match = match.NextMatch();
            }

            if (!string.IsNullOrWhiteSpace(matchResult))
            {
                tName = matchResult;
            }
            else
            {
                tName = lClassName;
            }
            /*if (!string.IsNullOrWhiteSpace(matchResult))
            {
                return tName;
            }*/
            #endregion

            return tName;
        }

        private static Type GetLayerTypeByTechnologyName(string techName)
        {
            Type result = null;

            //var layers = typeof(Layers.Layer).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Layers.Layer)));
            string l_tName = techName;

            Match match = RegexDigitStartTechnologyName.Match(techName);
            while (match.Success)
            {
                string sMatch = match.Groups[0].Value;
                l_tName = $"Layer{sMatch}";
                match = match.NextMatch();
            }

            try
            {
                //var a = techName;
                result = typeof(Layers.Layer).Assembly.GetTypes().Single(x => x.IsSubclassOf(typeof(Layers.Layer)) && x.Name.Equals(l_tName));
            }
            catch (Exception e)
            {

            }
            //var td = layers.Single(x => x.Name.Equals(technologyName));
            //result = typeof(layers.w)

            /*foreach (Type t in layers)
            {
                if (!t.Name.Contains("_base") && t.Name.Equals(technologyName))
                {
                    result = t;
                }
            }*/

            return result;
        }

        public static bool IsLayer(Type t)
        {
            bool layer = true;

            layer = t.IsSubclassOf(typeof(Layers.Layer));
            if (t.Name.Contains("_base"))
            {
                layer = false;
            }
            return layer;
        }

        public virtual void SaveLayerData(Type objectType, string layerDir)
        {
            if (!IsLayer(objectType))
            {
                throw new Exception("Object type is base layer");
            }

            if (imageNeedSave)
            {
                //copy file
                FileOperations.FileAccess.CopyFileToDirectory(layerDir, imagePath);
            }

            if (maskNeedSave)
            {
                //copy file
                FileOperations.FileAccess.CopyFileToDirectory(layerDir, maskPath);
            }
        }
        #endregion


        internal void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public enum StretchCrop
    {
        Stretch,
        Crop
    }
}
