using System;
using System.Drawing;

namespace Game
{
    public class Enemy : GameObj
    {

        static Random rand;

        public Enemy(Point pos, int speed,Size size) : base(pos,size)
        {
            this.speed = speed;
            rand = new Random();
        }

        public void Reverse()
        {
            switch (direction)
            {
                case Direction.bot:
                    {
                        direction = Direction.top;
                        break;
                    }
                case Direction.left:
                    {
                        direction = Direction.right;
                        break;
                    }
                case Direction.right:
                    {
                        direction = Direction.left;
                        break;
                    }
                case Direction.top:
                    {
                        direction = Direction.bot;
                        break;
                    }
            }
        }

        public new void Update()
        {
            base.Update();
            var tmp = rand.Next(0, 100);
            if (tmp < 5)
            {
                RandomDirection();
            }
        }

        public void RandomDirection()
        {
            switch (rand.Next(1, 4))
            {
                case 1:
                    {
                        direction = Direction.top;
                        break;
                    }
                case 2:
                    {
                        direction = Direction.right;
                        break;
                    }
                case 3:
                    {
                        direction = Direction.left;
                        break;
                    }
                case 4:
                    {
                        direction = Direction.bot;
                        break;
                    }
            }
        }
    }
}
