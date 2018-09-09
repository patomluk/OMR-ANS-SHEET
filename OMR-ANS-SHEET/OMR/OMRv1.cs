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

        public (List<Point> pointsList, List<int> rowSize) GetPositionPoint(bool getCheck = false)
        {
            throw new NotImplementedException();
        }
    }
}
