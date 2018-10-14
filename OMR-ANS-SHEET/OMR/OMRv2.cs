using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMR
{
    public class OMRv2 : OMR.IOMR
    {
        public (List<PointProperty> pointsList, List<int> rowSize) GetPositionPoint(Bitmap bitmap, bool getCheck = false)
        {
            throw new NotImplementedException();
        }
    }
}
