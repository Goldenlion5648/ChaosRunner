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



        KeyboardState kb, oldkb;
        SpriteFont testFont, scoreFont;
        List<Bouncer> bouncerList = new List<Bouncer>();
        List<BaseCollectible> collectibleObjectsList = new List<BaseCollectible>();
        List<BaseEnemy> enemiesList = new List<BaseEnemy>();
        List<BaseEnemy> activeEnemies = new List<BaseEnemy>(1);
        List<Character> allObjectsList = new List<Character>(1);

        List<Character> backgroundCharacterList = new List<Character>(6);
        Texture2D[] backgroundImages = new Texture2D[6];


        int numOfEachEnemyType = 5;

        Rectangle screenEncapsulation;

        Random rand = new Random(4);

        int screenHeight = 800;
        int screenWidth = 1280;

        bool shouldEnemiesLoop = false;

        int defaultCharacterHeight = 50;
        int defaultCharacterWidth = 50;

        int currentCharacterHeight = 50;
        int currentCharacterWidth = 50;

        int playerSpeed = 7;
        bool isPressingKey = false;

        int maxCollectiblesOnScreen = 3;


        int sideScrollSpeed = -7;

        bool isGamePaused = false;

        int score = 0;

        int gameClock = 0;

        int enemyStartingX = 0;

        int enemyLimit = 5;

        bool hasDoneStartOfGameCode = false;
        int enemiesMovingCurrently = 0;
        int randomDecider;



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

            player = new Character(Content.Load<Texture2D>("buttonOutline"), new Rectangle((screenWidth / 2) - (defaultCharacterWidth / 2),
                screenHeight / 2 - defaultCharacterHeight / 2, defaultCharacterWidth, defaultCharacterHeight));

            backgroundImages[0] = Content.Load<Texture2D>("redOrange");
            backgroundImages[1] = Content.Load<Texture2D>("orangeYellow");
            backgroundImages[2] = Content.Load<Texture2D>("yellowGreen");
            backgroundImages[3] = Content.Load<Texture2D>("greenBlue");
            backgroundImages[4] = Content.Load<Texture2D>("bluePurple");
            backgroundImages[5] = Content.Load<Texture2D>("purpleRed");

            for (int i = 0; i < 6; i++)
            {
                backgroundCharacterList.Add(null);
                backgroundCharacterList[i] = new Character(backgroundImages[i], new Rectangle(1400 * i, 0, 1400, screenHeight));

            }

            testFont = Content.Load<SpriteFont>("testFont");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            EnemyDecreaser tempEnemyDecreaser;


            for (int i = 0; i < maxCollectiblesOnScreen; i++)
            {
                tempEnemyDecreaser = new EnemyDecreaser(Content.Load<Texture2D>("triangleOutline"), new Rectangle(enemyStartingX + rand.Next(10, 80),
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth, defaultCharacterHeight));
            }


            Bouncer tempBouncer;
            MiniBouncer tempMiniBouncer;
            Missile tempMissile;
            int tempRandom = 1;
            for (int i = 0; i < numOfEachEnemyType; ++i)
            {
                tempBouncer = new Bouncer(Content.Load<Texture2D>("triangleOutline"), new Rectangle(enemyStartingX + rand.Next(10, 80),
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth, defaultCharacterHeight));
                tempMiniBouncer = new MiniBouncer(Content.Load<Texture2D>("triangleOutline"), new Rectangle(enemyStartingX + rand.Next(10, 80),
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth, defaultCharacterHeight));
                tempMissile = new Missile(Content.Load<Texture2D>("buttonOutline"), new Rectangle(enemyStartingX + rand.Next(10, 80),
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth * 3, defaultCharacterHeight * 2 / 3));

                if (tempRandom == 2)
                {
                    tempMiniBouncer.isMovingUp = true;
                }
                tempRandom = rand.Next(1, 3);
                if (tempRandom == 1)
                {
                    tempBouncer.isMovingUp = true;
                }
                //bouncerList.Add(tempBouncer);
                enemiesList.Add(tempBouncer);
                enemiesList.Add(tempMiniBouncer);
                enemiesList.Add(tempMissile);

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


            if (isGamePaused == false)
            {
                backgroundLogic();
                enemyMovement();
                sideScroll();
                //checkEnemyPositions();
                checkActiveEnemyList();
                endOfTickCode();
            }


            oldkb = kb;
            base.Update(gameTime);
        }

        public void userControls()
        {
            isPressingKey = false;

            if (kb.IsKeyDown(Keys.P) && oldkb.IsKeyUp(Keys.P))
            {
                isGamePaused = !isGamePaused;
            }

            if(isGamePaused == true)
            {
                return;
            }
            #region Movement

            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Top - 1 >= screenEncapsulation.Top)
                    {
                        player.addToRecY(-1);
                    }
                }
            }

            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Bottom + 1 <= screenEncapsulation.Bottom)
                    {
                        player.addToRecY(1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed + 2; i++)
                {
                    if (checkCollisions() == false && player.getRec().Left - 1 >= screenEncapsulation.Left)
                    {
                        player.addToRecX(-1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed; i++)
                {
                    if (checkCollisions() == false && player.getRec().Right + 1 <= screenEncapsulation.Right)
                    {
                        player.addToRecX(1);
                    }
                }
            }
            #endregion

           

        }

        public void startOfGameCode()
        {
            //if(hasDoneStartOfGameCode == true)
            //{
            //    return;
            //}
            if (hasDoneStartOfGameCode == false)
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

                chooseEnemiesToMove();


                hasDoneStartOfGameCode = true;

            }
        }

        public void endOfTickCode()
        {
            gameClock++;
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

        public void backgroundLogic()
        {
            for (int i = 0; i < backgroundCharacterList.Count; i++)
            {
                backgroundCharacterList[i].addToRecX(-10);
                if(backgroundCharacterList[i].getRec().Right < 0)
                {
                    if (i == 0)
                    {
                        backgroundCharacterList[i].setRecX(backgroundCharacterList[backgroundCharacterList.Count - 1].getRec().Right - 10);
                    }
                    else
                    {
                        backgroundCharacterList[i].setRecX(backgroundCharacterList[i - 1].getRec().Right);

                    }
                }
            }
        }

        //public void checkEnemyPositions()
        //{
        //    //shouldEnemiesLoop = true;
        //    //for (int i = 0; i < enemyLimit; ++i)
        //    //{
        //    //    if (shouldEnemiesLoop == true && activeEnemies[i].getRec().Right > screenEncapsulation.Left)
        //    //    {
        //    //        shouldEnemiesLoop = false;
        //    //        return;
        //    //    }
        //    //}
        //    //resetEnemies();

        //}

        public void setEnemyStartingPos(ref List<BaseEnemy> enemyToMove, int index)
        {
            enemyToMove[index].setRecX(enemyStartingX + rand.Next(10, 280));
            enemyToMove[index].setRecY(rand.Next(10, screenHeight - defaultCharacterHeight));
            //enemyToMove[index].setRec()


        }

        //public void resetEnemies()
        //{
        //    //if (shouldEnemiesLoop)
        //    //{
        //    //    for (int i = 0; i < activeEnemies.Count; i++)
        //    //    {
        //    //        //activeEnemies[i].setRecX(enemyStartingX + rand.Next(10, 80));
        //    //        setEnemyStartingPos(ref activeEnemies, i);
        //    //        //activeEnemies[i].setRecY(rand.Next(10, screenHeight - defaultCharacterHeight));
        //    //    }
        //    //    if (enemyLimit < enemiesList.Count)
        //    //    {
        //    //        enemyLimit++;
        //    //    }
        //    //    chooseEnemiesToMove();
        //    //}
        //}

        public void enemyMovement()
        {
            //wallBouncer.Move(screenEncapsulation);
            for (int i = 0; i < activeEnemies.Count; ++i)
            {
                activeEnemies[i].Move(screenEncapsulation);
            }
        }

        public void checkActiveEnemyList()
        {
            bool shouldChoose = false;
            for (int i = 0; i < activeEnemies.Count; ++i)
            {
                if(activeEnemies[i].getRec().Right <= screenEncapsulation.Left)
                {
                    activeEnemies[i].isMoving = false;
                    //activeEnemies.RemoveAt(i);
                    setEnemyStartingPos(ref activeEnemies, i);

                    activeEnemies.RemoveAt(i);

                    shouldChoose = true;
                }
            }


            if(shouldChoose)
            {
                if (enemyLimit < enemiesList.Count)
                {
                    enemyLimit++;
                }
                chooseEnemiesToMove();
            }
        }

        public void chooseEnemiesToMove()
        {
            //enemiesList[3].isMoving = true;
            //enemiesMovingCurrently = 0;
            //for (int i = 0; i < activeEnemies.Count;)
            //{
            //    activeEnemies[i].isMoving = false;
            //    activeEnemies.RemoveAt(i);

            //}
            //Console.WriteLine("Line 386 activeEnemyCount: " + activeEnemies.Count);
            enemiesMovingCurrently = activeEnemies.Count;

            while (enemiesMovingCurrently < enemyLimit)
            {
                randomDecider = rand.Next(0, enemiesList.Count);
                if (enemiesList[randomDecider].isMoving == false && activeEnemies.Contains(enemiesList[randomDecider]) == false)
                {
                    enemiesList[randomDecider].isMoving = true;
                    activeEnemies.Add(enemiesList[randomDecider]);
                    //enemiesMovingCurrently++;
                    enemiesMovingCurrently = activeEnemies.Count;


                }
            }

        }

        public void sideScroll()
        {
            for (int i = 0; i < activeEnemies.Count; ++i)
            {
                if (activeEnemies[i].getRec().Right > screenEncapsulation.Left && activeEnemies[i].isMoving)
                {
                    activeEnemies[i].addToRecX(sideScrollSpeed);
                }
            }

            if (isPressingKey == false)
            {
                for (int i = 0; i < (-1 * sideScrollSpeed / 2 + 2); ++i)
                {
                    if (player.getRecX() > screenEncapsulation.Left)
                    {
                        player.addToRecX(-1);
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleVioletRed);
            spriteBatch.Begin();

            for (int i = 0; i < backgroundCharacterList.Count; i++)
            {
                backgroundCharacterList[i].drawCharater(spriteBatch);

            }

            for (int i = 0; i < backgroundCharacterList.Count; i++)
            {
                spriteBatch.DrawString(scoreFont, "backgroundPos: " + backgroundCharacterList[i].getRecX(), new Vector2(200, 300 + i * 20), Color.Black);

            }
            player.drawCharater(spriteBatch, Color.Red);

            for (int i = 0; i < enemiesList.Count; i++)
            {
                enemiesList[i].drawCharater(spriteBatch);
                spriteBatch.DrawString(scoreFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth - 500, 300 + i * 20), Color.Black);
                spriteBatch.DrawString(scoreFont, "Y: " + enemiesList[i].getRecY(), new Vector2(screenWidth - 400, 300 + i * 20), Color.Black);
                spriteBatch.DrawString(scoreFont, "isMoving: " + enemiesList[i].isMoving, new Vector2(screenWidth - 300, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(testFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            }
            //spriteBatch.DrawString(testFont, "X: ", new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            spriteBatch.DrawString(scoreFont, "enemyListLength: " + enemiesList.Count, new Vector2(screenWidth - 300, screenHeight - 180), Color.Black);
            spriteBatch.DrawString(scoreFont, "activeEnemyCount: " + activeEnemies.Count, new Vector2(screenWidth - 300, screenHeight - 40), Color.Black);

            // TODO: Add your drawing code here


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
