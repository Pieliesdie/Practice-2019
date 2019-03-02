using System.Collections.Generic;
using System.Drawing;

namespace Game
{
    public class User : Enemy
    {
        public List<GameObj> bullets = new List<GameObj>();

        public User(PointF pos, Size size, int speed = 0,Bitmap image=null)
            : base(pos, speed, size,image){}

        public new void Fire()
        {
            bullets.Add(base.Fire());
        }

        public void Move(Direction direction, float dt)
        {
            base.Direction = direction;
            base.Update(dt);
        }
    }
}
