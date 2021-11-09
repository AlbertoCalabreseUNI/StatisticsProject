using StatisticsProject.Generators;
using StatisticsProject.GraphicsObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatisticsProject
{
    public partial class Form1 : Form
    {
        public List<Viewport> viewports = new List<Viewport>();
        public static int GraphTypeToDraw;

        //Random Paths Generator
        public static int NumberOfPaths = 100;
        public static int PathSize = 50;
        //public static int InstantToPlotInstogram = 50;
        public static double SuccessProbability = 50 / 100;
        public static int RelativeFrequencyMode = 0;
        
        public Form1()
        {
            InitializeComponent();

            //Prevents flickering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            DoubleBuffered = true;
            this.UpdateStyles();


            //Let's initialize our Viewport with an arbitrary size. I've chosen a rectangle that starts from the picturebox corner and is half of its size
            this.viewports.Add(new Viewport(this.pictureBox1, new Rectangle(this.pictureBox1.Location.X, this.pictureBox1.Location.Y, this.pictureBox1.Width / 2, this.pictureBox1.Height / 2), pictureBox1.CreateGraphics()));

            //Creating functions to move/resize viewports
            this.pictureBox1.MouseDown += this.PictureBox1_MouseDown;
            this.pictureBox1.MouseUp += this.PictureBox1_MouseUp;
            this.pictureBox1.MouseMove += this.PictureBox1_MouseMove;

            GraphTypeToDraw = this.comboBox1.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Viewport viewport in this.viewports)
                viewport.DrawViewport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Viewport viewport in this.viewports)
                viewport.RedrawAfterMoveOrResize();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Viewport viewport in this.viewports)
            {
                viewport.ResetData();
                viewport.RedrawAfterMoveOrResize();
            }  
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e) {  GraphTypeToDraw = this.comboBox1.SelectedIndex; }

        #region Viewport Controller
        //We handle picturebox mouse in Form1 as it makes more sense since it's an instanced object inside the Form1 instance.
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Viewport vp in this.viewports)
            {
                if (vp.Area.Contains(e.Location))
                {
                    vp.AreaOnMouseDown = vp.Area; //We temporary save the original area
                    vp.MouseClickLocation = e.Location;

                    if (e.Button == MouseButtons.Left) vp.dragMode = true;
                    else if (e.Button == MouseButtons.Right) vp.resizeMode = true;
                }
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Viewport vp in this.viewports)
            {
                vp.dragMode = false;
                vp.resizeMode = false;
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Viewport vp in this.viewports)
            {
                //We calculate how much to move the viewport
                int deltaX = e.X - vp.MouseClickLocation.X;
                int deltaY = e.Y - vp.MouseClickLocation.Y;

                if (vp.dragMode)
                    vp.MoveArea(deltaX, deltaY);
                else if (vp.resizeMode)
                    vp.ResizeArea(deltaX, deltaY);
            }
        }
        #endregion
    }
}
