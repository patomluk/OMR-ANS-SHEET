using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OMR
{
    public class OMRv1 : IOMR
    {
        (List<PointProperty> pointsList, List<int> rowSize) IOMR.GetPositionPoint(bool getCheck)
        {
            throw new NotImplementedException();
        }
    }
}
