using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DataContainers
{
    public class ColorProfileTemplate : ICloneable, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public String Name { get; set; }
        public String Created_By_User { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public String MinimalMake3Version { get; set; }
        public bool CustomTemplate { get; set; }

        public bool Symmetric { get; set; }

        public decimal ArcWidth { get; set; }

        private ProfileKeyPoint[] keyPoints;
        public ProfileKeyPoint[] KeyPoints //9 key points
        {
            get { return keyPoints; }
            set
            {
                keyPoints = value;
                CreateThumbnail();
            }
        }

        ///cpDT.KeyPoints = "{X=0, Y=27},

        private System.Windows.Controls.Image thumbnail;
        public System.Windows.Controls.Image Thumbnail
        {
            get { return thumbnail; }
            set
            {
                thumbnail = value;
                OnPropertyChanged("Thumbnail");
            }
        }

        #region New parameters

        #endregion

        public ColorProfileTemplate()
        {
            this.KeyPoints = new ProfileKeyPoint[9];
            this.CreationTime = DateTime.Now;
            this.LastEditedTime = this.CreationTime;
        }

        public bool CheckAsymm()
        {
            bool Asymm = false;

            if (this.KeyPoints != null)
            {
                Asymm = CheckAsymm(this.KeyPoints);
            }
            this.Symmetric = !Asymm;

            return Asymm;
        }

        public static bool CheckAsymm(ProfileKeyPoint[] keypointsArr)
        {
            if (keypointsArr == null) return false;
            bool Asymm = false;

            for (int i = 0; i < (int)Math.Floor(Convert.ToDecimal(keypointsArr.Length / 2.0)); i++)
            {
                if (!keypointsArr[i].Point.Y.Equals(keypointsArr[keypointsArr.Length - i - 1].Point.Y)) Asymm = true;
            }

            return Asymm;
        }

        public byte foundMax(byte[] profile)
        {
            byte ret = 0;
            for(int i = 0; i < 256; i++)
            {
                if (ret < profile[i]) ret = profile[i];
            }
            return ret;
        }
        public byte foundMin(byte[] profile)
        {
            byte ret = 255;
            for (int i = 0; i < 256; i++)
            {
                if (ret > profile[i]) ret = profile[i];
            }
            return ret;
        }

        public void CreateThumbnail()
        {
            int Width = 256;
            int Height = 256;
            byte GrayRangeHigh = 255, GrayRangeLow = 50;

            int cnt = -1;
            if (KeyPoints != null)
            {
                cnt = KeyPoints.Count(element => element != null);
            }
            if (cnt < 9) return;

            var a = this.Name;

            byte[] profile = ColorProfile.ColorProfile.KeyPointsToTraceProfile(KeyPoints);
            byte minCol = foundMin(profile);
            byte maxCol = foundMax(profile);
            byte col;

            double h1 = GrayRangeHigh - GrayRangeLow;
            double h2 = maxCol - minCol;
            if (h2 == 0) h2 = 0.001;
            double colcoeff = h1 / h2;

            Emgu.CV.Mat mat = Emgu.CV.Mat.Zeros(Width, Height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            for(int x = 0; x < Width; x++)
            {
                byte c = profile[x];
                col = Convert.ToByte((((double)c - (double)minCol) * colcoeff) + GrayRangeLow);
                //MyLine(mat, new System.Drawing.Point(x, Height), new System.Drawing.Point(x, Height - col), 200, col, 50);
                MyLine(mat, new System.Drawing.Point(x, Height), new System.Drawing.Point(x, Height - col), 255 - col / 2, col, col / 5);
                MyLine(mat, new System.Drawing.Point(x, 0), new System.Drawing.Point(x, Height - col), 100, 15, 10);
                MyLine(mat, new System.Drawing.Point(x, Height - col), new System.Drawing.Point(x, Height - col + 10), 255, 255, 250);
            }

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Source = ImageProcessing.ImageProcessingUI.GetImageSourceFromMat(mat);
            this.Thumbnail= img;
        }

        void MyLine(Emgu.CV.Mat img, System.Drawing.Point start, System.Drawing.Point end, int R, int G, int B)
        {
            int thickness = 2;
            Emgu.CV.CvEnum.LineType lineType = Emgu.CV.CvEnum.LineType.EightConnected;
            //int lineType = LINE_8;
            Emgu.CV.CvInvoke.Line(img,
              start,
              end,
              //new Emgu.CV.Structure.MCvScalar(255),
              new Emgu.CV.Structure.MCvScalar(R, G, B),
              thickness,
              lineType);
        }

        public bool Equals(ColorProfileTemplate other)
        {
            if (other == null)
            {
                return false;
            }

            bool equals = true;

            equals = this.Name.Equals(other.Name);

            if (equals)
            {
                for (int i = 0; i < 9; i++)
                {
                    var pointEqual = this.KeyPoints[i].Equals(other.KeyPoints[i]);
                    if (equals && !pointEqual)
                    {
                        equals = pointEqual;
                    }
                }
            }

            return equals;
        }

        public object Clone()
        {
            ColorProfileTemplate result = new ColorProfileTemplate();
            result = (ColorProfileTemplate)this.MemberwiseClone();
            result.KeyPoints = this.KeyPoints.Select(element => (ProfileKeyPoint)element.Clone()).ToArray();
            return result;
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                /*if (property.Equals("MainArcWidth") || property.Equals("MainArcWidthUnit") || property.Equals("AlternateArcWidthUnit"))
                {
                    CalcAlternateWidth();
                    if ((!property.Equals("AlternateArcWidthUnit")) && !Loading)
                    {
                        SetArcWidth();
                    }
                }
                else if (property.Equals("ArcWidth") && Loading)
                {
                    SetVisibleValues(ArcWidth);
                }*/
            }
        }
    }

    public class ProfileKeyPoint : ICloneable
    {
        private DataContainers.PointDec point;
        public DataContainers.PointDec Point
        {
            get { return point; }
            set { point = value; }
        }

        public decimal X
        {
            get { return point.X; }
            set
            {
                decimal y = Point.Y;
                Point = new PointDec(value, y);
            }
        }

        public decimal Y
        {
            get { return point.Y; }
            set
            {
                decimal x = Point.X;
                Point = new PointDec(x, value);
            }
        }

        public ProfileKeyPoint(DataContainers.PointDec p)
        {
            point = p;
        }

        public bool Equals(ProfileKeyPoint other)
        {
            /*bool result = false;
            result = kp != null ? kp.Point.X.Equals(this.Point.X) && kp.Point.X.Equals(this.Point.Y) : false;
            return result;*/

            return other != null ? other.Point.Equals(this.Point) : false;
        }

        public object Clone()
        {            
            return new ProfileKeyPoint(new PointDec(this.X, this.Y));
        }
    }
}
