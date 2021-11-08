using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StatisticsProject.DataObjects;
using StatisticsProject.Generators;

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

        //This is a List used in common by all types of Histograms
        List<Rectangle> RectanglesToDraw = new List<Rectangle>();

        //List of DataSets for each Chart Type
        #region DEFAULT HISTOGRAMS */
        //Let's create a DataSet of Random Elements and then Compute an Histogram
        public int MinWeight = 50;
        public int MaxWeight = 100;
        public int MinHeight = 150;
        public int MaxHeight = 190;
        public int NumberOfStudents;

        public Histogram StudentHistogram;
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
             *  ScatterPlot = 3
             *  ... = 4
             */

            switch (Form1.GraphTypeToDraw)
            {
                case 0:
                    //Let's avoid repopulating all the Lists everytime we move the Viewport
                    if(this.XIntervals.Count == 0)
                    {
                        this.XIntervals = new List<Interval>();
                        for (int i = 0; i < this.MaxWeight - this.MinWeight; i += this.Interval)
                            XIntervals.Add(new Interval(this.MinWeight + i, this.MinWeight + i + this.Interval));
                    }
                        
                    if(this.YIntervals.Count == 0)
                    {
                        //new Interval(50 + 0, 50 + 0 + 10)
                        for (int i = 0; i < this.MaxHeight - this.MinHeight; i += this.Interval)
                            YIntervals.Add(new Interval(this.MinHeight + i, this.MinHeight + i + this.Interval));
                    }

                    if(this.DataSet.Count == 0) //If we didn't generate the DataSet yet, then do it and generate the histogram
                    {
                        this.DataSet = RandomGenerator.GenerateRandomStudentsList(this.NumberOfStudents, this.MinWeight, this.MaxWeight, this.MinHeight, this.MaxHeight);
                        this.StudentHistogram = new Histogram(this.DataSet, this.XIntervals, this.YIntervals);
                    }

                    this.RectanglesToDraw = this.StudentHistogram.ComputeDefaultHistogram(this, 0/*0 = X Variable, 1 = Y Variable*/);

                    foreach (Rectangle Rectangle in this.RectanglesToDraw)
                    {
                        this.G.DrawRectangles(this.BorderColor, this.RectanglesToDraw.ToArray());
                        this.G.FillRectangles(Brushes.Red, this.RectanglesToDraw.ToArray());
                    }
                    break;
                case 1:
                    //this.DrawLines();
                    break;
                case 2:     //Paths
                    break;
                default:
                    break;
            }
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
    }
}
