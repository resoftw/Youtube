using System.Drawing.Drawing2D;

namespace Grafik
{
    public partial class FMain : Form
    {

        private System.Windows.Forms.Timer timer;
        private BufferedGraphicsContext context;
        private BufferedGraphics buffer;

        private Spiro spiro;

        private int R = 0, r = 0, d = 0;
        private double maxAngle;
        private int cx=0, cy=0;
        public FMain()
        {
            this.ClientSize = new Size(540, 960);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            context = BufferedGraphicsManager.Current;
            buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);
            spiro= new Spiro(this);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1; 
            timer.Tick += (s, e) =>
            {
                this.Invalidate();
            };

            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrafik(buffer.Graphics);
            buffer.Render(e.Graphics);
        }

        private void DrawGrafik(Graphics g)
        {
            spiro.Draw(g);
        }

   
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (context != null)
            {
                buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);
            }
        }

    }
}
