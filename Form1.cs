using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{


    public partial class Form1 : Form
    {

        const int WIDTH = 20, HEIGHT = 20;
        const int MAP_WIDTH = 200;
        int[,] field = new int[WIDTH, HEIGHT];
        Graphics graphics;
        Graphics map;
        Vector2Float player;
        float angle = 1.4f;
        float direction = 0f;
        const int RAYS_AMOUNT = 200;
        const float ANGLE_HEIGHT = 0.9f;
        public Form1()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            InitializeComponent();
            graphics = CreateGraphics();
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
                        field[i, j] = rand.Next(0,2);
                    }
                }
            }
            field[4, 4] = 0;
            field[5, 4] = 0;
            field[4, 5] = 0;
            field[5, 5] = 0;
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
        public void DrawMap()
        {
            graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, MAP_WIDTH + 1, MAP_WIDTH + 1));
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {

                    if (field[i, j] == 1)
                    {
                        graphics.FillRectangle(Brushes.Gray, new Rectangle(i * (MAP_WIDTH / WIDTH), j * (MAP_WIDTH / HEIGHT), (MAP_WIDTH / WIDTH), MAP_WIDTH / HEIGHT));
                    }

                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightBlue, new Rectangle(0, 0, Width, Height / 2));
            e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, Height / 2, Width, Height / 2));
            int ray_index = 0;
            for (double i = direction - angle / 2; i <= direction + angle / 2; i += angle / (float)RAYS_AMOUNT)
            {
                double len = CountRay(i);
                double x = player.x + len * Math.Cos(i);
                double y = player.y + len * Math.Sin(i);
                int height = (int)((1 / len) * 1000);
                //i<0?Math.PI*2:i / (angle / RAYS_AMOUNT) 
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle((int)(ray_index * ((float)Width / RAYS_AMOUNT)), (int)(Height / 2 - height / 2), (int)(Width / RAYS_AMOUNT), (int)(height)));
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
                direction += 0.05f;
                pictureBox1.Refresh();
                if (direction >= (float)Math.PI * 2)
                {
                    direction = direction - (float)Math.PI * 2;
                }

                Refresh();
            }
            if (e.KeyCode == Keys.Left)
            {
                direction -= 0.05f;
                if (direction <= 0)
                {
                    direction = direction + (float)Math.PI * 2;
                }
                pictureBox1.Refresh();
                Refresh();
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                if (CountRay(direction) - 0.05 > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction);
                    player.y += 0.05 * Math.Sin(direction);
                    pictureBox1.Refresh();
                    Refresh();
                }
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                if (CountRay(Math.PI + direction) - 0.05 > 0.1)
                {
                    player.x -= 0.05 * Math.Cos(direction);
                    player.y -= 0.05 * Math.Sin(direction);
                    pictureBox1.Refresh();
                    Refresh();
                }
            }
            if (e.KeyCode == Keys.A)
            {
                if (CountRay(direction - Math.PI/2) > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction - Math.PI / 2);
                    player.y += 0.05 * Math.Sin(direction - Math.PI / 2);
                    pictureBox1.Refresh();
                    Refresh();
                }
            }
            if (e.KeyCode == Keys.D)
            {
                if (CountRay(direction + Math.PI/2) > 0.1)
                {
                    player.x += 0.05 * Math.Cos(direction + Math.PI / 2);
                    player.y += 0.05 * Math.Sin(direction + Math.PI / 2);
                    pictureBox1.Refresh();
                    Refresh();
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            graphics = CreateGraphics();
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
