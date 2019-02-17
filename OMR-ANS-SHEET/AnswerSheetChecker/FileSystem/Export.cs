using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace AnswerSheetChecker.FileSystem
{
    public class Export
    {
        public static void ToExcel(List<AnswerResultData> resultList, Template template, string fileName)
        {
            using (var package = new ExcelPackage())
            {
                var workbook = package.Workbook;

                {
                    var worksheet = workbook.Worksheets.Add("สรุป");
                    for (int i = 0; i < resultList[0].Info.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = resultList[0].Info[i].Name;
                    }
                    worksheet.Cells[1, resultList[0].Info.Count + 1].Value = "คะแนน";
                    worksheet.Cells[1, resultList[0].Info.Count + 2].Value = "เต็ม";
                    int index = 2;
                    foreach (var item in resultList)
                    {
                        for (int i = 0; i < item.Info.Count; i++)
                        {
                            worksheet.Cells[index, i + 1].Value = item.Info[i].DataDisplay;
                        }
                        int max = 0;
                        foreach (var item2 in item.CheckData) if (item2.Key != 0) max++;
                        worksheet.Cells[index, resultList[0].Info.Count + 1].Value = item.Score;
                        worksheet.Cells[index, resultList[0].Info.Count + 2].Value = max;
                        index++;
                    }
                }

                foreach (var item in resultList)
                {
                    ExcelWorksheet worksheet = null;
                    int count = 0;
                    while (worksheet == null)
                    {
                        try
                        {
                            if (count == 0)
                                worksheet = workbook.Worksheets.Add(item.Info[0].DataDisplay);
                            else
                                worksheet = workbook.Worksheets.Add(item.Info[0].DataDisplay + " (" + count + ")");
                        }
                        catch (Exception)
                        {
                            count++;
                        }
                    }
                    worksheet.Cells["A1"].Value = "ข้อที่";
                    worksheet.Cells["B1"].Value = "ตอบ";
                    worksheet.Cells["C1"].Value = "เฉลย";
                    worksheet.Cells["D1"].Value = "ผลลัพธ์";

                    int maxScore = 0;
                    for (int i = 0; i < item.CheckData.Count; i++)
                    {
                        worksheet.Cells["A" + (i + 2)].Value = item.CheckData[i].Index + 1;
                        worksheet.Cells["B" + (i + 2)].Value = item.CheckData[i].Select;
                        worksheet.Cells["C" + (i + 2)].Value = item.CheckData[i].Key;
                        worksheet.Cells["D" + (i + 2)].Value = item.CheckData[i].Correct ? 1 : 0;
                        if (item.CheckData[i].Key > 0) maxScore++;
                    }
                    worksheet.Cells["F1"].Value = "คะแนน";
                    worksheet.Cells["G1"].Value = "เต็ม";
                    worksheet.Cells["F2"].Value = item.Score;
                    worksheet.Cells["G2"].Value = maxScore;

                    worksheet.Cells["I1"].Value = "ข้อมูล";
                    worksheet.Cells["J1"].Value = "";
                    for (int i = 0; i < item.Info.Count; i++)
                    {
                        worksheet.Cells["I" + (i + 2)].Value = item.Info[i].Name;
                        worksheet.Cells["J" + (i + 2)].Value = item.Info[i].DataDisplay;
                    }

                    List<OMR.PointProperty> c = new List<OMR.PointProperty>();
                    List<OMR.PointProperty> ic = new List<OMR.PointProperty>();
                    for (int i = 0; i < item.CheckData.Count; i++)
                    {
                        int index = item.CheckData[i].Index;
                        int select = item.CheckData[i].Key - 1;
                        if (select < 0) continue;

                        foreach (var ans in template.AnsData)
                        {
                            if (index >= ans.Offset && index < ans.Offset + ans.Count)
                            {
                                int x = ans.StartX + select;
                                int y = ans.StartY + index - ans.Offset;
                                var point = template.PointsList[template.RowOffset[y] + x];
                                if (item.CheckData[i].Correct)
                                    c.Add(point);
                                else
                                    ic.Add(point);
                                break;
                            }
                        }
                    }
                    var preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, item.Image, c, System.Drawing.Color.Green, 5);
                    preview = OMR.ImageDrawing.Draw(OMR.ImageDrawing.Mode.Circle, preview, ic, System.Drawing.Color.Red, 2);
                    var score = item.Score + "/" + maxScore;
                    int fontSize = 5;
                    preview = OMR.ImageDrawing.DrawText(preview, preview.Width - fontSize * score.Length * 15, fontSize * 15, score, System.Drawing.Color.Blue, fontSize);
                    // scale
                    float scale = 800f / preview.Width;
                    preview = new System.Drawing.Bitmap(preview, (int)(preview.Width * scale), (int)(preview.Height * scale));

                    var img = worksheet.Drawings.AddPicture("sheet", preview);
                    img.SetPosition(0, 0, 14, 0);
                }
                package.SaveAs(new System.IO.FileInfo(fileName));
            }
        }
    }
}
