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
    /// Interaction logic for PageSelectAnswer.xaml
    /// </summary>
    public partial class PageSelectAnswer : Page
    {
        private Template template;
        private List<AnswerData> key;
        private Action back;
        private int maxScore;
        private List<AnswerResultData> resultList;
        private int currectPage;

        public PageSelectAnswer(TextBlock textBlockTitle, Template template, List<AnswerData> key, Action back)
        {
            this.template = template;
            this.key = key;
            this.back = back;
            textBlockTitle.Text = "ตรวจคำตอบ";
            maxScore = 0;
            foreach (var item in key) if (item.Select != 0) maxScore++;
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = Helper.LoadImages("เรียกกระดาษคำตอบ");
            if (bitmap.Count == 0) return;
            resultList = new List<AnswerResultData>();
            foreach (var item in bitmap)
            {
                (var ansData, var info) = Helper.GetAnswerData(template, item, true);
                if (ansData == null) continue;
                var result = new AnswerResultData(info, key, ansData, item);
                resultList.Add(result);
            }
            currectPage = 0;
            ShowResult();
        }

        private void DataGridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ShowResult()
        {
            if (currectPage < 0 || currectPage >= resultList.Count)
            {
                DataGridInfo.ItemsSource = null;
                DataGridResult.ItemsSource = null;
                TextBoxScore.Text = string.Format("{0}/{1}", 0, maxScore);
            }
            else
            {
                DataGridInfo.ItemsSource = resultList[currectPage].Info;
                DataGridResult.ItemsSource = resultList[currectPage].CheckData;
                TextBoxScore.Text = string.Format("{0}/{1}", resultList[currectPage].Score, maxScore);
            }
            TextBoxPage.Text = string.Format("{0}/{1}", currectPage + 1, resultList.Count);
            ButtonAnsNext.IsEnabled = currectPage < resultList.Count - 1;
            ButtonAnsBack.IsEnabled = currectPage > 0;
        }

        private void ButtonAnsBack_Click(object sender, RoutedEventArgs e)
        {
            if (currectPage <= 0) return;
            currectPage--;
            ShowResult();
        }

        private void ButtonAnsNext_Click(object sender, RoutedEventArgs e)
        {
            if (currectPage >= resultList.Count - 1) return;
            currectPage++;
            ShowResult();
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Title = "บันทึกผลตรวจ",
                Filter = "Excel (*.xlsx)|*.xlsx",
            };
            if (saveFileDialog.ShowDialog() == true) 
            {
                FileSystem.Export.ToExcel(resultList, template, saveFileDialog.FileName);
            }
        }
    }
}
