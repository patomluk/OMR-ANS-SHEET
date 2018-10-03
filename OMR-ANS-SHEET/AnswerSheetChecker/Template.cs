using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnswerSheetChecker
{
    public class Template
    {
        public struct TemplateData
        {
            public enum Type
            {
                Horizontal,
                Vertical,
            }
            public String Name { get; }
            public Type OrderType { get; }
            public int Length { get; }
            public int Count { get; }
            public int StartX { get; }
            public int StartY { get; }
            public TemplateData(string name, Type type, int length, int count, int x, int y)
            {
                Name = name;
                OrderType = type;
                Length = length;
                Count = count;
                StartX = x;
                StartY = y;
            }
            public TemplateData(Type type, int length, int count, int x, int y)
            {
                Name = "";
                OrderType = type;
                Length = length;
                Count = count;
                StartX = x;
                StartY = y;
            }
        }

        public List<TemplateData> InfoData { get; }
        public List<TemplateData> AnsData { get; }
        public List<OMR.PointProperty> PointsList { get; }
        public List<int> RowSize { get; }
        public List<int> RowOffset { get; }
        public System.Drawing.Bitmap Image { get; }
        public Template(System.Drawing.Bitmap image, List<OMR.PointProperty> points, List<int> row)
        {
            InfoData = new List<TemplateData>();
            AnsData = new List<TemplateData>();
            RowOffset = new List<int>();
            Image = image;
            PointsList = points;
            RowSize = row;
            int sum = 0;
            foreach (var item in RowSize)
            {
                sum += item;
                RowOffset.Add(sum);
            }
        }
    }
}
