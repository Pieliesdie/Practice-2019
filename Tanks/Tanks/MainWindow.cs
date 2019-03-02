using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game;
using Controller;

namespace Tanks
{
    public partial class MainWindow : Form
    {
        public delegate void update();
        public event update OnUpdate;

        UserController controller;
        public Game.Game game;
        Graphics battlefield;
        int gameSpeed = 100;

        void Init()
        {
            game = new Game.Game(5, 5, gameSpeed, new Size(pictureBox1.Width, pictureBox1.Height));
            controller = new UserController(game, this);
            game.OnGameOver += GameOver;
            pictureBox1.BackgroundImage = Game.Game.background;
        }

        public MainWindow()
        {
            InitializeComponent();
            battlefield = pictureBox1.CreateGraphics();
            Init();
        }

        void GameOver()
        {
            timer1.Enabled = false;
            MessageBox.Show("GameOver");
            Init();
            timer1.Enabled = true;
        }

        void DrawObject(GameObj i)
        {
            if (i.Image != null)
            {
                battlefield.DrawImage(i.Image, i.pos.X, i.pos.Y, i.Size.Width, i.Size.Height);
            }
            else
            {
                Pen pen = new Pen(Color.Red)
                {
                    Width = 5
                };
                battlefield.DrawRectangle(pen, i.pos.X, i.pos.Y, i.Size.Width, i.Size.Height);
            }
        }

        Bitmap bitmap;
        DateTime lasttime = DateTime.Now;

        private void timer1_Tick(object sender, EventArgs e)
        {
            Score_label.Text = $"Score: {game.Score}";
            var now = DateTime.Now;
            var dt = (float)(now - lasttime).TotalSeconds;
            lasttime = now;

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            battlefield = Graphics.FromImage(bitmap);
            foreach (GameObj i in game.Objects)
            {
                DrawObject(i);
            }
            game.Update(dt);
            pictureBox1.Image = bitmap;
            OnUpdate?.Invoke();
            bitmap = null;
            GC.Collect();
        }

        private void objectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ObjectsForm(this).Show();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameOver();
        }
    }
}
