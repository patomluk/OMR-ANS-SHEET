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
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        Template template;
        List<AnswerData> key;

        public WindowMain()
        {
            InitializeComponent();
            ShowPageHome();
        }

        private void ShowPageHome()
        {
            FrameContent.Content = new Content.PageHome(TextBlockNamePage, () => { ShowPageSelectTemplate(); });
        }

        public void ShowPageSelectTemplate()
        {
            FrameContent.Content = new Content.PageSelectTemplate(TextBlockNamePage, ()=> {
                ShowPageHome();
            }, (System.Drawing.Bitmap bitmap)=> {
                ShowWindowCalibrateSize(bitmap);
            }, (Template template)=> {
                ShowPageEditTemplate(template);
            });
        }

        public void ShowPageEditTemplate(Template templateLoaded)
        {
            this.template = templateLoaded;
            FrameContent.Content = new Content.PageCreateTemplate(TextBlockNamePage, template,
                () => {
                    ShowPageSelectTemplate();
                },
                (AnswerSheetChecker.Template template) => {
                    this.template = template;
                    ShowPageSelectKey();
                });
        }

        public void ShowWindowCalibrateSize(System.Drawing.Bitmap bitmap)
        {
            var page = new CalibrateSize(bitmap, (int size) => {
                ShowPageCreateTemplate(bitmap, size);
            });
            page.ShowDialog();
        }

        public void ShowPageCreateTemplate(System.Drawing.Bitmap bitmap, int circleSize)
        {
            template = null;
            FrameContent.Content = new Content.PageCreateTemplate(TextBlockNamePage, bitmap, circleSize,
                () =>{
                    ShowPageSelectTemplate();
                },
                (AnswerSheetChecker.Template template) => {
                    this.template = template;
                    ShowPageSelectKey();
                });
        }

        public void ShowPageSelectKey()
        {
            FrameContent.Content = new Content.PageSelectKey(TextBlockNamePage, template,
                () => {
                    key = null;
                    ShowPageSelectTemplate();
                },
                () => {
                    ShowPageCreateKey();
                },
                (List<AnswerData> key, bool save) =>{
                    ShowPageEditKey(key, save);
                });
        }

        public void ShowPageCreateKey()
        {
            FrameContent.Content = new Content.PageCreateKey(TextBlockNamePage, template, 
            () => {
                key = null;
                ShowPageSelectKey();
            },
            (List<AnswerData> key) => {
                this.key = key;
                ShowPageSelectAnswer();
            });
        }

        public void ShowPageEditKey(List<AnswerData> keyLoaded, bool save)
        {
            FrameContent.Content = new Content.PageCreateKey(TextBlockNamePage, template,
            () => {
                ShowPageSelectKey();
            },
            (List<AnswerData> key) => {
                this.key = key;
                ShowPageSelectAnswer();
            }, keyLoaded, save);
        }

        public void ShowPageSelectAnswer()
        {
            FrameContent.Content = new Content.PageSelectAnswer(TextBlockNamePage, template, key, ()=> {
                ShowPageSelectKey();
            });
        }
    }
}
