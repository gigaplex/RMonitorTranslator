using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMonitorTranslator
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                comPortComboBox.Items.Add(port);
            }
        }

        public string Server
        {
            get
            {
                return serverTextBox.Text;
            }
        }

        public string COMPort
        {
            get
            {
                return comPortComboBox.SelectedItem?.ToString();
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Server))
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
