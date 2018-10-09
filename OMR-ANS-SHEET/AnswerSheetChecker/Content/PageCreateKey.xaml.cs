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
        Action back;
        Action<Dictionary<int, int>> next;
        Dictionary<int, int> key;

        public PageCreateKey(TextBlock textBlockTitle, Template template, Action back, Action<Dictionary<int, int>> next)
        {
            this.back = back;
            this.next = next;
            key = new Dictionary<int, int>();
            textBlockTitle.Text = "สร้างเฉลย";
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            back();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            next(key);
        }
    }
}
