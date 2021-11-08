using StatisticsProject.DataObjects;
using System;
using System.Collections.Generic;
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
    }
}
