using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Game
{

    public class Game
    {
        public List<Key> pressedKeys = new List<Key>();
        public int Score { get; set; }

        public delegate void GameOver();
        public event GameOver OnGameOver;

        public static readonly Bitmap background = new Bitmap(Properties.Resources.grass_3);
        public static readonly Bitmap enemySprite = new Bitmap(Properties.Resources.Enemy);
        public static readonly Bitmap playerSprite = new Bitmap(Properties.Resources.player);
        public static readonly Bitmap bulletSprite = new Bitmap(Properties.Resources.bullet);
        public static readonly Bitmap wallSprite = new Bitmap(Properties.Resources.RTS_Crate);
        public static readonly Bitmap starSprite = new Bitmap(Properties.Resources.Start_Explosion);

        public IEnumerable<GameObj> Objects => enemies.Concat(bullets).Concat(user.bullets).Concat(walls).Concat(stars).Append(user);

        private Size userSize = new Size(60, 40);
        private Size enemySize = new Size(60, 40);
        private Random rand = new Random();
        public readonly int countOfTanks;
        public readonly int countOfStars;
        public readonly int speed;
        private Size size;
        private RectangleF bounds;

        public User user;
        public List<Enemy> enemies = new List<Enemy>();
        public List<GameObj> walls = new List<GameObj>();
        public List<GameObj> bullets = new List<GameObj>();
        public List<GameObj> stars = new List<GameObj>();

        public Game(int countOfTanks, int countOfStars, int speed, Size size)
        {
            this.bounds = new RectangleF(0, 0, size.Width, size.Height);
            this.countOfTanks = countOfTanks;
            this.countOfStars = countOfStars;
            this.speed = speed;
            this.size = size;

            user = new User(new PointF(size.Width / 2, size.Height / 2), userSize, this.speed + 5, playerSprite);

            while (stars.Count() < countOfStars)
            {
                var star = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, image: starSprite);
                if (!user.HitBox.IntersectsWith(star.HitBox) &&!checkWalls(star))
                    stars.Add(star);
            }

            while (walls.Count < 8)
            {
                var wall = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, image: wallSprite);
                walls.Add(wall);
                if (user.HitBox.IntersectsWith(wall.HitBox)|| CheckEnenymiesCollisions(0).Count() != 0)
                {
                    walls.Remove(wall);
                }                  

            }
            while (enemies.Count()< countOfTanks) { 
                var enemy = new Enemy(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), speed, enemySize, enemySprite);
                enemies.Add(enemy);
                if (CheckEnenymiesCollisions(0).Count() != 0)
                {
                    enemies.Remove(enemy);
                }
            }          
        }
        DateTime lastFire = DateTime.Now;

        public void Update(float dt)
        {
            foreach (Enemy i in enemies)
            {
                i.Update(dt);

                if (rand.Next(0, 100) < 2) { bullets.Add(i.Fire()); }
            }

            CheckEnenymiesCollisions(dt);

            foreach (var i in enemies)
            {
                if (rand.Next(0, 100) < 5)
                {
                    i.RandomDirection();
                }
            }

            CheckBulletsCollisions();
            UserUpdate(dt);

            foreach (GameObj i in bullets.Concat(user.bullets))
            {
                i.Update(dt);
            }

            if (pressedKeys.Contains(Key.Space) && (DateTime.Now - lastFire).TotalSeconds > 1)
            {
                user.Fire();
                lastFire = DateTime.Now;
            }
        }

        void UserUpdate(float dt)
        {
            List<GameObj> starremove = new List<GameObj>();

            foreach(var star in stars)
            {
                if (star.HitBox.IntersectsWith(user.HitBox))
                {
                    Score++;
                    starremove.Add(star);
                }
            }
            starremove.ForEach(x=>stars.Remove(x));
            while (stars.Count() < countOfStars)
            {
                var star = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, image: starSprite);
                if (!user.HitBox.IntersectsWith(star.HitBox) && !checkWalls(star))
                    stars.Add(star);
            }
            if (!CheckBounds(user, dt))
            {
                if (checkWalls(user))
                {
                    user.Reverse();
                    user.Update(dt);
                    user.Reverse();
                }
                else if (pressedKeys.Contains(Key.W))
                {
                    user.Move(Direction.top, dt);
                }
                else if (pressedKeys.Contains(Key.S))
                {
                    user.Move(Direction.bot, dt);
                }
                else if (pressedKeys.Contains(Key.D))
                {
                    user.Move(Direction.right, dt);
                }
                else if (pressedKeys.Contains(Key.A))
                {
                    user.Move(Direction.left, dt);
                }
            }
        }

        IEnumerable<GameObj> CheckEnenymiesCollisions(float dt)//Можно ли меньше циклов ? хм ???
        {
            List<Enemy> collisions = new List<Enemy>();
            List<Enemy> shooted = new List<Enemy>();
            List<GameObj> splicebullet = new List<GameObj>();

            foreach (Enemy i in enemies)
            {
                if(checkWalls(i))
                        collisions.Add(i);


                if (CheckBounds(i, dt))
                {
                    collisions.Add(i);
                }

                if (user.HitBox.IntersectsWith(i.HitBox))
                {
                    OnGameOver?.Invoke();
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

            collisions = collisions.Distinct().ToList();
            foreach (var i in collisions)
            {
                i.Reverse();
                i.Update(dt);
            }

            shooted.ForEach(x => enemies.Remove(x));
            splicebullet.ForEach(x => user.bullets.Remove(x));
            return collisions;
        }

        bool checkWalls(GameObj obj)
        {
            foreach(var i in walls)
            {
                if (obj.HitBox.IntersectsWith(i.HitBox)&&i!=obj)
                {
                    return true;
                }
            }
            return false;
        }

        void CheckBulletsCollisions()//???
        {
            List<GameObj> outofrange = new List<GameObj>();

            foreach (GameObj i in bullets)
            {
                if(checkWalls(i))
                    outofrange.Add(i);

                if (i.HitBox.IntersectsWith(user.HitBox))
                {
                    OnGameOver?.Invoke();
                    break;
                }
                if (CheckBounds(i, 0))
                {
                    outofrange.Add(i);
                }
            }
            foreach (GameObj i in user.bullets)
            {
                if (checkWalls(i))
                    outofrange.Add(i);

                if (CheckBounds(i, 0))
                {
                    outofrange.Add(i);
                }
            }
            outofrange.ForEach(x => bullets.Remove(x));
            outofrange.ForEach(x => user.bullets.Remove(x));
        }

        bool CheckBounds(GameObj obj, float dt)
        {
            if (obj.pos.X < 0)
            {
                obj.pos.X = 0;
                return true;
            }
            if (obj.pos.Y < 0)
            {
                obj.pos.Y = 0;
                return true;
            }
            if (obj.pos.X > size.Width - obj.Size.Width)
            {
                obj.pos.X = size.Width - obj.Size.Width;
                return true;
            }
            if (obj.pos.Y > size.Height - obj.Size.Height)
            {
                obj.pos.Y = size.Height - obj.Size.Height;
                return true;
            }
            return false;
        }

    }
}
