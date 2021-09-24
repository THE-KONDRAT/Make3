using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public static class ImageProcessingUI
    {
        public static ImageSource LoadImageSourceFromFile(string fileName)
        {
            //Bitmap bmp = new Bitmap();
            Bitmap bmp = ImageProcessing.LoadBMPFromFile(fileName);

            //ImageSource imageSource = null;

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static ImageSource GetImageSourceFromMat(Emgu.CV.Mat mat)
        {
            //Bitmap bmp = new Bitmap();
            Bitmap bmp = ImageProcessing.GetBMPFromMat(mat);

            //ImageSource imageSource = null;

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }

    public class MatToSource : System.Windows.DependencyObject, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        private Emgu.CV.Mat matValue;
        public Emgu.CV.Mat MatValue
        {
            get { return matValue; }
            set
            {
                matValue = value;
                OnPropertyChanged("MatValue");
                if (matValue != null)
                {
                    ImageSourceValue = ImageProcessingUI.GetImageSourceFromMat(matValue);
                }
                
            }
        }

        private ImageSource imageSourceValue;
        public ImageSource ImageSourceValue
        {
            get { return imageSourceValue; }
            set
            {
                imageSourceValue = value;
                OnPropertyChanged("ImageSourceValue");
                //MatValue = GetMatFromImageSource
            }
        }

        public MatToSource(Emgu.CV.Mat mat)
        {
            MatValue = mat;
        }
        public MatToSource()
        {
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
