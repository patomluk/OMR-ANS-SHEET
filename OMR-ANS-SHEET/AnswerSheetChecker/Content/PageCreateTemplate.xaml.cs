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
        private Action back;
        private Action<Template> next;
        private OMR.IOMR omr;
        private Template template;
        public PageCreateTemplate(TextBlock textBlockTitle, System.Drawing.Bitmap bitmap, Action back, Action<Template> next)
        {
            textBlockTitle.Text = "สร้างรูปแบบกระดาษคำตอบ";
            this.back = back;
            this.next = next;
            omr = new OMR.OMRv1();
            textBlockTitle.Text = "";
            InitializeComponent();
            (var list, var size) = omr.GetPositionPoint(bitmap);
            template = new Template(bitmap, list, size);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
            CheckNext();
        }

        private void ImagePreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var xx = e.GetPosition(ImagePreview);
            System.Diagnostics.Debug.WriteLine(xx.ToString());
        }

        private void ButtonAddInfo_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddDataGroup(AddDataGroup.Type.Info, template, (AnswerSheetChecker.Template.TemplateData data) =>
            {
                template.InfoData.Add(data);
                DataGridInfo.ItemsSource = template.InfoData;
                DataGridInfo.Columns[0].Header = "ชื่อ";
                DataGridInfo.Columns[1].Header = "รูปแบบ";
                DataGridInfo.Columns[2].Header = "ตัวเลือก";
                DataGridInfo.Columns[3].Header = "จำนวนข้อ";
                DataGridInfo.Columns[4].Header = "เริ่มแนวที่";
                DataGridInfo.Columns[5].Header = "เริ่มแถวที่";
            });
            win.ShowDialog();
        }

        private void ButtonAddAns_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddDataGroup(AddDataGroup.Type.Ans, template, (AnswerSheetChecker.Template.TemplateData data) =>
            {
                template.AnsData.Add(data);
                DataGridAns.ItemsSource = template.AnsData;
                DataGridAns.Columns[1].Header = "รูปแบบ";
                DataGridAns.Columns[2].Header = "ตัวเลือก";
                DataGridAns.Columns[3].Header = "จำนวนข้อ";
                DataGridAns.Columns[4].Header = "เริ่มแนวที่";
                DataGridAns.Columns[5].Header = "เริ่มแถวที่";
                DataGridAns.Columns.RemoveAt(0);
                CheckNext();
            });
            win.ShowDialog();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (template.AnsData.Count == 0) return;
            //TODO::Save
            next(template);
        }

        private void CheckNext()
        {
            ButtonNext.IsEnabled = template.AnsData.Count != 0;
        }
    }
}
