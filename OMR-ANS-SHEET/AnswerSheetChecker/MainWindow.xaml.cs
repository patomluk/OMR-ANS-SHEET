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
using Microsoft.Win32;

namespace AnswerSheetChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum PageState
        {
            None,
            Welcome,
            Template,
            CreateTamplate,
            KeyAnswer,
            CreateKey,
            AnswerSheet,
            Result,
        }

        private PageState _state;
        private OMR.IOMR omr;

        public MainWindow()
        {
            InitializeComponent();//ห้ามพิมพ์คำสั่งใดๆก่อนหน้านี้
            omr = new OMR.OMRv1();
            ChangeState(PageState.Welcome);
        }

        public void ChangeState(PageState state)
        {
            if (_state == state) return;
            _state = state;
            gridTextSideManu.Children.Clear();
            gridBottonL.Children.Clear();
            gridBottonR.Children.Clear();
            gridContent.Children.Clear();
            switch (state)
            {
                case PageState.Welcome:
                    InitializePageWelcome();
                    break;
                case PageState.Template:
                    InitializePageTemplate();
                    break;
                case PageState.CreateTamplate:
                    InitializePageCreateTamplate();
                    break;
                case PageState.KeyAnswer:
                    InitializePageKeyAnswer();
                    break;
                case PageState.CreateKey:
                    InitializedCreateKey();
                    break;
                case PageState.AnswerSheet:
                    InitializePageAnswerSheet();
                    break;
                case PageState.Result:
                    InitializePageResult();
                    break;
            }
        }

        void InitializePageWelcome()
        {
            textBlockNamePage.Text = "ยินดีต้อนรับ";
            var richText = AddTextBlock(gridContent, new Thickness(20, 20, 20, 20), 20);
            richText.Text = "\tโปรแกรมตรวจกระดาษคำตอบแบบหลายตัวเลือกด้วยวิธีการประมวลผลภาพ";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "ต่อไป", () => { ChangeState(PageState.Template); });
        }

        void InitializePageTemplate()
        {
            textBlockNamePage.Text = "ตั้งค่ารูปแบบกระดาษคำตอบ";
            var richText = AddTextBlock(gridContent, new Thickness(20, 20, 20, 20), 20);
            richText.Text = "\tสร้างต้นแบบ" +
                "\n\tการสร้างต้นแบบ คือ สร้างรูปแบบกระดาษคำตอบเพื่อให้ใช้กับโปรแกรม" +
                "\n\n\tเรียกข้อมูล" +
                "\n\tการเรียกข้อมูล คือ การเรียกรูปแบบกระดาษคำตอบที่ผู้ใช้เคยสร้างไว้แล้ว (.ast)";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "สร้างต้นแบบ", () => { ChangeState(PageState.CreateTamplate); });
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "เรียกข้อมูล", () =>
            {
                var opFile = new OpenFileDialog()
                {
                    Title = "เรียกต้นแบบกระดาษคำตอบ",
                    Filter = "Answer Scoring Tamplate (*.ast)|*.ast",
                };
                if (opFile.ShowDialog() == true)
                {
                    ChangeState(PageState.KeyAnswer);
                }
            });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.Welcome); });
        }

        void InitializePageCreateTamplate()
        {
            textBlockNamePage.Text = "สร้างรูปแบบกระดาษคำตอบใหม่";
            var richText = AddTextBlock(gridContent, new Thickness(20, 20, 20, 20), 20);
            richText.Text = "\tเรียกข้อมูล" +
                "\n\tเรียกข้อมูล คือ เลือกรูปกระดาษคำตอบมาสร้างต้นแบบกระดาษตรวจคำตอบ (.jpg) ";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "เรียกข้อมูล", () =>
            {
                var opFile = new OpenFileDialog()
                {
                    Title = "เรียกต้นแบบกระดาษคำตอบ",
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
                            break;
                        default:
                            break;
                    }
                    if (bitmap == null) return;
                    (List<OMR.PointProperty> point, List<int> rowSize) = omr.GetPositionPoint(bitmap, false);
                    ChangeState(PageState.KeyAnswer);
                }
            });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.Template); });
        }


        void InitializePageKeyAnswer()
        {
            textBlockNamePage.Text = "ตั้งค่าเฉลยคำตอบ";
            var richText = AddTextBlock(gridContent, new Thickness(20, 20, 20, 20), 20);
            richText.Text = "\tสร้าง" +
                "\n\tสร้าง คือ เลือกวิธีสร้างเฉลยคำตอบ" +
                "\n\n\tเรียกข้อมูล" +
                "\n\tเรียกข้อมูล คือ เรียกไฟล์เฉลยที่ได้ทำการสร้างไว้กับโปรแกรมไว้แล้ว (.ask)";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "สร้าง", () => { ChangeState(PageState.CreateKey); });
            /*AddButton(gridBottonR, new Size(100, gridBotton.Height), "อ่านจากต้นแบบ", () => { ChangeState(PageState.Result); });*/
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "เรียกข้อมูล", () =>
            {
                var opFile = new OpenFileDialog()
                {
                    Title = "เรียกต้นแบบกระดาษคำตอบ",
                    Filter = "Answer Scoring Key (*.ask)|*.ask",
                };
                if (opFile.ShowDialog() == true)
                {
                    ChangeState(PageState.AnswerSheet);
                }
            });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.Template); });
        }

        void InitializedCreateKey()
        {
            textBlockNamePage.Text = "เลือกวิธีเฉลยคำตอบ";
            var richText = AddTextBlock(gridContent, new Thickness(20, 20, 20, 20), 20);
            richText.Text = "\tสร้าง" +
                "\n\tสร้างเฉลยใหม่ คือ การสร้างเฉลยจากรูปกระดาษคำตอบที่ได้เลือกมา" +
                "\n\n\tอ่านจากต้นแบบ" +
                "\n\tอ่านจากต้นแบบ คือ การใช้รูปกระดาษคำตอบที่มีการฝนเฉลยไว้แล้ว มาทำเฉลย" ;
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "สร้างเฉลยใหม่", () => { ChangeState(PageState.CreateKey); });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.Template); });
        }

        void InitializePageAnswerSheet()
        {
            textBlockNamePage.Text = "ข้อมูลกระดาษคำตอบ";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "ประมวลผล", () => { ChangeState(PageState.Result); });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.KeyAnswer); });
        }

        void InitializePageResult()
        {
            textBlockNamePage.Text = "ผลลัพธ์การประมวลผล";
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "ตรวจอีกครั้ง", () => { /*ChangeState(PageState.Result);*/ });
            AddButton(gridBottonR, new Size(100, gridBotton.Height), "บันทึกผล", () => { /*ChangeState(PageState.Result);*/ });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ย้อนกลับ", () => { ChangeState(PageState.AnswerSheet); });
            AddButton(gridBottonL, new Size(100, gridBotton.Height), "ตั้งค่าใหม่", () => { ChangeState(PageState.Template); });
        }

        private RichTextBox AddRichText(Panel panel, Thickness margin)
        {
            var richText = new RichTextBox
            {
                Width = panel.Width,
                Height = panel.Height,
                Margin = margin,
                Background = null,
                BorderBrush = null,
                SelectionBrush = null,
            };
            panel.Children.Add(richText);
            return richText;
        }

        TextBlock AddTextBlock(Panel panel, Thickness margin, double fontsize)
        {
            var textBlock = new TextBlock
            {
                Width = panel.Width,
                Height = panel.Height,
                Margin = margin,
                FontSize = fontsize,
            };
            panel.Children.Add(textBlock);
            return textBlock;
        }

        Button AddButton(Panel panel, Size size, string text, Action a)
        {
            var button = new Button()
            {
                Content = text,
                Width = size.Width,
                Height = size.Height,
            };
            button.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) { a(); });
            panel.Children.Add(button);
            return button;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("คุณกำลังจะปิดโปรแกรม", "Exit Program", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            } 
        }
    }
}
