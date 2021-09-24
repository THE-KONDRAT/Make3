using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Layers.Additional_containers
{
    [Serializable]
    public class ColorProfile
    {
        public int id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public int LayerID { get; set; }

        public decimal ArcWidth { get; set; }

        //arc width unit

        /// <summary>
        /// Color profile key points
        /// </summary>
        public DataContainers.PointDec KeyPoints { get; set; }
        //public PointF KeyPoints { get; set; }

        public bool Template { get; set; }

        #region Constructors
        public ColorProfile()
        {

        }
        #endregion

        #region Destructors
        ~ColorProfile()
        {

        }
        #endregion
    }
}
