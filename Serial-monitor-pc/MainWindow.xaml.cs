using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Serial_monitor_pc
{
    // Nog te doen: OxyPlot grafiek toevoegen
    // Nog te doen: Data opslaan in een tekstbestand
    // Nog te doen: Error handling verbeteren
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        SerialPort serialPort = new SerialPort();
        RecievedData data = new RecievedData();
        PlotterPoints plotterGrafiek = new PlotterPoints();

        bool plotterDefined = false;

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectPort() == true)
            {
                connectButton.IsEnabled = false;
                disconnectButton.IsEnabled = true;
                comPortComboBox.IsEnabled = false;
                baudRateComboBox.IsEnabled = false;
                scanButton.IsEnabled = false;
                sendButton.IsEnabled = true;
                plotterButton.IsEnabled = true;

            }
        }
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write("Connection closed");
            }  
            serialPort.Close();
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = false;
            comPortComboBox.IsEnabled = true;
            baudRateComboBox.IsEnabled = true;
            scanButton.IsEnabled = true;
            sendButton.IsEnabled = false;
            plotterButton.IsEnabled = false;
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(sendTextBox.Text))
            {
                if (typeData.Text == "Tijd:")
                {
                    try
                    {
                        if (!int.TryParse(sendTextBox.Text, out int seconds) || seconds < 100)
                        {
                            MessageBox.Show("Please enter a number bigger than 100 milliseconds.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }else
                        {
                            serialPort.Write("T" + seconds.ToString());
                        }
                        
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter a valid number of milliseconds.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (typeData.Text == "Servo:")
                {
                    try
                    {
                        if (int.TryParse(sendTextBox.Text, out int angle))
                        {
                            if (angle >= 0 && angle <= 180)
                            {
                                serialPort.Write("S" + angle.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Please enter an angle between 0 and 180.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid number for the angle.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        serialPort.Write("S" + sendTextBox.Text);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter a valid number for the angle.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            comPortComboBox.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comPortComboBox.Items.Add(port);
            }
        }

        public bool ConnectPort()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(comPortComboBox.Text) && !string.IsNullOrWhiteSpace(baudRateComboBox.Text))
                {
                    serialPort.PortName = comPortComboBox.Text;
                    serialPort.BaudRate = Int32.Parse(baudRateComboBox.Text);
                    serialPort.Encoding = Encoding.ASCII;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                    serialPort.Open();
                    serialPort.Write("Connection opened");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK);
            }
            return false;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string dump = serialPort.ReadExisting();
                data = decodeData(dump);
                Dispatcher.BeginInvoke(() =>
                {
                    dataTextBox.AppendText(dump + Environment.NewLine);
                    dataTextBox.ScrollToEnd();
                    plotterGrafiek.AddPotValue(data.potValue);
                    SetProgress(data.potValue);

                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    dataTextBox.AppendText("[Error] " + ex.Message + Environment.NewLine);
                });
            }
        }

        private void plotterButton_Click(object sender, RoutedEventArgs e)
        {
            Plotter plotter = new Plotter(plotterGrafiek);
            plotter.Show();
        }

        static RecievedData decodeData(string data)
        {
            var recievedData = new RecievedData();

            data = data.Trim().TrimEnd('.');

            string[] pairs = data.Split(',');

            foreach (var pair in pairs)
            {
                string[] parts = pair.Split(':');
                if (parts.Length != 2) continue;

                string key = parts[0].Trim().ToLower();
                int value = int.Parse(parts[1].Trim());

                switch (key)
                {
                    case "pot":
                        recievedData.potValue = value;
                        break;
                    case "eindeloop":
                        recievedData.eindeloop = Convert.ToBoolean(value); ;
                        break;
                    case "knop1":
                        recievedData.knop1 = Convert.ToBoolean(value); 
                        break;
                    case "knop 2": 
                        recievedData.knop2 = Convert.ToBoolean(value); 
                        break;
                }
            }

            return recievedData;
        }

        private const double TotalLength = 314; // halve cirkel lengte (radius 100)

        private void SetProgress(int waarde)
        {
            int potProcent = (int)((waarde / 1023.0) * 100);

            // Bereken offset
            double offset = TotalLength - (potProcent / 100.0 * TotalLength);

            // Direct instellen
            potProgress.StrokeDashOffset = offset;


            DoubleAnimation anim = new DoubleAnimation
            {
                To = offset,
                Duration = TimeSpan.FromMilliseconds(50)
            };
            potProgress.BeginAnimation(Path.StrokeDashOffsetProperty, anim);

        }
    }
}

