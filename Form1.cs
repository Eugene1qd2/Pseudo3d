using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Figures;

namespace WinFormsApp1
{


    public partial class Form1 : Form
    {
        List<Figure> figure_field;
        bool[] Keys_;
        const int WIDTH = 20, HEIGHT = 20;
        const int MAP_WIDTH = 200;
        int[,] field = new int[WIDTH, HEIGHT];
        Vector2Float player;
        float angle = (float)Math.PI/2;
        float direction = 0f;
        int RAYS_AMOUNT = 600;
        const float ANGLE_HEIGHT = 0.9f;
        public Form1()
        {
            figure_field = new List<Figure>();
            figure_field.Add(new Figure(new List<PointF>() { new PointF(5f, 5f) }, FigureType.Round));
            RAYS_AMOUNT = Width / 3;
            Keys_ = new bool[6];
            Random rand = new Random((int)DateTime.Now.Ticks);
            InitializeComponent();
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (i == WIDTH - 1 || i == 0 || j == HEIGHT - 1 || j == 0)
                    {
                        field[i, j] = 1;
                    }
                    else
                    {
                        field[i, j] = rand.Next(0, 2);
                    }
                }
            }
            field[5, 5] = 0;
            field[4, 5] = 0;
            field[4, 4] = 0;
            field[5, 4] = 0;
            pictureBox1.Size = new Size(MAP_WIDTH + 2, MAP_WIDTH + 2);
            player = new Vector2Float(5f, 5f);
        }
        public double CountRay(double ray_angle)
        {
            double c = 0;
            for (; c < 100; c += .01)
            {
                double x = player.x + c * Math.Cos(ray_angle);
                double y = player.y + c * Math.Sin(ray_angle);
                if (field[(int)(x), (int)(y)] != 0) break;
            }
            return c;
        }
        public double CountRayRound(double ray_angle,double X,double Y)
        {
            double c = 0;
            for (; c < 100; c += .01)
            {
                double x = player.x + c * Math.Cos(ray_angle);
                double y = player.y + c * Math.Sin(ray_angle);
                if (Math.Pow(x-X,2) + Math.Pow(y - Y, 2) <= 1) break;
            }
            return c;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool ShouldRefresh = false;
            if (Keys_[0])
            {
                direction += 0.05f;
                if (direction >= (float)Math.PI * 2)
                {
                    direction = direction - (float)Math.PI * 2;
                }
                ShouldRefresh = true;
            }
            if (Keys_[1])
            {
                direction -= 0.05f;
                if (direction <= 0)
                {
                    direction = direction + (float)Math.PI * 2;
                }
                ShouldRefresh = true;
            }
            if (Keys_[2])
            {
                if (CountRay(direction) - 0.05 > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction);
                    player.y += 0.05 * Math.Sin(direction);
                    ShouldRefresh = true;
                }
            }
            if (Keys_[3])
            {
                if (CountRay(Math.PI + direction) - 0.05 > 0.1)
                {
                    player.x -= 0.05 * Math.Cos(direction);
                    player.y -= 0.05 * Math.Sin(direction);
                    ShouldRefresh = true;
                }
            }
            if (Keys_[4])
            {
                if (CountRay(direction - Math.PI / 2) > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction - Math.PI / 2);
                    player.y += 0.05 * Math.Sin(direction - Math.PI / 2);
                    ShouldRefresh = true;
                }
            }
            if (Keys_[5])
            {
                if (CountRay(direction + Math.PI / 2) > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction + Math.PI / 2);
                    player.y += 0.05 * Math.Sin(direction + Math.PI / 2);
                    ShouldRefresh = true;
                }
            }
            if (ShouldRefresh)
            {
                pictureBox1.Refresh();
                Refresh();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.LightBlue, new Rectangle(0, 0, Width, Height / 2));
            //e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(0, Height / 2, Width, Height / 2));
            e.Graphics.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(0, Height / 2), Color.FromArgb(0, 255, 250), Color.LightBlue), new Rectangle(0, 0, Width, Height / 2));
            e.Graphics.FillRectangle(new LinearGradientBrush(new Point(0, Height / 2), new Point(0, 0), Color.FromArgb(0, 0, 150), Color.Black), new Rectangle(0, Height / 2, Width, Height / 2));
            int ray_index = 0;
            for (double i = direction - angle / 2; i <= direction + angle / 2; i += angle / (float)RAYS_AMOUNT)
            {
                double len = CountRay(i);

                //for (int j = 0; j < figure_field.Count; j++)
                //{
                //    switch (figure_field[j].type)
                //    {
                //        case FigureType.Round:
                //            double nlen = CountRayRound(i, figure_field[j].points[0].X, figure_field[j].points[0].Y);
                //            if (nlen < len)
                //                len = nlen;

                //            break;
                //        case FigureType.Closed:
                //            break;
                //        case FigureType.Opened:
                //            break;
                //        default:
                //            break;
                //    }
                //}
                double x = player.x + len * Math.Cos(i);
                double y = player.y + len * Math.Sin(i);
                double height = (int)((1 / len) * 1000);
                //e.Graphics.FillRectangle(Brushes.Green, new Rectangle((int)(ray_index * (Width / RAYS_AMOUNT)), (int)(Height / 2 - height / 2), (int)(Width / RAYS_AMOUNT), (int)(height)));
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, (int)((235 / (len*0.3)) > 235 ? 235 : (235 / (len*0.3))), 0)), new Rectangle((int)(ray_index * (Width / RAYS_AMOUNT)), (int)(Height / 2 - height / 2), (int)(Width / RAYS_AMOUNT), (int)(height)));

                ray_index++;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, MAP_WIDTH + 2, MAP_WIDTH + 2));
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {

                    if (field[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(i * (MAP_WIDTH / WIDTH) + 1, j * (MAP_WIDTH / HEIGHT) + 1, (MAP_WIDTH / WIDTH), MAP_WIDTH / HEIGHT));
                    }

                }
            }
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle((int)(player.x * (MAP_WIDTH / WIDTH) + 1 - (MAP_WIDTH / WIDTH) / 3), (int)(player.y * (MAP_WIDTH / HEIGHT) + 1 - (MAP_WIDTH / HEIGHT) / 3), (int)((MAP_WIDTH / WIDTH) / 1.5f), (int)((MAP_WIDTH / HEIGHT) / 1.5f)));
            for (double i = direction - angle / 2; i < direction + angle / 2; i += angle / RAYS_AMOUNT)
            {

                double len = CountRay(i);

                double x = player.x + len * Math.Cos(i);
                double y = player.y + len * Math.Sin(i);
                e.Graphics.DrawLine(Pens.White, (int)(MAP_WIDTH / WIDTH * player.x), (int)(MAP_WIDTH / HEIGHT * player.y), (int)(MAP_WIDTH / WIDTH * x), (int)(MAP_WIDTH / HEIGHT * y));
            }
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                Keys_[0] = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                Keys_[1] = true;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {


                Keys_[2] = true;

            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {

                Keys_[3] = true;

            }
            if (e.KeyCode == Keys.A)
            {

                Keys_[4] = true;

            }
            if (e.KeyCode == Keys.D)
            {

                Keys_[5] = true;

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                Keys_[0] = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                Keys_[1] = false;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {

                Keys_[2] = false;

            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {

                Keys_[3] = false;

            }
            if (e.KeyCode == Keys.A)
            {

                Keys_[4] = false;

            }
            if (e.KeyCode == Keys.D)
            {

                Keys_[5] = false;

            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            RAYS_AMOUNT = Width / 3;
            Refresh();
        }
    }
    public class Vector2Float
    {
        public double x;
        public double y;

        public Vector2Float(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
