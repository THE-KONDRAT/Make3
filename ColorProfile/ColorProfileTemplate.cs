using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainers
{
    public class ColorProfileTemplate
    {
        public String Name { get; set; }
        public String Created_By_User { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public String MinimalMake3Version { get; set; }
        public bool CustomTemplate { get; set; }

        public bool Symmetric { get; set; }

        public decimal ArcWidth { get; set; }

        public ProfileKeyPoint[] KeyPoints { get; set; } //9 key points

        ///cpDT.KeyPoints = "{X=0, Y=27},

        System.Windows.Controls.Image thumbnail { get; set; }

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

        public void CreateThumbnail()
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            this.thumbnail= img;
        }
    }

    public class ProfileKeyPoint
    {
        private DataContainers.PointDec point;
        public DataContainers.PointDec Point
        {
            get { return point; }
            set { point = value; }
        }

        public ProfileKeyPoint(DataContainers.PointDec p)
        {
            point = p;
        }
    }
}
