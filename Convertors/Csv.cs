using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Converter.Convertors
{
    class Csv : FormatConverter
    {
        public override void Save(string file_path, Dictionary<string, object> data)
        {
            List<string> headers = data.Keys.ToList();
            int rowsNum = ((List<object>)data.First().Value).Count;
            List<string> lines = new List<string>();
            lines.Add(string.Join(",", headers));
            for (int i = 0; i < rowsNum; i++)
            {
                List<object> row = new List<object>();
                foreach (string line in headers)
                {
                    row.Add(((List<object>)data[line])[i]);
                }
                lines.Add(string.Join(",", row));
            }
            File.WriteAllLines(file_path, lines, Encoding.UTF8);
        }

        public override Dictionary<string, object> Load(string file_path)
        {
            Dictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            string[] data = File.ReadAllLines(file_path);
            string[] header = data[0].Split(',');
            List<object>[] coloumns = new List<object>[header.Length];
            for (int i = 0; i < coloumns.Length; i++)
                coloumns[i] = new List<object>();
            for(int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(',');
                if (row.Length != header.Length)
                    continue;
                for(int j = 0; j < header.Length; j++)
                    coloumns[j].Add(TryConvertValue( row[j]));
            }
            for(int i = 0; i < header.Length; i++)
                result[header[i]] = coloumns[i];
            
            return result.ToDictionary(pair => pair.Key, pair => (object)pair.Value);
        }
    }
}
