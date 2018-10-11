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
            var bitmap = Helper.LoadImage("เรียกกระดาษคำตอบ");
            if (bitmap == null) return;
            (var ansData, var info) = Helper.GetAnswerData(template, bitmap, true);
            if (ansData == null) return;
            List<AnswerDataChecker> answerDataCheckers = new List<AnswerDataChecker>();
            for (int i = 0; i < key.Count; i++)
            {
                answerDataCheckers.Add(new AnswerDataChecker(key[i].Index, key[i].MaxChoice, ansData[i].Select, key[i].Select));
            }
            int score = 0;
            foreach (var item in answerDataCheckers) if (item.Correct) score++;
            DataGridInfo.ItemsSource = info;
            DataGridResult.ItemsSource = answerDataCheckers;
            TextBoxScore.Text = string.Format("{0}/{1}", score, maxScore);
        }

        private void DataGridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
