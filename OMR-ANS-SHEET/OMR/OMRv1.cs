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
        public bool GetPositionPoint(Bitmap bitmap, out List<PointProperty> points, out int pointSize)
        {
            throw new NotImplementedException();
        }
    }
}
