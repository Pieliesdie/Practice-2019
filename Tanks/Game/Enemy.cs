using System;
using System.Drawing;

namespace Game
{
    public class Enemy : GameObj
    {
        static Random rand;

        public Enemy(PointF pos, int speed, Size size, Bitmap image = null) : base(pos, size, image: image)
        {
            this.speed = speed;
            rand = new Random();
        }

        public void Reverse()
        {
            switch (Direction)
            {
                case Direction.bot:
                    {
                        Direction = Direction.top;
                        break;
                    }
                case Direction.left:
                    {
                        Direction = Direction.right;
                        break;
                    }
                case Direction.right:
                    {
                        Direction = Direction.left;
                        break;
                    }
                case Direction.top:
                    {
                        Direction = Direction.bot;
                        break;
                    }
            }
        }

        public new void Update(float dt)
        {
            base.Update(dt);

        
        }

        public void RandomDirection()
        {
            switch (rand.Next(1, 4))
            {
                case 1:
                    {
                        Direction = Direction.top;
                        break;
                    }
                case 2:
                    {
                        Direction = Direction.right;
                        break;
                    }
                case 3:
                    {
                        Direction = Direction.left;
                        break;
                    }
                case 4:
                    {
                        Direction = Direction.bot;
                        break;
                    }
            }
        }
    }
}
