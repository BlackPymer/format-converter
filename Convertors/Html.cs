using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Converter.Convertors
{
    class Html: FormatConverter
    {
        public override void Save(string file_path, Dictionary<string, object> data)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>\r\n <html lang=\"ru\">\n<body>\n\t<table border='1'>\n\t\t<tr>");
            foreach (var item in data.Keys)
                html.AppendLine($"\t\t\t<th>{item}</th>");
            html.AppendLine("\t\t</tr>");
            List<List<object>> list = new List<List<object>>();
            foreach(var item in data.Values)
            {
                if (!(item is List<object>))
                    throw ParsingFailException;
                list.Add((List<object>)item);
            }
            for(int i =0; i < list[0].Count; i++)
            {
                html.AppendLine("\t\t<tr>");
                for(int j = 0; j < list.Count; j++)
                {
                    html.AppendLine($"\t\t\t<td>{System.Net.WebUtility.HtmlEncode(Convert.ToString(list[j][i]))}</th>");
                }
                html.AppendLine("\t\t</tr>");
            }
            html.AppendLine("\t</table>\n</body>\n</html>");
            File.WriteAllText(file_path, html.ToString());

        }
        private object ConvertFromRow(string str)
        {
            string tmp = str.Substring(str.IndexOf('>')+1);
            return TryConvertValue((tmp.Substring(0, tmp.IndexOf('<'))).Trim());
        }
        public override Dictionary<string, object> Load(string file_path)
        {
            string[] data = File.ReadAllLines(file_path);
            int current_line = 0;
            Dictionary<string, List<object>> result = new Dictionary<string, List<object>>();
            List<string> headers = new List<string>();
            bool headers_added = false;
            for (; current_line < data.Length; current_line++)
            {
                if (data[current_line].Contains("<tr>"))
                {
                    int adding = 1;
                    while (!data[current_line + adding].Contains("</tr>")){
                        string str = data[current_line+adding].Trim();
                        if (!headers_added)
                        {
                            headers.Add(Convert.ToString(ConvertFromRow(str)));
                            result[headers[adding-1]] = new List<object>();
                        }
                        else
                            result[headers[adding-1]].Add(ConvertFromRow(str));
                        adding++;
                    }
                    headers_added = true;
                }
            }
            return result.ToDictionary(h=>h.Key,h=>(object)h.Value);
        }
    }
}
