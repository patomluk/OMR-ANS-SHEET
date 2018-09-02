using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMR
{
    public class OMRv1 : IOMR
    {
        public bool GetPositionPoint(byte[] data, int width, int height, int pitch, out List<PointProperty> points, out int pointSize)
        {
            throw new NotImplementedException();
        }
    }
}
