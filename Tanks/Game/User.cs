using System.Collections.Generic;
using System.Drawing;

namespace Game
{
    public class User : GameObj
    {
        public List<GameObj> bullets = new List<GameObj>();

        public User(Point pos, Size size, int speed = 0)
            : base(pos, size, speed, Direction.bot)
        {
        }

        public new void Fire()
        {
            bullets.Add(base.Fire());
        }
    }
}
