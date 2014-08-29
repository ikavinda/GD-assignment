using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;

namespace BreakingOutIT11186816
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Paddle paddle;
        Rectangle screenRectangle;
        int bricksWide = 10;
        int bricksHigh = 5;
        int noOfBricks = 50;
        Texture2D brickImage, backgroundImage, gameOverImage, gameWonImage;
        Brick[,] bricks, frozenBricks;
        //FallingObject[] fallingObjects;

        //FallingObject fallingObject;

        Song backgroundMusic;

        Ball ball;
        float ballSpeed;

        float noOfLives = 2;
        SpriteFont font1;
        bool collided, gamePaused, objectFalling;
        int fallingObjectVisibility;
        GameTime levelUpTimer;
        public int points = 0;
        int level = 1;
        Timer t = new Timer(1000);
        public Rectangle paddlePosition;

        GameState currentScreen;
        private StartScreen startScreen;
        private GameOverScreen gameOverScreen;
        //private GameWonLvl1Screen gameWonLevel1;

        KeyboardState keyboardState;
        GamePadState gamePadState;


        public enum GameState
        {
            StartScreen,
            GamePlay,
            GameOver,
            GameWon
        }

        GameplayScreen gameScreen;
        private Texture2D objectImage;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 750;
            graphics.PreferredBackBufferHeight = 650;

            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

        }

        public void StartGame()
        {
            gameScreen = new GameplayScreen(this);
            currentScreen = GameState.GamePlay;

            paddle.SetInStartPosition();
            ball.SetStartingPosition(paddle.GetBounds());

            bricks = new Brick[bricksWide, bricksHigh];
            frozenBricks = new Brick[bricksWide, bricksHigh];
            for (int y = 0; y < bricksHigh; y++)
            {             
                Color tint = Color.White;
                Random r = new Random();
                Color newColor = new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
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
                    default:
                        tint = newColor;
                        break;

                }
                
            for (int x = 0; x < bricksWide; x++)
            {
                
                //if ( r.Next(2) == 1)
                //    objectFalling = true;
                
                if(bricksHigh/3 <= y && bricksHigh*2/3 >y && x >= 4 && x < 6 && level>1)
                    bricks[x, y] = new Brick(brickImage, new Rectangle(x * brickImage.Width, y * brickImage.Height + 30, brickImage.Width, brickImage.Height), Color.White, true, objectImage);
                else
                    bricks[x, y] = new Brick(brickImage, new Rectangle(x * brickImage.Width, y * brickImage.Height + 30, brickImage.Width, brickImage.Height), tint, false, objectImage);
                
            }
        }
        }

        public void ResumeGame()
        {
            paddle.SetInStartPosition();
            ball.SetStartingPosition(paddle.GetBounds());
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startScreen = new StartScreen(this);
            gameOverScreen = new GameOverScreen(this);
            //gameWonLevel1 = new GameWonLvl1Screen(this);
            currentScreen = GameState.StartScreen;

            
            Song backgroundMusic = Content.Load<Song>("space_age");          
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            gameOverImage = Content.Load<Texture2D>("gameOver");
            //gameWonImage = Content.Load<Texture2D>("gameWon");
            backgroundImage = Content.Load<Texture2D>("backgroundImage");
            


            font1 = Content.Load<SpriteFont>("pointsFont");
            //fontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            Texture2D tempTexture = Content.Load<Texture2D>("paddle");
            paddle = new Paddle(tempTexture, screenRectangle);

            tempTexture = Content.Load<Texture2D>("ball");
            ball = new Ball(tempTexture, screenRectangle);

            

            //tempTexture = Content.Load<Texture2D>("object");

            brickImage = Content.Load<Texture2D>("brick");
            objectImage = Content.Load<Texture2D>("ball");

            //StartGame();
            currentScreen = GameState.StartScreen;
        }

        

        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.IsButtonDown(Buttons.Start))
                currentScreen = GameState.GamePlay;

            if (currentScreen == GameState.StartScreen)
                startScreen.Update();

            if (currentScreen == GameState.GamePlay)
                GamePlayUpdate();

            if (currentScreen == GameState.GameOver)
                gameOverScreen.Update();

            //if (currentScreen == GameState.GameWon)
            //    gameWonLevel1.Update();
                


            //GamePlayUpdate();
            
            base.Update(gameTime);
            //Console.WriteLine(gameTime.ElapsedGameTime);
        }

        private void GamePlayUpdate()
        {
            paddle.Update();
            ball.Update();
            paddlePosition = paddle.GetBounds();
            //Console.WriteLine(paddlePosition);
            foreach (Brick brick in bricks)
            {
                //brick.ObjectCollision(paddle);
                collided = brick.CheckCollision(ball);
                if (collided)
                {
                    //Random r = new Random();
                    //fallingObjectVisibility = r.Next(0,1);
                    //if (fallingObjectVisibility == 1)
                    //    fallingObject.SetStartingPosition(fallingObject.GetBounds());
                    points = points + 10 ;
                    noOfBricks--;
                    //Console.WriteLine(noOfBricks);
                }
                if (noOfBricks == 0)
                {
           
                    //Console.WriteLine(levelUpTimer.ElapsedGameTime);

                    //currentScreen = GameState.GameWon;
                    level++;
                    bricksHigh++;
                    noOfBricks = (bricksWide * bricksHigh) - (2 * bricksHigh / 3);
                    ballSpeed = ball.ballSpeed;
                    ballSpeed = ballSpeed + 1;
                    ball.ballSpeed = ballSpeed;
                    StartGame();                    
                }
            }

            ball.PaddleCollision(paddle.GetBounds());
            if (ball.OffBottom())
            {
                if (noOfLives == 0)
                    currentScreen = Game1.GameState.GameOver;

                if (noOfLives > 0)
                {
                    noOfLives--;
                    ResumeGame();
                }
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (currentScreen == GameState.StartScreen)
                startScreen.Draw(spriteBatch);
            if (currentScreen == GameState.GamePlay)
                GamePlayDraw(spriteBatch);

            if (currentScreen == GameState.GameOver)
                gameOverScreen.Draw(spriteBatch);

            //if (currentScreen == GameState.GameWon)
            //    gameWonLevel1.Draw(spriteBatch);
            
            spriteBatch.End();


            base.Draw(gameTime);
        }

        private void GamePlayDraw(SpriteBatch spriteBatch)
        {
            //if (currentScreen == GameState.StartScreen)
            //    startScreen.Draw(spriteBatch);
            //else if (currentScreen == GameState.GameOver)
            //{
            //    spriteBatch.Draw(gameOverImage, screenRectangle, Color.White);
            //}
            //else if (currentScreen == GameState.GameWon)
            //    spriteBatch.Draw(gameWonImage, screenRectangle, Color.White);
            //else
                spriteBatch.Draw(backgroundImage, screenRectangle, Color.White);
            spriteBatch.DrawString(font1, "Points : " + points, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font1, "Lives : " + noOfLives, new Vector2(graphics.PreferredBackBufferWidth - 150, 0), Color.Red);
            spriteBatch.DrawString(font1, "Level : "+ level, new Vector2( graphics.PreferredBackBufferWidth / 2 - 150, 0), Color.YellowGreen);
            foreach (Brick brick in bricks)
                brick.Draw(spriteBatch);

            //foreach (FallingObject fallingObj in fallingObjects)
            //    fallingObject.Draw(spriteBatch);


            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            //fallingObject.Draw(spriteBatch);
        }

        public void RestartGameValues()
        {
            points = 0;
            level = 1;
            noOfLives = 2;
            bricksHigh = 5;
            bricksWide = 10;
            noOfBricks = bricksHigh * bricksWide;
            ball.ballSpeed = 8;
        }

        public void ContinueGameValues()
        {
            points = 0;
            noOfLives = 2;
        }
    }
}
