﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game
{
    // -------------------- begin devil code ------------------------
    //                             ,-.
    //        ___,---.__          /'|`\          __,---,___
    //     ,-'    \`    `-.____,-'  |  `-.____,-'    //    `-.
    //  ,'        |           ~'\     /`~           |        `.
    // /      ___//              `. ,'          ,  , \___      \
    // |    ,-'   `-.__   _         |        ,    __,-'   `-.    |
    // |   /          /\_  `   .    |    ,      _/\          \   |
    // \  |           \ \`-.___ \   |   / ___,-'/ /           |  /
    //  \  \           | `._   `\\  |  //'   _,' |           /  /
    //   `-.\         /'  _ `---'' , . ``---' _  `\         /,-'
    //      ``       /     \    ,='/ \`=.    /     \       ''
    //              |__   /|\_,--.,-.--,--._/|\   __|
    //              /  `./  \\`\ |  |  | /,//' \,'  \
    //             /   /     ||--+--|--+-/-|     \   \
    //            |   |     /'\_\_\ | /_/_/`\     |   |
    //             \   \__, \_     `~'     _/ .__/   /
    //              `-._,-'   `-._______,-'   `-._,-'
    // -------------------------------------------------------------- 
    public class Game
    {
        #region variables
        public List<Key> pressedKeys = new List<Key>();
        public int Score { get; set; }

        public delegate void GameOver();
        public event GameOver OnGameOver;

        public delegate void WinGame();
        public event WinGame OnWinGame;

        public static readonly Bitmap background = new Bitmap(Properties.Resources.grass_3);
        public static readonly Bitmap enemySprite = new Bitmap(Properties.Resources.Enemy);
        public static readonly Bitmap playerSprite = new Bitmap(Properties.Resources.player);
        public static readonly Bitmap bulletSprite = new Bitmap(Properties.Resources.bullet);
        public static readonly Bitmap wallSprite = new Bitmap(Properties.Resources.RTS_Crate);
        public static readonly Bitmap starSprite = new Bitmap(Properties.Resources.stars);
        public static readonly Bitmap boomSprite = new Bitmap(Properties.Resources.boom);
        public static readonly Bitmap waterSprite = new Bitmap(Properties.Resources.water);

        public IEnumerable<GameObj> Objects => enemies.Concat(walls).Concat(bullets).Concat(user.bullets).Concat(stars).Concat(animations).Append(user);

        private Size waterSize = new Size(140,60);
        private Size userSize = new Size(50, 50);
        private Size enemySize = new Size(50, 50);
        private Size exploisonsSize = new Size(50, 50);
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
        public List<GameObj> animations = new List<GameObj>();
        #endregion

        public Game(int countOfTanks, int countOfStars, int speed, Size size)
        {
            this.bounds = new RectangleF(0, 0, size.Width, size.Height);
            this.countOfTanks = countOfTanks;
            this.countOfStars = countOfStars;
            this.speed = speed;
            this.size = size;

            user = new User(new PointF(size.Width / 2, size.Height / 2), userSize, this.speed + 5, playerSprite) { name = "user", moveable = true };

          
            while (walls.Count < 8)
            {
                bool canDestroy = (rand.NextDouble() < 0.5) ? true : false;
                var wallsprite = canDestroy ? Properties.Resources.RTS_Crate1 : wallSprite;
                var wall = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, image: wallsprite) { name = "wall", CanDestroy = canDestroy };
                walls.Add(wall);
                if (user.HitBox.IntersectsWith(wall.HitBox) || CheckEnenymiesCollisions().Count() != 0)
                {
                    walls.Remove(wall);
                }
            }

            while (walls.Count < 12)
            {
                bool canAdd = true;
                var randDirection = (rand.NextDouble() < 0.5) ? Direction.bot : Direction.right;
                var water = new GameObj(new PointF(rand.Next(size.Width - waterSize.Width), rand.Next(size.Height - waterSize.Height)), waterSize, image: waterSprite,direction:randDirection) { name = "water" };
                foreach(var i in walls.Append(user))
                {
                    if (i.name == "water")
                        continue;
                    if (i.HitBox.IntersectsWith(water.HitBox))
                        canAdd = false;
                }
                if(canAdd)
                    walls.Add(water);
            }          

            while (enemies.Count() < countOfTanks)
            {
                //Возможно лишнее и CheckEnenymiesCollisions() хватало. не уверен
                var enemy = new Enemy(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), speed, enemySize, enemySprite) { name = "enemy", moveable = true };
                bool canAdd = true;
                foreach (var i in enemies.Concat(walls).Append(user))
                {
                    var r1 = new PointF(enemy.pos.X + enemy.HitBox.Width / 2, enemy.pos.Y + enemy.HitBox.Height / 2);
                    var r2 = new PointF(i.pos.X + i.HitBox.Width / 2, i.pos.Y + i.HitBox.Height / 2);
                    if (Math.Sqrt(Math.Pow(r2.X - r1.X, 2) + Math.Pow(r2.Y - r1.Y, 2)) < Math.Max(enemySize.Width * 2, enemySize.Height * 2))
                    {
                        canAdd = false;
                        break;
                    }
                }
                if (canAdd)
                    enemies.Add(enemy);
            }

            while (stars.Count() < countOfStars)
            {
                var star = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, animation: GetListFromImage(starSprite, new Size(84, starSprite.Height), 6)) { name = "star" };
                if (!user.HitBox.IntersectsWith(star.HitBox) && !checkWalls(star, out _))
                    stars.Add(star);
            }
        }
        DateTime lastFire = DateTime.Now;

        public void Update(float dt)
        {
            if (enemies.Count <= 0)
            {
                OnWinGame?.Invoke();
            }

            foreach (Enemy i in enemies)
            {
                if (rand.Next(0, 100) < 2) { bullets.Add(i.Fire()); }
                i.Update(dt);
            }

            var tmp = CheckEnenymiesCollisions();
            foreach (Enemy i in tmp)
            {
                i.Update(-dt);
                i.Reverse();
            }

            foreach (Enemy i in enemies.Except(tmp))
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

            List<GameObj> doneAnimation = new List<GameObj>();
            foreach (var anim in animations)
            {
                if (anim.Done == true)
                    doneAnimation.Add(anim);
            }

            doneAnimation.ForEach(x => animations.Remove(x));
            List<GameObj> starremove = new List<GameObj>();

            foreach (var star in stars)
            {
                if (star.HitBox.IntersectsWith(user.HitBox))
                {
                    Score++;
                    starremove.Add(star);
                }
            }
            starremove.ForEach(x => stars.Remove(x));
            while (stars.Count() < countOfStars)
            {
                var star = new GameObj(new PointF(rand.Next(size.Width - enemySize.Width), rand.Next(size.Height - enemySize.Height)), enemySize, animation: GetListFromImage(starSprite, new Size(84, starSprite.Height), 6)) { name = "star" };
                if (!user.HitBox.IntersectsWith(star.HitBox) && !checkWalls(star,out _))
                    stars.Add(star);
            }
            user.Update(dt);
            if (checkWalls(user, out _) || CheckBounds(user))
            {
                user.Update(-dt);
                user.Reverse();
            }
            else if (pressedKeys.Contains(Key.W))
            {
                user.Move(Direction.top, 0);
            }
            else if (pressedKeys.Contains(Key.S))
            {
                user.Move(Direction.bot, 0);
            }
            else if (pressedKeys.Contains(Key.D))
            {
                user.Move(Direction.right, 0);
            }
            else if (pressedKeys.Contains(Key.A))
            {
                user.Move(Direction.left, 0);
            }
        }

        IEnumerable<GameObj> CheckEnenymiesCollisions()
        {
            List<Enemy> collisions = new List<Enemy>();
            List<Enemy> shooted = new List<Enemy>();
            List<GameObj> splicebullet = new List<GameObj>();
            List<Enemy> enemywenemy = new List<Enemy>();
            foreach (Enemy i in enemies)
            {
                if (checkWalls(i, out _))
                    collisions.Add(i);


                if (CheckBounds(i))
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
                        var boom = new GameObj(j.pos, exploisonsSize, animation: GetListFromImage(boomSprite, new Size(39, boomSprite.Height), 13), once: true) { name = "animation" };
                        animations.Add(boom);
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
            shooted.ForEach(x => enemies.Remove(x));
            splicebullet.ForEach(x => user.bullets.Remove(x));
            return collisions;
        }

        bool checkWalls(GameObj obj,out GameObj collis)
        {
            collis = null;
            foreach (var i in walls)
            {
                if (obj.HitBox.IntersectsWith(i.HitBox) && i != obj)
                {
                    collis = i;
                    return true;
                }
            }           
            return false;
        }

        void CheckBulletsCollisions()
        {
            List<GameObj> outofrange = new List<GameObj>();

            foreach (GameObj i in bullets.Concat(user.bullets))
            {
                if (checkWalls(i, out GameObj col))
                {
                    if (col.name == "wall")
                    {
                        outofrange.Add(i);
                        var boom = new GameObj(i.pos, exploisonsSize, animation: GetListFromImage(boomSprite, new Size(39, boomSprite.Height), 7), once: true) { name = "animation" };
                        animations.Add(boom);
                        if (col.CanDestroy == true)
                        {
                            walls.Remove(col);
                        }
                    }
                }

                if (i.HitBox.IntersectsWith(user.HitBox)&&i.name=="enemybullet")
                {
                    OnGameOver?.Invoke();
                    break;
                }
                if (CheckBounds(i))
                {
                    outofrange.Add(i);
                }
            }
            outofrange.ForEach(x => bullets.Remove(x));
            outofrange.ForEach(x => user.bullets.Remove(x));
        }

        bool CheckBounds(GameObj obj)
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

        static List<Bitmap> GetListFromImage(Bitmap img, Size sizeOfOneSprite, int length)
        {
            List<Bitmap> tmp = new List<Bitmap>();
            var currentWidth = 0;
            for (int i = 0; i < length; i++)
            {
                var btmp = new Bitmap(sizeOfOneSprite.Width, sizeOfOneSprite.Height);
                Graphics g = Graphics.FromImage(btmp);
                var rec = new Rectangle(currentWidth, 0, sizeOfOneSprite.Width, sizeOfOneSprite.Height);
                g.DrawImage(img, 0, 0, rec, GraphicsUnit.Pixel);
                tmp.Add(btmp);
                currentWidth += sizeOfOneSprite.Width;
                g.Dispose();
            }

            return tmp;
        }
    }
}
