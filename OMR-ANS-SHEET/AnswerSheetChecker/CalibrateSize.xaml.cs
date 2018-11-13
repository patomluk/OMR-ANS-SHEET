using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnswerSheetChecker
{
    /// <summary>
    /// Interaction logic for CalibrateSize.xaml
    /// </summary>
    public partial class CalibrateSize : Window
    {
        private System.Drawing.Bitmap origin;
        private Action<int> next;
        private int size;
        private OMR.IOMR omr;
        public CalibrateSize(System.Drawing.Bitmap bitmap, Action<int> next)
        {
            size = 16;
            origin = bitmap;
            this.next = next;
            omr = new OMR.OMRv1();
            InitializeComponent();
            TextBoxSize.Text = size.ToString();
            ImageOrigin.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxSize_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 0) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void TextBoxSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 0) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            size = int.Parse(TextBoxSize.Text);
            ShowPreview();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            next(int.Parse(TextBoxSize.Text));
            Close();
        }

        private void ShowPreview()
        {
            (var list, var rowsize) = omr.GetPositionPoint(origin, size);
            var preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, origin.Width, origin.Height, list, System.Drawing.Color.Black, 2);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                preview.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(preview.Width, preview.Height));
        }
    }
}
