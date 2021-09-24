using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace DataContainers
{
    public struct RectangleDec : IEquatable<RectangleDec>
    {
        public decimal Width { get; set; }

        [Browsable(false)]
        public decimal Top
        {
            get { return _location.Y; }
        }
        /*[Browsable(false)]
        public SizeF Size { get; set; }*/
        [Browsable(false)]
        public decimal Right
        {
            get { return _location.X + Width; }
        }
        [Browsable(false)]
        private PointDec _location;
        public PointDec Location
        {
            get { return _location; }
            set { _location = value; }
        }
        [Browsable(false)]
        public decimal Left
        {
            get { return _location.X; }
        }
        /*[Browsable(false)]
        public decimal IsEmpty { get; }*/
        public decimal Height { get; set; }
        [Browsable(false)]
        public decimal Bottom
        {
            get { return _location.Y - Height; }
        }
        public decimal X
        {
            get { return Location.X; }
            set { _location.X = value; }
        }
        public decimal Y
        {
            get { return Location.Y; }
            set { _location.Y = value; }
        }

        #region Constructors
        public RectangleDec(PointDec location, decimal width, decimal height)
        {
            _location = location;
            Width = width;
            Height = height;
            //Width = Math.Abs(Right - Left);
            //Height = Math.Abs(Top - Bottom);
        }
        public RectangleDec(decimal x, decimal y, decimal width, decimal height)
        {
            _location = new PointDec(x, y);
            Width = width;
            Height = height;
            //Width = Math.Abs(Right - Left);
            //Height = Math.Abs(Top - Bottom);
        }
        #endregion

        #region Destructors
        /*~Project()
        {

        }*/
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RectangleDec other)
        {
            if (this.Equals(other))
            {
                return true;
            }
            else return false;
        }
    }
}
