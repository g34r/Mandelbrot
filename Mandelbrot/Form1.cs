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
using System.Diagnostics;

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

            e.Graphics.FillRectangle(brush, pos.X, pos.Y, 1, 1);
        }

        private uint? GetMandelbrotDivergePoint(Complex c, uint n)
        {
            Complex z = c;
            
            for(uint i=0;i<n;i++)
            {
                z = z * z + c;
                if (Complex.Abs(z) > 2)
                {
                    return i;
                }
            }

            return null;
        }

        int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
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
                        DrawComplexPoint(new Complex((x - rect.Width / 2), (y - rect.Height / 2)), e, rect, Brushes.White);
                    }
                    else
                    {
                        int nsmooth = IntPow(Convert.ToInt32(result), 2) % 255;
                        Brush brush = new SolidBrush(Color.FromArgb(255, nsmooth, nsmooth, nsmooth));

                        DrawComplexPoint(new Complex((x - rect.Width / 2), (y - rect.Height / 2)), e, rect, brush);
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
                XMove -= 50;
            }
            else if(e.KeyCode == Keys.Right)
            {
                XMove += 50;
            }
            else if(e.KeyCode == Keys.Up)
            {
                YMove += 50;
            }
            else if(e.KeyCode == Keys.Down)
            {
                YMove -= 50;
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
