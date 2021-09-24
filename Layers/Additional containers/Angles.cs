using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Layers.Additional_containers
{
    public struct Angles
    {
        private List<BlueRecord> blueRecords;
        public List<BlueRecord> BlueRecords
        {
            get
            {
                return blueRecords;
            }
            set
            {
                blueRecords = value;
            }
        }

        public Angles(string filePath)
        {
            blueRecords = new List<BlueRecord>();
        }

        internal void GetBlueRecordsFromFile_old(string filePath)
        {
            //сунуть в другой класс
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] items = line.Split(' ');
                int blueColorValue = int.Parse(items[0]);
                int sectorsCount = 0;
                if (items != null)
                {
                    sectorsCount = items.Length - 1;
                }
                List<ColorArc> sectors = new List<ColorArc>();
                for (int i = 1; i < sectorsCount + 1; i += 2)
                {
                    ArcRange arcRange = new ArcRange(decimal.Parse(items[i]), decimal.Parse(items[i + 1]));
                    AddColorArc(arcRange, ref sectors);
                }

                if (sectors.Count > 0)
                {
                    AddBlueRecord(sectors, blueColorValue, ref blueRecords);
                }
            }
        }

        private void AddColorArc(ArcRange arcRange, ref List<ColorArc> sectors)
        {
            if (sectors == null)
            {
                sectors = new List<ColorArc>();
            }
            //rename to sector
            ColorArc sector = new ColorArc(arcRange);
            sectors.Add(sector);
        }

        private void AddBlueRecord(List<ColorArc> colorArcs, int blueColorValue, ref List<BlueRecord> blueRecords)
        {
            if (blueRecords == null)
            {
                blueRecords = new List<BlueRecord>();
            }
            BlueRecord blueRecord = new BlueRecord(blueColorValue, colorArcs);
            blueRecords.Add(blueRecord);
        }

        public List<ColorArc> GetColorArcsByColor(int blueColorValue)
        {
            if (BlueRecords == null)
            {
                return null;
            }
            else
            {
                BlueRecord br = null;
                br = BlueRecords.Find(x => x.BlueColorValue == blueColorValue);
                if (br == null)
                {
                    return null;
                }
                else
                {
                    return br.ColorArcs;
                }
            }
        }

        /*public Angles(int aa)
        {
            //a = aa;
        }*/
    }

    /// <summary>
    /// Class that represents correlation with color (blue color) and color arcs
    /// </summary>
    public class BlueRecord
    {
        //public DataContainers.PointDec CenterLocation { get; set; }
        public int BlueColorValue { get; set; }
        ArcRange ArcRange { get; set; }

        public List<ColorArc> ColorArcs { get; set; }

        BlueRecord()
        {
        }

        public BlueRecord(int colorValue, List<ColorArc> arcs)
        {
            //ColorArcs = new List<ColorArc>();
            BlueColorValue = colorValue;
            ColorArcs = arcs;
        }

        public BlueRecord(int colorValue)
        {
            ColorArcs = new List<ColorArc>();
            BlueColorValue = colorValue;
        }

        public void CreateColorArcs(string[] arcValues)
        {
            try
            {
                for (int i = 1; i < arcValues.Length; i += 2)
                {
                    ColorArc colArc = new ColorArc(int.Parse(arcValues[i]), int.Parse(arcValues[i + 1]));
                    ColorArcs.Add(colArc);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }

    /// <summary>
    /// Degree range
    /// </summary>
    public struct ArcRange
    {
        public decimal StartAngle { get; set; }
        public decimal EndAngle { get; set; }

        public decimal SweepAngle
        {
            get
            {
                return Math.Abs(EndAngle - StartAngle);
            }
        }

        public ArcRange(decimal start, decimal end)
        {
            StartAngle = start;
            EndAngle = end;
        }
    }

    /// <summary>
    /// Start point and degree range
    /// also we have sweeping angle and mmtopixcoeff
    /// </summary>
    public class ColorArc
    {
        /// <summary>
        /// Start and end radius
        /// Unit: radians
        /// </summary>
        public ArcRange ArcRange { get; set; }
        //or list?
        //public List<ArcRange> ArcRanges { get; set; }

        //public decimal SweepAngleAnimation { get; set; } // 1 degree for preview
        
        DataContainers.RectangleDec ArcRectangle { get; set; }
        
        public int MmToPixelsCoeff { get; set; }

        /*public RectDouble RectCircumf
        {
            get { return _rectCircumf; }
            set { _rectCircumf = value; }
        }*/

        ColorArc()
        {

        }

        public ColorArc(decimal startAngle, decimal endAngle)
        {
            ArcRange = new ArcRange(startAngle, endAngle);
            
            //_rect = CalcRectangleOfArc();
        }

        public ColorArc(ArcRange arcRange)
        {
            ArcRange = arcRange;

            //_rect = CalcRectangleOfArc();
        }

        /*public ColorArc(decimal x, decimal y, decimal startAngle, decimal endAngle, decimal Rad_X, decimal Rad_Y)
        {
            CenterLocation = new DataContainers.PointDec(x, y);
            ArcRange = new ArcRange(startAngle, endAngle);
            
            _rect = CalcRectangleOfArc();
            _rectCircumf = CalcCircumfRectangleOfArc();
        }*/

        public ColorArc(/*DataContainers.PointDec centerLocation, */decimal startAngle, decimal endAngle, decimal Rad_X, decimal Rad_Y)
        {
            //CenterLocation = centerLocation;
            ArcRange = new ArcRange(startAngle, endAngle);

            /*_rect = CalcRectangleOfArc();
            _rectCircumf = CalcCircumfRectangleOfArc();
            */
        }

        // We have radius in mm
        // The width of frame 0.2 mm or 1024 pixels
        // So the size of radius in pixels Rpix = _radius*1024/0.2
        // This is rectangle only for draw arc
        
        /*
        public RectDouble CalcRectangleOfArc()
        {
            //_rect = new Rectangle(20, 20, 20, 20);
            //return;
            _rect = null;
            if (_radius_X > 0 && _radius_Y > 0)
            {
                // distances to point 0 of arc from center of arc segment
                double dxStart = _radius_X * Math.Cos(_angleStartRad);
                double dyStart = _radius_Y * Math.Sin(_angleStartRad);

                // distances to point 1 of arc from center of arc segment
                double dxEnd = _radius_X * Math.Cos(_angleEndRad);
                double dyEnd = _radius_Y * Math.Sin(_angleEndRad);

                // coordinates of point 0 of arc 
                double x0 = _x + dxStart;
                double y0 = _y + dyStart;

                // coordinates of point 1 of arc 
                double x1 = _x + dxEnd;
                double y1 = _y + dyEnd;

                if (x1 - x0 < 0)
                {
                    x0 = _x + dxEnd;
                    x1 = _x + dxStart;
                }

                if (y1 - y0 < 0)
                {
                    y0 = _y + dyEnd;
                    y1 = _y + dyStart;
                }

                _rect = new RectDouble(x0, y0, x1 - x0, y1 - y0); // !!!
                //_rect = new Rectangle(10, 10, 20, 20);
            }

            return _rect;
        }

        // We have radius in mm
        // The width of frame 0.2 mm or 1024 pixels
        // So the size of radius in pixels Rpix = _radius*1024/0.2
        // This is rectangle only for draw arc
        public RectDouble CalcCircumfRectangleOfArc()
        {
            //_rect = new Rectangle(20, 20, 20, 20);
            double x0, y0, width, height;
            //return;
            _rectCircumf = null;
            if (_radius_X > 0 && _radius_Y > 0)
            {
                x0 = _x - _radius_X;
                y0 = _y - _radius_Y;

                width = 2 * _radius_X;
                height = 2 * _radius_Y;
                if (width < 0)
                    x0 = 0;

                _rectCircumf = new RectDouble(x0, y0, width, height); // !!!
                if (_rectCircumf == null)
                {
                    int a = 0;
                }
                //_rect = new Rectangle(10, 10, 20, 20);
            }

            return _rectCircumf;
        }

        //public bool DrawArcInFrame(RectDouble rect, Graphics g)
        //{
        //    if (_radius1 != 0)
        //    {
        //        //Rect.DrawArcInFrame(rect, _rectCircumf, g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angle - _sweepAngle / 2), Convert.ToSingle(_sweepAngle));
        //        //_rect.DrawGradationArcInFrame(rect, _rectCircumf, g, _mmToPixelsCoeff, Convert.ToSingle(_angle - _sweepAngle / 2), Convert.ToSingle(_sweepAngle));
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        public bool DrawArcInFrame(RectDouble rect, Graphics g, int mmToPixelsCoeff, int[] profileColor, float ArkW, bool IA, double? mirrorAng)
        {
            if (_radius_X != 0 && _radius_Y != 0)
            {
                //Rect.DrawArcInFrame(rect, _rectCircumf, g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angle - _sweepAngle / 2), Convert.ToSingle(_sweepAngle));
                if (_rectCircumf != null)
                    RectCircumf.DrawGradationArcInFrame(rect, _rectCircumf, g, mmToPixelsCoeff, Convert.ToSingle(_angleStart), Convert.ToSingle(_sweepAngle), profileColor, ArkW, IA, mirrorAng);
                return true;
            }
            else
                return false;
        }

        public bool DrawArcInFrameI(RectDouble rect, Graphics g, int mmToPixelsCoeff, int profileColor, double r, int ArkW, bool IA, double? mirrorAng)
        {
            if (_radius_X != 0 && _radius_Y != 0)
            {
                if (_rectCircumf != null)
                    RectCircumf.DrawGradationArcInFrameI(rect, _rectCircumf, g, mmToPixelsCoeff, Convert.ToSingle(_angleStart), Convert.ToSingle(_sweepAngle), profileColor, r, ArkW, IA, mirrorAng);
                return true;
            }
            else
                return false;
        }

        public bool DrawArcByLine(Graphics g, bool IA)
        {
            //int mmToPixelsCoeff = 50; // !!!
            if (_radius_X != 0 && _radius_Y != 0)
            {
                Pen pen = new Pen(Color.White, 1);

                if (IA == false)
                {
                    _rectCircumf.DrawArc(g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angleStart), Convert.ToSingle(_angleEnd - _angleStart), IA, null);
                    //_rectCircumf.DrawArc(g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angleEnd / 2), Convert.ToSingle(_angleEnd / 2 - _angleStart), IA);
                }
                else
                {
                    _rectCircumf.DrawArc(g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angleStart), Convert.ToSingle(_angleEnd / 2 - _angleStart), IA, null);
                    _rectCircumf.DrawArc(g, pen, _mmToPixelsCoeff, Convert.ToSingle(_angleEnd / 2), Convert.ToSingle(_angleEnd / 2 - _angleStart), IA, null);
                }
                pen.Dispose();
                //Rect.DrawGradationArc(g, _mmToPixelsCoeff, Convert.ToSingle(_angle - _sweepAngle/2), Convert.ToSingle(_sweepAngle));
                return true;
            }
            else
                return false;
        }

        //public void DrawArc(Graphics g, int[] profileColor)
        //{
        //    if (_radius1 != 0)
        //    {
        //        _rectCircumf.DrawGradationArc(g, Convert.ToSingle(_angleStart), Convert.ToSingle(_angleEnd - _angleStart), profileColor);
        //    }
        //}

        public void DrawAnimationArc(Graphics g, Pen penWhite, Pen penRed, Pen penBlue, int angPreview, int parallaxAng, bool IA)
        {
            double yCorr = 0, yTmp = 0;
            double Y1 = 0, Y2 = 0, Alpha;
            double SA = 0;
            int AP = 0;
            if (_radius_X != 0 && _radius_Y != 0)
            {
                SA = _sweepAngleAnimation;
                AP = angPreview;

                if (parallaxAng == 0)
                {
                    if (IA == true)
                    {
                        if (angPreview >= _angleStart && angPreview <= _angleStart + _sweepAngle)
                            _rectCircumf.DrawArc(g, penWhite, _mmToPixelsCoeff, Convert.ToSingle(angPreview - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        if (angPreview + 180 >= _angleStart && angPreview + 180 <= _angleStart + _sweepAngle)
                            _rectCircumf.DrawArc(g, penWhite, _mmToPixelsCoeff, Convert.ToSingle(angPreview + 180 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                    }
                    else
                    {
                        if (angPreview >= _angleStart && angPreview <= _angleStart + _sweepAngle)
                            _rectCircumf.DrawArc(g, penWhite, _mmToPixelsCoeff, Convert.ToSingle(angPreview - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        if (angPreview + 180 >= _angleStart && angPreview + 180 <= _angleStart + _sweepAngle)
                            _rectCircumf.DrawArc(g, penWhite, _mmToPixelsCoeff, Convert.ToSingle(angPreview + 180 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                    }
                }
                else
                {
                    //Расчет компенсации верт смещения синего относит красного изображений в стереопаре. Дописать для эллипса./////////////////////////////////////////////////////////////////
                    Alpha = angPreview - 90.0;
                    Y1 = Math.Cos(Math.PI * Alpha / 180);
                    Y1 = RadiusY * (1 - Y1);
                    Y2 = Math.Cos(Math.PI * (Alpha + (parallaxAng / 2)) / 180);
                    Y2 = RadiusY * (1 - Y2);
                    yCorr = Y1 - Y2;
                    yTmp = _rectCircumf.Y0;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (IA == true)
                    {
                        if (angPreview >= _angleStart && angPreview <= _angleStart + _sweepAngle)
                        {
                            _rectCircumf.DrawArc(g, penRed, _mmToPixelsCoeff, Convert.ToSingle(angPreview - parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                            _rectCircumf.Y0 = _rectCircumf.Y0 - yCorr;
                            _rectCircumf.DrawArc(g, penBlue, _mmToPixelsCoeff, Convert.ToSingle(angPreview + parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        }
                        if (angPreview + 180 >= _angleStart && angPreview + 180 <= _angleStart + _sweepAngle)
                        {
                            _rectCircumf.DrawArc(g, penRed, _mmToPixelsCoeff, Convert.ToSingle(angPreview + 180 - parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                            _rectCircumf.Y0 = _rectCircumf.Y0 - yCorr;
                            _rectCircumf.DrawArc(g, penBlue, _mmToPixelsCoeff, Convert.ToSingle(angPreview + 180 + parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        }
                    }
                    else
                    {
                        if (angPreview >= _angleStart && angPreview <= _angleStart + _sweepAngle)
                        {
                            _rectCircumf.DrawArc(g, penRed, _mmToPixelsCoeff, Convert.ToSingle(angPreview - parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                            _rectCircumf.Y0 = _rectCircumf.Y0 - yCorr;
                            _rectCircumf.DrawArc(g, penBlue, _mmToPixelsCoeff, Convert.ToSingle(angPreview + parallaxAng / 2 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        }
                        if (angPreview + 180 >= _angleStart && angPreview + 180 <= _angleStart + _sweepAngle)
                        {
                            _rectCircumf.DrawArc(g, penRed, _mmToPixelsCoeff, Convert.ToSingle(angPreview + 180 - parallaxAng - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                            _rectCircumf.Y0 = _rectCircumf.Y0 - yCorr;
                            _rectCircumf.DrawArc(g, penBlue, _mmToPixelsCoeff, Convert.ToSingle(angPreview + parallaxAng / 2 + 180 - _sweepAngleAnimation), Convert.ToSingle(_sweepAngleAnimation), IA, null);
                        }
                    }
                    _rectCircumf.Y0 = yTmp; //Вернуть координату
                    _sweepAngleAnimation = SA;
                    angPreview = AP;
                }
            }
        }
        */

    }
}
