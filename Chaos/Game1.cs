using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

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

        Song ambientMusic;
        SoundEffect healthSound, hurtSound;


        int numOfEachEnemyType = 5;
        

        Character screenEncapsulation;

        Random rand = new Random();

        int screenHeight = 800;
        int screenWidth = 1280;

        //bool shouldEnemiesLoop = false;

        int defaultCharacterHeight = 50;
        int defaultCharacterWidth = 50;

        int currentCharacterHeight = 50;
        int currentCharacterWidth = 50;

        int powerUpScoreWorth = 300;
        double scoreMultiplier = 1;
        int powerUpSpawnFrequency = 120;

        int powerUpSpawnAttemptsFailed = 0;

        int playerHitCooldown = 0;
        int addEnemyCooldown = 0;

        bool didAnimate = true;

        int addEnemyInterval = 300;

        int playerSpeed = 7;
        bool isPressingKey = false;

        int currentCollectiblesOnScreen = 0;
        int maxCollectiblesOnScreen = 3;

        int enemyFreezeCooldown = 0;
        int sideScrollSpeed = -7;

        bool isGamePaused = false;
        bool hasLost = false;

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

            ambientMusic = Content.Load<Song>("shortGameJamMusic2");
            healthSound = Content.Load<SoundEffect>("healthSound2");
            hurtSound = Content.Load<SoundEffect>("hurtSound3");

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

            //screenEncapsulation.getRec() = new Rectangle(0, 0, screenWidth, screenHeight);
            screenEncapsulation = new Bouncer(Content.Load<Texture2D>("whiteSquare"), new Rectangle(0, 0, screenWidth, screenHeight));

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

        public void sound()
        {
            //MediaPlayer.IsRepeating = true;

        }

        public void gameplay()
        {
            startOfGameCode();
            userControls();


            if (isGamePaused == false && hasLost == false)
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
                increaseDifficulty();
                endOfTickCode();
            }

        }

        public void increaseDifficulty()
        {
            if (gameClock > 1000 && gameClock % 250 == 0 && gameClock != 0)
            {
                if (gameClock % 500 == 0)
                {
                    playerSpeed += 1;

                    if (powerUpSpawnFrequency > 100)
                    {
                        powerUpSpawnFrequency -= 5;

                    }
                    if (maxCollectiblesOnScreen < 5)
                    {
                        maxCollectiblesOnScreen += 1;
                    }

                    if (enemyLimit < enemiesList.Count)
                    {
                        enemyLimit++;
                    }
                }


                
                if (screenEncapsulation.getRec().Height + 10 > defaultCharacterHeight)
                {
                    screenEncapsulation.shrinkUniformly(2);
                    player.adjustToBeInBounds(screenEncapsulation.getRec());
                }


            }

            if(gameClock > 1000 && gameClock != 0 && gameClock % 400 == 0)
            {
                //if (enemyLimit < enemiesList.Count)
                //{
                //    enemyLimit++;
                //}
            }
        }

        public void lose()
        {
            if(hasLost)
            {

            }
        }

        public void getInput()
        {


            Window.TextInput += Window_TextInput;
        }

        public void resetGame()
        {

        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            var pressedKeys = e.Key;
            var singleChar = e.Character;

            //if(Window.TextInput == "t")
            //{

            //}
        }

        public void spawnCollectible()
        {
            if (gameClock % powerUpSpawnFrequency == 0 && gameClock != 0 && currentCollectiblesOnScreen < maxCollectiblesOnScreen)
            {
                randomDecider = rand.Next(1, 4);
                if (randomDecider == 1 || powerUpSpawnAttemptsFailed >= 2)
                {
                    powerUpSpawnAttemptsFailed = 0;
                    randomDecider = rand.Next(0, collectibleObjectsList.Count);
                    while (collectibleObjectsList[randomDecider].isOnScreen == true)
                    {
                        randomDecider = rand.Next(0, collectibleObjectsList.Count);

                    }
                    currentCollectiblesOnScreen++;
                    collectibleObjectsList[randomDecider].isOnScreen = true;
                    setCharacterPos(ref collectibleObjectsList, randomDecider);
                }
                else
                {
                    //powerUpSpawnAttemptsFailed += 1;
                }

            }
        }

        public void setCharacterPos(ref List<BaseCollectible> listWithObject, int index)
        {
            randomDecider = rand.Next(screenEncapsulation.getRec().Left, screenEncapsulation.getRec().Right - listWithObject[index].lengthX);
            listWithObject[index].setRecX(randomDecider);
            randomDecider = rand.Next(screenEncapsulation.getRec().Top, screenEncapsulation.getRec().Bottom - listWithObject[index].lengthY);
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

            if (isGamePaused == true)
            {
                return;
            }
            #region Movement

            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed; i++)
                {
                    if (player.getRec().Top - 1 >= screenEncapsulation.getRec().Top)
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
                    if (player.getRec().Bottom + 1 <= screenEncapsulation.getRec().Bottom)
                    {
                        player.addToRecY(1);
                    }
                }
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                isPressingKey = true;

                for (int i = 0; i < playerSpeed; i++)
                {
                    if (player.getRec().Left - 1 >= screenEncapsulation.getRec().Left)
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
                    if (player.getRec().Right + 1 <= screenEncapsulation.getRec().Right)
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
                //MediaPlayer.Play(ambientMusic);
                //MediaPlayer.IsRepeating = true;

                MediaPlayer.Play(ambientMusic);
                MediaPlayer.IsRepeating = true;



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
            if (enemyFreezeCooldown <= 90)
            {
                addEnemyCooldown++;
            }
            if (enemyFreezeCooldown > 0)
            {
                enemyFreezeCooldown--;
            }
            if (gameClock % 6 == 0 && gameClock != 0 && hasLost == false)
            {
                score += 1 * enemiesMovingCurrently * scoreMultiplier;
            }

            if (playerHealth < 100 && gameClock % 30 == 0 && playerHitCooldown == 0)
                adjustPlayerHealth(-1);


            gameClock++;
        }

        public void adjustPlayerHealth(int positiveAmountToSubtract)
        {
            playerHealth -= positiveAmountToSubtract;
            if (playerHealth <= 0)
            {
                playerHealth = 100;
                //state = gameState.lose;
                hasLost = true;
            }
            healthBar.setRecWidth(playerHealth * 2);
        }

        public void checkCollectibleCollisions()
        {
            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                if (collectibleObjectsList[i].texturesArray == enemyDecreaserImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref enemyLimit))
                {
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;
                    healthSound.Play();

                    score += powerUpScoreWorth * scoreMultiplier;
                }
                else if (collectibleObjectsList[i].texturesArray == timeFreezerImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref enemyFreezeCooldown))
                {
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;
                    healthSound.Play();
                    score += powerUpScoreWorth * scoreMultiplier;
                }
                else if (collectibleObjectsList[i].texturesArray == healthPackImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref playerHealth))
                {
                    adjustPlayerHealth(0);
                    healthSound.Play();

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
                    if (playerHitCooldown == 0)
                    {
                        hurtSound.Play();
                        playerHitCooldown = 150;
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

                //if (backgroundCharacterList[i].getRec().Right < 0 || backgroundCharacterList[i].getRec().Left > screenEncapsulation.getRec().Right)
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
            if (addEnemyCooldown % addEnemyInterval == 0 && addEnemyCooldown != 0)
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
            enemyToMove[index].setRecY(rand.Next(screenEncapsulation.getRec().Top, screenEncapsulation.getRec().Bottom- defaultCharacterHeight));
            //enemyToMove[index].setRec()


        }

        public void enemyMovement()
        {
            //wallBouncer.Move(screenEncapsulation.getRec());
            if (enemyFreezeCooldown == 0)
            {
                for (int i = 0; i < activeEnemies.Count; ++i)
                {
                    activeEnemies[i].Move(screenEncapsulation.getRec(), ref playerSpeed);
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
                if (activeEnemies[i].getRec().Right <= screenEncapsulation.getRec().Left)
                {
                    activeEnemies[i].isMoving = false;
                    //activeEnemies.RemoveAt(i);
                    setEnemyStartingPos(ref activeEnemies, i);

                    activeEnemies.RemoveAt(i);

                    shouldChoose = true;
                }
            }


            if (shouldChoose && enemyFreezeCooldown == 0)
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
                    if (activeEnemies[i].getRec().Right > screenEncapsulation.getRec().Left && activeEnemies[i].isMoving)
                    {
                        activeEnemies[i].addToRecX(sideScrollSpeed);
                    }
                }
            }

            //if (isPressingKey == false)
            //{
            for (int i = 0; i < (-1 * sideScrollSpeed / 2 + 2); ++i)
            {
                if (player.getRecX() > screenEncapsulation.getRec().Left)
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
            screenEncapsulation.drawCharacter(spriteBatch);


            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                collectibleObjectsList[i].drawCharacter(spriteBatch);
                //spriteBatch.DrawString(testFont, "X: " + collectibleObjectsList[i].getRecX(), new Vector2(screenWidth - 340, i * 30 + 40), Color.Black);
                //spriteBatch.DrawString(testFont, "Y: " + collectibleObjectsList[i].getRecY(), new Vector2(screenWidth - 250, i * 30 + 40), Color.Black);
                //spriteBatch.DrawString(testFont, "Image: " + enemyDecreaserImages[i], new Vector2(screenWidth - 300, screenHeight - 30), Color.Black);

            }
            //spriteBatch.DrawString(testFont, "texture: " + collectibleObjectsList[0].texture, new Vector2(screenWidth - 300, screenHeight - 110), Color.Black);


            healthBarOutline.drawCharacter(spriteBatch);
            healthBar.drawCharacter(spriteBatch, Color.Red);
            spriteBatch.DrawString(testFont, playerHealth.ToString(), new Vector2(healthBarOutline.getRec().Center.X - 12, healthBarOutline.getRec().Center.Y - 8), Color.White);



            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemyFreezeCooldown == 0 || enemyFreezeCooldown > 60 || (enemyFreezeCooldown < 60 && enemyFreezeCooldown % 6 == 0))
                {
                    enemiesList[i].drawCharacter(spriteBatch);
                }
                //spriteBatch.DrawString(testFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth - 500, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(testFont, "Y: " + enemiesList[i].getRecY(), new Vector2(screenWidth - 400, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(testFont, "isMoving: " + enemiesList[i].isMoving, new Vector2(screenWidth - 300, 300 + i * 20), Color.Black);
                //spriteBatch.DrawString(testFont, "X: " + enemiesList[i].getRecX(), new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            }
            //spriteBatch.DrawString(testFont, "X: ", new Vector2(screenWidth * 2 / 3, screenHeight * 10 / 9), Color.Black);

            spriteBatch.DrawString(testFont, "playerSpeed: " + playerSpeed, new Vector2(screenWidth - 300, screenHeight - 180), Color.White);
            spriteBatch.DrawString(testFont, "activeEnemyCount: " + activeEnemies.Count, new Vector2(screenWidth - 300, screenHeight - 40), Color.White);
            spriteBatch.DrawString(testFont, "gameClock: " + gameClock, new Vector2(screenWidth - 300, screenHeight - 70), Color.White);


            //score += 5;
            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 30, 50), Color.White);
            if(hasLost)
            spriteBatch.DrawString(scoreFont, "Your health has hit 0 ", new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 30, 150), Color.White);
            //spriteBatch.DrawString(scoreFont, Window_TextInput(), new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 30, 190), Color.White);

            //spriteBatch.DrawString(testFont, "didAnimate: " + didAnimate, new Vector2(screenWidth - 300, screenHeight - 150), Color.Black);

            if (playerHitCooldown == 0 || playerHitCooldown % 6 != 0)
            {
                player.drawCharacter(spriteBatch, Color.Red);
            }

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
