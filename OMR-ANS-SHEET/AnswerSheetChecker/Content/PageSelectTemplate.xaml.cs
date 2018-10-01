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
        Action back;
        Action create;
        Action load;

        public PageSelectTemplate(TextBlock textBlockTitle, Action back, Action create, Action load)
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
            create();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            load();
        }
    }
}
