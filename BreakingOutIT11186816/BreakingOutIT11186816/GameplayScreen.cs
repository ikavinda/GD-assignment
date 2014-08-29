using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BreakingOutIT11186816
{
    class GameplayScreen
    {
        private Game1 game;

        //GraphicsDeviceManager graphics;
        Paddle paddle;
        Rectangle screenRectangle;
        int bricksWide = 10;
        int bricksHigh = 5;
        Texture2D brickImage, backgroundImage, gameOverImage;
        Brick[,] Bricks;

        Song backgroundMusic;

        Ball ball;

        float noOfLives = 2;
        SpriteFont font1;
        Vector2 fontPos;
        bool collided;
        public int points = 0;
        GameplayScreen gameScreen;
        Game1.GameState cureentScreen;
        //Game1 game;

        public void StartGame()
        {
            
            gameScreen = new GameplayScreen(game);
            cureentScreen = Game1.GameState.GamePlay;

            //startScreen = null;
            paddle.SetInStartPosition();
            ball.SetStartingPosition(paddle.GetBounds());

            Bricks = new Brick[bricksWide, bricksHigh];

            for (int y = 0; y < bricksHigh; y++)
            {
                Color tint = Color.White;

                switch (y)
                {
                    case 0:
                        tint = Color.Blue;
                        break;
                    case 1:
                        tint = Color.Red;
                        break;
                    case 2:
                        tint = Color.Green;
                        break;
                    case 3:
                        tint = Color.Yellow;
                        break;
                    case 4:
                        tint = Color.Purple;
                        break;
                }

                for (int x = 0; x < bricksWide; x++)
                {
                    //Bricks[x, y] = new Brick(brickImage, new Rectangle(x * brickImage.Width, y * brickImage.Height + 30, brickImage.Width, brickImage.Height), tint);
                }
            }
        }

        public GameplayScreen(Game1 game)
        {
            //this.game = game;

            //backgroundMusic = game.Content.Load<Song>("backgroundMusic");
            //MediaPlayer.Play(backgroundMusic);

            gameOverImage = game.Content.Load<Texture2D>("gameOver");
            backgroundImage = game.Content.Load<Texture2D>("backgroundImage");


            font1 = game.Content.Load<SpriteFont>("pointsFont");
            //fontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            Texture2D tempTexture = game.Content.Load<Texture2D>("paddle");
            paddle = new Paddle(tempTexture, screenRectangle);

            tempTexture = game.Content.Load<Texture2D>("ball");
            ball = new Ball(tempTexture, screenRectangle);

            brickImage = game.Content.Load<Texture2D>("brick");

            //game.StartGame();
        }

        public void Update()
        {
            paddle.Update();
            ball.Update();

            foreach (Brick brick in Bricks)
            {
                collided = brick.CheckCollision(ball);
                if (collided)
                    points++;


            }



            ball.PaddleCollision(paddle.GetBounds());

            if (ball.OffBottom())
            {
                if (noOfLives == 0)
                    cureentScreen = Game1.GameState.GameOver;

                if (noOfLives > 0)
                {
                    noOfLives--;
                    game.ResumeGame();
                }


            }
 
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (cureentScreen == Game1.GameState.GameOver)
            {
                spriteBatch.Draw(gameOverImage, screenRectangle, Color.White);
            }
            else
                spriteBatch.Draw(backgroundImage, screenRectangle, Color.White);
            spriteBatch.DrawString(font1, "Points : " + points, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font1, "Lives : " + noOfLives, new Vector2(750 - 150, 0), Color.Red);

            foreach (Brick brick in Bricks)
                brick.Draw(spriteBatch);

            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);
        }

        

    }
}
