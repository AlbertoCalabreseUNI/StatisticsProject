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
        public Color PathColor;
        public List<DataPoint> Path = new List<DataPoint>();
        public List<Double> PathPoints = new List<Double>();

        public RandomPath()
        {
            this.PathColor = RandomGenerator.GenerateRandomColor();
            this.PopulatePathPoints();
        }

        //This generates an array of 0 and 1s where 0 means the point was not created and 1 it was.
        public void PopulatePathPoints()
        {
            for (int i = 0; i < Form1.PathSize; i++)
                this.PathPoints.Add(RandomGenerator.BernoulliGenerator());
        }

        public List<DataPoint> ComputePath()
        {
            //This is needed to recalculate Data Points every time viewport is moved/mode changes
            this.Path.Clear();
            
            if (Form1.FrequencyMode == 0)
            {
                double count = 0;
                for (int i = 0; i < PathPoints.Count; i++)
                {
                    if (PathPoints[i] == 1)
                        count++;

                    double NormalisedY = (count) / (i + 1);
                    Path.Add(new DataPoint(i, NormalisedY));
                }
            }
            else if (Form1.FrequencyMode == 1)
            {
                //Path must have at least one DataPoint in order to work correctly.
                //The first point will be the data point of X = 0 (first point) and Y = the
                //First element of PathPoints (Either 0 or 1 from the bernoulli generator)
                Path.Add(new DataPoint(0,this.PathPoints[0]));
                for (int i = 1; i < this.PathPoints.Count; i++)
                    Path.Add(new DataPoint(i, this.Path[i-1].Y + this.PathPoints[i]));
            }
            else
            {
                double count = 0;
                double q = 1 - Form1.SuccessProbability;
                for(int i = 0; i < PathPoints.Count; i++)
                {
                    if (PathPoints[i] == 1)
                        count++;

                    Path.Add(new DataPoint(i, ((count / (i + 1)) - Form1.SuccessProbability) / Math.Sqrt(Form1.SuccessProbability * q / (i + 1))));
                }
            }

            return this.Path;
        }

        public void ResetPath()
        {
            this.Path.Clear();
            this.PathPoints.Clear();
            this.PathColor = RandomGenerator.GenerateRandomColor();
        }
    }
}
