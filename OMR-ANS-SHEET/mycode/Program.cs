using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
//using System.Diagnostics;
using Emgu.CV.CvEnum;

namespace DetectItemOnPage
{
    class Program
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

        static void Main(string[] args)
        {
            var bitmap = new System.Drawing.Bitmap(@"D:\WORK\sheetStart.jpg");
            Image<Rgb, byte> img = new Image<Rgb, byte>(bitmap);
            Image<Gray, byte> imageGray = img.Convert<Gray, byte>();
            imageGray.Bitmap.Save(@"D:\WORK\sheetStart2.jpg");
            Console.WriteLine("convert to img-gray complete : D:\\WORK\\sheetStart2.jpg");
            Console.ReadKey(true);

            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);
            img.Bitmap.Save(@"D:\WORK\sheetStart3.jpg");
            Console.WriteLine("convert to Color complete : D:\\WORK\\sheetStart3.jpg");
            Console.ReadKey(true);
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            uimage.Bitmap.Save(@"D:\WORK\sheetStart4.jpg");
            Console.WriteLine("clear noise complete : D:\\WORK\\sheetStart4.jpg");
            Console.ReadKey(true);
            CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 1, 20, 20, 15, 16, 18);
            uimage.Bitmap.Save(@"D:\WORK\sheetStart5.jpg");
            Console.ReadKey(true);
            Image<Rgb, Byte> circleImage = img.CopyBlank();
            double x;//row
            double y;//col
            double r;//rad
            var MyPoints = new List<MyPoint>();
            for (int i = 0; i < circles.Length; i++)
            {
                circleImage.Draw(circles[i], new Rgb(Color.Red), 2);

                x = Math.Round(circles[i].Center.X);
                y = Math.Round(circles[i].Center.Y);
                r = Math.Round(circles[i].Radius, 0);

                MyPoints.Add(new MyPoint(x, y, r));
            }
            MyPoints = MyPoints.OrderBy(item => item.Y).ToList();
            foreach (var item in MyPoints)
            {
                Console.WriteLine("x :" + item.X + ",y :" + item.Y + ",rad :" + item.Rad);
            }
            var SortRow = new List<MyPoint>();
            var ThisRow = MyPoints[0].Y;
            var DistRow = MyPoints[0].Rad;
            var MyRows = new List<MyRow>();
            for (var i = 0; i < MyPoints.Count; i++)
            {
                if (MyPoints[i].Y <= ThisRow + DistRow && MyPoints[i].Y >= ThisRow - DistRow)
                {
                    MyRows.Add(new MyRow(MyPoints[i]));
                }
                else
                {
                    ThisRow = MyPoints[i].Y;
                    MyRows = MyRows.OrderBy(item => item.point.X).ToList();
                    foreach (var item in MyRows)
                    {
                        SortRow.Add(new MyPoint(item.point.X, item.point.Y, DistRow));
                    }
                    MyRows.Clear();
                }
            }

            foreach (var item in SortRow)
            {
                Console.WriteLine("x :" + item.X + ",y :" + item.Y + ",rad :" + item.Rad);
            }
            Console.ReadKey(true);
            circleImage.Bitmap.Save(@"D:\WORK\sheetStart6.jpg");
            Console.WriteLine("write circle border complete : D:\\WORK\\sheetStart6.jpg");
            Console.ReadKey(true);
        }
    }
}
