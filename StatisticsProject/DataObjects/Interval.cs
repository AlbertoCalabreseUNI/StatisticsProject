using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.DataObjects
{
    public class Interval
    {
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public Interval(double LowerBound, double UpperBound)
        {
            this.LowerBound = LowerBound;
            this.UpperBound = UpperBound;
        }

        public Boolean BelongsToThisInterval(DataPoint DP, int Variable = 0)
        {
            if (DP == null) return false;
            if (Variable == 0) return DP.X >= this.LowerBound && DP.X < this.UpperBound;
            if (Variable == 1) return DP.Y >= this.LowerBound && DP.Y < this.UpperBound;
            return false;
        }
    }
}
