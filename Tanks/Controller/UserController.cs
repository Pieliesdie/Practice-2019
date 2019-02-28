using System.Windows.Forms;
using Game;

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
                        game.user.Move(Direction.top);
                        break;
                    }
                case Keys.S:
                    {
                        game.user.Move(Direction.bot);
                        break;
                    }
                case Keys.D:
                    {
                        game.user.Move(Direction.right);
                        break;
                    }
                case Keys.A:
                    {
                        game.user.Move(Direction.left);
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
