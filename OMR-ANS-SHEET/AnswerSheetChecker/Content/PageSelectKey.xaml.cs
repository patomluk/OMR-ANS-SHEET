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
    /// Interaction logic for PageSelectKey.xaml
    /// </summary>
    public partial class PageSelectKey : Page
    {
        Action back;
        Action make;
        Action<Dictionary<int, int>> next;

        public PageSelectKey(TextBlock textBlockTitle, AnswerSheetChecker.Template template, Action back, Action make, Action<Dictionary<int, int>> next)
        {
            this.back = back;
            this.next = next;
            this.make = make;
            textBlockTitle.Text = "ตั้งค่าเฉลย";
            InitializeComponent();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            OMR.IOMR omr = new OMR.OMRv1();
            var opFile = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "เรียกกระดาษคำตอบ(เฉลย)",
                Filter = "Image (*.jpg *.png)|*.jpg;*.png|Adobe Portable Document Format(*.pdf)|*.pdf",
            };
            if (opFile.ShowDialog() == true) /* ข้อมูลตารางจากรูป*/
            {
                string ext = System.IO.Path.GetExtension(opFile.FileName);
                System.Drawing.Bitmap bitmap = null;
                switch (ext)
                {
                    case ".png":
                    case ".jpg":
                        bitmap = new System.Drawing.Bitmap(opFile.FileName);
                        break;
                    case ".pdf":
                        var images = new List<System.Drawing.Image>();
                        //var pdf = new org.pdfclown.files.File(opFile.FileName);
                        //var renderer = new org.pdfclown.tools.Renderer();
                        //for (int i = 0; i < pdf.Document.Pages.Count; i++) images.Add(renderer.Render(pdf.Document.Pages[i], pdf.Document.Pages[i].Size));
                        var winSelect = new WindowSelectPage(images, (int page) => { if (page < 0) return; bitmap = new System.Drawing.Bitmap(images[page]); });
                        winSelect.ShowDialog();
                        break;
                    default:
                        break;
                }
                if (bitmap == null) return;
                (List<OMR.PointProperty> point, List<int> rowSize) = omr.GetPositionPoint(bitmap, true);
                //ChangeState(PageState.KeyAnswer);
            }
        }

        private void ButtonMake_Click(object sender, RoutedEventArgs e)
        {
            make();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }
    }
}
