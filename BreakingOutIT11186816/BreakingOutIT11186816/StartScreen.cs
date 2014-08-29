using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BreakingOutIT11186816
{
    public class StartScreen
    {
        private Texture2D texture;
        private Game1 game;
        private KeyboardState lastState;

        //private GameplayScreen gameScreen = new GameplayScreen(game);

        public StartScreen(Game1 game)
        {
            this.game = game;
            texture = game.Content.Load<Texture2D>("startScreen");
            lastState = Keyboard.GetState();
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter) || gamepadState.IsButtonDown(Buttons.Start))
                game.StartGame();
            lastState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
        }
    }

    
}
