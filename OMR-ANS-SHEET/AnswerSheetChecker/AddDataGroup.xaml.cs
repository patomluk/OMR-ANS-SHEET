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
        public enum Type
        {
            Info,
            Ans,
        }
        private Type type;
        private Template template;
        private Action<Template.TemplateData> callback;
        private System.Drawing.Bitmap preview;

        public AddDataGroup(Template template, Template.TemplateData data, Action<Template.TemplateData> f)
        {
            AddDataGroupInit(Type.Ans, template, data, f);
        }

        public AddDataGroup(Template template, Action<Template.TemplateData> f)
        {
            AddDataGroupInit(Type.Info, template, new Template.TemplateData(), f);
        }

        private void AddDataGroupInit(Type type, Template template, Template.TemplateData data, Action<Template.TemplateData> f)
        {
            this.type = type;
            this.template = template;
            callback = f;
            InitializeComponent();
            if (type == Type.Info)
            {
                TextBlockNumberChoice.Text = "";
                TextBoxNumberChoice.IsEnabled = false;
                TextBoxNumberChoice.Text = "10";
            }
            else
            {
                TextBlockName.Text = "เริ่มข้อที่";
                TextBoxName.Text = (data.Offset + data.Count + 1).ToString();
                TextBoxName.IsEnabled = false;
                TextBlockNumberAns.Text = "จำนวนข้อ";
            }
            preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, template.Image.Width, template.Image.Height, template.PointsList, System.Drawing.Color.Black, 2);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                preview.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(preview.Width, preview.Height));
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBoxNumberChoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonOK != null) ButtonOK.IsEnabled = false;
        }

        private void TextBoxNumberAns_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonOK != null) ButtonOK.IsEnabled = false;
        }

        private void TextBoxCol_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonOK != null) ButtonOK.IsEnabled = false;
        }

        private void TextBoxRow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonOK != null) ButtonOK.IsEnabled = false;
        }
        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonOK != null) ButtonOK.IsEnabled = false;
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
                if (r < 0) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void TextBoxRow_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int r))
            {
                if (r < 0) ((TextBox)sender).Text = "1";
                return;
            }
            ((TextBox)sender).Text = "1";
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            ButtonOK.IsEnabled = false;
            if (type == Type.Info)
            {
                if (TextBoxName.Text.Length == 0) return;
            }
            int w = (bool)RadioButtonVertical.IsChecked ? int.Parse(TextBoxNumberChoice.Text) : int.Parse(TextBoxNumberAns.Text);
            int h = (bool)RadioButtonVertical.IsChecked ? int.Parse(TextBoxNumberAns.Text) : int.Parse(TextBoxNumberChoice.Text);
            int x = int.Parse(TextBoxCol.Text) - 1;
            int y = int.Parse(TextBoxRow.Text) - 1;
            if (y + h > template.RowSize.Count) return;
            for (int i = y; i < y + h; i++)
            {
                if (x + w > template.RowSize[i]) return;
            }
            ButtonOK.IsEnabled = true;

            var pointProperties = Helper.AreaToPointList(x, y, w, h, template);
            var preview2 = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Cross, preview, pointProperties, System.Drawing.Color.Blue, 3);
            ImagePreview.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                preview2.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(preview2.Width, preview2.Height));
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (type == Type.Info)
                callback(new AnswerSheetChecker.Template.TemplateData(TextBoxName.Text, (bool)RadioButtonVertical.IsChecked ? AnswerSheetChecker.Template.TemplateData.Type.Vertical : AnswerSheetChecker.Template.TemplateData.Type.Horizontal, int.Parse(TextBoxNumberChoice.Text), int.Parse(TextBoxNumberAns.Text), int.Parse(TextBoxCol.Text) - 1, int.Parse(TextBoxRow.Text) - 1));
            else
                callback(new AnswerSheetChecker.Template.TemplateData(int.Parse(TextBoxName.Text) - 1, (bool)RadioButtonVertical.IsChecked ? AnswerSheetChecker.Template.TemplateData.Type.Vertical : AnswerSheetChecker.Template.TemplateData.Type.Horizontal, int.Parse(TextBoxNumberChoice.Text), int.Parse(TextBoxNumberAns.Text), int.Parse(TextBoxCol.Text) - 1, int.Parse(TextBoxRow.Text) - 1));
            Close();
        }
    }
}
