using System.Windows.Forms;

namespace Controller
{
    public class UserController
    {
        Game.Game game;

        public UserController(Game.Game game, Form form)
        {
            this.game = game;
            form.KeyDown += KeyDown;
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    {
                        game.user.pos.Y -= game.user.speed;
                        game.user.direction = Game.Direction.top;
                        break;
                    }
                case Keys.S:
                    {
                        game.user.pos.Y += game.user.speed;
                        game.user.direction = Game.Direction.bot;
                        break;
                    }
                case Keys.D:
                    {
                        game.user.pos.X += game.user.speed;
                        game.user.direction = Game.Direction.right;
                        break;
                    }
                case Keys.A:
                    {
                        game.user.pos.X -= game.user.speed;
                        game.user.direction = Game.Direction.left;
                        break;
                    }
                case Keys.Space:
                    {
                        game.user.Fire();
                        break;
                    }
            }
        }
    }

}
