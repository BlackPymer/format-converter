using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using YamlDotNet.Serialization;
namespace Converter.Convertors
{
    class Yaml: FormatConverter
    {
        public override void Save(string file_path, Dictionary<string, object> data)
        {
            SerializerBuilder builder = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve);
            string result = builder.Build().Serialize(data);
            File.WriteAllText(file_path, result);
        }
        public override Dictionary<string, object> Load(string file_path)
        {
            string yaml = File.ReadAllText(file_path);
            YamlStream yamlStream = new YamlStream();
            yamlStream.Load(new StringReader(yaml));
            YamlNode root = yamlStream.Documents[0].RootNode;
            return ConvertFromYamlNode(root) as Dictionary<string,object>;
        }


        private object ConvertFromYamlNode(YamlNode node) {
            try
            {
                switch (node.NodeType)
                {
                    case YamlNodeType.Scalar:
                        return TryConvertValue(((YamlScalarNode)node).Value);

                    case YamlNodeType.Sequence:
                        YamlSequenceNode sequence = (YamlSequenceNode)node;
                        List<object> list = new List<object>();
                        foreach (YamlNode child in sequence.Children)
                            list.Add(ConvertFromYamlNode(child));
                        return list;

                    case YamlNodeType.Mapping:
                        YamlMappingNode mapping = (YamlMappingNode)node;
                        Dictionary<string, object> result = new Dictionary<string, object>();
                        foreach (var child in mapping.Children)
                            result[((YamlScalarNode)child.Key).Value] = ConvertFromYamlNode(child.Value);
                        return result;

                    case YamlNodeType.Alias:
                        throw ParsingFailException;
                    default:
                        return null;
                }
            }
            catch
            {
                throw ParsingFailException;
            }
        }
    }
}
