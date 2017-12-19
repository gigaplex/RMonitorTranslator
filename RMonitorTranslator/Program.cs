using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMonitorTranslator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConnectForm connectForm = new ConnectForm();

            DialogResult result = connectForm.ShowDialog();

            if (result != DialogResult.OK)
                return;

            Properties.Settings.Default.Server = connectForm.Server;
            Properties.Settings.Default.COMPort = connectForm.COMPort;

            Properties.Settings.Default.Save();

            Application.Run(new RMonitorForm(connectForm.Server, connectForm.COMPort));
        }
    }
}
