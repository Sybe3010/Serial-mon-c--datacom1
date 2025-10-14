using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Serial_monitor_pc
{
    /// <summary>
    /// Interaction logic for TFT_controls.xaml
    /// </summary>
    public partial class TFT_controls : Window
    {

        SerialPort serialPort;
        public TFT_controls(SerialPort port)
        {
            InitializeComponent();
            serialPort = port;
        }


    }
}
