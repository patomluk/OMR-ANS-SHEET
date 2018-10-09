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
        private bool dirty;
        public PageCreateTemplate(TextBlock textBlockTitle, System.Drawing.Bitmap bitmap, Action back, Action<Template> next)
        {
            dirty = true;
            textBlockTitle.Text = "สร้างรูปแบบกระดาษคำตอบ";
            this.back = back;
            this.next = next;
            omr = new OMR.OMRv1();
            InitializeComponent();
            ButtonEdit.Visibility = Visibility.Hidden;
            ButtonAddInfo.IsEnabled = true;
            ButtonAddAns.IsEnabled = true;
            (var list, var size) = omr.GetPositionPoint(bitmap);
            template = new Template(bitmap, list, size);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));
            CheckNext();
        }

        public PageCreateTemplate(TextBlock textBlockTitle, Template template, Action back, Action<Template> next)
        {
            dirty = false;
            textBlockTitle.Text = "รูปแบบกระดาษคำตอบ";
            this.back = back;
            this.next = next;
            this.template = template;
            InitializeComponent();
            ButtonEdit.IsEnabled = true;
            ButtonAddInfo.IsEnabled = false;
            ButtonAddAns.IsEnabled = false;
            ButtonRemoveInfo.IsEnabled = false;
            ButtonRemoveAns.IsEnabled = false;
            DataGridInfo.ItemsSource = template.InfoData;
            DataGridAns.ItemsSource = template.AnsData;
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                template.Image.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(template.Image.Width, template.Image.Height));
            CheckNext();
        }

        private void ImagePreview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var xx = e.GetPosition(ImagePreview);
            System.Diagnostics.Debug.WriteLine(xx.ToString());
        }

        private void ButtonAddInfo_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddDataGroup(template, (AnswerSheetChecker.Template.TemplateData data) =>
            {
                template.InfoData.Add(data);
                DataGridInfo.ItemsSource = null;
                DataGridInfo.ItemsSource = template.InfoData;
                CheckNext();
            });
            win.ShowDialog();
        }

        private void ButtonAddAns_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddDataGroup(template, template.AnsData.Count == 0 ? new AnswerSheetChecker.Template.TemplateData() : template.AnsData[template.AnsData.Count - 1], (AnswerSheetChecker.Template.TemplateData data) =>
            {
                template.AnsData.Add(data);
                DataGridAns.ItemsSource = null;
                DataGridAns.ItemsSource = template.AnsData;
                CheckNext();
            });
            win.ShowDialog();
        }

        private void ButtonRemoveInfo_Click(object sender, RoutedEventArgs e)
        {
            if (template.InfoData.Count <= 0) return;
            if (DataGridInfo.SelectedIndex >= 0 && DataGridInfo.SelectedIndex < template.InfoData.Count)
            {
                template.InfoData.RemoveAt(DataGridInfo.SelectedIndex);
                DataGridInfo.ItemsSource = null;
                DataGridInfo.ItemsSource = template.InfoData;
                CheckNext();
            }
        }

        private void ButtonRemoveAns_Click(object sender, RoutedEventArgs e)
        {
            if (template.AnsData.Count <= 0) return;
            template.AnsData.RemoveAt(template.AnsData.Count - 1);
            DataGridAns.ItemsSource = null;
            DataGridAns.ItemsSource = template.AnsData;
            CheckNext();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (template.AnsData.Count == 0) return;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Title = "เรียกต้นแบบกระดาษคำตอบ",
                Filter = "AnsSheetTemplate (*.ast)|*.ast",
            };
            if (saveFileDialog.ShowDialog() == true) /* ข้อมูลตารางจากรูป*/
            {
                if (dirty) FileSystem.TemplateFile.Save(template, saveFileDialog.FileName);
                next(template);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            dirty = true;
            ButtonEdit.IsEnabled = false;
            ButtonAddInfo.IsEnabled = true;
            ButtonAddAns.IsEnabled = true;
            CheckNext();
        }

        private void CheckNext()
        {
            ButtonNext.IsEnabled = template.AnsData.Count != 0;
            ButtonRemoveInfo.IsEnabled = template.InfoData.Count != 0 && DataGridInfo.SelectedIndex >= 0;
            ButtonRemoveAns.IsEnabled = template.AnsData.Count != 0;
        }

        private void DataGridInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonRemoveInfo.IsEnabled = template.InfoData.Count != 0 && DataGridInfo.SelectedIndex >= 0;
        }
    }
}
