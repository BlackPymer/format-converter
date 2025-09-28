using Converter.Convertors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Converter
{
    internal static class Program
    {
        private static Dictionary<string, object> _currentData;
        private static List<Convertors.FormatConverter> formatConverters = new List<Convertors.FormatConverter>() { new JSON(), new Xml(), new Yaml(), new Csv(), new Html() };
        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormatConverter());

        }
        public static bool LoadData(string filename)
        {
            string fileType = filename.Substring(filename.LastIndexOf(".") + 1);
            int ind = 0;
            switch (fileType)
            {
                case "json":
                    ind = 0;
                    break;

                case "xml":
                    ind = 1;
                    break;

                case "yaml":
                    ind = 2;
                    break;

                case "csv":
                    ind = 3;
                    break;

                case "html":
                    ind = 4;
                    break;

                default:
                    return false;
            }
            try
            {
                _currentData = formatConverters[ind].Load(filename);
            }
            catch { return false; }
            return true;
        }
        public static bool SaveData(string filename, int ind)
        {
            if(_currentData == null) return false;
            try
            {
                formatConverters[ind].Save(filename, _currentData);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
