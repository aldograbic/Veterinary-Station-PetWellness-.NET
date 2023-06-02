using System;
using System.IO;
using System.Windows.Forms;

namespace Veterinarska_stanica__.NET_
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string dataDirectoryPath = Path.Combine(Application.StartupPath, "App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectoryPath);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
