using System;
using System.Collections.Generic;

namespace Converter.Convertors
{
    abstract class FormatConverter
    {
        public abstract void Save(string file_path, Dictionary<string, object> data);
        public abstract Dictionary<string, object> Load(string file_path);
        public object TryConvertValue(string value)
        {
            if (int.TryParse(value, out var i)) return i;
            if (double.TryParse(value, out var d)) return d;
            if (bool.TryParse(value, out var b)) return b;
            if (DateTime.TryParse(value, out var dt)) return dt;
            return value;
        }
        public Exception ParsingFailException;
    }
}
