using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnswerSheetChecker
{
    public class Helper
    {
        public static System.Drawing.Bitmap LoadImage(string title)
        {
            var opFile = new Microsoft.Win32.OpenFileDialog()
            {
                Title = title,
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
                        //var images = new List<System.Drawing.Image>();
                        //var pdf = new org.pdfclown.files.File(opFile.FileName);
                        //var renderer = new org.pdfclown.tools.Renderer();
                        //for (int i = 0; i < pdf.Document.Pages.Count; i++) images.Add(renderer.Render(pdf.Document.Pages[i], pdf.Document.Pages[i].Size));
                        //var winSelect = new WindowSelectPage(images, (int page) => { if (page < 0) return; bitmap = new System.Drawing.Bitmap(images[page]); });
                        //winSelect.ShowDialog();
                        break;
                    default:
                        break;
                }
                return bitmap;
            }
            return null;
        }

        public static List<AnswerData> GetAnswerData(Template template, System.Drawing.Bitmap bitmap)
        {
            OMR.IOMR omr = new OMR.OMRv1();
            (List<OMR.PointProperty> point, List<int> rowSize) = omr.GetPositionPoint(bitmap, true);
            if (point.Count != template.PointsList.Count || rowSize.Count != template.RowSize.Count) return null;
            for (int i = 0; i < rowSize.Count; i++) if (rowSize[i] != template.RowSize[i]) return null;

            var key = new List<AnswerData>();
            foreach (var item in template.AnsData)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    int select = 0;
                    for (int j = 0; j < item.Length; j++)
                    {
                        if (point[template.RowOffset[item.StartY] + item.StartX].IsCheck)
                        {
                            if (select == 0)
                            {
                                select = j + 1;
                            }
                            else
                            {
                                select = -1;
                                break;
                            }
                        }
                    }
                    key.Add(new AnswerData(item.Offset + i, item.Length, select));
                }
            }
            return key;
        }
    }
}
