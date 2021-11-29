using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        //private PointF[] SetBrightnessFromSplinePoints(PointF[] KeyPointArr)
        private static byte[] SetBrightnessFromSplinePoints(DataContainers.ProfileKeyPoint[] KeyPointArr)
        {
            int j = 0;
            double step = (double)1 / (double)256, mincol = double.MaxValue, maxcol = 0;
            byte[] result = new byte[256];
            maxcol = 0;
            for (double t = 0; t < 1; t += step)
            {
                double ytmp = 0;
                double xtmp = 0;
                for (int i = 0; i < KeyPointArr.Length; i++)
                {
                    double b = MathLogic.Math.polinomBershtain(i, KeyPointArr.Length - 1, t);
                    xtmp += Convert.ToDouble(KeyPointArr[i].X) * b;
                    ytmp += Convert.ToDouble(KeyPointArr[i].Y) * b;
                }
                result[j] = Convert.ToByte(ytmp);
                if (ytmp > maxcol) maxcol = ytmp;
                if (ytmp <= mincol) mincol = ytmp;
                j++;
            }
            return result;
        }
        internal static byte[] KeyPointsToTraceProfile(DataContainers.ProfileKeyPoint[] keyPoints)
        {
            byte[] profile = null;

            profile = new byte[256];

            profile = SetBrightnessFromSplinePoints(keyPoints);

            return profile;
        }



        internal void SetKeyPointsToTraceProfile(DataContainers.ProfileKeyPoint[] keyPoints)
        {
            byte[] profile = KeyPointsToTraceProfile(keyPoints);

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
