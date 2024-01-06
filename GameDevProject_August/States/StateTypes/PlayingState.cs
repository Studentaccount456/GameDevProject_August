using GameDevProject_August.Levels;
using GameDevProject_August.Levels.Level1;
using GameDevProject_August.Levels.Level2;
using GameDevProject_August.Levels.Level3;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.FallingCode;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Terminal;
using GameDevProject_August.Sprites.DSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters;
using GameDevProject_August.States.LevelStates;
using GameDevProject_August.States.MenuStates;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States.StateTypes

{

    public class PlayingState : State
    {

        private static Level level;
        private List<Sprite> spriteList;
        protected float _timer;
        protected int nextLevelIndicator;

        public static int whichCodeFalls = 0;
        protected FallingCode fallingCode;

        public static Level Level
        {
            get { return level; }
            set { level = value; }
        }

        public List<Sprite> SpriteList
        {
            get { return spriteList; }
            set { spriteList = value; }
        }

        public Texture2D RegularPointTexture { get; private set; }

        public Texture2D personMoveTexture { get; private set; }
        public Texture2D personShootTexture { get; private set; }
        public Texture2D personIdleTexture { get; private set; }
        public Texture2D personDeathTexture { get; private set; }
        public Texture2D personStandStillTexture { get; private set; }
        public Texture2D personJumpTexture { get; private set; }
        public Texture2D personBowDownTexture { get; private set; }


        public Texture2D ratMoveTexture { get; private set; }
        public Texture2D ratCastTexture { get; private set; }
        public Texture2D ratIdleTexture { get; private set; }
        public Texture2D ratStandStillTexture { get; private set; }

        public SpriteFont fontOfScoreLoaded { get; private set; }

        public Texture2D porcupineMoveTexture { get; private set; }
        public Texture2D porcupineStandStillTexture { get; private set; }


        public Texture2D dragonflyMoveTexture { get; private set; }
        public Texture2D dragonflyStandStillTexture { get; private set; }


        public Texture2D glitchDeathTexture { get; private set; }

        public Texture2D minotaurMoveTexture { get; private set; }
        public Texture2D minotaurCastTexture { get; private set; }
        public Texture2D minotaurIdleTexture { get; private set; }
        public Texture2D minotaurStandStillTexture { get; private set; }

        public Texture2D finalTerminalTexture { get; private set; }

        public Texture2D playerBullet { get; private set; }

        public Texture2D ratProjectile { get; private set; }

        public static Texture2D GrassTexture { get; private set; }
        public static Texture2D DirtTexture { get; private set; }
        public static Texture2D StoneTexture { get; private set; }
        public static Texture2D MarmerTexture { get; private set; }
        public static Texture2D ThreePointsTexture { get; private set; }
        public static Texture2D SevenPointsTexture { get; private set; }
        public static Texture2D InvisibleBlockTexture { get; private set; }



        public Texture2D BackgroundOfLevel { get; set; }

        public Texture2D FallingCodeMinotaur { get; private set; }
        public Texture2D FallingCodeDragonFly { get; private set; }
        public Texture2D FallingCodePorcupine { get; private set; }
        public Texture2D FallingCodeRatMage { get; private set; }
        public Texture2D FallingCodePoint { get; private set; }

        public Texture2D BackgroundStandardTexture { get; private set; }
        public Texture2D BackgroundGlitchScore_0_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_1_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_2_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_3_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_4_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_5_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_6_Texture { get; private set; }
        public Texture2D BackgroundGlitchScore_7_Texture { get; private set; }

        public static bool isNextLevelTrigger = false;

        public static Random Random;

        public Player player_1;

        public static int ScreenWidth;
        public static int ScreenHeight;

        protected int levelNumber;


        public PlayingState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
                            : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;

            Random = new Random();
            LoadContent(content);

            player_1 = new Player();

            fallingCode = new FallingCode(playerBullet);
        }


        public override void LoadContent(ContentManager content)
        {
            LoadMain_Character(content);
            LoadMinotaur(content);
            LoadRatMage(content);
            LoadPorcupine(content);
            LoadDragonfly(content);
            LoadProjectiles(content);
            LoadPoints(content);
            LoadGlitchDeathEffect(content);
            LoadBlockTextures(content);
            LoadSpriteFonts(content);
            LoadFallingCodeTextures(content);
            LoadFinalTerminalTexture(content);
            LoadBackGrounds(content);
        }

        public Input arrowInput()
        {
            Input input;

            return input = new Input()
            {
                Down = System.Windows.Forms.Keys.Down,
                Up = System.Windows.Forms.Keys.Up,
                Left = System.Windows.Forms.Keys.Left,
                Right = System.Windows.Forms.Keys.Right,
                Shoot = System.Windows.Forms.Keys.Space
            };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            DrawBackground(spriteBatch);

            level.Draw(spriteBatch);

            /* Empty atm
            foreach (var component in _gameComponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
            */

            Game1.PlayerScore.Draw(spriteBatch);

            DrawSprites(spriteList, spriteBatch);

            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            /* empty atm
            foreach (var component in _gameComponents)
            {
                component.Update(gameTime);
            }
            */

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in SpriteList.ToArray())
            {
                sprite.Update(gameTime, SpriteList, Level.TileList);

                switch (whichCodeFalls)
                {
                    case 1:
                        fallingCode.LetCodeFall(SpriteList, FallingCodeMinotaur);
                        break;
                    case 2:
                        fallingCode.LetCodeFall(SpriteList, FallingCodeDragonFly);
                        break;
                    case 3:
                        fallingCode.LetCodeFall(SpriteList, FallingCodePorcupine);
                        break;
                    case 4:
                        fallingCode.LetCodeFall(SpriteList, FallingCodeRatMage);
                        break;
                    case 5:
                        fallingCode.LetCodeFall(SpriteList, FallingCodePoint);
                        break;
                    default:
                        break;
                }
                // Stop code from falling
                whichCodeFalls = 0;
            }

            PostUpdate(gameTime, SpriteList);

            if (isNextLevelTrigger)
            {
                isNextLevelTrigger = false;
                switch (nextLevelIndicator)
                {
                    case 2:
                        _game.ChangeState(new Level2State(_game, _graphicsDevice, _content, false));
                        break;
                    case 3:
                        _game.ChangeState(new Level3State(_game, _graphicsDevice, _content, false));
                        break;
                    default:
                        break;
                }
            }
        }

        public override void PostUpdate(GameTime gameTime, List<Sprite> sprites)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite_1 = sprites[i];

                if (sprite_1 is Sentient sentient && sentient.IsKilled || sprite_1 is NotSentient notSentient && notSentient.IsDestroyed)
                {
                    sprites.RemoveAt(i);
                    i--;
                }

                if (sprite_1 is Archeologist)
                {
                    var player = sprite_1 as Archeologist;
                    if (player.HasDied)
                    {
                        _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
                    }
                }

                if (sprite_1 is FinalTerminal)
                {
                    var finalTerminal = sprite_1 as FinalTerminal;
                    if (finalTerminal.IsDestroyed)
                    {
                        _game.ChangeState(new VictoryState(_game, _graphicsDevice, _content));
                    }
                }
            }
        }

        public Level GenerateLevel(Level level, int tileSize)
        {
            Level newlevel = null;

            if (level is Level1)
            {
                newlevel = new Level1(new Level1BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }
            else if (level is Level2)
            {
                newlevel = new Level2(new Level2BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }
            else if (level is Level3)
            {
                newlevel = new Level3(new Level3BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }

            return newlevel;
        }

        public void InitializeContent()
        {
            Level = GenerateLevel(Level, 38);
        }


        public virtual List<Sprite> GenerateLevelSpriteList()
        {
            List<Sprite> spriteList = new List<Sprite>();
            return spriteList;
        }



        public void DrawBackground(SpriteBatch spriteBatch)
        {
            switch (Game1.PlayerScore.MainScore)
            {
                case 0:
                    spriteBatch.Draw(BackgroundGlitchScore_0_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(BackgroundGlitchScore_1_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(BackgroundGlitchScore_2_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 3:
                    spriteBatch.Draw(BackgroundGlitchScore_3_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 4:
                    spriteBatch.Draw(BackgroundGlitchScore_4_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 5:
                    spriteBatch.Draw(BackgroundGlitchScore_5_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 6:
                    spriteBatch.Draw(BackgroundGlitchScore_6_Texture, new Vector2(0, 0), Color.White);
                    break;
                case 7:
                    spriteBatch.Draw(BackgroundGlitchScore_7_Texture, new Vector2(0, 0), Color.White);
                    break;
                default:
                    spriteBatch.Draw(BackgroundStandardTexture, new Vector2(0, 0), Color.White);
                    break;
            }
        }

        public void InitializeScore(int levelIndicator, bool isFromMainMenu)
        {
            if (isFromMainMenu)
            {
                Game1.PlayerScore = new Score(fontOfScoreLoaded, Game1.ScreenWidth, Game1.ScreenHeight);
                switch (levelIndicator)
                {
                    case 2:
                        Game1.PlayerScore.MainScore = 3;
                        break;
                    case 3:
                        Game1.PlayerScore.MainScore = 7;
                        break;
                    default:
                        Game1.PlayerScore.MainScore = 0;
                        break;
                }
            }
        }

        private void DrawSprites(List<Sprite> spriteList, SpriteBatch spriteBatch)
        {
            foreach (var sprite in spriteList)
            {
                sprite.Draw(spriteBatch);

            }
        }

        private void LoadMain_Character(ContentManager content)
        {
            personMoveTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\MainCharacter\\MC_MoveRight");
            personShootTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\MainCharacter\\MC_ShootRight"); ;
            personIdleTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\MainCharacter\\MC_Idle"); ;
            personDeathTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\MainCharacter\\MC_Dies");
            personJumpTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\MainCharacter\\MC_Jump");
            personBowDownTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\MC_BowDown");
            personStandStillTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\MC_StandStill");
        }

        private void LoadMinotaur(ContentManager content)
        {
            minotaurMoveTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\Minotaur\\Minotaur_RunRight");
            minotaurCastTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\Minotaur\\Minotaur_Attack");
            minotaurIdleTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\Minotaur\\Minotaur_Idle");
            minotaurStandStillTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\Minotaur_StandStill");
        }

        private void LoadRatMage(ContentManager content)
        {
            ratMoveTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\RatMage\\Rat_MoveRight");
            ratCastTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\RatMage\\Rat_Cast_One");
            ratIdleTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\RatMage\\Rat_Idle");
            ratStandStillTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\Rat_StandStill");
        }

        private void LoadPorcupine(ContentManager content)
        {
            porcupineMoveTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\Porcupine\\Porcupine_MoveRight");
            porcupineStandStillTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\Porcupine_StandStill");
        }

        private void LoadDragonfly(ContentManager content)
        {
            dragonflyMoveTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\DragonFly\\DragonFly_MoveRight");
            dragonflyStandStillTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\DragonFly_StandStill");
        }

        private void LoadGlitchDeathEffect(ContentManager content)
        {
            glitchDeathTexture = content.Load<Texture2D>("LivingEntityTextures\\AnimationTextures\\Death\\GlitchDeathEffect");
        }

        private void LoadProjectiles(ContentManager content)
        {
            playerBullet = content.Load<Texture2D>("InvokedEntities\\Projectiles\\GoToeBullet");
            ratProjectile = content.Load<Texture2D>("InvokedEntities\\Projectiles\\RatArrow");
        }
        private void LoadPoints(ContentManager content)
        {
            RegularPointTexture = content.Load<Texture2D>("LevelTextures\\Collectibles\\Point_Plus1");
        }

        private void LoadBlockTextures(ContentManager content)
        {
            GrassTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\DirtAboveGround");
            DirtTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\DirtUnderGround");
            StoneTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\Stone");
            MarmerTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\Marmer");
            ThreePointsTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\ThreePointsTexture");
            SevenPointsTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\SevenPointsTexture");
            InvisibleBlockTexture = content.Load<Texture2D>("LevelTextures\\Tiles\\InvisibleBlock");
        }

        private void LoadSpriteFonts(ContentManager content)
        {
            fontOfScoreLoaded = content.Load<SpriteFont>("MenuTextures\\Fonts\\Font_Score");
        }

        private void LoadFallingCodeTextures(ContentManager content)
        {
            FallingCodeMinotaur = content.Load<Texture2D>("InvokedEntities\\FallingCode\\FallingCode_Minotaur");
            FallingCodeDragonFly = content.Load<Texture2D>("InvokedEntities\\FallingCode\\FallingCode_DragonFly");
            FallingCodePorcupine = content.Load<Texture2D>("InvokedEntities\\FallingCode\\FallingCode_Porcupine");
            FallingCodeRatMage = content.Load<Texture2D>("InvokedEntities\\FallingCode\\FallingCode_RatMage");
            FallingCodePoint = content.Load<Texture2D>("InvokedEntities\\FallingCode\\FallingCode_Point");
        }

        private void LoadFinalTerminalTexture(ContentManager content)
        {
            finalTerminalTexture = content.Load<Texture2D>("LivingEntityTextures\\SpritesStationaryTextures\\Terminal");
        }

        private void LoadBackGrounds(ContentManager content)
        {
            BackgroundStandardTexture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackGround_Standard");
            BackgroundGlitchScore_0_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_0");
            BackgroundGlitchScore_1_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_1");
            BackgroundGlitchScore_2_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_2");
            BackgroundGlitchScore_3_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_3");
            BackgroundGlitchScore_4_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_4");
            BackgroundGlitchScore_5_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_5");
            BackgroundGlitchScore_6_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_6");
            BackgroundGlitchScore_7_Texture = content.Load<Texture2D>("LevelTextures\\BackGrounds\\GlitchBackground\\BackgroundScore_7");
        }
    }
}

