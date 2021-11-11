using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.DataObjects
{
    public class RegressionLine
    {
        public List<DataPoint> DataSet;
        public int Variable;
        /* Variable: X = 0, Y = 1, Both = 2 */
        public RegressionLine(List<DataPoint> DataSet, int Variable = 0)
        {
            this.DataSet = DataSet;
            this.Variable = Variable;
        }

        public List<Point> ComputeRegressionLine()
        {
            List<Point> ComputedLine = new List<Point>();


            return ComputedLine;
        }
    }
}
