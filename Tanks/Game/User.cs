using System.Collections.Generic;
using System.Drawing;

namespace Game
{
    public class User : GameObj
    {
        public List<GameObj> bullets = new List<GameObj>();

        public User(Point pos, Size size, int speed = 0,Bitmap image=null)
            : base(pos, size, speed, Direction.bot,image){}

        public new void Fire()
        {
            bullets.Add(base.Fire());
        }

        public void Move(Direction direction)
        {
            base.Direction = direction;
            switch (direction)
            {
                case Direction.bot:
                    {
                        pos.Y += speed;
                        break;
                    }
                case Direction.left:
                    {
                        pos.X -= speed;
                        break;
                    }
                case Direction.right:
                    {
                        pos.X += speed;
                        break;
                    }
                case Direction.top:
                    {
                        pos.Y -= speed;
                        break;
                    }
            }
        }
    }
}
