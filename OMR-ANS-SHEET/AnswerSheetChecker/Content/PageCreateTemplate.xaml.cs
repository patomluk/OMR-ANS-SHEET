﻿using System;
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
        private System.Drawing.Bitmap preview;
        public PageCreateTemplate(TextBlock textBlockTitle, System.Drawing.Bitmap bitmap, int circleSize, Action back, Action<Template> next)
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
            (var list, var size) = omr.GetPositionPoint(bitmap, circleSize);
            template = new Template(bitmap, circleSize, list, size);
            preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, bitmap, list, System.Drawing.Color.Black, 2);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                preview.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(preview.Width, preview.Height));
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
            preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, template.Image, template.PointsList, System.Drawing.Color.Black, 2);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                preview.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(preview.Width, preview.Height));
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

            if (dirty)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                {
                    Title = "บันทึกต้นแบบกระดาษคำตอบ",
                    Filter = "AnsSheetTemplate (*.ast)|*.ast",
                };
                if (saveFileDialog.ShowDialog() == true) /* ข้อมูลตารางจากรูป*/
                {
                    FileSystem.TemplateFile.Save(template, saveFileDialog.FileName);
                    next(template);
                }
            }
            else
            {
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
            ButtonRemoveInfo.IsEnabled = template.InfoData.Count != 0 && DataGridInfo.SelectedIndex >= 0 && dirty;
            ButtonRemoveAns.IsEnabled = template.AnsData.Count != 0 && dirty;
        }

        private void DataGridInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonRemoveInfo.IsEnabled = template.InfoData.Count != 0 && DataGridInfo.SelectedIndex >= 0;
            // preview
            System.Drawing.Bitmap preview2;
            if (!ButtonRemoveInfo.IsEnabled)
            {
                preview2 = preview;
            }
            else
            {
                int x = template.InfoData[DataGridInfo.SelectedIndex].StartX;
                int y = template.InfoData[DataGridInfo.SelectedIndex].StartY;
                int c = template.InfoData[DataGridInfo.SelectedIndex].Count;
                int l = template.InfoData[DataGridInfo.SelectedIndex].Length;
                var o = template.InfoData[DataGridInfo.SelectedIndex].OrderType;
                var pointList = Helper.AreaToPointList(x, y, o == AnswerSheetChecker.Template.TemplateData.Type.Horizontal ? c : l, o == AnswerSheetChecker.Template.TemplateData.Type.Horizontal ? l : c, template);
                preview2 = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Cross, preview, pointList, System.Drawing.Color.Blue, 3);
            }
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    preview2.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(preview2.Width, preview2.Height));
        }

        private void DataGridAns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = template.AnsData.Count != 0 && DataGridAns.SelectedIndex >= 0;
            System.Drawing.Bitmap preview2;
            if (!select)
            {
                preview2 = preview;
            }
            else
            {
                int x = template.AnsData[DataGridAns.SelectedIndex].StartX;
                int y = template.AnsData[DataGridAns.SelectedIndex].StartY;
                int c = template.AnsData[DataGridAns.SelectedIndex].Count;
                int l = template.AnsData[DataGridAns.SelectedIndex].Length;
                var o = template.AnsData[DataGridAns.SelectedIndex].OrderType;
                var pointList = Helper.AreaToPointList(x, y, o == AnswerSheetChecker.Template.TemplateData.Type.Horizontal ? c : l, o == AnswerSheetChecker.Template.TemplateData.Type.Horizontal ? l : c, template);
                preview2 = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Cross, preview, pointList, System.Drawing.Color.Blue, 3);
            }
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    preview2.GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(preview2.Width, preview2.Height));
        }
    }
}
