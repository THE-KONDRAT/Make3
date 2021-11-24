using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace DataContainers
{
    public struct PointDec : IEquatable<PointDec>
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        #region Constructors
        public PointDec(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region Destructors
        /*~PointDec()
        {

        }*/
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PointDec other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }
    }
}
