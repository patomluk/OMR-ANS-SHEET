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
            }, ()=> {
                var opFile = new Microsoft.Win32.OpenFileDialog()
                {
                    Title = "กระดาษคำตอบ",
                    Filter = "Image (*.jpg *.png)|*.jpg;*.png|Adobe Portable Document Format(*.pdf)|*.pdf",
                };
                if (opFile.ShowDialog() == true) /* ข้อมูลตารางจากรูป*/
                {
                    string ext = System.IO.Path.GetExtension(opFile.FileName);
                    System.Drawing.Bitmap bitmap = null;
                    switch (ext)
                    {
                        case ".png":
                        case ".jpg":
                            bitmap = new System.Drawing.Bitmap(opFile.FileName);
                            break;
                        case ".pdf":
                            var images = new List<System.Drawing.Image>();
                            //var pdf = new org.pdfclown.files.File(opFile.FileName);
                            //var renderer = new org.pdfclown.tools.Renderer();
                            //for (int i = 0; i < pdf.Document.Pages.Count; i++) images.Add(renderer.Render(pdf.Document.Pages[i], pdf.Document.Pages[i].Size));
                            var winSelect = new WindowSelectPage(images, (int page) => { if (page < 0) return; bitmap = new System.Drawing.Bitmap(images[page]); });
                            winSelect.ShowDialog();
                            break;
                        default:
                            break;
                    }
                    if (bitmap == null) return;
                    ShowPageCreateTemplate(bitmap);
                    //(List<OMR.PointProperty> point, List<int> rowSize) = omr.GetPositionPoint(bitmap, false);
                    //ChangeState(PageState.KeyAnswer);
                }
            }, ()=> {
                var opFile = new Microsoft.Win32.OpenFileDialog()
                {
                    Title = "เรียกต้นแบบกระดาษคำตอบ",
                    Filter = "Answer Scoring Tamplate (*.ast)|*.ast",
                };
                if (opFile.ShowDialog() == true)
                {
                    var template = FileSystem.TemplateFile.Load(opFile.FileName);
                    if (template != null)
                    {
                        ShowPageEditTemplate(template);
                    }
                }
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

        public void ShowPageCreateTemplate(System.Drawing.Bitmap bitmap)
        {
            template = null;
            FrameContent.Content = new Content.PageCreateTemplate(TextBlockNamePage, bitmap, 
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
                (List<AnswerData> key) =>{
                    ShowPageEditKey(key);
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

        public void ShowPageEditKey(List<AnswerData> keyLoaded)
        {
            FrameContent.Content = new Content.PageCreateKey(TextBlockNamePage, template,
            () => {
                ShowPageSelectKey();
            },
            (List<AnswerData> key) => {
                this.key = key;
                ShowPageSelectAnswer();
            }, keyLoaded);
        }

        public void ShowPageSelectAnswer()
        {
            FrameContent.Content = new Content.PageSelectAnswer(TextBlockNamePage);
        }
    }
}
