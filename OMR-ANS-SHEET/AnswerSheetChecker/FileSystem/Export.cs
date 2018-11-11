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
        public static void ToExcel(List<AnswerResultData> resultList, string fileName)
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
                    worksheet.Cells[1, resultList[0].Info.Count + 1].Value = "ได้คะแนน";
                    worksheet.Cells[1, resultList[0].Info.Count + 2].Value = "คะแนนเต็มเต็ม";
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
                    var worksheet = workbook.Worksheets.Add(item.Info[0].DataDisplay);
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
                    worksheet.Cells["F1"].Value = "ได้คะแนน";
                    worksheet.Cells["G1"].Value = "คะแนนเต็ม";
                    worksheet.Cells["F2"].Value = item.Score;
                    worksheet.Cells["G2"].Value = maxScore;

                    worksheet.Cells["I1"].Value = "ข้อมูล";
                    worksheet.Cells["J1"].Value = "";
                    for (int i = 0; i < item.Info.Count; i++)
                    {
                        worksheet.Cells["I" + (i + 2)].Value = item.Info[i].Name;
                        worksheet.Cells["J" + (i + 2)].Value = item.Info[i].DataDisplay;
                    }
                }
                package.SaveAs(new System.IO.FileInfo(fileName));
            }
        }
    }
}
