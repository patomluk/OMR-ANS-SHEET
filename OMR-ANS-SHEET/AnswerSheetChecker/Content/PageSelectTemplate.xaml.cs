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
    /// Interaction logic for SelectTemplate.xaml
    /// </summary>
    public partial class PageSelectTemplate : Page
    {
        private Action back;
        private Action<System.Drawing.Bitmap> create;
        private Action<Template> load;

        public PageSelectTemplate(TextBlock textBlockTitle, Action back, Action<System.Drawing.Bitmap> create, Action<Template> load)
        {
            this.back = back;
            this.create = create;
            this.load = load;
            textBlockTitle.Text = "ตั้งค่ารูปแบบกระดาษคำตอบ";
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = Helper.LoadImage("กระดาษคำตอบ");
            if (bitmap == null) return;
            create(bitmap);
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var opFile = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "เรียกต้นแบบกระดาษคำตอบ",
                Filter = "Answer Scoring Tamplate (*.ast)|*.ast",
            };
            if (opFile.ShowDialog() == true)
            {
                var template = FileSystem.TemplateFile.Load(opFile.FileName);
                if (template != null)
                {
                    load(template);
                }
            }
        }
    }
}
