using System.Drawing;

namespace Game
{
    public class GameObj
    {
        public string name => this.ToString();
        public int speed;
        public Point pos;
        public string ImageUrl;//??????????
        public bool CanDestroy; //??????
        public readonly Size size;
        public Direction direction;

        public GameObj(Point pos,Size size,int speed=0,Direction direction=Direction.bot)
        {
            this.pos = pos;
            this.size = size;
            this.direction = direction;
            this.speed = speed;
        }

        public Rectangle HitBox => new Rectangle(pos, new Size(size.Width*120/100,size.Height*120/100));

        public void Update()
        {
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

        public GameObj Fire()
        {  
            return new GameObj(new Point(pos.X+size.Width/2,pos.Y+size.Height/2),new Size(5,5),speed*150/100,direction);
        }
    }
}
