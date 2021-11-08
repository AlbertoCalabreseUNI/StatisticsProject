using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.DataObjects
{
    public class DataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DataPoint(double inX, double inY)
        {
            this.X = inX;
            this.Y = inY;
        }
    }
}
