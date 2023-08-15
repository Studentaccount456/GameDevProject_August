using GameDevProject_August.Levels;
using GameDevProject_August.Sprites;
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

        public Texture2D playerBullet { get; private set; }

        public Texture2D ratProjectile { get; private set; }

        public static Texture2D GrassTexture { get; private set; }
        public static Texture2D DirtTexture { get; private set; }
        public static Texture2D StoneTexture { get; private set; }

        public Texture2D BackgroundOfLevel { get; set; }

        public static Texture2D FallingCodeMinotaur { get; private set; }

        public PlayingState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
                            : base(game, graphicsDevice, content)
        {
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
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawBackground(BackgroundOfLevel, spriteBatch);

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



        public void DrawBackground(Texture2D backgroundTexture, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
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
        }

        private void LoadSpriteFonts(ContentManager content)
        {
            fontOfScoreLoaded = content.Load<SpriteFont>("Fonts\\Font_Score");
        }

        private void LoadFallingCodeTextures(ContentManager content)
        {
            FallingCodeMinotaur = content.Load<Texture2D>("FallingCode\\FallingCode_Minotaur");
        }
    }
}

