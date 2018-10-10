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
        private Action back;
        private Action make;
        private Action<List<AnswerData>, bool> next;
        private Template template;

        public PageSelectKey(TextBlock textBlockTitle, Template template, Action back, Action make, Action<List<AnswerData>, bool> next)
        {
            this.template = template;
            this.back = back;
            this.next = next;
            this.make = make;
            textBlockTitle.Text = "ตั้งค่าเฉลย";
            InitializeComponent();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var opFile = new Microsoft.Win32.OpenFileDialog()
            {
                Title = "เรียกชุดคำตอบ",
                Filter = "AnsSheetKey (*.ask)|*.ask",
            };
            if (opFile.ShowDialog() == true)
            {
                var key = FileSystem.KeyFile.Load(opFile.FileName, template);
                if (key != null)
                {
                    next(key, false);
                }
            }
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = Helper.LoadImage("เรียกกระดาษคำตอบ(เฉลย)");
            if (bitmap == null) return;

            var key = Helper.GetAnswerData(template, bitmap);

            next(key, true);
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
