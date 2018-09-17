﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace OMR
{
    public struct PointProperty
    {
        public PointProperty(Point position, int rad, bool isCheck) { Position = position; IsCheck = isCheck; Rad = rad; }
        public Point Position { get; set; }
        public bool IsCheck { get; set; }
        public int Rad { get; set; }
    }

    public interface IOMR
    {
        (List<PointProperty> pointsList, List<int> rowSize) GetPositionPoint(Bitmap bitmap, bool getCheck = false);
    }
}
