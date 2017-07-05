using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

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
            if(!SingleInstance.Start())
            {
                MessageBox.Show("There is already an instance of this program.", "PermissionChanger", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (ProcessIcon processIcon = new ProcessIcon())
            {
                processIcon.Display();

                Application.Run();
            }

            SingleInstance.Stop();
        }
    }
}
