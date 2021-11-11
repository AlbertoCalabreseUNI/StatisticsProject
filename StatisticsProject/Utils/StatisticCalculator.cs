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

        /* Variables: X = 0, Y = 1 */
        public static double ComputeStandardDeviation(List<DataPoint> DataSet, int Variable = 0)
        {
            //We need the Mean to compute Standard Deviation, we can just use the function we wrote above.
            double Mean = ComputeOfflineMean(DataSet, Variable);
            double SumOfDerivation = 0;
            if(Variable == 0)
            {
                foreach(DataPoint DP in DataSet)
                    SumOfDerivation += DP.X * DP.X;
            }else if(Variable == 1)
            {
                foreach (DataPoint DP in DataSet)
                    SumOfDerivation += DP.Y * DP.Y;
            }
            double SumOfDerivationAverage = SumOfDerivation / DataSet.Count();
            double StandardDeviation = Math.Sqrt(SumOfDerivationAverage - (Mean * Mean));

            return StandardDeviation;
        }

        /* Variables: X = 0, Y = 1 */
        public static double ComputeVariance(List<DataPoint> DataSet, int Variable = 0)
        {
            double OldMean = 0;
            double CurrentMean = 0;
            double SS = 0;

            if(Variable == 0)
            {
                for (int i = 0; i < DataSet.Count; i++)
                {
                    OldMean = CurrentMean;
                    CurrentMean += (DataSet[i].X - OldMean) / (i + 1);
                    SS = SS + (DataSet[i].X - CurrentMean) * (DataSet[i].X - OldMean);
                }
            }
            else if(Variable == 1)
            {
                for (int i = 0; i < DataSet.Count; i++)
                {
                    OldMean = CurrentMean;
                    CurrentMean += (DataSet[i].Y - OldMean) / (i + 1);
                    SS = SS + (DataSet[i].Y - CurrentMean) * (DataSet[i].Y - OldMean);
                }
            }

            double Variance = SS / DataSet.Count;
            return Variance;
        }

        public static double ComputeCovariance(List<DataPoint> DataSet)
        {
            double SC = 0;
            double OldXMean = 0;
            double CurrentYMean = 0;

            for (int i = 0; i < DataSet.Count; i++)
            {
                CurrentYMean += (DataSet[i].Y - CurrentYMean) / (i + 1);
                SC = SC + (DataSet[i].X - OldXMean) * (DataSet[i].Y - CurrentYMean);
                OldXMean += (DataSet[i].X - OldXMean) / (i + 1);
            }

            double Covariance = SC / DataSet.Count;
            return Covariance;
        }

        /* Variables: X = 0, Y = 1 */
        /*
        public static double ComputeSumOfSquares(List<DataPoint> DataSet, int Variable = 0)
        {
            double SumOfSquares = 0.0;
            if(Variable == 0)
            {
                foreach (DataPoint DP in DataSet)
                    SumOfSquares += DP.X * DP.X;
            }
            else if(Variable == 1)
            {
                foreach (DataPoint DP in DataSet)
                    SumOfSquares += DP.Y * DP.Y;
            }

            double average = average(values);
            SumOfSquares -= average * average * values.length;
            return SumOfSquares;
        }
        */

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
