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
    /// Interaction logic for PageCreateKey.xaml
    /// </summary>
    public partial class PageCreateKey : Page
    {
        private bool dirty;
        private Action back;
        private Action<List<AnswerData>> next;
        private List<AnswerData> key;

        public PageCreateKey(TextBlock textBlockTitle, Template template, Action back, Action<List<AnswerData>> next, List<AnswerData> key = null)
        {
            this.back = back;
            this.next = next;
            if (key == null)
            {
                dirty = true;
                this.key = new List<AnswerData>();
                foreach (var item in template.AnsData)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        this.key.Add(new AnswerData(item.Offset + i, item.Length, 0));
                    }
                }
            }
            else
            {
                dirty = false;
                this.key = key;
            }
            textBlockTitle.Text = "สร้างเฉลย";
            InitializeComponent();
            DataGridKey.ItemsSource = this.key;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (dirty)
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                {
                    Title = "บันทึกชุดคำตอบ",
                    Filter = "AnsSheetKey (*.ask)|*.ask",
                };
                if (saveFileDialog.ShowDialog() == true) /* ข้อมูลตารางจากรูป*/
                {
                    FileSystem.KeyFile.Save(key, saveFileDialog.FileName);
                    next(key);
                }
            }
            else
            {
                next(key);
            }
        }

        private void DataGridKey_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dirty = true;
        }
    }
}
