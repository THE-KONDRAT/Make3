using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    //fd
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



        public string MaskPath { get; set; }

        [XmlIgnoreAttribute]
        private Emgu.CV.Mat mask;
        [XmlIgnoreAttribute]
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

        public virtual Layers.Additional_containers.ColorProfile ColorProfile { get; set; }
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
        #endregion

        public DataContainers.Resolution PreviewResolution { get; set; }

        #region Basic methods
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
        #endregion

        #region Destructors
        ~Layer()
        {

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
