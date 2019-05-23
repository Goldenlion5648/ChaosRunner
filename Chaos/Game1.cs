using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace ChaosRunner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Character player;
        //Bouncer wallBouncer;
        List<Bouncer> bouncerList= new List<Bouncer>(5);


        KeyboardState kb, oldkb;
        SpriteFont testFont, scoreFont;

        List<Character> collectibleObjectsList = new List<Character>(1);
        List<BaseEnemy> enemiesList = new List<BaseEnemy>(1);
        List<Character> allObjectsList = new List<Character>(1);

        Rectangle screenEncapsulation;

        System.Random rand = new Random(30);

        int screenHeight = 800;
        int screenWidth = 1280;

        int characterHeight = 50;
        int characterWidth = 50;

        int movementSpeed = 6;

        int sideScrollSpeed = 7;

        int score = 0;

        int gameClock = 0;

        int enemyStartingX =0;

        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            this.graphics.PreferredBackBufferHeight = screenHeight;
            this.graphics.PreferredBackBufferWidth = screenWidth;
            this.IsMouseVisible = true;

            this.Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
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
            enemyStartingX = screenWidth + (screenWidth / 10);

            player = new Character(Content.Load<Texture2D>("buttonOutline"), new Rectangle((screenWidth / 2) - (characterWidth / 2),
                screenHeight / 2 - characterHeight / 2, characterWidth, characterHeight));

            testFont = Content.Load<SpriteFont>("testFont");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            for (int i = 0; i < bouncerList.Count; ++i)
            {
                bouncerList[i] = new Bouncer(Content.Load<Texture2D>("triangleOutline"), new Rectangle(enemyStartingX,
                (screenHeight / 2) + rand.Next(10, 200), characterWidth, characterHeight));
                enemiesList.Add(bouncerList[i]);

            }

            screenEncapsulation = new Rectangle(0, 0, screenWidth, screenHeight);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            kb = Keyboard.GetState();
            startOfGameCode();
            userControls();
            enemyMovement();




            endOfGameCode();
            base.Update(gameTime);
        }

        public void userControls()
        {
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                for (int i = 0; i < movementSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Top - 1 >= screenEncapsulation.Top)
                    {
                        player.addToRecY(-1);
                    }
                }
            }

            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                for (int i = 0; i < movementSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Bottom + 1 <= screenEncapsulation.Bottom)
                    {
                        player.addToRecY(1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                for (int i = 0; i < movementSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Left - 1 >= screenEncapsulation.Left)
                    {
                        player.addToRecX(-1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                for (int i = 0; i < movementSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Right + 1 <= screenEncapsulation.Right)
                    {
                        player.addToRecX(1);
                    }
                }
            }

        }

        public void startOfGameCode()
        {
            for (int i = 0; i < enemiesList.Count; ++i)
            {
                if (allObjectsList.Contains(enemiesList[i]) == false)
                {
                    allObjectsList.Add(enemiesList[i]);
                }
            }

            for (int i = 0; i < collectibleObjectsList.Count; ++i)
            {
                if (allObjectsList.Contains(collectibleObjectsList[i]) == false)
                {
                    allObjectsList.Add(collectibleObjectsList[i]);
                }
            }
        }

        public void endOfGameCode()
        {
            gameClock++;
            oldkb = kb;
        }

        public bool checkCollisions()
        {
            bool didCollide = false;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (player.getRec().Intersects(enemiesList[i].getRec()))
                {
                    didCollide = true;
                }
            }

            return didCollide;

        }

        public void enemyMovement()
        {
            //wallBouncer.Move(screenEncapsulation);
            for (int i = 0; i < enemiesList.Count; ++i)
            {
                enemiesList[i].Move(screenEncapsulation);
            }
        }

        public void sideScroll()
        {
            for (int i = 0; i < enemiesList.Count; ++i)
            {
                enemiesList[i].addToRecX(-1 * sideScrollSpeed);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            player.drawCharater(spriteBatch, Color.Red);
            spriteBatch.Begin();
            for (int i = 0; i < enemiesList.Count; i++)
            {
                enemiesList[i].drawCharater(spriteBatch);
                spriteBatch.DrawString(scoreFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth  -300, screenHeight -300), Color.Black);
                spriteBatch.DrawString(testFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            }
            //spriteBatch.DrawString(testFont, "X: ", new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            spriteBatch.DrawString(scoreFont, "X: ", new Vector2(screenWidth - 300, screenHeight - 300), Color.Black);
            spriteBatch.DrawString(scoreFont, "X: ", new Vector2(screenWidth - 300, screenHeight - 300), Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
