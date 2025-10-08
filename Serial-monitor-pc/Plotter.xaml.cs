using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Plotter.xaml
    /// </summary>
    public partial class Plotter : Window
    {
        public Plotter(PlotterPoints sharedPlotter)
        {
            InitializeComponent();
            DataContext = sharedPlotter;
            this.Closing += Plotter_Closing;
        }

        private void Plotter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;      // Voorkom dat het venster sluit
        }
    }
}
