namespace bitmap_zoom_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Zoomer zoomer;
        Brush b = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        private void Form1_Load(object sender, EventArgs e)
        {
            //FormBorderStyle = FormBorderStyle.None;
            //Bounds = Screen.PrimaryScreen.Bounds;
            zoomer = new Zoomer();
            WindowState = FormWindowState.Maximized;
            LoadImage();
            DoubleBuffered = true;
            MouseWheel += Form1_MouseWheel;
            //FormBorderStyle = FormBorderStyle.None;
        }
        private void Form1_MouseWheel(object? sender, MouseEventArgs e)
        {
            zoomer.TargetScale -= e.Delta / 15f;
            zoomer.Zoom(e.Location);
        }
        private void LoadImage()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Open Image";
                ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;.PNG|All files (*.*)|*.*";
                Bitmap temp = new Bitmap(1, 1);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    temp = (Bitmap)Image.FromFile(ofd.FileName);
                }
                else if (File.Exists("test.bmp"))
                {
                    temp = (Bitmap)Image.FromFile("test.bmp");
                }
                else
                {
                    MessageBox.Show("No image selected");
                }
                zoomer.ZoomBounds = new Rectangle(temp.Width, 0, 500, 500);
                zoomer.Bmp = temp;
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            zoomer.Zoom(e.Location);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //e.Graphics.DrawImageUnscaled(scaled, 0, 0);
            //e.Graphics.DrawImage(scaled, new Rectangle(zoomBounds.Width, 0, 500, 500));

            //not stretched
            //e.Graphics.DrawImage(bmp, new Rectangle(bmp.Width, 0, zoomRect.Width, zoomRect.Height), zoomRect, GraphicsUnit.Pixel);
            //
            //stretched

            zoomer.DrawZoomedRegion(e.Graphics);
            //
            e.Graphics.DrawImageUnscaledAndClipped(zoomer.Bmp, new Rectangle(0, 0, zoomer.Bmp.Width, zoomer.Bmp.Height));
            e.Graphics.DrawRectangle(Pens.Black, zoomer.ZoomRect);
            e.Graphics.FillRectangle(b, zoomer.ZoomRect.X, zoomer.ZoomRect.Y, zoomer.ZoomRect.Width-1, zoomer.ZoomRect.Height-1);

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zoomer.TargetScale /= 2;
                zoomer.Zoom(e.Location);
            }
            else if (e.Button == MouseButtons.Right)
            {
                zoomer.TargetScale *= 2;
                zoomer.Zoom(e.Location);
            }
        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.L)
            {
                LoadImage();
            }
        }
    }
}