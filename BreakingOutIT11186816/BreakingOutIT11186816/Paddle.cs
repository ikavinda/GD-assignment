using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BreakingOutIT11186816
{
    class Paddle
    {
        Vector2 motion, position;
        float paddleSpeed = 20;
        //missing code
        KeyboardState keyboardState;
        GamePadState gamePadState;

        Texture2D paddleTexture;
        Rectangle screenBounds;

        public Paddle(Texture2D texture, Rectangle screenBounds)
        {
            this.paddleTexture = texture;
            this.screenBounds = screenBounds;
            SetInStartPosition();
 
        }

        public void Update()
        {
            motion = Vector2.Zero;

            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Left) || gamePadState.IsButtonDown(Buttons.LeftThumbstickLeft) || gamePadState.IsButtonDown(Buttons.DPadLeft))
                motion.X = -1;

            if (keyboardState.IsKeyDown(Keys.Right) || gamePadState.IsButtonDown(Buttons.LeftThumbstickRight) || gamePadState.IsButtonDown(Buttons.DPadRight))
                motion.X = 1;

            motion.X *= paddleSpeed;
            position += motion;
            LockPaddle();
        }

        private void LockPaddle()
        {
            if (position.X < 0)
                position.X = 0;
            if (position.X + paddleTexture.Width > screenBounds.Width)
                position.X = screenBounds.Width - paddleTexture.Width;
        }

        public void SetInStartPosition()
        {
            position.X = (screenBounds.Width - paddleTexture.Width) / 2;
            position.Y = screenBounds.Height - paddleTexture.Height - 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(paddleTexture, position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, paddleTexture.Width, paddleTexture.Height);
        }
    }
}
