using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;

namespace AnswerSheetChecker.FileSystem
{
    public class TemplateFile
    {
        enum Header
        {
            IMAGE = 0x0000,
            POINT,
            ROW_SIZE,
            INFO_DATA,
            ANS_DATA,
        }

        static private void WriteInt(FileStream fileStream, int data)
        {
            fileStream.Write(BitConverter.GetBytes(data), 0, sizeof(int));
        }

        static public void Save(Template template, string path)
        {
            FileStream fileStream = File.Create(path);

            fileStream.Write(new byte[] { (byte)Header.IMAGE, }, 0, 1);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                template.Image.Save(memoryStream, ImageFormat.Png);
                var imageData = memoryStream.ToArray();
                var size = BitConverter.GetBytes(imageData.Length);
                fileStream.Write(size, 0, size.Length);
                fileStream.Write(imageData, 0, imageData.Length);
            }

            fileStream.Write(new byte[] { (byte)Header.POINT, }, 0, 1);
            {// point
                var size = BitConverter.GetBytes(template.PointsList.Count);
                fileStream.Write(size, 0, size.Length);
                foreach (var item in template.PointsList)
                {
                    WriteInt(fileStream, item.Position.X);
                    WriteInt(fileStream, item.Position.Y);
                    WriteInt(fileStream, item.Rad);
                }
            }

            fileStream.Write(new byte[] { (byte)Header.ROW_SIZE, }, 0, 1);
            {// row data
                var size = BitConverter.GetBytes(template.RowSize.Count);
                fileStream.Write(size, 0, size.Length);
                foreach (var item in template.RowSize)
                {
                    WriteInt(fileStream, item);
                }
            }

            fileStream.Write(new byte[] { (byte)Header.INFO_DATA, }, 0, 1);
            {// info data
                var size = BitConverter.GetBytes(template.InfoData.Count);
                fileStream.Write(size, 0, size.Length);
                foreach (var item in template.InfoData)
                {
                    var text = UTF8Encoding.UTF8.GetBytes(item.Name);
                    WriteInt(fileStream, text.Length);
                    fileStream.Write(text, 0, text.Length);
                    WriteInt(fileStream, (int)item.OrderType);
                    WriteInt(fileStream, item.Length);
                    WriteInt(fileStream, item.Count);
                    WriteInt(fileStream, item.StartX);
                    WriteInt(fileStream, item.StartY);
                }
            }

            fileStream.Write(new byte[] { (byte)Header.ANS_DATA, }, 0, 1);
            {// ans data
                var size = BitConverter.GetBytes(template.InfoData.Count);
                fileStream.Write(size, 0, size.Length);
                foreach (var item in template.InfoData)
                {
                    var text = UTF8Encoding.UTF8.GetBytes(item.Name);
                    WriteInt(fileStream, item.Offset);
                    WriteInt(fileStream, (int)item.OrderType);
                    WriteInt(fileStream, item.Length);
                    WriteInt(fileStream, item.Count);
                    WriteInt(fileStream, item.StartX);
                    WriteInt(fileStream, item.StartY);
                }
            }

            fileStream.Close();
        }
    }
}