using StatisticsProject.GraphicsObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticsProject.DataObjects
{
    public class Histogram
    {
        public List<DataPoint> DataPoints;
        public List<Interval> XIntervals;
        public List<Interval> YIntervals;
        public int HistogramOrientation { get; set; } //0 = Vertical, 1 = Horizontal
        public Histogram(List<DataPoint> DataPoints, List<Interval> XIntervals, List<Interval> YIntervals, int HistogramOrientation = 0)
        {
            this.DataPoints = DataPoints;
            this.XIntervals = XIntervals;
            this.YIntervals = YIntervals;
            this.HistogramOrientation = HistogramOrientation;
        }

        //This creates the necessary rectangles to draw a Histogram INSIDE the Viewport to represent a Classical Histogram Chart
        //The Drawing is handled by the Viewport Class as this is ONLY to compute the necessary elements
        public List<Rectangle> ComputeDefaultHistogram(Viewport Viewport, int Variable)
        {
            List<Rectangle> DefaultHistogramRectangles = new List<Rectangle>();
            double InitialX = Viewport.Area.X;
            double InitialY = Viewport.Area.Y + Viewport.Area.Height; //Starting from the bottom and then "moving" the rectangle up.

            double RectangleWidth = Viewport.Area.Width / this.XIntervals.Count; //This never changes during Computing
            double RectangleHeightUpdate = Viewport.Area.Height / this.DataPoints.Count; //Chunk to "add" to the rectangle.

            //We need a certain amount of rectangles based on whether we group by value of X (0) or Y (1).
            int NumberOfRectangles = XIntervals.Count;
            if (Variable == 1) NumberOfRectangles = YIntervals.Count;

            for(int i = 0; i < NumberOfRectangles; i++)
            {
                Rectangle rect = new Rectangle((int)InitialX + (i*(int)RectangleWidth), (int)InitialY, (int)RectangleWidth, 0); //Initial "Empty" rectangle
                foreach(DataPoint point in this.DataPoints)
                {
                    //The Variable check is needed only because we might want to group data points based on X or Y.
                    if(Variable == 0 && XIntervals[i].BelongsToThisInterval(point) || Variable == 1 && YIntervals[i].BelongsToThisInterval(point, Variable))
                    {
                        //We simply move up the rectangle and increase its height.
                        rect.Y -= (int)RectangleHeightUpdate;
                        rect.Height += (int)RectangleHeightUpdate;
                    }
                }
                DefaultHistogramRectangles.Add(rect);
            }

            return DefaultHistogramRectangles;
        }

        public List<Rectangle> ComputeXTopHistogram(Viewport Viewport)
        {
            List<Rectangle> DefaultHistogramRectangles = new List<Rectangle>();
            double InitialX = Viewport.Area.X;
            double InitialY = Viewport.Area.Y; //Starting from the top

            double RectangleWidth = Viewport.Area.Width / this.XIntervals.Count; //This never changes during Computing
            double RectangleHeightUpdate = Viewport.Area.Height / this.DataPoints.Count; //Chunk to "add" to the rectangle.

            //We need a certain amount of rectangles based on whether we group by value of X (0) or Y (1).
            int NumberOfRectangles = XIntervals.Count;

            for (int i = 0; i < NumberOfRectangles; i++)
            {
                Rectangle rect = new Rectangle((int)InitialX + (i * (int)RectangleWidth), (int)InitialY, (int)RectangleWidth, 0); //Initial "Empty" rectangle
                foreach (DataPoint point in this.DataPoints)
                {
                    //The Variable check is needed only because we might want to group data points based on X or Y.
                    if (XIntervals[i].BelongsToThisInterval(point))
                    {
                        //We simply move up the rectangle and increase its height.
                        rect.Y -= (int)RectangleHeightUpdate;
                        rect.Height += (int)RectangleHeightUpdate;
                    }
                }
                DefaultHistogramRectangles.Add(rect);
            }

            return DefaultHistogramRectangles;
        }

        public List<Rectangle> ComputeYHorizontalHistogram(Viewport Viewport)
        {
            List<Rectangle> DefaultHistogramRectangles = new List<Rectangle>();
            double InitialX = Viewport.Area.X + Viewport.Area.Width; //We want to draw the Histogram on the right of the Viewport
            double InitialY = Viewport.Area.Y + Viewport.Area.Height; //Starting from the bottom and then "moving" the rectangle up.

            double RectangleWidthUpdate = Viewport.Area.Width / this.DataPoints.Count; //This never changes during Computing
            double RectangleHeight = Viewport.Area.Height / this.YIntervals.Count; //Chunk to "add" to the rectangle.

            //We need a certain amount of rectangles based on whether we group by value of X (0) or Y (1).
            int NumberOfRectangles = YIntervals.Count;

            for (int i = 0; i < NumberOfRectangles; i++)
            {
                Rectangle rect = new Rectangle((int)InitialX, (int)InitialY - ((i+1) * (int)RectangleHeight), 0, (int)RectangleHeight); //Initial "Empty" rectangle
                foreach (DataPoint point in this.DataPoints)
                {
                    if (YIntervals[i].BelongsToThisInterval(point, 1))
                    {
                        //We simply make the rectangle wider
                        rect.Width += (int)RectangleWidthUpdate;
                    }
                }
                DefaultHistogramRectangles.Add(rect);
            }

            return DefaultHistogramRectangles;
        }
    }
}
