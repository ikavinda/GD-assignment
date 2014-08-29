using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakingOutIT11186816
{
    class Ball
    {
        Vector2 motion, position;
        Rectangle bounds;
        public float ballSpeed = 8;
        const float ballStartSpeed = 8;

        Texture2D ballTexture;
        Rectangle screenBounds;

        bool collided;

        public Rectangle Bounds
        {
            get
            {
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
                return bounds;
            }
        }

        public Ball(Texture2D texture, Rectangle screenBounds)
        {
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            this.ballTexture = texture;
            this.screenBounds = screenBounds;
        }

        public void Update()
        {
            collided = false;
            position += motion * ballSpeed;
            ballSpeed += 0.001f;
            CheckWallCollision();
        }

        public void CheckWallCollision()
        {
            if (position.X < 0)
            {
                position.X = 0;
                motion.X *= -1;
            }

            if (position.X + ballTexture.Width > screenBounds.Width)
            {
                position.X = screenBounds.Width - ballTexture.Width;
                motion.X *= -1;
            }

            if (position.Y < 30)
            {
                position.Y = 30;
                motion.Y *= -1;
            }
        }

        public void SetStartingPosition(Rectangle paddleLocation)
        {
            Random rand = new Random();
            motion = new Vector2(rand.Next(2, 6), -rand.Next(2, 6));
            motion.Normalize();
            //ballSpeed = ballStartSpeed;
            position.Y = paddleLocation.Y - ballTexture.Height;
            position.X = paddleLocation.X + (paddleLocation.Width - ballTexture.Width) / 2;
        }

        public bool OffBottom()
        {
            if (position.Y > screenBounds.Height)
                return true;
            else
                return false;
        }

        public void PaddleCollision(Rectangle paddleLocation)
        {
            Rectangle ballLocation = new Rectangle((int)position.X, (int)position.Y, ballTexture.Width, ballTexture.Height);

         
            if (paddleLocation.Intersects(ballLocation))
            {
                position.Y = paddleLocation.Y-ballTexture.Height;
                motion.Y *= -1;
            }
        }

        public void Deflection(Brick brick)
        {
            if (!collided)
            {
                motion.Y *= -1;
                collided = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballTexture, position, Color.White);
        }


    }


}
