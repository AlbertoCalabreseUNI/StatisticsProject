using StatisticsProject.DataObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.Generators
{
    public class RandomGenerator
    {
        public static Random Random = new Random();
        public static List<DataPoint> GenerateRandomStudentsList(int StudentsToGenerate, int MinWeight, int MaxWeight, int MinHeight, int MaxHeight)
        {
            List<DataPoint> TempList = new List<DataPoint>();
            for (int i = 0; i < StudentsToGenerate; i++)
                TempList.Add(new DataPoint((double)Random.Next(MinWeight, MaxWeight), (double)Random.Next(MinHeight, MaxHeight)));

            return TempList;
        }

        public static double BernoulliGenerator()
        {
            double RandomChance = Random.NextDouble();
            if (RandomChance < Form1.SuccessProbability) return 1;
            return 0;
        }

        /* Implementation taken from: https://en.wikipedia.org/wiki/Marsaglia_polar_method */
        public static double GenerateNormalVariable()
        {
            double u, v, s;
            do
            {
                u = Random.NextDouble() * 2 - 1;
                v = Random.NextDouble() * 2 - 1;
                s = u * u + v * v;
            } while (s > 1);

            double x = v * Math.Sqrt(-2.0 * Math.Log(s) / s);
            return x;
        }

        public static Color GenerateRandomColor()
        {
            return Color.FromArgb(Random.Next(255), Random.Next(255), Random.Next(255));
        }
    }
}
