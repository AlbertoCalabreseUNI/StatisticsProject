using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StatisticsProject.DataObjects;
using StatisticsProject.Generators;
using StatisticsProject.Utils;

namespace StatisticsProject.GraphicsObjects
{
    public class Viewport
    {
        //Viewport elements we need
        //Constructor
        private PictureBox PictureBox;
        public Rectangle Area { get; set; }
        //Temporary rectangle used for dragMode
        public Rectangle AreaOnMouseDown { get; set; }

        //Drawing elements. The bitmap is used ONLY to get the Graphics object, otherwise we'd have to pass it everytime in each function
        //Or call drawing functions from the Form1 object instance which is way messier.
        public Bitmap Bitmap { get; set; }
        public Graphics G { get; set; }
        public Pen BorderColor = Pens.Black;
        public int Interval = 10;
        public int lineLength;

        public FontFamily FontFamily = new FontFamily("Arial");
        public Font Font;

        //Booleans to decide which operation to do
        public bool dragMode;
        public bool resizeMode;

        //This gets set ONLY when we click on the viewport instance. It's used to calculate deltas
        public Point MouseClickLocation { get; set; }


        #region Charts Data
        //List of Intervals used in Charts
        List<Interval> XIntervals = new List<Interval>();
        List<Interval> YIntervals = new List<Interval>();

        List<DataPoint> DataSet = new List<DataPoint>();
        List<RandomPath> RandomPaths = new List<RandomPath>();

        Sequence Sequence;
        List<DataPoint> PointsAtTimeN;
        List<DataPoint> PointsAtTimeT;

        //This is a List used in common by all types of Histograms
        List<Rectangle> RectanglesToDraw = new List<Rectangle>();

        //List of DataSets for each Chart Type
        #region HISTOGRAMS
        //Let's create a DataSet of Random Elements and then Compute an Histogram
        public int XAttributeMin = 40;
        public int XAttributeMax = 100;
        public int YAttributeMin = 140;
        public int YAttributeMax = 200;
        public int NumberOfStudents;

        public Histogram StudentHistogram;
        public Histogram SequenceHistogram;
        public Histogram SequenceHistogramAtT;
        #endregion
        #region SCATTERPLOT
        public Scatterplot Scatterplot;
        public List<Point> PointsToDraw;
        public int PointSize = 5;
        #endregion

        #endregion

        public Viewport(PictureBox picturebox, Rectangle area, Graphics g)
        {
            this.PictureBox = picturebox;
            this.Area = area;

            this.Bitmap = new Bitmap(this.PictureBox.Width, this.PictureBox.Height);
            this.G = g;

            //Graphics options
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.lineLength = this.Area.Height / this.Interval;
            this.Font = new Font(this.FontFamily, 10, FontStyle.Regular, GraphicsUnit.Pixel);

            //Mode initialization
            this.dragMode = false;
            this.resizeMode = false;

            #region DEFAULT HISTOGRAMS */
            //Let's create a DataSet of Random Elements and then Compute an Histogram
            this.NumberOfStudents = RandomGenerator.Random.Next(20, 100);
            #endregion
        }

        #region Viewport Draw/Redraw after Move/Resize
        public void DrawViewport()
        {
            G.Clear(Color.White);
            G.DrawRectangle(this.BorderColor, this.Area);
        }
        public void RedrawAfterMoveOrResize()
        {
            this.DrawViewport();
            /*  Default Histogram = 0
             *  Top Vertical Histogram = 1
             *  Right Edge Horizontal Histogram = 2
             *  Both Top and Right Histograms = 3
             *  Scatterplot + Both top and Right Histograms = 4
             *  RegressionLines + Scatterplot + Both top and Right Histograms = 5
             *  Random Paths = 6
             */

            switch (Form1.GraphTypeToDraw)
            {
                case 0: case 1: case 2: case 3:
                    this.DrawHistogram(Form1.GraphTypeToDraw);
                    this.DrawIntervals();
                    break;
                case 4: //To Draw a Scatterplot I need to generate the data from the Histogram
                    this.DrawHistogram(3);
                    this.DrawScatterPlot();
                    this.DrawIntervals();
                    break;
                case 5:
                    this.DrawHistogram(3);
                    this.DrawScatterPlot();
                    this.DrawRegressionLines(2); /* 0 = X Axis Only, 1 = Y Axis Only, 2 =  Both*/
                    this.DrawIntervals();
                    break;
                case 6:
                    this.DrawPaths();
                    this.DrawHistogram(4);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Chart Drawing Functions
        public void DrawIntervals()
        {
            if(this.XIntervals.Count != 0)
            {
                for(int i = 0; i <= this.XIntervals.Count; i++)
                {
                    Point P1 = new Point(this.Area.X + (i * (this.Area.Width / this.XIntervals.Count)), this.Area.Y + this.Area.Height - 5);
                    Point P2 = new Point(this.Area.X + (i * (this.Area.Width / this.XIntervals.Count)), this.Area.Y + this.Area.Height + 5);
                    G.DrawLine(Pens.Black, P1, P2);
                    //"Hardcoded" fix for more pleasant rendering of Intervals
                    if(i != this.XIntervals.Count) G.DrawString(XIntervals[i].LowerBound.ToString(), this.Font, Brushes.Black, P2);
                    else G.DrawString(XIntervals[i-1].UpperBound.ToString(), this.Font, Brushes.Black, P2);
                }
            }
            
            if (this.YIntervals.Count != 0)
            {
                for (int i = 0; i <= this.YIntervals.Count; i++)
                {
                    Point P1 = new Point(this.Area.X - 5, this.Area.Y + (i * (this.Area.Height / this.YIntervals.Count)));
                    Point P2 = new Point(this.Area.X + 5, this.Area.Y + (i * (this.Area.Height / this.YIntervals.Count)));
                    G.DrawLine(Pens.Black, P1, P2);

                    //We do not want to draw Y intervals when we draw the classical Histogram, we want to draw count
                    //"Hardcoded" fix for more pleasant rendering of Intervals
                    if(Form1.GraphTypeToDraw != 0)
                    {
                        if (i != this.YIntervals.Count) G.DrawString(YIntervals[YIntervals.Count - i - 1].UpperBound.ToString(), this.Font, Brushes.Black, P1.X - 20, P1.Y);
                        else G.DrawString(YIntervals[0].LowerBound.ToString(), this.Font, Brushes.Black, P1.X - 20, P1.Y);
                    } 
                    else
                    {
                        if (i != this.YIntervals.Count) G.DrawString(YIntervals[YIntervals.Count - i - 1].UpperBound.ToString(), this.Font, Brushes.Black, P1.X - 20, P1.Y);
                        else G.DrawString(YIntervals[0].LowerBound.ToString(), this.Font, Brushes.Black, P1.X - 20, P1.Y);
                    }  
                }
            }
        }
        public void DrawHistogram(int GraphType)
        {
            //Let's avoid repopulating the X Intervals everytime we move the Viewport. But we need to repopulate the Y Intervals
            //for visualization purposes
            //We manually define the intervals for the classic Histogram on Students.
            //For next charts we'll have intervals be different

            if (this.XIntervals.Count == 0)
            {
                this.XIntervals = new List<Interval>();
                for (int i = 0; i < this.XAttributeMax - this.XAttributeMin; i += this.Interval)
                    XIntervals.Add(new Interval(this.XAttributeMin + i, this.XAttributeMin + i + this.Interval));
            }

            YIntervals.Clear();
            //If we display DefaultHistogram (GraphTypeToDraw == 0) then we want the Y intervals to be a count of elements
            if (GraphType != 0)
            {
                //new Interval(50 + 0, 50 + 0 + 10)
                for (int i = 0; i < this.YAttributeMax - this.YAttributeMin; i += this.Interval)
                    YIntervals.Add(new Interval(this.YAttributeMin + i, this.YAttributeMin + i + this.Interval));
            }
            else
            {
                for (int i = 0; i < this.NumberOfStudents; i += this.Interval)
                    YIntervals.Add(new Interval(i, i + this.Interval));
            }

            if (Form1.FrequencyMode == 3 && GraphType == 4)
            {
                YIntervals.Clear();
                /* Creating intervals for Brownian at time N */
                /* We need to compute the PointsAtTimeN here since we need it to calculate the intervals */
                this.PointsAtTimeN = this.Sequence.TakePointsAtTimeN(Form1.PathSize);
                double MinY = WindowToViewportConverter.GetMinY(this.PointsAtTimeN);
                double MaxY = WindowToViewportConverter.GetMaxY(this.PointsAtTimeN);

                //We want to have 10 intervals only for now it's hardcoded.
                double RangeY = (MaxY - MinY) / 10;

                for (int i = 0; i < 10; i++)
                    YIntervals.Add(new Interval(MinY + (i * RangeY), MinY + ((i + 1)* RangeY)));

                //For Brownian
                this.SequenceHistogram = new Histogram(this.PointsAtTimeN, XIntervals, YIntervals, 1);

                YIntervals.Clear();
                /* Creating intervals for Brownian at time N */
                /* We need to compute the PointsAtTimeN here since we need it to calculate the intervals */
                this.PointsAtTimeT = this.Sequence.TakePointsAtTimeN(Form1.TimeT);
                MinY = WindowToViewportConverter.GetMinY(this.PointsAtTimeT);
                MaxY = WindowToViewportConverter.GetMaxY(this.PointsAtTimeT);

                //We want to have 10 intervals only for now it's hardcoded.
                RangeY = (MaxY - MinY) / 10;

                for (int i = 0; i < 10; i++)
                    YIntervals.Add(new Interval(MinY + (i * RangeY), MinY + ((i + 1) * RangeY)));

                //For Brownian
                this.SequenceHistogramAtT = new Histogram(this.PointsAtTimeT, XIntervals, YIntervals, 1);
            }           

            if (this.DataSet.Count == 0) //If we didn't generate the DataSet yet, then do it and generate the histogram
            {
                this.DataSet = RandomGenerator.GenerateRandomStudentsList(this.NumberOfStudents, this.XAttributeMin, this.XAttributeMax, this.YAttributeMin, this.YAttributeMax);
                this.StudentHistogram = new Histogram(this.DataSet, this.XIntervals, this.YIntervals);
            }

            switch(GraphType)
            {
                case 0:
                    this.RectanglesToDraw = this.StudentHistogram.ComputeDefaultHistogram(this, 0/*0 = X Variable, 1 = Y Variable*/);
                    break;
                case 1:
                    this.RectanglesToDraw = this.StudentHistogram.ComputeXTopHistogram(this);
                    break;
                case 2:
                    this.RectanglesToDraw = this.StudentHistogram.ComputeYHorizontalHistogram(this);
                    break;
                case 3:
                    //We simply draw both XTop and YHorizontal
                    this.RectanglesToDraw = this.StudentHistogram.ComputeXTopHistogram(this);
                    this.RectanglesToDraw.AddRange(this.StudentHistogram.ComputeYHorizontalHistogram(this));
                    break;
                default:
                    break;
            }

            /* Brownian has priority over the other histograms */
            if(Form1.FrequencyMode == 3 && GraphType == 4)
            {
                this.RectanglesToDraw = this.SequenceHistogram.ComputeYHorizontalHistogram(this);
                this.RectanglesToDraw.AddRange(this.SequenceHistogramAtT.ComputeYHorizontalHistogramAtT(this, Form1.TimeT));
            }

            this.G.DrawRectangles(this.BorderColor, this.RectanglesToDraw.ToArray());
            this.G.FillRectangles(Brushes.Red, this.RectanglesToDraw.ToArray());
        }
        public void DrawScatterPlot()
        {
            if (this.XIntervals.Count == 0)
            {
                this.XIntervals = new List<Interval>();
                for (int i = 0; i < this.XAttributeMax - this.XAttributeMin; i += this.Interval)
                    XIntervals.Add(new Interval(this.XAttributeMin + i, this.XAttributeMin + i + this.Interval));
            }

            if (this.YIntervals.Count == 0)
            {
                for (int i = 0; i < this.YAttributeMax - this.YAttributeMin; i += this.Interval)
                    YIntervals.Add(new Interval(this.YAttributeMin + i, this.YAttributeMin + i + this.Interval));
            }

            if (this.Scatterplot == null) //We need to generate the scatterplot only once
                this.Scatterplot = new Scatterplot(this.DataSet, this.XIntervals, this.YIntervals);

            PointsToDraw = Scatterplot.ComputeScatterPlot(this);
            this.PointSize = this.Area.Width / (this.XIntervals.Count * 20);
            int index = 0; //This is used only to iterate the DataSet for drawing strings

            foreach (Point ViewportPoint in this.PointsToDraw)
            {
                /* This allows me to draw the points using the point as Center and not as start corner */
                float x = ViewportPoint.X - this.PointSize;
                float y = ViewportPoint.Y - this.PointSize;
                float width = 2 * this.PointSize;
                float height = 2 * this.PointSize;

                this.G.DrawEllipse(this.BorderColor, x, y, width, height);
                this.G.FillEllipse(Brushes.Blue, x, y, width, height);
                this.G.DrawString(this.DataSet.ElementAt(index).X.ToString() + "," + this.DataSet.ElementAt(index).Y.ToString(), this.Font, Brushes.Red, new PointF(ViewportPoint.X,ViewportPoint.Y));
                index++;
            }
        }
        public void DrawRegressionLines(int Variable)
        {

        }
        public void DrawPaths()
        {
            if(Form1.NumberOfPaths != this.RandomPaths.Count)
            {
                this.RandomPaths.Clear();
                for (int i = 0; i < Form1.NumberOfPaths; i++)
                    this.RandomPaths.Add(new RandomPath());
            }

            if(Form1.FrequencyMode != 3 && this.RandomPaths.Count != 0)
                foreach(RandomPath path in this.RandomPaths)
                    path.ComputePath();
            

            this.Sequence = new Sequence(this.RandomPaths);

            foreach(RandomPath path in Sequence.AllPaths)
                G.DrawLines(new Pen(path.PathColor), Sequence.ComputePointsToDraw(this, path).ToArray());
        }
        #endregion

        #region Viewport Move/Resize Region
        public void MoveArea(int DeltaX, int DeltaY)
        {
            //Let's avoid null pointer exceptions
            if (this.AreaOnMouseDown == null) return;
            this.Area = new Rectangle(this.AreaOnMouseDown.X + DeltaX, this.AreaOnMouseDown.Y + DeltaY, this.AreaOnMouseDown.Width, this.AreaOnMouseDown.Height);

            //Let's redraw everything after moving the viewport
            this.RedrawAfterMoveOrResize();
        }

        public void ResizeArea(int DeltaX, int DeltaY)
        {
            if (this.AreaOnMouseDown == null) return;
            this.Area = new Rectangle(this.AreaOnMouseDown.X, this.AreaOnMouseDown.Y, this.AreaOnMouseDown.Width + DeltaX, this.AreaOnMouseDown.Height + DeltaY);

            this.RedrawAfterMoveOrResize();
        }
        #endregion

        #region Create new data
        public void ResetData()
        {
            this.NumberOfStudents = RandomGenerator.Random.Next(20, 100);
            
            this.DataSet.Clear();
            this.Scatterplot = null;
            
            this.XIntervals.Clear();
            this.YIntervals.Clear();

            this.RandomPaths.Clear();
        }
        #endregion
    }
}
