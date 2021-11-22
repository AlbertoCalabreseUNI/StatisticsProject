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
    public class Sequence
    {
        public List<RandomPath> AllPaths;
        public List<DataPoint> AllPoints;
        public Sequence(List<RandomPath> AllRandomPaths)
        {
            this.AllPaths = AllRandomPaths;
            this.AllPoints = new List<DataPoint>();
            this.ComputeAllPoints();
        }

        public void ComputeAllPoints()
        {
            foreach(RandomPath path in this.AllPaths)
                AllPoints.AddRange(path.Path);
        }

        public List<Point> ComputePointsToDraw(Viewport VP, RandomPath Path)
        {
            List<Point> ToDraw = WindowToViewportConverter.WorldToViewportPointsConversionPaths(VP, Path.Path, this.AllPoints);
            return ToDraw;
        }
    }
}
