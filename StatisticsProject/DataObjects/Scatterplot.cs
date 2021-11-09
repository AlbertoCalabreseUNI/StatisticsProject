using StatisticsProject.GraphicsObjects;
using StatisticsProject.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.DataObjects
{
    public class Scatterplot
    {
        public List<DataPoint> DataPoints;
        public List<Interval> XIntervals;
        public List<Interval> YIntervals;
        public Scatterplot(List<DataPoint> DataPoints, List<Interval> XIntervals, List<Interval> YIntervals)
        {
            this.DataPoints = DataPoints;
            this.XIntervals = XIntervals;
            this.YIntervals = YIntervals;
        }

        public List<Point> ComputeScatterPlot(Viewport VP)
        {
            List<Point> ConvertedPoints = new List<Point>();
            ConvertedPoints = WindowToViewportConverter.WorldToViewportPointsConversion(VP, this.DataPoints);

            return ConvertedPoints;
        }
    }
}
