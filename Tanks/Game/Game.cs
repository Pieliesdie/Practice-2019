using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Game
{

    public class Game
    {
        public delegate void AfterGameOver();
        public event AfterGameOver GameOver;

        public static readonly Bitmap background = new Bitmap(Properties.Resources.grass_3);
        public static readonly Bitmap enemySprite = new Bitmap(Properties.Resources.Enemy);
        public static readonly Bitmap playerSprite = new Bitmap(Properties.Resources.player);
        public static readonly Bitmap bulletSprite = new Bitmap(Properties.Resources.bullet);

        public IEnumerable<GameObj> Objects => enemies.Concat(bullets).Concat(user.bullets).Append(user);

        private Size userSize = new Size(60, 40);
        private Size enemySize = new Size(60, 40);
        private Random rand = new Random();
        private readonly int countOfTanks;
        private readonly int countOfApples;
        public readonly int speed;
        private Size size;


        public User user;
        public List<Enemy> enemies = new List<Enemy>();
        public List<GameObj> walls = new List<GameObj>();
        public List<GameObj> bullets = new List<GameObj>();

        public Game(int countOfTanks, int countOfApples, int speed, Size size)
        {
            this.countOfTanks = countOfTanks;
            this.countOfApples = countOfApples;
            this.speed = speed;
            this.size = size;

            for (int i = 0; i < countOfTanks; i++)
            {
                enemies.Add(new Enemy(new Point(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), speed, enemySize, enemySprite));
            }

            user = new User(new Point(size.Width / 2, size.Height / 2), userSize,this.speed+5,playerSprite);
        }

        public void Update()
        {
            CheckEnenymiesCollisions();
            CheckBulletsCollisions();

            foreach (Enemy i in enemies)
            {
                i.Update();

                if (rand.Next(0, 100) < 2) { bullets.Add(i.Fire()); }
            }

            foreach(GameObj i in bullets.Concat(user.bullets))
            {
                i.Update();
            }

        }

        void CheckEnenymiesCollisions()//Можно ли без 5 циклов ? хм ???
        {
            List<Enemy> collisions = new List<Enemy>();
            List<Enemy> shooted = new List<Enemy>();
            List<GameObj> splicebullet = new List<GameObj>();

            foreach (Enemy i in enemies)
            {
                if (CheckBounds(i))
                {
                    i.Reverse();//переделать
                }
                if (user.HitBox.IntersectsWith(i.HitBox))
                {
                    GameOver?.Invoke();
                    break;
                }

                foreach (GameObj j in user.bullets)
                {
                    if (j.HitBox.IntersectsWith(i.HitBox))
                    {
                        splicebullet.Add(j);
                        shooted.Add(i);
                    }
                }

                foreach (Enemy k in enemies)
                {
                    if (i.HitBox.IntersectsWith(k.HitBox) && i != k)
                    {
                        collisions.Add(i);
                        collisions.Add(k);
                    }
                }
            }
            collisions.Distinct().ToList().ForEach(x => x.Reverse());
            shooted.ForEach(x => enemies.Remove(x));
            splicebullet.ForEach(x=>user.bullets.Remove(x));
        }

        void CheckBulletsCollisions()//???
        {
            List<GameObj> outofrange = new List<GameObj>();

            foreach(GameObj i in bullets)
            {
                if (i.HitBox.IntersectsWith(user.HitBox))
                {
                    GameOver?.Invoke();
                    break;
                }
                if (CheckBounds(i))
                {
                    outofrange.Add(i);
                }
            }
            foreach (GameObj i in user.bullets)
            {
                if (CheckBounds(i))
                {
                    outofrange.Add(i);
                }
            }
            outofrange.ForEach(x => bullets.Remove(x));
            outofrange.ForEach(x=> user.bullets.Remove(x));
        }

        bool CheckBounds(GameObj obj)
        {
            return obj.pos.X < 0 || obj.pos.Y < 0 || obj.pos.X > size.Width - obj.Size.Width || obj.pos.Y > size.Height - obj.Size.Height;
        }

    }
}
