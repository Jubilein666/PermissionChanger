using System;
using System.Windows.Forms;

namespace PermissionChanger
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (ProcessIcon processIcon = new ProcessIcon())
            {
                processIcon.Display();

                Application.Run();
            }
        }
    }
}
