using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafik
{
    class Spiro
    {
        private int R = 0, r = 0, d = 0;
        private double maxAngle;
        private int cx = 0, cy = 0;
        private PointF? prev = null;
        private double angle = 0.0;
        private Color pencolor = Color.White;
        private readonly FMain fmain;

        public Spiro(FMain mainf)
        {
            fmain = mainf;
            RandomizeParameters();
        }
        private void RandomizeParameters()
        {
            Random rand = new Random();
            R = rand.Next(20, 100);
            r = rand.Next(-R, R);
            d = rand.Next(-Math.Abs(r * 2), Math.Abs(r * 2));
            pencolor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            // RANDOM POSITION
            //cx = rand.Next(64,ClientSize.Width-64);
            //cy = rand.Next(64,ClientSize.Height-64);
            // CENTER
            cx = fmain.ClientSize.Width / 2;
            cy = fmain.ClientSize.Height / 2;

        }
        private int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private bool Increment(double dt)
        {
            angle += dt;
            if (angle >= maxAngle * 1.5)
            {
                RandomizeParameters();
                angle = 0;
                prev = null;
                maxAngle = (2 * Math.PI * r) / GCD(R, r);
                return false;
            }
            return true;
        }

        public void Draw(Graphics g)
        {
            if (prev == null)
            {
                //g.Clear(Color.FromArgb(10,0,0,0));
                Brush br = new SolidBrush(Color.FromArgb(16, 0, 0, 0));
                g.FillRectangle(br, 0, 0, fmain.ClientSize.Width, fmain.ClientSize.Height);
            }
            g.SmoothingMode = SmoothingMode.AntiAlias;
            double dt = 0.01;
            double f = maxAngle / 6.28;
            int n = (int)(f / dt) / 2;
            if (angle <= maxAngle)
            {

                for (int i = 0; i <= n; i++)
                {
                    Pen pen = new Pen(pencolor, 1.5f);
                    double x = cx + (R - r) * Math.Cos(angle) + d * Math.Cos(((R - r) / (double)r) * angle);
                    double y = cy + (R - r) * Math.Sin(angle) - d * Math.Sin(((R - r) / (double)r) * angle);
                    if (prev != null) g.DrawLine(pen, prev.Value, new PointF((float)x, (float)y));
                    prev = new PointF((float)x, (float)y);
                    if (!Increment(dt)) break;
                }
            }
            else Increment(dt * n);
        }

    }
}