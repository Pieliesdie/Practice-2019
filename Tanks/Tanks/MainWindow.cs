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

        public MainWindow()
        {
            InitializeComponent();
            battlefield = pictureBox1.CreateGraphics();
            game = new Game.Game(5, 5, 5, new Size(pictureBox1.Width, pictureBox1.Height));
            controller = new UserController(game, this);
            game.GameOver += GameOver;
            this.DoubleBuffered = true;                
            pictureBox1.BackgroundImage = Properties.Resources.bg;
        }


        void GameOver()
        {
            timer1.Enabled = false;
            MessageBox.Show("GameOver");
            game = new Game.Game(5, 5, 5, new Size(pictureBox1.Width, pictureBox1.Height));
            controller = new UserController(game, this);
            timer1.Enabled = true;
        }

        void DrawObject(GameObj i)
        {
            Pen pen = new Pen(Color.Red)
            {
                Width = 5
            };
            battlefield.DrawRectangle(pen, i.pos.X, i.pos.Y, i.size.Width, i.size.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            battlefield = Graphics.FromImage(bitmap);
            foreach (GameObj i in game.Objects)
            {
                DrawObject(i);
            }
            game.Update();
            pictureBox1.Image = bitmap;
            OnUpdate?.Invoke();
        }

        private void objectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ObjectsForm(this).Show();
        }
    }
}
