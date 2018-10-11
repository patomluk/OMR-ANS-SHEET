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

        public static (List<AnswerData>, List<InfoData>) GetAnswerData(Template template, System.Drawing.Bitmap bitmap, bool getInfo = false)
        {
            OMR.IOMR omr = new OMR.OMRv1();
            (List<OMR.PointProperty> point, List<int> rowSize) = omr.GetPositionPoint(bitmap, true);
            if (point.Count != template.PointsList.Count || rowSize.Count != template.RowSize.Count) return (null, null);
            for (int i = 0; i < rowSize.Count; i++) if (rowSize[i] != template.RowSize[i]) return (null, null);

            var key = new List<AnswerData>();
            var info = new List<InfoData>();
            foreach (var item in template.AnsData)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    int select = 0;
                    for (int j = 0; j < item.Length; j++)
                    {
                        int x = item.StartX;
                        int y = item.StartY;
                        if (item.OrderType == Template.TemplateData.Type.Horizontal)
                            y += j;
                        else
                            x += j;
                        if (point[template.RowOffset[y] + x].IsCheck)
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

            if (getInfo)
            {
                foreach (var item in template.InfoData)
                {
                    string sum = "";
                    for (int i = 0; i < item.Count; i++)
                    {
                        int value = 0;
                        for (int j = 0; j < item.Length; j++)
                        {
                            int x = item.StartX;
                            int y = item.StartY;
                            if (item.OrderType == Template.TemplateData.Type.Horizontal)
                                y += j;
                            else
                                x += j;
                            if (point[template.RowOffset[y] + x].IsCheck)
                            {
                                if (value == 0)
                                {
                                    value = j;
                                }
                                else
                                {
                                    value = 0;
                                    break;
                                }
                            }
                        }
                        sum += value.ToString();
                    }
                    info.Add(new InfoData(item.Name, item.Count, int.Parse(sum)));
                }
            }

            return (key, info);
        }
    }
}
