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

        public PlotterPoints() {
            this.MyModel = new PlotModel { Title = "Waardes" };
            potSeries = new LineSeries
            {
                Title = "Potentiometer",
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
