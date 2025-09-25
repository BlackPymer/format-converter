using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Converter.Convertors;
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
            xml.Save("D:\\MyProjects\\labs\\pivo\\Converter\\result.xml", json.Load("D:\\MyProjects\\labs\\pivo\\Converter\\test.json"));
            json.Save("D:\\MyProjects\\labs\\pivo\\Converter\\result.json", xml.Load("D:\\MyProjects\\labs\\pivo\\Converter\\result.xml"));
        }
    }
}
