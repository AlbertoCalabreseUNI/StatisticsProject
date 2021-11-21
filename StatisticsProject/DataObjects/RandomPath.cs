using StatisticsProject.Generators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatisticsProject.Utils;
using StatisticsProject.GraphicsObjects;

namespace StatisticsProject.DataObjects
{
    public class RandomPath
    {
        public Color PathColor { get; }
        public List<DataPoint> Path = new List<DataPoint>();
        public List<Double> PathPoints = new List<Double>();

        public RandomPath()
        {
            this.PathColor = RandomGenerator.GenerateRandomColor();
            this.PopulatePathPoints();
            this.Path = ComputePath();
        }

        public void PopulatePathPoints()
        {
            for (int i = 0; i < Form1.PathSize; i++)
            {
                this.PathPoints.Add(RandomGenerator.BernoulliGenerator());
            }
        }

        public List<DataPoint> ComputePath()
        {
            this.Path.Clear();
            //Trajectory Calculation
            if (Form1.FrequencyMode == 0)
            {
                int count = 0;
                for (int i = 0; i < PathPoints.Count; i++)
                {
                    if (PathPoints[i] == 1)
                        count++;

                    Path.Add(new DataPoint(i, (double) count / (i + 1)));
                }
            }
            else if (Form1.FrequencyMode == 1)
            {
                //Path must have at least one DataPoint in order to work correctly.
                //The first point will be the data point of X = 0 (first point) and Y = the
                //First element of PathPoints (Either 0 or 1 from the bernoulli generator)
                Path.Add(new DataPoint(0,PathPoints.First()));
                for (int i = 1; i < PathPoints.Count; i++)
                    Path.Add(new DataPoint(i, PathPoints[i-1] + PathPoints[i]));
            }
            else
            {
                int count = 0;
                double q = 1 - Form1.SuccessProbability;
                for(int i = 0; i < PathPoints.Count; i++)
                {
                    if (PathPoints[i] == 1)
                        Path.Add(new DataPoint(i, (((double)count / (i + 1)) - Form1.SuccessProbability) / Math.Sqrt((double)Form1.SuccessProbability * q / (i + 1))));
                }
            }

            return this.Path;
        }

        public List<Point> ComputePointsToDraw(Viewport VP)
        {
            List<Point> points = WindowToViewportConverter.WorldToViewportPointsConversion(VP, this.Path);

            return points;
        }
    }
}
