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
using System.Windows.Shapes;

namespace AnswerSheetChecker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowSelectPage : Window
    {
        private List<System.Drawing.Image> images;
        private bool isClose;
        private int currectPage;
        private Action<int> onSelectPage;

        public WindowSelectPage(List<System.Drawing.Image> images, Action<int> onSelectPage)
        {
            isClose = false;
            this.images = images;
            this.onSelectPage = onSelectPage;
            currectPage = 0;
            InitializeComponent();
            UpdatePage();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isClose) onSelectPage(-1);
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            onSelectPage(currectPage);
            isClose = true;
            Close();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currectPage + 1 >= images.Count) return;
            currectPage++;
            UpdatePage();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currectPage - 1 < 0) return;
            currectPage--;
            UpdatePage();
        }

        private void UpdatePage()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                images[currectPage].Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, System.IO.SeekOrigin.Begin);

                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                ImagePreview.Source = image;
            }
            TextBlockPage.Text = string.Format("{0}/{1}", currectPage + 1, images.Count);
            ButtonPrevious.IsEnabled = currectPage > 0;
            ButtonNext.IsEnabled = currectPage < images.Count - 1;
        }
    }
}
