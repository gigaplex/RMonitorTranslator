using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace RMonitorTranslator
{
    public partial class RMonitorForm : Form
    {
        TcpClient m_tcpClient = new TcpClient();
        System.IO.Ports.SerialPort m_serialPort;
        Thread m_readerThread;
        StreamReader m_reader;

        TextBox[] positionTextBoxes;

        Dictionary<string, Driver> m_drivers = new Dictionary<string, Driver>();
        Dictionary<int, string> m_positions = new Dictionary<int, string>();
        string m_raceTime;
        string m_timeRemaining;

        public RMonitorForm()
        {
            InitializeComponent();

            positionTextBoxes = new TextBox[]
            {
                position1TextBox,
                position2TextBox,
                position3TextBox,
                position4TextBox,
                position5TextBox,
                position6TextBox,
                position7TextBox,
                position8TextBox,
                position9TextBox,
                position10TextBox
            };

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                comPortComboBox.Items.Add(port);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_tcpClient.Close();
            if (m_readerThread != null)
                m_readerThread.Abort();

            base.OnFormClosing(e);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            m_tcpClient.Connect(addressTextBox.Text, 12345);

            m_reader = new StreamReader(m_tcpClient.GetStream(), Encoding.ASCII);

            m_readerThread = new Thread(ReaderThread);
            m_readerThread.Start();

            string comPort = comPortComboBox.SelectedItem?.ToString();
            string baudStr = baudTextBox.Text;
            if (!string.IsNullOrEmpty(comPort) && !string.IsNullOrEmpty(baudStr))
            {
                m_serialPort = new System.IO.Ports.SerialPort(comPort, int.Parse(baudStr));
                m_serialPort.Open();
            }
        }

        void ReaderThread()
        {
            while (m_tcpClient.Connected)
            {
                string message = m_reader.ReadLine();
                HandleMessage(message);
            }
        }

        void HandleMessage(string message)
        {
            string[] components = message.Split(',');

            if (components.Length < 2)
                return;

            switch (components[0])
            {
                case "$A": // Competitor information (a)
                    CompetitorInfoMessage1(components);
                    break;
                case "$COMP": // Competitor information (b)
                    CompetitorInfoMessage2(components);
                    break;
                case "$B": // Run information
                    RunInfoMessage(components);
                    break;
                case "$C": // Class information
                    ClassInfoMessage(components);
                    break;
                case "$E": // Setting information
                    SettingInfoMessage(components);
                    break;
                case "$F": // Heartbeat
                    HeartBeatMessage(components);
                    break;
                case "$G": // Race information
                    RaceInfoMessage(components);
                    break;
                case "$H": // Practice/Qualifier information
                    PracticeQualifyInfoMessage(components);
                    break;
                case "$I": // Init record
                    InitRecordMessage(components);
                    break;
                case "$J": // Passing information
                    break;
                default:
                    break;
            }
        }

        void HeartBeatMessage(string[] components)
        {
            m_timeRemaining = components[2].Replace("\"", "");
            string timeOfDay = components[3].Replace("\"", "");
            m_raceTime = components[4].Replace("\"", "");

            Invoke(new Action(() =>
            {
                timeRemainingTextBox.Text = m_timeRemaining;
                clockTextBox.Text = timeOfDay;
                raceClockTextBox.Text = m_raceTime;
            }));

            SendTranslatedStatusMessage();
        }

        void RunInfoMessage(string[] components)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Run Info: {0}", components[2]));
        }

        void CompetitorInfoMessage1(string[] components)
        {
            Driver driver = new Driver();

            driver.Number = components[2].Replace("\"", "");
            driver.FirstName = components[4].Replace("\"", "");
            driver.LastName = components[5].Replace("\"", "");

            m_drivers[driver.Number] = driver;
        }

        void CompetitorInfoMessage2(string[] components)
        {
            Driver driver = new Driver();

            driver.Number = components[2].Replace("\"", "");
            driver.FirstName = components[4].Replace("\"", "");
            driver.LastName = components[5].Replace("\"", "");

            m_drivers[driver.Number] = driver;
        }

        void ClassInfoMessage(string[] components)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Class: {0}", components[2]));
        }

        void SettingInfoMessage(string[] components)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", components[1], components[2]));
        }

        void RaceInfoMessage(string[] components)
        {
            string positionString = components[1].Replace("\"", "");
            string driverNumber = components[2].Replace("\"", "");
            int position = int.Parse(positionString);
            m_positions[position] = driverNumber;

            Driver driver = null;
            m_drivers.TryGetValue(driverNumber, out driver);

            if (driver != null)
            {
                Invoke(new Action(() =>
                {
                   positionTextBoxes[position - 1].Text = string.Format("Car {0}: {1} {2}", driverNumber, driver.FirstName, driver.LastName);
                }));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Driver number {0} in position {1} is not known", driverNumber, positionString));
            }
        }

        void PracticeQualifyInfoMessage(string[] components)
        {
            string position = components[1].Replace("\"", "");
            string driverNumber = components[2].Replace("\"", "");
            System.Diagnostics.Debug.WriteLine(string.Format("Prac/Qual Position {0}: {1}", position, driverNumber));
        }
        void InitRecordMessage(string[] components)
        {
            System.Diagnostics.Debug.WriteLine("Init record received");

            m_drivers = new Dictionary<string, Driver>();
            m_positions = new Dictionary<int, string>();
            m_raceTime = null;
            m_timeRemaining = null;

            string timeOfDay = components[1].Replace("\"", "").Split('.')[0];

            Invoke(new Action(() =>
            {
                timeRemainingTextBox.Text = null;
                clockTextBox.Text = timeOfDay;
                raceClockTextBox.Text = null;

                for (int i = 0; i < positionTextBoxes.Length; ++i)
                {
                    positionTextBoxes[i].Text = null;
                }
            }));

            SendTranslatedStatusMessage();
        }

        void SendTranslatedStatusMessage()
        {
            StringBuilder positions = new StringBuilder();

            for (int i = 0; i < m_positions.Count; ++i)
            {
                string driverNumber;
                if (!m_positions.TryGetValue(i + 1, out driverNumber))
                    break;

                positions.AppendFormat("{0},", driverNumber);
            }

            // Remove trailing comma
            if (positions.Length > 0)
                positions.Remove(positions.Length - 1, 1);

            int round = 0;
            int heat = 0;
            int minutes = 0;
            int seconds = 0;
            int lapsPlaceholder = 1;

            if (!string.IsNullOrEmpty(m_timeRemaining))
            {
                string[] tokens = m_timeRemaining.Split(':');

                // Protocol documentation states that the format is always HH:MM:SS, however LiveTime regularly sends MM:SS
                if (tokens.Length == 2)
                {
                    minutes = int.Parse(tokens[0]);
                    seconds = int.Parse(tokens[1]);
                }
                else if (tokens.Length == 3)
                {
                    minutes = int.Parse(tokens[1]);
                    seconds = int.Parse(tokens[2]);
                }
            }

            //[roundno:heatno:minutes:seconds:car1,carN,:laps1,lapsN,:improver1, improverN,:]
            string scoreboardString = string.Format("[{0}:{1}:{2}:{3}:{4}:{5}::]", round, heat, minutes, seconds, positions.ToString(), lapsPlaceholder);
            System.Diagnostics.Debug.WriteLine(string.Format("Scoreboard string is {0}", scoreboardString));

            if (m_serialPort != null && m_serialPort.IsOpen)
                m_serialPort.Write(scoreboardString);
        }

        class Driver
        {
            public string Number { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
