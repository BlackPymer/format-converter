using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Converter.Convertors
{
    class Xml : FormatConverter
    {
        public override void Save(string file_path, Dictionary<string, object> data)
        {
            XDocument document = new XDocument();
            foreach (var item in data)
            {
                var node = ConvertToXmlNode(item.Key, item.Value);
                document.Add(node);
            }
            document.Save(file_path);
        }

        public override Dictionary<string, object> Load(string file_path)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            XDocument doc = XDocument.Load(file_path);
            XElement root = doc.Root;
            result[root.Name.ToString()] = ConvertFromXmlNode(root);
            return result;
        }

        private XElement ConvertToXmlNode(string name, object obj)
        {
            XElement element = new XElement(name);

            if (obj is Dictionary<string, object> map)
            {
                foreach (var item in map)
                {
                    if (item.Key == "#text")
                        element.Add(new XText((string)item.Value));
                    else if (item.Key.StartsWith("@"))
                        element.Add(new XAttribute(item.Key.Substring(1), item.Value));
                    else if (item.Value is List<object> list)
                    {
                        foreach (var val in list)
                            element.Add(ConvertToXmlNode(item.Key, val));
                    }
                    else
                        element.Add(ConvertToXmlNode(item.Key, item.Value));
                }
            }
            else
            {
                element.Value = obj?.ToString() ?? "";
            }

            return element;
        }


        private object ConvertFromXmlNode(XElement node)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (!node.HasElements && !node.HasAttributes)
                return TryConvertValue(node.Value);
            string text = node.Nodes().OfType<XText>().Aggregate("", (acc, t) => acc + t.Value);

            if (text != "")
                data["#text"] = text;
            foreach (XElement item in node.Elements())
            {
                string key = item.Name.ToString();
                var value = ConvertFromXmlNode(item);

                if (data.TryGetValue(key, out var existing))
                {
                    if (existing is List<object> list)
                        list.Add(value);
                    else
                        data[key] = new List<object> { existing, value };
                }
                else
                    data[key] = value;
            }

            foreach (XAttribute item in node.Attributes())
            {
                string key = "@" + item.Name.ToString();
                var value = item.Value;

                if (data.TryGetValue(key, out var existing))
                {
                    if (existing is List<object> list)
                        list.Add(value);
                    else
                        data[key] = new List<object> { existing, value };
                }
                else
                    data[key] = value;
            }
            return data;
        }
    }
}
