using StatisticsProject.DataObjects;
using StatisticsProject.GraphicsObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatisticsProject.Utils
{
    public class WindowToViewportConverter
    {
        public static List<Point> WorldToViewportPointsConversion(Viewport Viewport, List<DataPoint> PointsToConvert)
        {
            List<Point> ConvertedPoints = new List<Point>();

            double MinX = GetMinX(PointsToConvert);
            double MinY = GetMinY(PointsToConvert);
            double MaxX = GetMaxX(PointsToConvert);
            double MaxY = GetMaxY(PointsToConvert);

            double RangeX = MaxX - MinX;
            double RangeY = MaxY - MinY;

            foreach(DataPoint Point in PointsToConvert)
            {
                int XConverted = XPosScale(Viewport, Point.X, MinX, RangeX);
                int YConverted = YPosScale(Viewport, Point.Y, MinY, RangeY);

                ConvertedPoints.Add(new Point(XConverted, YConverted));
            }

            return ConvertedPoints;
        }

        #region Scale Factor Calculator
        //Homework 4 Functions to calculate the scale for Window to Viewport position transformation for element's X axis
        private static int XPosScale(Viewport Viewport, double WorldX, double MinX, double RangeX)
        {
            return (int)(Viewport.Area.Left + Viewport.Area.Width * (WorldX - MinX) / RangeX);

        }
        //Homework 4 Functions to calculate the scale for Window to Viewport position transformation for element's Y axis
        private static int YPosScale(Viewport Viewport, double WorldY, double MinY, double RangeY)
        {
            return (int)(Viewport.Area.Top + Viewport.Area.Height - Viewport.Area.Height * (WorldY - MinY) / RangeY);
        }
        #endregion


        #region Getters for Min and Max X/Y
        private static double GetMinY(List<DataPoint> PointsToConvert)
        {
            double MinY = PointsToConvert[0].Y; //We must declare an initial value
            foreach (DataPoint p in PointsToConvert)
                if (p.Y < MinY)
                    MinY = p.Y;

            return MinY;
        }

        private static double GetMinX(List<DataPoint> PointsToConvert)
        {
            double MinX = PointsToConvert[0].X; //We must declare an initial value
            foreach (DataPoint p in PointsToConvert)
                if (p.X < MinX)
                    MinX = p.X;

            return MinX;
        }

        private static double GetMaxX(List<DataPoint> PointsToConvert)
        {
            double MaxX = PointsToConvert[0].X; //We must declare an initial value
            foreach (DataPoint p in PointsToConvert)
                if (p.X > MaxX)
                    MaxX = p.X;

            return MaxX;
        }

        private static double GetMaxY(List<DataPoint> PointsToConvert)
        {
            double MaxY = PointsToConvert[0].Y; //We must declare an initial value
            foreach (DataPoint p in PointsToConvert)
                if (p.Y > MaxY)
                    MaxY = p.Y;

            return MaxY;
        }
        #endregion
    }
}
