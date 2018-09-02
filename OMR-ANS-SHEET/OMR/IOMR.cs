using System;
using System.Collections.Generic;
using System.Drawing;

namespace OMR
{
    public struct PointProperty
    {
        public PointProperty(Point pixel, Point position) { Pixel = pixel; Position = position; }
        public Point Pixel { get; }
        public Point Position { get; }
    }

    interface IOMR
    {
        /// <summary>
        /// Get position and pixel of total point in image support only format rgb888 (bpp 3)
        /// </summary>
        /// <param name="data">byte array of image</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="pitch">image pitch</param>
        /// <param name="points">out put of point property</param>
        /// <returns>result if success return true else return false</returns>
        bool GetPositionPoint(byte[] data, int width, int height, int pitch, out List<PointProperty> points, out int pointSize);
    }
}
