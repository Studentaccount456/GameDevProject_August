using GameDevProject_August.Levels;
using GameDevProject_August.Sprites;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States

{

    public class PlayingState : State
    {

        private static Level level;
        private List<Sprite> spriteList;

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




        public static Random Random;


        public PlayingState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
                            : base(game, graphicsDevice, content)
        {
            Random = new Random();
            LoadContent(content);
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


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);

            spriteBatch.Begin();

            level.Draw(spriteBatch);

            DrawSprites(spriteList, spriteBatch);

            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {

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

            personMoveTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_MoveRight");
            personShootTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_ShootRight"); ;
            personIdleTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_Idle"); ;
            personDeathTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_Dies");
            personStandStillTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_StandStill");
            personJumpTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_Jump");

        }

        private void LoadMinotaur(ContentManager content)
        {
            minotaurMoveTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_RunRight");
            minotaurCastTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Attack");
            minotaurIdleTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Idle");
            minotaurStandStillTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_StandStill");
        }

        private void LoadRatMage(ContentManager content)
        {
            ratMoveTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_MoveRight");
            ratCastTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_Cast_One");
            ratIdleTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_Idle");
            ratStandStillTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_StandStill");
        }

        private void LoadPorcupine(ContentManager content)
        {
            porcupineMoveTexture = content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_MoveRight");
            porcupineStandStillTexture = content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_StandStill");
        }

        private void LoadDragonfly(ContentManager content)
        {
            dragonflyMoveTexture = content.Load<Texture2D>("Animations\\DragonFly\\DragonFly_MoveRight");
            dragonflyStandStillTexture = content.Load<Texture2D>("Animations\\DragonFly\\DragonFly_StandStill");
        }

        private void LoadProjectiles(ContentManager content)
        {
            playerBullet = content.Load<Texture2D>("Textures\\GoToeBullet");
            ratProjectile = content.Load<Texture2D>("Textures\\RatArrow");
        }
        private void LoadPoints(ContentManager content)
        {
            RegularPointTexture = content.Load<Texture2D>("Textures\\Point_1");
        }

        private void LoadGlitchDeathEffect(ContentManager content)
        {
            glitchDeathTexture = content.Load<Texture2D>("Animations\\Death\\GlitchDeathEffect");
        }

        private void LoadBlockTextures(ContentManager content)
        {
            GrassTexture = content.Load<Texture2D>("Textures\\Tiles\\DirtAboveGround");
            DirtTexture = content.Load<Texture2D>("Textures\\Tiles\\DirtUnderGround");
            StoneTexture = content.Load<Texture2D>("Textures\\Tiles\\Stone");
            MarmerTexture = content.Load<Texture2D>("Textures\\Tiles\\Marmer");
            ThreePointsTexture = content.Load<Texture2D>("Textures\\Tiles\\ThreePointsTexture");
            SevenPointsTexture = content.Load<Texture2D>("Textures\\Tiles\\SevenPointsTexture");
        }

        private void LoadSpriteFonts(ContentManager content)
        {
            fontOfScoreLoaded = content.Load<SpriteFont>("Fonts\\Font_Score");
        }

        private void LoadFallingCodeTextures(ContentManager content)
        {
            FallingCodeMinotaur = content.Load<Texture2D>("FallingCode\\FallingCode_Minotaur");
            FallingCodeDragonFly = content.Load<Texture2D>("FallingCode\\FallingCode_DragonFly");
            FallingCodePorcupine = content.Load<Texture2D>("FallingCode\\FallingCode_Porcupine");
            FallingCodeRatMage = content.Load<Texture2D>("FallingCode\\FallingCode_RatMage");
            FallingCodePoint = content.Load<Texture2D>("FallingCode\\FallingCode_Point");
        }

        private void LoadFinalTerminalTexture(ContentManager content)
        {
            finalTerminalTexture = content.Load<Texture2D>("Textures\\Terminal\\Terminal");
        }

        private void LoadBackGrounds(ContentManager content)
        {
            BackgroundStandardTexture = content.Load<Texture2D>("BackGrounds\\BackGround_Standard");
            BackgroundGlitchScore_0_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_0");
            BackgroundGlitchScore_1_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_1");
            BackgroundGlitchScore_2_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_2");
            BackgroundGlitchScore_3_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_3");
            BackgroundGlitchScore_4_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_4");
            BackgroundGlitchScore_5_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_5");
            BackgroundGlitchScore_6_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_6");
            BackgroundGlitchScore_7_Texture = content.Load<Texture2D>("BackGrounds\\GlitchBackground\\BackgroundScore_7");
        }
    }
}

