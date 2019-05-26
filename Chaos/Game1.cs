using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Linq;

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

        //List<Character> backgroundCharacterList = new List<Character>(6);
        Texture2D[] backgroundImages1 = new Texture2D[3];
        Texture2D[] backgroundImages2 = new Texture2D[3];
        Texture2D[] enemyDecreaserImages = new Texture2D[2];
        Texture2D[] timeFreezerImages = new Texture2D[2];
        Texture2D[] healthPackImages = new Texture2D[2];
        Texture2D[] expanderImages = new Texture2D[2];

        Song ambientMusic;
        SoundEffect healthSound, hurtSound;

        Character backgroundCharacter1, backgroundCharacter2;


        int numOfEachEnemyType = 4;


        Character screenEncapsulation;

        Random rand = new Random();

        Keys[] allKeys = new Keys[26];

        int screenHeight = 800;
        int screenWidth = 1280;

        bool isExpanderGrown = false;

        int gameStartCooldown = 240;

        int defaultCharacterHeight = 50;
        int defaultCharacterWidth = 50;

        int powerUpScoreWorth = 300;
        int scoreMultiplier = 1;
        int powerUpSpawnFrequency = 120;

        bool isDoneEnteringScore = false;

        bool shouldEnterName = false;

        int powerUpSpawnAttemptsFailed = 0;

        int playerHitCooldown = 0;
        int addEnemyCooldown = 0;

        bool didAnimate = true;

        bool hasInsertedScore = false;

        int addEnemyInterval = 300;

        int playerSpeed = 7;
        bool isPressingKey = false;

        int[] highScores = new int[3];
        string[] playerNames = new string[3];

        int currentCollectiblesOnScreen = 0;
        int maxCollectiblesOnScreen = 3;

        int enemyFreezeCooldown = 0;
        int sideScrollSpeed = -7;

        string playerName = "";

        bool isGamePaused = false;
        bool hasLost = false;

        int score = 0;

        int playerHealth = 100;

        int gameClock = 0;

        int enemyStartingX = 0;

        int enemyLimit = 5;

        bool hasDoneStartOfGameCode = false;
        int enemiesMovingCurrently = 0;
        int randomDecider;

        enum gameState
        {
            titleScreen, gameplay, lose, highScoreScreen
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
            enemyStartingX = screenWidth + (screenWidth / 10);
            for (int i = 0; i < 26; i++)
            {
                allKeys[i] = (Keys)(65 + i);
            }

            //Chaos.Properties.Settings.Default.highScore1 = 30000;
            //Chaos.Properties.Settings.Default.highScore2 = 20000;
            //Chaos.Properties.Settings.Default.highScore3 = 10000;

            //Chaos.Properties.Settings.Default.names = "ShapeMaster_TimeGuy_Jukes_";


            //Chaos.Properties.Settings.Default.Save();
            //Chaos.Properties.Settings.Default.names = "";
            highScores[0] = Chaos.Properties.Settings.Default.highScore1;
            highScores[1] = Chaos.Properties.Settings.Default.highScore2;
            highScores[2] = Chaos.Properties.Settings.Default.highScore3;


            for (int i = 0; i < highScores.Length; i++)
            {
                //Chaos.Properties.Settings.Default.names += playerNames[i] + "_";

            }

            string settingEntry = Chaos.Properties.Settings.Default.names;
            int test = Chaos.Properties.Settings.Default.highScore1;

            int underScoreIndex = 0;
            underScoreIndex = settingEntry.IndexOf("_", underScoreIndex);
            int currentPos = 0;
            for (int i = 0; i < highScores.Length; i++)
            {
                playerNames[i] = settingEntry.Substring(currentPos, underScoreIndex - currentPos);
                currentPos += underScoreIndex - currentPos + 1;
                underScoreIndex = settingEntry.IndexOf("_", underScoreIndex + 1);

            }


            Chaos.Properties.Settings.Default.Save();


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


            player = new Character(Content.Load<Texture2D>("buttonOutline"), new Rectangle((screenWidth / 2) - (defaultCharacterWidth / 2),
                screenHeight / 2 - defaultCharacterHeight / 2, defaultCharacterWidth, defaultCharacterHeight));

            healthBar = new Character(Content.Load<Texture2D>("blankSquare"), new Rectangle(screenWidth - 250,
                40, defaultCharacterWidth * 4, defaultCharacterHeight - 10));

            healthBarOutline = new Character(Content.Load<Texture2D>("blackSquare"), new Rectangle(screenWidth - 255,
                35, defaultCharacterWidth * 4 + 10, defaultCharacterHeight));


            backgroundImages1[0] = Content.Load<Texture2D>("redOrange");
            backgroundImages2[0] = Content.Load<Texture2D>("orangeYellow");
            backgroundImages1[1] = Content.Load<Texture2D>("yellowGreen");
            backgroundImages2[1] = Content.Load<Texture2D>("greenBlue");
            backgroundImages1[2] = Content.Load<Texture2D>("bluePurple");
            backgroundImages2[2] = Content.Load<Texture2D>("purpleRed");

            backgroundCharacter1 = new Character(new Rectangle(1400 * 0, 0, 1400, screenHeight), ref backgroundImages1);
            backgroundCharacter2 = new Character(new Rectangle(1400 * 1, 0, 1400, screenHeight), ref backgroundImages2);


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

            for (int i = 0; i < expanderImages.Length; i++)
            {
                expanderImages[i] = Content.Load<Texture2D>("expander" + (i + 1));
            }

            

            ambientMusic = Content.Load<Song>("shortGameJamMusic2");
            healthSound = Content.Load<SoundEffect>("healthSound2");
            hurtSound = Content.Load<SoundEffect>("hurtSound3");

            testFont = Content.Load<SpriteFont>("testFont");
            scoreFont = Content.Load<SpriteFont>("scoreFont");

            EnemyDecreaser tempEnemyDecreaser;
            TimeFreezer tempTimeFreezer;
            HealthPack tempHealthPack;
            Expander tempExpander;

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

                //tempEnemyDecreaser.texture.Dispose();
                //tempTimeFreezer.texture.Dispose();
                //tempHealthPack.texture.Dispose();


            }
            for (int i = 0; i < maxCollectiblesOnScreen; i++)
            {
                tempExpander = new Expander(expanderImages[0], new Rectangle(screenWidth + 50,
                rand.Next(10, screenHeight - defaultCharacterHeight), defaultCharacterWidth * 3 / 2, defaultCharacterHeight * 3 / 2), expanderImages);
                collectibleObjectsList.Add(tempExpander);

                //tempExpander.texture.Dispose();

            }

            tempEnemyDecreaser = null;
            tempTimeFreezer = null;
            tempHealthPack = null;
            tempExpander = null;

            //tempEnemyDecreaser.texture.Dispose();
            //tempTimeFreezer.texture.Dispose();
            //tempHealthPack.texture.Dispose();
            //tempExpander.texture.Dispose();

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
                //tempBouncer.texture.Dispose();
                //tempMiniBouncer.texture.Dispose();
                //tempMissile.texture.Dispose();
                tempBouncer = null;
                tempMiniBouncer = null;
                tempMissile = null;

            }

            //tempBouncer = null;
            //tempMiniBouncer = null;
            //tempMissile = null;

            screenEncapsulation = new Character(Content.Load<Texture2D>("whiteSquare"), new Rectangle(0, 0, screenWidth, screenHeight));

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
                case gameState.highScoreScreen:
                    highScoreScreen();
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

        public void startOfGameCode()
        {
            if (hasDoneStartOfGameCode == false)
            {

                MediaPlayer.Play(ambientMusic);
                MediaPlayer.IsRepeating = true;

                //backgroundCharacter2.currentFrame = 1;

                chooseEnemiesToMove();
                hasDoneStartOfGameCode = true;
                //Chaos.Properties.Settings.Default.highScore1 = 40000;
                //Chaos.Properties.Settings.Default.Save();


                //highScores[0] = Chaos.Properties.Settings.Default.highScore1;
                //highScores[1] = Chaos.Properties.Settings.Default.highScore2;
                //highScores[2] = Chaos.Properties.Settings.Default.highScore3;

                //string settingEntry = Chaos.Properties.Settings.Default.names;

                //Chaos.Properties.Settings.Default.names = "TimeFreezer_ShapeMaster_Jukes_";
                //Chaos.Properties.Settings.Default.highScore1 = 8;
                //Chaos.Properties.Settings.Default.Upgrade();

                //highScores[1] = Chaos.Properties.Settings.Default.highScore2;

                //Chaos.Properties.Settings.Default.names = "ShapeMaster_TimeFreezer_Jukes_";

                //Chaos.Properties.Settings.Default.highScore1 = 30000;

                //Chaos.Properties.Settings.Default.highScore2 = 20000;
                //Chaos.Properties.Settings.Default.highScore3 = 8000;

                //Chaos.Properties.Settings.Default.Save();

                //for (int i = 0; i < highScores.Length; i++)
                //{
                //    Chaos.Properties.Settings.Default.names += playerNames[i] + "_";

                //}

                //Chaos.Properties.Settings.Default.Save();

                //string settingEntry = Chaos.Properties.Settings.Default.names;
                //int test= Chaos.Properties.Settings.Default.highScore1;

                //int underScoreIndex = 0;
                //underScoreIndex = settingEntry.IndexOf("_", underScoreIndex);
                //int currentPos = 0;
                //for (int i = 0; i < highScores.Length; i++)
                //{
                //    playerNames[i] = settingEntry.Substring(currentPos, underScoreIndex - currentPos);
                //    currentPos += underScoreIndex - currentPos + 1;
                //    underScoreIndex = settingEntry.IndexOf("_", underScoreIndex + 1);

                //}



            }
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
            else
            {
                MediaPlayer.Pause();
                
            }

        }

        public void increaseDifficulty()
        {
            if (gameClock > 1000 && gameClock % 250 == 0 && gameClock != 0)
            {
                if (gameClock % 500 == 0)
                {
                    if (playerSpeed < 16)
                        playerSpeed += 1;

                    if (powerUpSpawnFrequency > 100)
                    {
                        powerUpSpawnFrequency -= 10;

                    }
                    if (maxCollectiblesOnScreen < 6)
                    {
                        maxCollectiblesOnScreen += 1;
                    }

                    //if (enemyLimit < enemiesList.Count)
                    //{
                    //    enemyLimit++;
                    //}
                }

            }
            if (gameClock > 1000 && gameClock % 250 == 0)
            {
                if (enemyLimit <= 5)
                {
                    enemyLimit++;
                }
            }
            if (gameClock % 1000 == 0 && gameClock != 0)
            {
                scoreMultiplier += 1;
            }

            if (gameClock > 1000 && gameClock != 0 && gameClock % 120 == 0)
            {
                //if (enemyLimit < enemiesList.Count)
                //{
                //    enemyLimit++;
                //}

                if (screenEncapsulation.getRec().Height + 10 > defaultCharacterHeight * 8 && enemyFreezeCooldown == 0)
                {
                    screenEncapsulation.shrinkUniformly(2);
                    player.adjustToBeInBounds(screenEncapsulation.getRec());
                }
            }
        }

        public void highScoreScreen()
        {
            
            if(score >= highScores[highScores.Length - 1] && isDoneEnteringScore == false)
            {
                shouldEnterName = true;
                enterHighScore();
            }
            else if (hasInsertedScore == false)
            {
                for (int i = 0; i < highScores.Length; i++)
                {
                    if (score > highScores[i])
                    {

                        for (int j = highScores.Length - 1; j > i; j--)
                        {
                            highScores[j] = highScores[j - 1];
                            playerNames[j] = playerNames[j - 1];

                        }
                        highScores[i] = score;
                        playerNames[i] = playerName;
                        i = highScores.Length;
                    }
                }
                Chaos.Properties.Settings.Default.names = "";
                Chaos.Properties.Settings.Default.highScore1 = highScores[0];
                Chaos.Properties.Settings.Default.highScore2 = highScores[1];
                Chaos.Properties.Settings.Default.highScore3 = highScores[2];
                for (int i = 0; i < highScores.Length; i++)
                {
                    Chaos.Properties.Settings.Default.names += playerNames[i] + "_";

                }

                Chaos.Properties.Settings.Default.Save();

                hasInsertedScore = true;

            }

            else if(hasInsertedScore || score < highScores[highScores.Length - 1])
            {
                if (kb.IsKeyDown(Keys.Enter) && oldkb.IsKeyUp(Keys.Enter))
                {
                    resetGame();
                    state = gameState.gameplay;
                }
            }

        }

        public void lose()
        {
            if (hasLost)
            {
                MediaPlayer.Stop();
            }
            if(kb.IsKeyDown(Keys.Enter) && oldkb.IsKeyUp(Keys.Enter))
            {
                state = gameState.highScoreScreen;
            }
        }

        public void resetGame()
        {
            addEnemyCooldown = 0;
            addEnemyInterval = 300;
            adjustPlayerHealth(-100);
            currentCollectiblesOnScreen = 0;
            defaultCharacterHeight = 50;
            defaultCharacterWidth = 50;
            didAnimate = true;
            enemiesMovingCurrently = 0;
            enemyFreezeCooldown = 0;
            enemyLimit = 5;
            enemyStartingX = screenWidth + (screenWidth / 10);

            gameClock = 0;
            gameStartCooldown = 240;
            hasDoneStartOfGameCode = false;
            hasInsertedScore = false;
            hasLost = false;
            isDoneEnteringScore = false;
            isExpanderGrown = false;
            isGamePaused = false;
            isPressingKey = false;
            maxCollectiblesOnScreen = 3;
            playerHitCooldown = 0;
            playerSpeed = 7;
            powerUpScoreWorth = 300;
            powerUpSpawnAttemptsFailed = 0;
            powerUpSpawnFrequency = 120;
            score = 0;
            scoreMultiplier = 1;
            screenWidth = 1280;
            shouldEnterName = false;
            sideScrollSpeed = -7;
            screenHeight = 800;


            player.setRecX(screenWidth / 2);
            player.setRecY(screenHeight/ 2);
            //screenEncapsulation = new Bouncer(Content.Load<Texture2D>("whiteSquare"), new Rectangle(0, 0, screenWidth, screenHeight));
            screenEncapsulation.setRecX(0);
            screenEncapsulation.setRecY(0);
            screenEncapsulation.setRecWidth(screenWidth);
            screenEncapsulation.setRecHeight(screenHeight);

            //powerUpSpawnFrequency = 120;
            //powerUpSpawnAttemptsFailed = 0;
            //playerHitCooldown = 0;
            //addEnemyCooldown = 0;
            //addEnemyInterval = 300;
            //playerSpeed = 7;
            //isPressingKey = false;
            //currentCollectiblesOnScreen = 0;
            //maxCollectiblesOnScreen = 3;
            //enemyFreezeCooldown = 0;
            //isGamePaused = false;
            //hasLost = false;
            //score = 0;
            //adjustPlayerHealth(-100);

            //hasInsertedScore = false;

            //gameClock = 0;
            ////enemyStartingX = 0;
            //hasDoneStartOfGameCode = false;
            //enemyLimit = 5;
            //hasDoneStartOfGameCode = false;
            //enemiesMovingCurrently = 0;

            for (int i = 0; i < enemiesList.Count; i++)
            {
                //enemiesList[i].isMoving = false;
                enemiesList[i].setRecX(enemyStartingX + rand.Next(10, 80));

            }

            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                setCharacterPosOffScreen(ref collectibleObjectsList, i);
            }



        }

        public void enterHighScore()
        {
            //Keys[] pressedKeys = kb.GetPressedKeys();
            //Keys[] lastPressedKeys = new Keys[0];

            foreach (Keys key in allKeys)
            {
                if(kb.IsKeyDown(Keys.Enter) && oldkb.IsKeyUp(Keys.Enter) && playerName.Length > 0)
                {
                    isDoneEnteringScore = true;
                }
                else if(kb.IsKeyDown(key) && oldkb.IsKeyUp(key))
                {
                    if (playerName.Length <= 12)
                    {
                        if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift))
                        {
                            playerName += key.ToString();

                        }
                        else
                        {
                            playerName += key.ToString().ToLower();

                        }

                    }
                    
                }

            }
            if (kb.IsKeyDown(Keys.Back) && oldkb.IsKeyUp(Keys.Back))
            {
                if (playerName.Length > 0)
                {
                    playerName = playerName.Substring(0, playerName.Length - 1);
                }

            }

        }

        public void spawnCollectible()
        {
            int amountToSubtract = 3;
            if(gameClock > 1000 && screenEncapsulation.getRec().Height + 10 < screenHeight)
            {
                amountToSubtract = 0;
            }

            if (gameClock % powerUpSpawnFrequency == 0 && gameClock != 0 && currentCollectiblesOnScreen < maxCollectiblesOnScreen)
            {
                randomDecider = rand.Next(1, 4);
                if (randomDecider == 1 || powerUpSpawnAttemptsFailed >= 2)
                {
                    powerUpSpawnAttemptsFailed = 0;
                    randomDecider = rand.Next(0, collectibleObjectsList.Count - amountToSubtract);
                    while (collectibleObjectsList[randomDecider].isOnScreen == true)
                    {
                        randomDecider = rand.Next(0, collectibleObjectsList.Count - amountToSubtract);

                    }
                    currentCollectiblesOnScreen++;
                    collectibleObjectsList[randomDecider].isOnScreen = true;
                    setCharacterPos(ref collectibleObjectsList, randomDecider);
                }
                else
                {
                    powerUpSpawnAttemptsFailed += 1;
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

                if(MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
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
                //playerHealth = 100;
                //state = gameState.lose;
                hasLost = true;
                state = gameState.lose;
                healthBar.setRecWidth(0);

            }
            else
            {
                healthBar.setRecWidth(playerHealth * 2);
            }
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
                if (collectibleObjectsList[i].texturesArray == timeFreezerImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref enemyFreezeCooldown))
                {
                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;
                    healthSound.Play();
                    score += powerUpScoreWorth * scoreMultiplier;
                }
                if (collectibleObjectsList[i].texturesArray == healthPackImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref playerHealth))
                {
                    adjustPlayerHealth(0);
                    healthSound.Play();

                    setCharacterPosOffScreen(ref collectibleObjectsList, i);
                    currentCollectiblesOnScreen--;

                    score += powerUpScoreWorth * scoreMultiplier;
                }
                if (collectibleObjectsList[i].texturesArray == expanderImages && collectibleObjectsList[i].OnIntersect(player.getRec(), ref playerHealth))
                {
                    healthSound.Play();
                    screenEncapsulation.shrinkUniformly(-10);

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

            if (backgroundCharacter1.getRec().Right < 0)
            {

                backgroundCharacter1.setRecX(backgroundCharacter2.getRec().Right);
                backgroundCharacter1.animate();

                //backgroundCharacterList[i].setRecX(backgroundCharacterList[i - 1].getRec().Right);
            }

            if (backgroundCharacter2.getRec().Right < 0)
            {

                backgroundCharacter2.setRecX(backgroundCharacter1.getRec().Right);
                backgroundCharacter2.animate();


                //backgroundCharacterList[i].setRecX(backgroundCharacterList[i - 1].getRec().Right);
            }

            backgroundCharacter1.addToRecX(-10);
            backgroundCharacter2.addToRecX(-10);

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
            enemyToMove[index].setRecY(rand.Next(screenEncapsulation.getRec().Top +
                defaultCharacterHeight, screenEncapsulation.getRec().Bottom - defaultCharacterHeight));
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

                    if (collectibleObjectsList[i].texturesArray == expanderImages)
                    {
                        if (isExpanderGrown == false)
                        {
                            collectibleObjectsList[i].shrinkUniformly(-6);
                            isExpanderGrown = true;
                        }
                        else
                        {
                            collectibleObjectsList[i].shrinkUniformly(6);
                            isExpanderGrown = false;
                        }

                    }

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

            backgroundCharacter1.drawCharacter(spriteBatch);
            backgroundCharacter2.drawCharacter(spriteBatch);

            //for (int i = 0; i < backgroundCharacterList.Count; i++)
            //{
            //    backgroundCharacterList[i].drawCharacter(spriteBatch);

            //}
            screenEncapsulation.drawCharacter(spriteBatch);

            //for (int i = 0; i < highScores.Length; i++)
            //{
            //    spriteBatch.DrawString(testFont, playerNames[i], new Vector2(screenWidth - 340, i * 30 + 100), Color.White);

            //}


            for (int i = 0; i < collectibleObjectsList.Count; i++)
            {
                if(collectibleObjectsList[i].texturesArray == expanderImages)
                {
                    collectibleObjectsList[i].drawCharacter(spriteBatch, Color.Orange);

                }
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

            //spriteBatch.DrawString(testFont, "nameFromFile " + Chaos.Properties.Settings.Default.names, new Vector2(screenWidth - 400, screenHeight - 180), Color.White);
            //spriteBatch.DrawString(testFont, "activeEnemyCount: " + activeEnemies.Count, new Vector2(screenWidth - 300, screenHeight - 40), Color.White);
           // spriteBatch.DrawString(testFont, "gameClock: " + gameClock, new Vector2(screenWidth - 300, screenHeight - 70), Color.White);
            
            
            //score += 5;
            spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 30, 50), Color.White);
            if (hasLost)
                spriteBatch.DrawString(scoreFont, "Your health has hit 0 ", new Vector2(((screenWidth / 2) - ((score.ToString().Length / 2) * 10)) - 30, 150), Color.White);

            if (playerHitCooldown == 0 || playerHitCooldown % 6 != 0)
            {
                player.drawCharacter(spriteBatch, Color.Red);
            }

        }

        public void drawLose()
        {
            //if (hasInsertedScore == false)
            //{
            //    spriteBatch.DrawString(testFont, "Enter name: " + playerName, new Vector2(screenWidth / 2 - 70, screenHeight / 2 - 40), Color.Black);
            //}
            //else
            //{
            //    for (int i = 0; i < highScores.Length; i++)
            //    {
            //        spriteBatch.DrawString(testFont, playerName, new Vector2(screenWidth / 2 - 70, screenHeight / 2 - 40), Color.Black);

            //    }
            //}

            int seconds = gameClock / 60;

            if(gameClock > 3600)
            {
                seconds = (gameClock / 3600) / 60;
            }

            spriteBatch.DrawString(scoreFont, "You survived for " + gameClock / 3600 + " minute(s)\n  and " + seconds + " second(s)",
                new Vector2(screenWidth / 2 - 170, screenHeight / 2 - 180), Color.White);

            spriteBatch.DrawString(scoreFont, "Press Enter", new Vector2(screenWidth - 170, screenHeight - 40), Color.White);


        }

        public void drawHighScoreScreen()
        {
            if (shouldEnterName)
            {
                spriteBatch.DrawString(scoreFont, "Enter name: " + playerName, new Vector2(screenWidth / 2 - 130, screenHeight - 80), Color.White);

            }

            if (isDoneEnteringScore)
            {
                spriteBatch.DrawString(scoreFont, "Press Enter", new Vector2(screenWidth - 170, screenHeight - 40), Color.White);
            }
            else
            {
                spriteBatch.DrawString(scoreFont, "Press Enter\nwhen done", new Vector2(screenWidth - 190, screenHeight - 100), Color.White);

            }

            spriteBatch.DrawString(scoreFont, "Your Score: " + score, new Vector2(screenWidth / 2 - 130, screenHeight /2 - 110), Color.White);



            for (int i = 0; i < highScores.Length; i++)
            {
                spriteBatch.DrawString(scoreFont, i + 1 + ". " + playerNames[i], new Vector2(screenWidth / 2 - 220, i * 40 + 40), Color.White);
                spriteBatch.DrawString(scoreFont, highScores[i].ToString(), new Vector2(screenWidth / 2 + 40, i * 40 + 40), Color.White);


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
                case gameState.highScoreScreen:
                    drawHighScoreScreen();
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
