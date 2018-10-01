using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnswerSheetChecker
{
    /// <summary>
    /// Interaction logic for AddDataGroup.xaml
    /// </summary>
    public partial class AddDataGroup : Window
    {
        public AddDataGroup()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxNumberChoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse(((TextBox)sender).Text) < 1) e.Handled = false;
        }

        private void TextBoxNumberAns_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse(((TextBox)sender).Text) < 1) e.Handled = false;
        }

        private void TextBoxCol_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse(((TextBox)sender).Text) < 0) e.Handled = false;
        }

        private void TextBoxRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (int.Parse(((TextBox)sender).Text) < 0) e.Handled = false;
        }

        private void TextBoxNumberChoice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 1) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void TextBoxNumberAns_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 1) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void TextBoxCol_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 0) ((TextBox)sender).Text = "0";
                return;
            }
            ((TextBox)sender).Text = "0";
        }

        private void TextBoxRow_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 0) ((TextBox)sender).Text = "0";
                return;
            }
            ((TextBox)sender).Text = "0";
        }
    }
}
