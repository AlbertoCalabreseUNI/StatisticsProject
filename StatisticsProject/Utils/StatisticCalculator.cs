using StatisticsProject.DataObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.Utils
{
    public class StatisticCalculator
    {
        /* Variables: X = 0, Y = 1 */
        public static double ComputeOfflineMean(List<DataPoint> DataSet, int Variable = 0)
        {
            double Mean = 0;
            double TempSum = 0;
            if(Variable == 0)
            {
                foreach(DataPoint DP in DataSet)
                    TempSum += DP.X;
                Mean = TempSum / DataSet.Count();
            }else if(Variable == 1)
            {
                foreach (DataPoint DP in DataSet)
                    TempSum += DP.Y;
                Mean = TempSum / DataSet.Count();
            }
            return Mean;
        }

        public static double ComputeMeanAtN(List<DataPoint> DataSet, int Variable = 0, int N = 1)
        {
            double TempSum = 0;
            if(Variable == 0)
            {
                for (int i = 0; i < N; i++)
                {
                    TempSum += DataSet[i].X;
                }
            }
            else if(Variable == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    TempSum += DataSet[i].Y;
                }
            }

            double Mean = TempSum / N;
            return Mean;
        }

        /* Variables: X = 0, Y = 1 */
        public static double ComputeMedian(List<DataPoint> DataSet, int Variable = 0)
        {
            double Median = 0;
            List<DataPoint> sorted;

            if (Variable == 0)
            {
                sorted = DataSet.OrderBy(x => x.X).ToList();
                if (sorted.Count() % 2 == 1)
                    Median = sorted[sorted.Count() / 2].X;
                else
                    Median = (sorted[sorted.Count() / 2].X + sorted[sorted.Count() / 2 + 1].X) / 2;
            }
            else if(Variable == 1)
            {
                sorted = DataSet.OrderBy(x => x.Y).ToList();
                if (sorted.Count() % 2 == 1)
                    Median = sorted[sorted.Count() / 2].Y;
                else
                    Median = (sorted[sorted.Count() / 2].Y + sorted[sorted.Count() / 2 + 1].Y) / 2;
            }
            return Median;
        }

        /* Variables: X = 0, Y = 1 */
        /* Variance is Standard Deviation squared */
        public static double ComputeVariance(List<DataPoint> DataSet, int Variable = 0)
        {
            double Mean = ComputeOfflineMean(DataSet, Variable);
            double SumOfDistances = 0;

            if(Variable == 0)
            {
                foreach(DataPoint DP in DataSet)
                {
                    //We should take the absolute values of the subtractions but since the result is squared
                    //it'll always be positive.
                    SumOfDistances += (DP.X - Mean) * (DP.X - Mean);
                }
            }
            else if(Variable == 1)
            {
                foreach (DataPoint DP in DataSet)
                {
                    //We should take the absolute values of the subtractions but since the result is squared
                    //it'll always be positive.
                    SumOfDistances += (DP.Y - Mean) * (DP.Y - Mean);
                }
            }

            double Variance = (1 / DataSet.Count()) * SumOfDistances; 
            return Variance;
        }

        public static double ComputeVarianceAtStepN(List<DataPoint> DataSet, int Variable = 0, int N = 1)
        {
            double SumOfDistances = 0;
            double Mean = ComputeMeanAtN(DataSet, Variable, N);
            if(Variable == 0)
            {
                for(int i = 0; i < N; i++)
                {
                    SumOfDistances += (DataSet[i].X - Mean) * (DataSet[i].X - Mean);
                }
            }
            else if(Variable == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    SumOfDistances += (DataSet[i].X - Mean) * (DataSet[i].X - Mean);
                }
            }

            double VarianceAtN = (1 / N) * SumOfDistances;
            return VarianceAtN;
        }

        /* Variables: X = 0, Y = 1 */
        public static double ComputeStandardDeviation(List<DataPoint> DataSet, int Variable = 0)
        {
            return Math.Sqrt(ComputeVariance(DataSet, Variable));
        }

        /* Variables: X = 0, Y = 1 */
        /* Welford Algorithm */
        public static double ComputeOnlineVariance(List<DataPoint> DataSet, int Variable = 0)
        {
            double Variance = 0;
            if (Variable == 0)
            {
                for (int i = 2; i < DataSet.Count(); i++)
                {
                    Variance += (DataSet[i].X - ComputeMeanAtN(DataSet, Variable, i - 1)) * (DataSet[i].X - ComputeMeanAtN(DataSet, Variable, i));
                }
            }
            else if (Variable == 1)
            {
                for (int i = 2; i < DataSet.Count(); i++)
                {
                    Variance += (DataSet[i].Y - ComputeMeanAtN(DataSet, Variable, i - 1)) * (DataSet[i].Y - ComputeMeanAtN(DataSet, Variable, i));
                }
            }

            return Variance;
        }

        public static double ComputeCrossProduct(List<DataPoint> DataSet, int Index = 0)
        {
            double XMean = ComputeOfflineMean(DataSet, 0);
            double YMean = ComputeOfflineMean(DataSet, 1);

            double CrossProduct = (DataSet[Index].X - XMean) * (DataSet[Index].Y - YMean);
            return CrossProduct;
        }

        public static double ComputeCovariance(List<DataPoint> DataSet)
        {
            double TempCrossProductSum = 0;
            for(int i = 0; i < DataSet.Count(); i++)
                TempCrossProductSum += ComputeCrossProduct(DataSet, i);

            double Covariance = (1 / DataSet.Count()) * TempCrossProductSum;
            return Covariance;
        }

        /* Variables: X = 0, Y = 1 */
        public static double ComputeCorrelation(List<DataPoint> DataSet)
        {
            double Correlation = 0;
            for (int i = 0; i < DataSet.Count(); i++)
                Correlation += DataSet[i].X * DataSet[i].Y;

            /* https://stackoverflow.com/questions/15623129/simple-linear-regression-for-data-set/31474739 */
            double XAvg = ComputeOfflineMean(DataSet, 0);
            double YAve = ComputeOfflineMean(DataSet, 1);
            Correlation -= XAvg * YAve * DataSet.Count;

            return Correlation;
        }

        /* Variables: X = 0, Y = 1 */
        //TODO
        public static List<Point> ComputeLinearRegressionPoints(List<DataPoint> DataSet, int Variable = 0)
        {
            List<Point> PointsForLine = new List<Point>();

            return PointsForLine;
        }
    }
}
