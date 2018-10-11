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
            public int Offset { get; }
            public Type OrderType { get; }
            public int Length { get; }
            public int Count { get; }
            public int StartX { get; }
            public int StartY { get; }
            public TemplateData(string name, Type type, int length, int count, int x, int y)
            {
                Offset = 0;
                Name = name;
                OrderType = type;
                Length = length;
                Count = count;
                StartX = x;
                StartY = y;
            }
            public TemplateData(int offset, Type type, int length, int count, int x, int y)
            {
                Name = "";
                Offset = offset;
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
        public Template(System.Drawing.Bitmap image, List<OMR.PointProperty> points, List<int> row, List<TemplateData> infoData, List<TemplateData> ansData)
        {
            InfoData = infoData;
            AnsData = ansData;
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

    public class AnswerData
    {
        public int Index { get; }
        public int MaxChoice { get; }
        public int Select { get { return select; } set { if (value < 0) select = 0; else if (value > MaxChoice) select = MaxChoice; else select = value; } }
        private int select;
        public AnswerData(int index, int max, int select = 0)
        {
            Index = index;
            MaxChoice = max;
            this.select = select;
        }
    }

    public class AnswerDataChecker
    {
        public int Index { get; }
        public int MaxChoice { get; }
        public int Select { get; }
        public int Key { get; }
        public bool Correct { get { return Key != 0 && Select == Key; } }
        public AnswerDataChecker(int index, int max, int select, int key)
        {
            Index = index;
            MaxChoice = max;
            Select = select;
            Key = key;
        }
    }

    public class InfoData
    {
        public string Name { get; }
        public int DataLength { get; }
        public int Data { get; }
        public string DataDisplay { get { return Data.ToString(new string('0', DataLength)); } }
        public InfoData(string name, int len, int data)
        {
            Name = name;
            DataLength = len;
            Data = data;
        }
    }
}
