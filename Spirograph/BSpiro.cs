using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafik
{
    class BSpiro
    {
        private double R = 0, r = 0, d = 0;
        private double maxAngle;
        private int cx = 0, cy = 0;
        private PointF? prev = null;
        private double angle = 0.0;
        private Color pencolor = Color.White;
        private readonly FMain fmain;

        public BSpiro(FMain mainf)
        {
            fmain = mainf;
            RandomizeParameters();
            R = 200;
            r = 100;
            d = 50;
            maxAngle = (2 * Math.PI * r) / GCD(R, r);
            //RandomizeParameters();
        }
        private void RandomizeParameters()
        {
            Random rand = new Random();
            R = rand.Next(20, 100);
            r = rand.Next((int)-R, (int)R);
            d = rand.Next((int)-Math.Abs(r * 2), (int)Math.Abs(r * 2));
            pencolor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            angle = 0;
            // RANDOM POSITION
            //cx = rand.Next(64,ClientSize.Width-64);
            //cy = rand.Next(64,ClientSize.Height-64);
            // CENTER
            cx = fmain.ClientSize.Width / 2;
            cy = fmain.ClientSize.Height / 2;

        }
        private double GCD(double a, double b)
        {
            return b == 0 ? Math.Ceiling(a) : GCD(Math.Ceiling(b), a % b);
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
        double Rinc = 0.01;
        double rinc = -0.01;
        double dinc = 0.02;
        private void Update()
        {
            prev = null;
            R -= Rinc;
            if (R < 50 || R > 200) Rinc = -Rinc;
            r += rinc;
            if (r < 10 || r > R) rinc = -rinc;
            d += dinc;
            if (d < -r || d > r * 2) dinc = -dinc;
            maxAngle = (2 * Math.PI * r) / GCD(R, r);
            //maxAngle = (2 * Math.PI * r) / GCD(R, r);
        }
        struct Warna
        {
            float r, g, b;
            float rinc, ginc, binc;
            Random rand = new Random();
            public Warna()
            {
                r = g = b = 127f;
                rinc = rand.Next(-16, 16) / 10f;
                ginc = rand.Next(-16, 16) / 10f;
                binc = rand.Next(-16, 16) / 10f;
            }
            public Warna(int r, int g, int b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                rinc = rand.Next(-16, 16) / 10f;
                ginc = rand.Next(-16, 16) / 10f;
                binc = rand.Next(-16, 16) / 10f;
            }
            public Color GetColor()
            {
                return Color.FromArgb((int)r, (int)g, (int)b);
            }
            public void RandomizeStep()
            {
                rinc = rand.Next(-16, 16) / 10f;
                ginc = rand.Next(-16, 16) / 10f;
                binc = rand.Next(-16, 16) / 10f;
            }
            public void Update()
            {
                r += rinc;
                if (r < 0 || r > 255) rinc = -rinc;
                g += ginc;
                if (g < 0 || g > 255) ginc = -ginc;
                b += binc;
                if (b < 0 || b > 255) binc = -binc;
                r = Math.Max(0, Math.Min(255, r));
                g = Math.Max(0, Math.Min(255, g));
                b = Math.Max(0, Math.Min(255, b));
            }
        }

        Warna warna = new Warna();
        int ccc = 0;
        public void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Black);
            //g.DrawString("R: " + R + " r: " + r + " d: " + d + " maxAngle: " + maxAngle, new Font("Arial", 12), Brushes.White, 10, 10);
            double dt = 0.01;
            double f = maxAngle / 6.28;


            int n = (int)(f / dt) / 2;
            prev = null;
            angle += 0.05;
            //angle = 0;
            int cc = 0;
            for (double ang = 0; ang < maxAngle; ang += dt)
            {
                Pen pen = new Pen(warna.GetColor(), 1.5f);
                double a = ang + angle;
                double x = cx + (R - r) * Math.Cos(a) + d * Math.Cos(((R - r) / (double)r) * ang);
                double y = cy + (R - r) * Math.Sin(a) - d * Math.Sin(((R - r) / (double)r) * ang);
                if (prev != null) g.DrawLine(pen, prev.Value, new PointF((float)x, (float)y));
                prev = new PointF((float)x, (float)y);
                //Increment(dt);
                cc++;
                ccc++;
                if (cc > 250)
                {
                    warna.Update();
                    cc = 0;
                }
                if (ccc > 10000)
                {
                    warna.RandomizeStep();
                    ccc = 0;
                }
            }
            Update();
            //g.FillRectangle(Brushes.Black, 0, 0, fmain.ClientSize.Width, fmain.ClientSize.Height);
        }

    }
}
