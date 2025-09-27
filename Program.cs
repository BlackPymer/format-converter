using Converter.Convertors;
using System;
namespace Converter
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            JSON json = new JSON();
            Xml xml = new Xml();
            Yaml yaml = new Yaml();
            Csv csv = new Csv();
            Html html = new Html();
            csv.Save("D:\\MyProjects\\labs\\pivo\\Converter\\result.csv",html.Load("D:\\MyProjects\\labs\\pivo\\Converter\\result.html"));
        }
    }
}
