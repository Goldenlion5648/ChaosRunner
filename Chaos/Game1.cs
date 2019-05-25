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

        Character player, healthBar, healthBarOutline;
        //Bouncer wallBouncer;
        

        KeyboardState kb, oldkb;
        SpriteFont testFont, scoreFont;
        List<BaseCollectible> collectibleObjectsList = new List<BaseCollectible>();
        List<BaseEnemy> enemiesList = new List<BaseEnemy>();
        List<BaseEnemy> activeEnemies = new List<BaseEnemy>(1);
        //List<Character> allObjectsList = new List<Character>(1);

        List<Character> backgroundCharacterList = new List<Character>(6);
        Texture2D[] backgroundImages = new Texture2D[6];
        Texture2D[] enemyDecreaserImages = new Texture2D[2];
        Texture2D[] timeFreezerImages = new Texture2D[2];
        Texture2D[] healthPackImages = new Texture2D[2];


        int numOfEachEnemyType = 5;

        Rectangle screenEncapsulation;

        Random rand = new Random(4);

        int screenHeight = 800;
        int screenWidth = 1280;

        //bool shouldEnemiesLoop = false;

        int defaultCharacterHeight = 50;
        int defaultCharacterWidth = 50;

        int currentCharacterHeight = 50;
        int currentCharacterWidth = 50;

        int powerUpScoreWorth = 300;
        double scoreMultiplier = 1;

        int playerHitCooldown = 0;
        int addEnemyCooldown = 0;

        bool didAnimate = true;

        int playerSpeed = 7;
        bool isPressingKey = false;

        int currentCollectiblesOnScreen = 0;
        int maxCollectiblesOnScreen = 3;

        int enemyFreezeCooldown = 0;
        int sideScrollSpeed = -7;

        bool isGamePaused = false;

        double score = 0;

        int playerHealth = 100;

        int gameClock = 0;

        int enemyStartingX = 0;

        int enemyLimit = 5;

        bool hasDoneStartOfGameCode = false;
        int enemiesMovingCurrently = 0;
        int randomDecider;

        enum gameState
        {
            titleScreen, gameplay, lose
        }

        gameState state = gameState.gameplay;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            this.graphics.PreferredBackBufferHeight = screenHeight;
            this.graphics.PreferredBackBufferWidth = screenWidth;
            this.IsMouseVisible = true;
            //this.Window.AllowUserResizing = true;
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

            healthBar = new Character(Content.Load<Texture2D>("blankSquare"), new Rectangle(screenWidth - 250,
                40, defaultCharacterWidth * 4, defaultCharacterHeight - 10));

            healthBarOutline = new Character(Content.Load<Texture2D>("blackSquare"), new Rectangle(screenWidth - 255,
                35, defaultCharacterWidth * 4 + 10, defaultCharacterHeight));

            backgroundImages[0] = Content.Load<Texture2D>("redOrange");
            backgroundImages[1] = Content.Load<Texture2D>("orangeYellow");
            backgroundImages[2] = Content.Load<Texture2D>("yellowGreen");
            backgroundImages[3] = Content.Load<Texture2D>("greenBlue");
            backgroundImages[4] = Content.Load<Texture2D>("bluePurple");
            backgroundImages[5] = Content.Load<Texture2D>("purpleRed");

            //enemyDecreaserImages[0] = Content.Load<Texture2D>("lightningOutline1");
            //enemyDecreaserImages[1] = Content.Load<Texture2D>("lightningOutline2");


            for (int i = 0; i < enemyDecreaserImages.Length; i++)
            {
                enemyDecreaserImages[i] = Content.Load<Texture2D>("lightningOutline" + (i + 1));
            }

            for (int i = 0; i < timeFreezerImages.Length; i++)
            {
                timeFreezerImages[i] = Content.Load<Texture2D>("clock" + (i + 1));
            }

            for (int i = 0; i < healthPackImages.Length; i++)
            {
                healthPackImages[i] = Content.Load<Texture2D>("healthPack" + (i + 1));
            }


            for (int i = 0; i < 6; i++)
            {
                backgroundCharacterList.Add(null);
                backgroundCharacterList[i] = new Character(backgroundImages[i], new Rectangle(1400 * i, 0, 1400, screenHeight));

                    //backgroundCharacterList[i].texture = null;
                

            }


            testFont = Content.Load<SpriteFont>("testFont");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            EnemyDecreaser tempEnemyDecreaser;
            TimeFreezer tempTimeFreezer;
            HealthPack tempHealthPack;


            for (int i = 0; i < maxCollectiblesOnScreen; i++)
            {
                tempEnemyDecreaser = new EnemyDecreaser(enemyDecreaserImages[0], new Rectangle(screenWidth + 50,
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth, defaultCharacterHeight * 2), enemyDecreaserImages);
                tempTimeFreezer = new TimeFreezer(timeFreezerImages[0], new Rectangle(screenWidth + 50,
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth * 2, defaultCharacterHeight * 2), timeFreezerImages);
                tempHealthPack = new HealthPack(healthPackImages[0], new Rectangle(screenWidth + 50,
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth * 3 / 2, defaultCharacterHeight * 3 / 2), healthPackImages);

                collectibleObjectsList.Add(tempEnemyDecreaser);
                collectibleObjectsList.Add(tempTimeFreezer);
                collectibleObjectsList.Add(tempHealthPack);


            }
            tempEnemyDecreaser = null;
            tempTimeFreezer = null;
            tempHealthPack = null;

            Bouncer tempBouncer;
            MiniBouncer tempMiniBouncer;
            Missile tempMissile;
            int tempRandom = 1;
            for (int i = 0; i < numOfEachEnemyType; ++i)
            {
                tempBouncer = new Bouncer(Content.Load<Texture2D>("triangleOutline"), new Rectangle(enemyStartingX + rand.Next(10, 80),
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth * 3 / 2, defaultCharacterHeight * 3 / 2));
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

            tempBouncer = null;
            tempMiniBouncer = null;
            tempMissile = null;

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

            //Content.Unload();
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

            

            //if (this.IsActive)
            //{
                switch (state)
                {
                    case gameState.titleScreen:
                        titleScreen();
                        break;
                    case gameState.gameplay:
                        gameplay();
                        break;
                    case gameState.lose:
                        lose();
                        break;
                }
            //}
            

            oldkb = kb;
            base.Update(gameTime);
        }

        public void titleScreen()
        {

        }

        public void gameplay()
        {
            startOfGameCode();
            userControls();


            if (isGamePaused == false)
            {
                backgroundLogic();
                collectibleAnimations();
                spawnCollectible();
                checkEnemyCollisions();
                enemyMovement();
                sideScroll();
                checkCollectibleCollisions();
                //checkEnemyPositions();
                checkActiveEnemyList();
                addEnemies();
                endOfTickCode();
            }

        }

        public void lose()
        {

        }

        public void spawnCollectible()
        {
            if(gameClock % 120 == 0 && gameClock != 0 && currentCollectiblesOnScreen < maxCollectiblesOnScreen)
            {
                randomDecider = rand.Next(1, 4);
                if(randomDecider == 1)
                {

                    randomDecider = rand.Next(0, collectibleObjectsList.Count);
                    while(collectibleObjectsList[randomDecider].isOnScreen == true)
                    {
                        randomDecider = rand.Next(0, collectibleObjectsList.Count);

                    }
                    currentCollectiblesOnScreen++;
                    collectibleObjectsList[randomDecider].isOnScreen = true;
                    setCharacterPos(ref collectibleObjectsList, randomDecider);
                }

            }
        }

        public void setCharacterPos(ref List<BaseCollectible> listWithObject, int index)
        {
            randomDecider = rand.Next(screenEncapsulation.Left, screenEncapsulation.Right - listWithObject[index].lengthX);
            listWithObject[index].setRecX(randomDecider);
            randomDecider = rand.Next(screenEncapsulation.Top, screenEncapsulation.Bottom - listWithObject[index].lengthY);
            listWithObject[index].setRecY(randomDecider);
        }

        public void setCharacterPosOffScreen(ref List<BaseCollectible> listWithObject, int index)
        {
            listWithObject[index].setRecX(screenWidth + 100);
            
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
                    if (player.getRec().Top - 1 >= screenEncapsulation.Top)
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
                    if ( player.getRec().Bottom + 1 <= screenEncapsulation.Bottom)
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
                    if ( player.getRec().Left - 1 >= screenEncapsulation.Left)
                    {
                        player.addToRecX(-1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed + 2; i++)
                {
                    if (player.getRec().Right + 1 <= screenEncapsulation.Right)
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
                //for (int i = 0; i < enemiesList.Count; ++i)
                //{
                //    if (allObjectsList.Contains(enemiesList[i]) == false)
                //    {
                //        allObjectsList.Add(enemiesList[i]);
                //    }
                //}

                //for (int i = 0; i < collectibleObjectsList.Count; ++i)
                //{
                //    if (allObjectsList.Contains(collectibleObjectsList[i]) == false)
                //    {
                //        allObjectsList.Add(collectibleObjectsList[i]);
                //    }
                //}

                chooseEnemiesToMove();


                hasDoneStartOfGameCode = true;

            }
        }

        public void endOfTickCode()
        {
            if (playerHitCooldown > 0)
            {
                playerHitCooldown--;
            }
            if (enemyFreezeCooldown == 0)
            {
                addEnemyCooldown++;
            }
            if (enemyFreezeCooldown > 0)
            {
                enemyFreezeCooldown--;
            }
            if(gameClock % 60 == 0 && gameClock != 0)
            {
                score += 10 * enemiesMovingCurrently * scoreMultiplier;
            }

            if (playerHealth < 100 && gameClock % 30 == 0 && playerHitCooldown == 0)
                adjustPlayerHealth(-1);


            gameClock++;
        }

        public void adjustPlayerHealth(int positiveAmountToSubtract)
        {
            playerHealth -= positiveAmountToSubtract;
            if (playerHealth < 0)
            {
                playerHealth = 100;
                //state = gameState.lose;
            }
            healthBar.setRecWidth(playerHealth * 2);
        }

        public void checkCollectibleCollisions()
        {
            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                if(collectibleObjectsList[i].texturesArray == enemyDecreaserImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref enemyLimit))
                {
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;
                    score += powerUpScoreWorth * scoreMultiplier;
                }
                else if(collectibleObjectsList[i].texturesArray == timeFreezerImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref enemyFreezeCooldown))
                {
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;
                    score += powerUpScoreWorth * scoreMultiplier;
                }
                else if (collectibleObjectsList[i].texturesArray == healthPackImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref playerHealth))
                {
                    adjustPlayerHealth(0);
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;

                    score += powerUpScoreWorth * scoreMultiplier;
                }

            }
        }

        public void checkEnemyCollisions()
        {
            bool didCollide = false;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (player.getRec().Intersects(enemiesList[i].getRec()) && enemyFreezeCooldown == 0)
                {
                    didCollide = true;
                    if(playerHitCooldown == 0)
                    {
                        playerHitCooldown = 90;
                        adjustPlayerHealth(37);

                    }
                }
            }

            //return didCollide;
            //return false;

        }

        public void backgroundLogic()
        {
            for (int i = 0; i < backgroundCharacterList.Count; i++)
            {
                backgroundCharacterList[i].addToRecX(-10);
                if (backgroundCharacterList[i].getRec().Right < 0)
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

                //if (backgroundCharacterList[i].getRec().Right < 0 || backgroundCharacterList[i].getRec().Left > screenEncapsulation.Right)
                //{
                //    backgroundCharacterList[i].texture.Dispose();
                //}


                    //backgroundCharacterList[i].texture = backgroundImages[i];

                

                //if (backgroundCharacterList[i % 6].texture.IsDisposed)
                //{
                //    backgroundCharacterList[i % 6].texture = backgroundImages[i % 6];
                //}

            }
        }

        public void addEnemies()
        {
            if(addEnemyCooldown % 300 == 0 && addEnemyCooldown != 0)
            {
                if (enemyLimit < enemiesList.Count && enemyFreezeCooldown == 0)
                {
                    enemyLimit++;
                }
            }
        }

        public void setEnemyStartingPos(ref List<BaseEnemy> enemyToMove, int index)
        {
            enemyToMove[index].setRecX(enemyStartingX + rand.Next(10, 280));
            enemyToMove[index].setRecY(rand.Next(10, screenHeight - defaultCharacterHeight));
            //enemyToMove[index].setRec()


        }

        public void enemyMovement()
        {
            //wallBouncer.Move(screenEncapsulation);
            if (enemyFreezeCooldown == 0)
            {
                for (int i = 0; i < activeEnemies.Count; ++i)
                {
                    activeEnemies[i].Move(screenEncapsulation);
                }
            }
        }

        public void collectibleAnimations()
        {
            didAnimate = false;
            if (gameClock % 20 == 0)
            {
                for (int i = 0; i < collectibleObjectsList.Count; i++)
                {
                    collectibleObjectsList[i].animate();
                    didAnimate = true;

                }
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


            if(shouldChoose && enemyFreezeCooldown == 0)
            {
                //if (enemyLimit < enemiesList.Count)
                //{
                //    enemyLimit++;
                //}
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
            if (enemyFreezeCooldown == 0)
            {
                for (int i = 0; i < activeEnemies.Count; ++i)
                {
                    if (activeEnemies[i].getRec().Right > screenEncapsulation.Left && activeEnemies[i].isMoving)
                    {
                        activeEnemies[i].addToRecX(sideScrollSpeed);
                    }
                }
            }

            //if (isPressingKey == false)
            //{
                for (int i = 0; i < (-1 * sideScrollSpeed / 2 + 2); ++i)
                {
                    if (player.getRecX() > screenEncapsulation.Left)
                    {
                        player.addToRecX(-1);
                    }
                }
            //}
        }

        public void drawTitleScreen()
        {

        }

        public void drawGameplay()
        {
            for (int i = 0; i < backgroundCharacterList.Count; i++)
            {
                backgroundCharacterList[i].drawCharacter(spriteBatch);

            }

            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                collectibleObjectsList[i].drawCharacter(spriteBatch);
                //spriteBatch.DrawString(scoreFont, "X: " + collectibleObjectsList[i].getRecX(), new Vector2(screenWidth - 340, i * 30 + 40), Color.Black);
                //spriteBatch.DrawString(scoreFont, "Y: " + collectibleObjectsList[i].getRecY(), new Vector2(screenWidth - 250, i * 30 + 40), Color.Black);
                //spriteBatch.DrawString(scoreFont, "Image: " + enemyDecreaserImages[i], new Vector2(screenWidth - 300, screenHeight - 30), Color.Black);

            }
            //spriteBatch.DrawString(scoreFont, "texture: " + collectibleObjectsList[0].texture, new Vector2(screenWidth - 300, screenHeight - 110), Color.Black);

            if (playerHitCooldown == 0 || playerHitCooldown % 6 != 0)
            {
                player.drawCharacter(spriteBatch, Color.Red);
            }
            healthBarOutline.drawCharacter(spriteBatch);
            healthBar.drawCharacter(spriteBatch, Color.Red);
            spriteBatch.DrawString(scoreFont, playerHealth.ToString(), new Vector2(healthBarOutline.getRec().Center.X - 12, healthBarOutline.getRec().Center.Y - 8), Color.White);



            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemyFreezeCooldown == 0 || enemyFreezeCooldown > 60 ||(enemyFreezeCooldown < 60 && enemyFreezeCooldown % 6 == 0))
                {
                    enemiesList[i].drawCharacter(spriteBatch);
                }
                //spriteBatch.DrawString(scoreFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth - 500, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(scoreFont, "Y: " + enemiesList[i].getRecY(), new Vector2(screenWidth - 400, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(scoreFont, "isMoving: " + enemiesList[i].isMoving, new Vector2(screenWidth - 300, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(testFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            }
            //spriteBatch.DrawString(testFont, "X: ", new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            //spriteBatch.DrawString(scoreFont, "gameFreezeCooldown: " + enemyFreezeCooldown, new Vector2(screenWidth - 300, screenHeight - 180), Color.Black);
            spriteBatch.DrawString(scoreFont, "activeEnemyCount: " + activeEnemies.Count, new Vector2(screenWidth - 300, screenHeight - 40), Color.Black);
            //spriteBatch.DrawString(scoreFont, "gameClock: " + gameClock, new Vector2(screenWidth - 300, screenHeight - 70), Color.Black);


            //score += 5;
            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 10, 50), Color.Black);

            //spriteBatch.DrawString(scoreFont, "didAnimate: " + didAnimate, new Vector2(screenWidth - 300, screenHeight - 150), Color.Black);

        }

        public void drawLose()
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            switch (state)
            {
                case gameState.titleScreen:
                    drawTitleScreen();
                    break;
                case gameState.gameplay:
                    drawGameplay();
                    break;
                case gameState.lose:
                    drawLose();
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
