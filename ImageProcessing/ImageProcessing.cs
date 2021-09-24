using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace ImageProcessing
{
    public static class ImageProcessing
    {
        public static Emgu.CV.Mat LoadMatFromFile(string fileName)
        {
            return Emgu.CV.CvInvoke.Imread(fileName, Emgu.CV.CvEnum.ImreadModes.AnyColor);
        }

        public static Emgu.CV.Mat LoadMatFromFile(string fileName, Emgu.CV.CvEnum.ImreadModes imreadModes)
        {
            return Emgu.CV.CvInvoke.Imread(fileName, imreadModes);
        }

        public static Bitmap LoadBMPFromFile(string fileName)
        {
            Mat mat = LoadMatFromFile(fileName);
            return mat.ToBitmap();
        }

        public static Bitmap GetBMPFromMat(Mat mat)
        {
            return mat.ToBitmap();
        }

        /*static Emgu.CV.Image<TColor, TDepth> ConvertImageColors(Emgu.CV.Image<TColorN, TDepthN> src, Emgu.CV.CvEnum.ColorConversion colorConversion)
        {
            //Emgu.CV.Image<colo>
            //Emgu.CV.CvInvoke.CvtColor(m, la.Image, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
        }*/
        /*static Emgu.CV.Image<Rgb, T> MatToRGB(Emgu.CV.Mat src)
        {
            //Emgu.CV.Image<Bgr, TDepth> img = src.ToImage<Bgr, TDepth>();
            Emgu.CV.Image<Rgb, T> result = src.ToImage<Rgb, T>();
            Emgu.CV.CvInvoke.CvtColor(src, result, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            return result;
        }*/

        public static void SaveImageGray1Bit(Emgu.CV.Mat mat, string filepath)
        {
            //???
        }
        public static void SaveImageGray8Bit(Emgu.CV.Mat matGray, string filepath)
        {
            Image<Gray, Byte> img = matGray.ToImage<Gray, Byte>();
            if (matGray.NumberOfChannels > 1)
            {
                Emgu.CV.CvInvoke.CvtColor(matGray, img, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            }
            img.Save(filepath);
        }
        public static void SaveImageRGB8Bit(Emgu.CV.Mat matGray, string filepath)
        {
            Image<Rgb, Byte> img = matGray.ToImage<Rgb, Byte>();
            Emgu.CV.CvInvoke.CvtColor(matGray, img, Emgu.CV.CvEnum.ColorConversion.Bgr2Rgb);
            img.Save(filepath);
        }
    }
}

