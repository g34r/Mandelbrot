using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Mandelbrot
{
    public partial class Mandelbrot : Form
    {
        private int XMove = 0, YMove = 0;
        private uint Zoom = 100;

        public Mandelbrot()
        {
            InitializeComponent();
            this.Width = 600;
            this.Height = 400;
        }

        private void DrawComplexPoint(Complex c, PaintEventArgs e, Rectangle rect, Brush brush)
        {
            Point pos = new Point(Convert.ToInt32(c.Real + rect.Width / 2), 
                                  rect.Height - Convert.ToInt32(c.Imaginary + rect.Height / 2));

            Graphics g = CreateGraphics();
            g.FillRectangle(brush, pos.X, pos.Y, 1, 1);
        }

        private uint? GetMandelbrotDivergePoint(Complex c, uint n)
        {
            Complex z = c;
            
            for(uint i=0;i<n;i++)
            {
                z = z * z + c;
                if (Complex.Abs(z) > 2)
                {
                    return n;
                }
            }

            return null;
        }

        private void DrawMandelbrot(Rectangle rect, PaintEventArgs e, uint n, int xMove, int yMove, uint zoom)
        {
            for (uint x = 0; x < rect.Width; x++)
            {
                for (uint y = 0; y < rect.Height; y++)
                {
                    uint? result = GetMandelbrotDivergePoint(new Complex((x - rect.Width / 2) + xMove, ((y - rect.Height / 2)) + yMove) / zoom, n);
                    if (!result.HasValue)
                    {
                        DrawComplexPoint(new Complex((x - rect.Width / 2), (y - rect.Height / 2)), e, rect, Brushes.Blue);
                    }
                }
            }
        }

        private void Mandelbrot_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            e.Graphics.DrawLine(Pens.Black, new Point(rect.Width / 2, 0), new Point(rect.Width / 2, rect.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(0, rect.Height / 2), new Point(rect.Width, rect.Height / 2));

            DrawMandelbrot(rect, e, 1000, XMove, YMove, Zoom);
        }

        private void Mandelbrot_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                XMove -= 10;
            }
            else if(e.KeyCode == Keys.Right)
            {
                XMove += 10;
            }
            else if(e.KeyCode == Keys.Up)
            {
                YMove += 10;
            }
            else if(e.KeyCode == Keys.Down)
            {
                YMove -= 10;
            }

            this.Invalidate();
        }

        private void Mandelbrot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zoom += 100;
            }
            else if(Zoom > 10)
            {
                Zoom -= 100;
            }

            this.Invalidate();
        }
    }
}
