using System;
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
            form.KeyUp += KeyUp;
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            game.pressedKeys.RemoveAll(x=>x==(Key)e.KeyValue);
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            game.pressedKeys.Add((Key)e.KeyValue);
        }
    }

}
