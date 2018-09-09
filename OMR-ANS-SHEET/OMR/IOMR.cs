using System;
using System.Collections.Generic;
using System.Drawing;

namespace OMR
{
    public struct PointProperty
    {
        public PointProperty(Point position, bool isCheck) { Position = position; IsCheck = isCheck; }
        public Point Position { get; }
        public bool IsCheck { get; }
    }

    public interface IOMR
    {
        (List<PointProperty> pointsList, List<int> rowSize) GetPositionPoint(bool getCheck = false);
    }
}
