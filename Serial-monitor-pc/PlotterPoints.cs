using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_monitor_pc
{
    public class PlotterPoints
    {
        private LineSeries potSeries;
        private LineSeries battSeries;

        public PlotterPoints() {
            this.MyModel = new PlotModel { Title = "Waardes" };
            this.MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = 0,
                Maximum = 1023,
                Title = "Value"
            });
            potSeries = new LineSeries
            {
                Title = "Potentiometer",
                Color = OxyColors.Blue,
            };
            MyModel.Series.Add(potSeries);
        }

        public PlotModel MyModel { get; private set; }

        public void AddPotValue(int potValue)
        {
            potSeries.Points.Add(new DataPoint(potSeries.Points.Count, potValue));
            if (!MyModel.Series.Contains(potSeries))
            {
                MyModel.Series.Add(potSeries);
            }
            MyModel.InvalidatePlot(true);
        }
    }
}
