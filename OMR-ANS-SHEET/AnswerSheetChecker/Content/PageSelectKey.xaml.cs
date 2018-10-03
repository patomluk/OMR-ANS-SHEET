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
        public PageSelectKey(TextBlock textBlockTitle)
        {
            textBlockTitle.Text = "ตั้งค่าเฉลย";
            InitializeComponent();
        }
    }
}
