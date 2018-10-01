using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnswerSheetChecker.Content
{
    /// <summary>
    /// Interaction logic for PageCreateTemplate.xaml
    /// </summary>
    public partial class PageCreateTemplate : Page
    {
        private OMR.IOMR omr;
        public PageCreateTemplate(TextBlock textBlockTitle, System.Drawing.Bitmap bitmap)
        {
            omr = new OMR.OMRv1();
            textBlockTitle.Text = "";
            InitializeComponent();
            (var list, var size) = omr.GetPositionPoint(bitmap);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
        }

        private void ImagePreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var xx = e.GetPosition(ImagePreview);
            System.Diagnostics.Debug.WriteLine(xx.ToString());
        }

        private void ButtonAddInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddAns_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
