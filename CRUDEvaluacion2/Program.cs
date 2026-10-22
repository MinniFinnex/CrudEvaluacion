using System;
using System.IO;
using System.Windows.Forms;

namespace CRUDEvaluacion2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup_error.txt"), ex.ToString());
            }
        }
    }
}
