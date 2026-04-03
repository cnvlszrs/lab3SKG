using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Form1 : Form
    {
        private const int default_a = 120;
        private const double angle30 = Math.PI / 6;

        private int dir = 1;
        private double dx = 1.5;
        private double xc, yc;
        private int a;
        private int v = 1;
        private double fi;
        private double c_fi = 0;
        private int rotation_rate = 1;

        private Point[] p1 = new Point[6];
        private Point[] p2 = new Point[3];
        private double[] x = new double[6];
        private double[] y = new double[6];

        // Компоненты
        private PictureBox pictureBox1;
        private Timer timer1;
        private TrackBar trackBar1, trackBar2, trackBar3;
        private Button button1, button2, button3, button4;
        private Label label1, label2, label3, label4, label5;

        public Form1()
        {
            InitializeComponents();
            SetupDefaults();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(800, 700);
            this.Text = "Движение фигуры";
            this.StartPosition = FormStartPosition.CenterScreen;

            // PictureBox
            pictureBox1 = new PictureBox();
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Size = new Size(760, 400);
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.BackColor = SystemColors.Control;

            // Timer
            timer1 = new Timer();
            timer1.Interval = 30;
            timer1.Tick += Timer1_Tick;

            // TrackBar1 - скорость вращения
            trackBar1 = new TrackBar();
            trackBar1.Location = new Point(140, 475);
            trackBar1.Size = new Size(250, 45);
            trackBar1.Minimum = 1;
            trackBar1.Maximum = 10;
            trackBar1.Value = 1;
            trackBar1.Scroll += TrackBar1_Scroll;

            // TrackBar2 - размер
            trackBar2 = new TrackBar();
            trackBar2.Location = new Point(140, 515);
            trackBar2.Size = new Size(250, 45);
            trackBar2.Minimum = 40;
            trackBar2.Maximum = 200;
            trackBar2.Value = 120;
            trackBar2.Scroll += TrackBar2_Scroll;

            // TrackBar3 - скорость движения
            trackBar3 = new TrackBar();
            trackBar3.Location = new Point(140, 555);
            trackBar3.Size = new Size(250, 45);
            trackBar3.Minimum = 1;
            trackBar3.Maximum = 20;
            trackBar3.Value = 1;
            trackBar3.Scroll += TrackBar3_Scroll;

            // Buttons
            button1 = new Button();
            button1.Location = new Point(12, 430);
            button1.Size = new Size(100, 35);
            button1.Text = "Нарисовать";
            button1.Click += Button1_Click;

            button2 = new Button();
            button2.Location = new Point(120, 430);
            button2.Size = new Size(100, 35);
            button2.Text = "Старт";
            button2.Click += Button2_Click;

            button3 = new Button();
            button3.Location = new Point(230, 430);
            button3.Size = new Size(100, 35);
            button3.Text = "Стоп";
            button3.Click += Button3_Click;

            button4 = new Button();
            button4.Location = new Point(340, 430);
            button4.Size = new Size(150, 35);
            button4.Text = "Размер по умолчанию";
            button4.Click += Button4_Click;

            // Labels
            label1 = new Label();
            label1.Text = "Скорость вращения:";
            label1.Location = new Point(12, 480);
            label1.AutoSize = true;

            label2 = new Label();
            label2.Text = "Размер фигуры:";
            label2.Location = new Point(12, 520);
            label2.AutoSize = true;

            label3 = new Label();
            label3.Text = "Скорость движения:";
            label3.Location = new Point(12, 560);
            label3.AutoSize = true;

            label4 = new Label();
            label4.Text = "Координаты центра:";
            label4.Location = new Point(550, 430);
            label4.AutoSize = true;

            label5 = new Label();
            label5.Location = new Point(550, 455);
            label5.AutoSize = true;
            label5.Text = "X: 0, Y: 0";

            // Добавляем на форму
            this.Controls.Add(pictureBox1);
            this.Controls.Add(trackBar1);
            this.Controls.Add(trackBar2);
            this.Controls.Add(trackBar3);
            this.Controls.Add(button1);
            this.Controls.Add(button2);
            this.Controls.Add(button3);
            this.Controls.Add(button4);
            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(label4);
            this.Controls.Add(label5);
        }

        private void SetupDefaults()
        {
            a = default_a;
            xc = (a / 2.0) * Math.Cos(angle30);
            yc = pictureBox1.Height - a / 2.0;
            fi = 3 * dir * Math.PI / 180;

            trackBar1.Value = rotation_rate;
            trackBar2.Value = a;
            trackBar3.Value = v;

            DrawFigure();
        }

        private double[] RotateX(double[] x0, double[] y0)
        {
            double[] x1 = new double[x0.Length];
            for (int i = 0; i < x0.Length; i++)
                x1[i] = x0[i] * Math.Cos(c_fi) - y0[i] * Math.Sin(c_fi);
            return x1;
        }

        private double[] RotateY(double[] x0, double[] y0)
        {
            double[] y1 = new double[x0.Length];
            for (int i = 0; i < x0.Length; i++)
                y1[i] = y0[i] * Math.Cos(c_fi) + x0[i] * Math.Sin(c_fi);
            return y1;
        }

        private byte ColorIncrease()
        {
            try
            {
                double minX = (a / 2.0) * Math.Cos(angle30);
                double maxX = pictureBox1.Width - a * Math.Cos(angle30);
                if (maxX <= minX) return 0;
                double col = 255 * (xc - minX) / (maxX - minX);
                return (byte)Math.Max(0, Math.Min(255, col));
            }
            catch { return 0; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1288, 447);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        private byte ColorDecrease()
        {
            try
            {
                double minX = (a / 2.0) * Math.Cos(angle30);
                double maxX = pictureBox1.Width - a * Math.Cos(angle30);
                if (maxX <= minX) return 255;
                double col = -255 * (xc - minX) / (maxX - minX) + 255;
                return (byte)Math.Max(0, Math.Min(255, col));
            }
            catch { return 255; }
        }

        private void DrawFigure()
        {
            Graphics g = pictureBox1.CreateGraphics();

            // Шестиугольник: от жёлтого к синему
            Brush brushHex = new SolidBrush(Color.FromArgb(ColorDecrease(), ColorDecrease(), ColorIncrease()));
            // Треугольник: от зелёного к пурпурному
            Brush brushTri = new SolidBrush(Color.FromArgb(ColorIncrease(), ColorDecrease(), ColorIncrease()));

            g.Clear(pictureBox1.BackColor);

            // Координаты вершин относительно центра
            x[0] = 0; y[0] = -a / 2.0;
            x[1] = (a / 2.0) * Math.Cos(angle30); y[1] = -(a / 2.0) * Math.Sin(angle30);
            x[2] = (a / 2.0) * Math.Cos(angle30); y[2] = (a / 2.0) * Math.Sin(angle30);
            x[3] = 0; y[3] = a / 2.0;
            x[4] = -(a / 2.0) * Math.Cos(angle30); y[4] = (a / 2.0) * Math.Sin(angle30);
            x[5] = -(a / 2.0) * Math.Cos(angle30); y[5] = -(a / 2.0) * Math.Sin(angle30);

            double[] tempX = (double[])x.Clone();
            double[] rotatedX = RotateX(tempX, y);
            double[] rotatedY = RotateY(tempX, y);

            for (int i = 0; i < 6; i++)
                p1[i] = new Point((int)(rotatedX[i] + xc), (int)(rotatedY[i] + yc));

            p2[0] = new Point((int)(rotatedX[0] + xc), (int)(rotatedY[0] + yc));
            p2[1] = new Point((int)(rotatedX[2] + xc), (int)(rotatedY[2] + yc));
            p2[2] = new Point((int)(rotatedX[4] + xc), (int)(rotatedY[4] + yc));

            g.FillPolygon(brushHex, p1);
            g.FillPolygon(brushTri, p2);

            label5.Text = string.Format("X: {0:F2}, Y: {1:F2}", xc, yc);

            brushHex.Dispose();
            brushTri.Dispose();
            g.Dispose();
        }

        // ========== ОБРАБОТЧИКИ ==========

        private void Timer1_Tick(object sender, EventArgs e)
        {
            xc += dx * v * dir;

            double maxX = x.Max() + xc;
            if (maxX > pictureBox1.Width)
            {
                xc = pictureBox1.Width - x.Max();
                dir = -1;
                fi = 3 * dir * Math.PI / 180;
            }

            double minX = x.Min() + xc;
            if (minX < 0)
            {
                xc = -x.Min();
                dir = 1;
                fi = 3 * dir * Math.PI / 180;
            }

            c_fi += fi;
            DrawFigure();
        }

        private void Button1_Click(object sender, EventArgs e) => DrawFigure();
        private void Button2_Click(object sender, EventArgs e) => timer1.Start();

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            xc = (a / 2.0) * Math.Cos(angle30);
            yc = pictureBox1.Height - a / 2.0;
            c_fi = 0;
            dir = 1;
            fi = 3 * dir * Math.PI / 180;
            DrawFigure();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            a = default_a;
            trackBar2.Value = a;
            if (!timer1.Enabled)
            {
                xc = (a / 2.0) * Math.Cos(angle30);
                yc = pictureBox1.Height - a / 2.0;
                DrawFigure();
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            rotation_rate = trackBar1.Value;
            fi = rotation_rate * dir * Math.PI / 180;
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            a = trackBar2.Value;
            if (!timer1.Enabled)
            {
                xc = (a / 2.0) * Math.Cos(angle30);
                yc = pictureBox1.Height - a / 2.0;
                DrawFigure();
            }
        }

        private void TrackBar3_Scroll(object sender, EventArgs e) => v = trackBar3.Value;
    }
}