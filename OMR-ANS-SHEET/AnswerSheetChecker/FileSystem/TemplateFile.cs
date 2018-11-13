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
            CIRCLE_SIZE,
        }

        static public void Save(Template template, string path)
        {
            BinaryWriter binaryWriter = new BinaryWriter(File.Create(path));

            // image
            binaryWriter.Write((int)Header.IMAGE);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                template.Image.Save(memoryStream, ImageFormat.Png);
                var imageData = memoryStream.ToArray();
                binaryWriter.Write(imageData.Length);
                binaryWriter.Write(imageData);
            }

            //point
            binaryWriter.Write((int)Header.POINT);
            binaryWriter.Write(template.PointsList.Count);
            foreach (var item in template.PointsList)
            {
                binaryWriter.Write(item.Position.X);
                binaryWriter.Write(item.Position.Y);
                binaryWriter.Write(item.Rad);
            }

            //row data
            binaryWriter.Write((int)Header.ROW_SIZE);
            binaryWriter.Write(template.RowSize.Count);
            foreach (var item in template.RowSize)
            {
                binaryWriter.Write(item);
            }

            // info data
            binaryWriter.Write((int)Header.INFO_DATA);
            binaryWriter.Write(template.InfoData.Count);
            foreach (var item in template.InfoData)
            {
                binaryWriter.Write(item.Name);
                binaryWriter.Write((int)item.OrderType);
                binaryWriter.Write(item.Length);
                binaryWriter.Write(item.Count);
                binaryWriter.Write(item.StartX);
                binaryWriter.Write(item.StartY);
            }

            // ans data
            binaryWriter.Write((int)Header.ANS_DATA);
            binaryWriter.Write(template.AnsData.Count);
            foreach (var item in template.AnsData)
            {
                binaryWriter.Write(item.Offset);
                binaryWriter.Write((int)item.OrderType);
                binaryWriter.Write(item.Length);
                binaryWriter.Write(item.Count);
                binaryWriter.Write(item.StartX);
                binaryWriter.Write(item.StartY);
            }

            binaryWriter.Write((int)Header.CIRCLE_SIZE);
            binaryWriter.Write(template.CircleSize);

            binaryWriter.Close();
        }
        static public Template Load(string path)
        {
            int circleSize = 16;
            System.Drawing.Bitmap bitmap = null;
            List<OMR.PointProperty> pointsList = new List<OMR.PointProperty>();
            List<int> rowSize = new List<int>();
            List<Template.TemplateData> infoData = new List<Template.TemplateData>();
            List<Template.TemplateData> ansData = new List<Template.TemplateData>();

            BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                var h = (Header)binaryReader.ReadInt32();
                switch (h)
                {
                    case Header.IMAGE:
                        {
                            int len = binaryReader.ReadInt32();
                            byte[] imageData = binaryReader.ReadBytes(len);
                            bitmap = new System.Drawing.Bitmap(new MemoryStream(imageData));
                            break;
                        }
                    case Header.POINT:
                        {
                            int len = binaryReader.ReadInt32();
                            for (int i = 0; i < len; i++)
                            {
                                int x = binaryReader.ReadInt32();
                                int y = binaryReader.ReadInt32();
                                int rad = binaryReader.ReadInt32();
                                pointsList.Add(new OMR.PointProperty(new System.Drawing.Point(x, y), rad));
                            }
                            break;
                        }
                    case Header.ROW_SIZE:
                        {
                            int len = binaryReader.ReadInt32();
                            for (int i = 0; i < len; i++)
                            {
                                rowSize.Add(binaryReader.ReadInt32());
                            }
                        }
                        break;
                    case Header.INFO_DATA:
                        {
                            int len = binaryReader.ReadInt32();
                            for (int i = 0; i < len; i++)
                            {
                                string name = binaryReader.ReadString();
                                var type = (Template.TemplateData.Type)binaryReader.ReadInt32();
                                int length = binaryReader.ReadInt32();
                                int count = binaryReader.ReadInt32();
                                int x = binaryReader.ReadInt32();
                                int y = binaryReader.ReadInt32();
                                infoData.Add(new Template.TemplateData(name, type, length, count, x, y));
                            }
                        }
                        break;
                    case Header.ANS_DATA:
                        {
                            int len = binaryReader.ReadInt32();
                            for (int i = 0; i < len; i++)
                            {
                                int offset = binaryReader.ReadInt32();
                                var type = (Template.TemplateData.Type)binaryReader.ReadInt32();
                                int length = binaryReader.ReadInt32();
                                int count = binaryReader.ReadInt32();
                                int x = binaryReader.ReadInt32();
                                int y = binaryReader.ReadInt32();
                                ansData.Add(new Template.TemplateData(offset, type, length, count, x, y));
                            }
                        }
                        break;
                    case Header.CIRCLE_SIZE:
                        {
                            circleSize = binaryReader.ReadInt32();
                        }
                        break;
                    default:
                        break;
                }
            }

            if (bitmap == null || pointsList.Count == 0 || rowSize.Count == 0 || ansData.Count == 0) return null;
            return new Template(bitmap, circleSize, pointsList, rowSize, infoData, ansData);
        }
    }
}