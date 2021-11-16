using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ColorProfile
{
    [Serializable]
    public class ColorProfile : INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public int id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public int LayerID { get; set; }

        private decimal arcWidth;
        public decimal ArcWidth
        {
            get { return arcWidth; }
            set
            {
                arcWidth = value;
                OnPropertyChanged("ArcWidth");
            }
        }

        //arc width unit

        /// <summary>
        /// Color profile
        /// </summary>
        public byte[] TraceProfile { get; private set; }
        //public PointF KeyPoints { get; set; }

        public bool Template { get; set; }

        #region Constructors
        public ColorProfile()
        {
            TraceProfile = new byte[256];
        }
        #endregion

        #region Destructors
        ~ColorProfile()
        {

        }
        #endregion

        #region Logic
        internal void KeyPointsToTraceProfile(DataContainers.ProfileKeyPoint[] keyPoints)
        {
            byte[] profile = null;

            profile = new byte[256];

            TraceProfile = profile;
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
