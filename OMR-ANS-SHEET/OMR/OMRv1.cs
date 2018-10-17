using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace OMR
{
    public class OMRv1 : IOMR
    {
        struct MyPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Rad { get; set; }// r
            public MyPoint(double x, double y, double rad) { X = x; Y = y; Rad = rad; }
        }
        struct MyRow
        {
            public MyPoint point { get; set; }
            public MyRow(MyPoint mp) { point = mp; }
        }

        (List<PointProperty> pointsList, List<int> rowSize) IOMR.GetPositionPoint(Bitmap bitmap, bool getCheck)
        {
            List<PointProperty> pointProperty = new List<PointProperty>();
            List<int> rowSize = new List<int>();
            /////////////////////////////////////////////////////////////////////////////////////////////
            Image<Rgb, byte> img = new Image<Rgb, byte>(bitmap);
            {
                //UMat uimage = new UMat();
                Image<Rgb, byte> mImage = new Image<Rgb, byte>(img.Width, img.Height, new Rgb());
                CvInvoke.CvtColor(img, mImage, ColorConversion.Bgr2Gray);
                Image<Rgb, byte> tempImage = new Image<Rgb, byte>(img.Width, img.Height, new Rgb());
                CvInvoke.PyrDown(mImage, tempImage);
                CvInvoke.PyrUp(tempImage, mImage);
                CircleF[] circles = CvInvoke.HoughCircles(mImage, HoughType.Gradient, 1, 20, 20, 15, 16, 18);
                Image<Rgb, Byte> circleImage = img.CopyBlank();
                var MyPoints = new List<MyPoint>();
                for (int i = 0; i < circles.Length; i++)
                {
                    circleImage.Draw(circles[i], new Rgb(Color.Red), 2);
                    double x = Math.Round(circles[i].Center.X, 0);
                    double y = Math.Round(circles[i].Center.Y, 0);
                    double r = Math.Round(circles[i].Radius, 0);
                    MyPoints.Add(new MyPoint(x, y, r));
                }
                MyPoints = MyPoints.OrderBy(item => item.Y).ToList();
                var SortRow = new List<MyPoint>();
                var Dist = MyPoints[0].Rad;
                var y_min = MyPoints[0].Y - Dist / 2;
                var y_max = MyPoints[0].Y + Dist / 2;
                var this_row = 1;
                var MyRows = new List<MyRow>();
                for (var i = 0; i < MyPoints.Count; i++)
                {
                    if (MyPoints[i].Y <= y_max && MyPoints[i].Y >= y_min)
                    {
                        MyRows.Add(new MyRow(MyPoints[i]));
                    }
                    else
                    {
                        MyRows = MyRows.OrderBy(item => item.point.X).ToList();
                        foreach (var item in MyRows)
                        {
                            pointProperty.Add(new PointProperty(new Point((int)item.point.X, (int)item.point.Y), (int)Dist, false));
                        }
                        rowSize.Add(MyRows.Count);
                        MyRows.Clear();
                        y_min = MyPoints[i].Y - Dist / 2;
                        y_max = MyPoints[i].Y + Dist / 2;
                        this_row++;
                        MyRows.Add(new MyRow(MyPoints[i]));
                    }
                }
                foreach (var item in MyRows)
                {
                    pointProperty.Add(new PointProperty(new Point((int)item.point.X, (int)item.point.Y), (int)Dist, false));
                }
                rowSize.Add(MyRows.Count);
            }
            if (getCheck) // check ans
            {
                /// start
                foreach (var item in pointProperty)
                {
                    img.Draw(new CircleF(new PointF((float)item.Position.X, (float)item.Position.Y), (float)item.Rad), new Rgb(Color.White), 9);
                }

                Image<Rgb, byte> mImage = new Image<Rgb, byte>(img.Width, img.Height, new Rgb());
                CvInvoke.CvtColor(img, mImage, ColorConversion.Rgba2Gray);
                Image<Rgb, byte> tempImage = new Image<Rgb, byte>(img.Width, img.Height, new Rgb());
                CvInvoke.PyrDown(mImage, tempImage);
                CvInvoke.PyrUp(tempImage, mImage);
                CvInvoke.AdaptiveThreshold(mImage, mImage, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 999, 99);//149,99,79,59,39,19
                CircleF[] circles2 = CvInvoke.HoughCircles(mImage, HoughType.Gradient, 1, 20, 20, 8, 9, 14);
                var AnsPoint = new List<MyPoint>();
                for (int i = 0; i < circles2.Length; i++)
                {
                    img.Draw(circles2[i], new Rgb(Color.Green), 3);
                    double x = Math.Round(circles2[i].Center.X, 0);
                    double y = Math.Round(circles2[i].Center.Y, 0);
                    double r = Math.Round(circles2[i].Radius, 0);
                    AnsPoint.Add(new MyPoint(x, y, r));
                }

                for (int i = 0; i < AnsPoint.Count; i++)
                {
                    for (int k = 0; k < pointProperty.Count; k++)
                    {
                        if ( (AnsPoint[i].X < pointProperty[k].Position.X + pointProperty[k].Rad ) && ( AnsPoint[i].Y < pointProperty[k].Position.Y + pointProperty[k].Rad ) && ( AnsPoint[i].X > pointProperty[k].Position.X - pointProperty[k].Rad ) && ( AnsPoint[i].Y > pointProperty[k].Position.Y - pointProperty[k].Rad ) )
                        {
                            pointProperty[k] = new PointProperty(pointProperty[k].Position, pointProperty[k].Rad, true);
                        }
                    }
                }
            }
            return (pointProperty, rowSize);
        }
    }
}
