using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Converter.Convertors
{
    class JSON : FormatConverter
    {
        public override void Save(string file_path, Dictionary<string, object> data)
        {
            string output = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(file_path, output);
        }

        public override Dictionary<string, object> Load(string file_path)
        {
            string file_data = File.ReadAllText(file_path);
            Dictionary<string, JToken> data = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(file_data);
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var item in data)
                result[item.Key] = ConvertJsonNode(item.Value);
            return result;
        }


        private object ConvertJsonNode(JToken node)
        {
            switch (node.Type)
            {
                case JTokenType.Array:
                    List<object> list = new List<object>();
                    foreach (var el in ((JArray)node).Children())
                        list.Add(ConvertJsonNode(el));
                    break;
                case JTokenType.Object:
                    Dictionary<string, object> tmp = new Dictionary<string, object>();
                    foreach (var prop in ((JObject)node).Properties())
                        tmp[prop.Name] = ConvertJsonNode(prop.Value);
                    return tmp;
                default:
                    return ((JValue)node).Value;
            }
            return 0;
        }
    }
}
