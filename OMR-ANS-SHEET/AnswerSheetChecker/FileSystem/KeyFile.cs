using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnswerSheetChecker.FileSystem
{
    public class KeyFile
    {
        static public void Save(List<AnswerData> key, string path)
        {
            BinaryWriter binaryWriter = new BinaryWriter(File.Create(path));

            binaryWriter.Write(key.Count);
            foreach (var item in key)
            {
                binaryWriter.Write(item.MaxChoice);
                binaryWriter.Write(item.Select);
            }

            binaryWriter.Close();
        }

        static public List<AnswerData> Load(string path, Template template)
        {
            List<AnswerData> key = new List<AnswerData>();
            foreach (var item in template.AnsData)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    key.Add(new AnswerData(item.Offset + i, item.Length, 0));
                }
            }

            BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
            int count = binaryReader.ReadInt32();
            if (count != key.Count) return null;

            for (int i = 0; i < count; i++)
            {
                int max = binaryReader.ReadInt32();
                int select = binaryReader.ReadInt32();
                if (key[i].MaxChoice != max) return null;
                key[i].Select = select;
            }

            binaryReader.Close();

            return key;
        }
    }
}
