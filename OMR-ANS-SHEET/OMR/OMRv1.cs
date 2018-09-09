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
        public bool GetPositionPoint(byte[] data, int width, int height, int pitch, out List<PointProperty> points, out int pointSize)
        {
            Bitmap bitmap = new Bitmap(width, height, pitch, System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
            throw new NotImplementedException();
        }
    }
}
