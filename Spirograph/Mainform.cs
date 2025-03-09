using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Grafik
{
    public partial class FMain : Form
    {

        private System.Windows.Forms.Timer timer;

        private ASpiro spiro;
        private Bitmap bitmap;
        private Graphics bufferGraphics;
        private int frmnum = 0;
        public FMain()
        {
            this.ClientSize = new Size(540, 960);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            bufferGraphics = Graphics.FromImage(bitmap);
            spiro = new(this);
            System.IO.Directory.CreateDirectory("./Output");

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1; 
            timer.Tick += (s, e) =>
            {
                this.Invalidate();
                //bitmap.Save("./Output/Spiro_" + frmnum.ToString("00000") + ".jpg",ImageFormat.Jpeg);
                frmnum++;
            };

            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrafik(bufferGraphics);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
 
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            bufferGraphics = Graphics.FromImage(bitmap);
        }
        private void DrawGrafik(Graphics g)
        {
            spiro.Draw(g);
        }

    }
}
