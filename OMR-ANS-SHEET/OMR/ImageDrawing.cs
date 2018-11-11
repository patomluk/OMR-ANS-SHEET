using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace OMR
{
    public class ImageDrawing
    {
        public enum Mode
        {
            Circle,
            Cross,
        }

        public static System.Drawing.Bitmap Draw(Mode mode, int w, int h, List<PointProperty> pointsList, System.Drawing.Color color, int size)
        {
            Image<Rgb, byte> img = new Image<Rgb, byte>(w, h, new Rgb(System.Drawing.Color.White));
            switch (mode)
            {
                case Mode.Circle:
                    return DrawCircle(img, pointsList, new Rgb(color), size).Bitmap;
                case Mode.Cross:
                    return DrawCross(img, pointsList, new Rgb(color), size).Bitmap;
                default:
                    return img.Bitmap;
            }
        }

        public static System.Drawing.Bitmap Draw(Mode mode, System.Drawing.Bitmap bitmap, List<PointProperty> pointsList, System.Drawing.Color color, int size)
        {
            Image<Rgb, byte> img = new Image<Rgb, byte>(bitmap);
            switch (mode)
            {
                case Mode.Circle:
                    return DrawCircle(img, pointsList, new Rgb(color), size).Bitmap;
                case Mode.Cross:
                    return DrawCross(img, pointsList, new Rgb(color), size).Bitmap;
                default:
                    return bitmap;
            }
        }

        private static Image<Rgb, byte> DrawCircle(Image<Rgb, byte> image, List<PointProperty> pointsList, Rgb rgb, int size)
        {
            foreach (var item in pointsList) image.Draw(new CircleF(new System.Drawing.PointF(item.Position.X, item.Position.Y), item.Rad), rgb, size);
            return image;
        }

        private static Image<Rgb, byte> DrawCross(Image<Rgb, byte> image, List<PointProperty> pointsList, Rgb rgb, int size)
        {
            foreach (var item in pointsList) image.Draw(new Cross2DF(new System.Drawing.PointF(item.Position.X, item.Position.Y), item.Rad, item.Rad), rgb, size);
            return image;
        }
    }
}
