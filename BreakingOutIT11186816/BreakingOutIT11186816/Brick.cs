using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BreakingOutIT11186816
{
    class Brick
    {
        Texture2D brickTexture, fallingObjectTexture;
        Rectangle location;
        Color tint;
        bool alive, collided, isFrozen;
        bool objectFalling = false;
        Game1 main = new Game1();
        int fallingObjectSpeed;
        int count = 0;
        private Rectangle fallingObjectPos;
        public Rectangle Location
        {
            get { return location; }
        }
        Paddle paddle;
        Rectangle paddlePos;
        //default brick
        public Brick(Texture2D brickTexture, Rectangle location, Color tint, bool isFrozen, Texture2D objectFallingTexture)
        {
            this.brickTexture = brickTexture;
            this.fallingObjectTexture = objectFallingTexture;
            this.location = location;
            this.tint = tint;
            this.alive = true;
            this.isFrozen = isFrozen;
            paddlePos = main.paddlePosition;
            
        }

        public bool CheckCollision(Ball ball)
        {
            Random r = new Random();
            
            collided = false;
            if (alive && ball.Bounds.Intersects(location) && !isFrozen)
            {
                if (r.Next(2) == 1)
                {
                    objectFalling = true;

                }
                alive = false;
                ball.Deflection(this);
                collided = true;
                
                //Console.WriteLine(paddlePos);
            }
            else if (alive && ball.Bounds.Intersects(location) && isFrozen)
            {
                ball.Deflection(this);
            }
            return collided;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(brickTexture, location, tint);
            }
            if (objectFalling)
            {
                fallingObjectPos = new Rectangle(location.X + brickTexture.Width / 2, location.Y++, fallingObjectTexture.Width, fallingObjectTexture.Height);
                spriteBatch.Draw(fallingObjectTexture, fallingObjectPos, Color.Firebrick);
                //if (paddlePos.X == fallingObjectPos.X)
                //{

                //    Console.WriteLine("hari");
                //}

            }
        }
    }
}
