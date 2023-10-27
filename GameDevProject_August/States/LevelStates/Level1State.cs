using GameDevProject_August.Levels;
using GameDevProject_August.Levels.Level1;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Terminal;
using GameDevProject_August.Sprites.DSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using GameDevProject_August.States.MenuStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.LevelStates
{
    public class Level1State : PlayingState
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<NotSentient> _notsentients;

        private List<Sentient> _sentients;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        public static bool isNextLevelTrigger;

        private bool _isFromMainMenu;

        public Level1State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;
            nextLevelIndicator = 2;

            _isFromMainMenu = isFromMainMenu;

            Level = new Level1(new Level1BlockFactory());
            LoadContent(content);
            InitializeContent();
            InitializeScore(1, isFromMainMenu);
            fallingCode = new FallingCode(playerBullet);

            GenerateLevelSprites();


        }

        public Level GenerateLevel(Level level, int tileSize)
        {
            Level newlevel = null;

            if (level is Level1)
            {
                newlevel = new Level1(new Level1BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }

            return newlevel;
        }

        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(10,564), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore, true),

                new Minotaur(minotaurMoveTexture, minotaurCastTexture, minotaurIdleTexture, glitchDeathTexture, new Vector2(1085,566), new Vector2(285,71), 354, 115)
                {
                    Speed = 11f,
                },

                new Porcupine(porcupineMoveTexture, glitchDeathTexture)
                {
                    Position = new Vector2(210,600),
                    Speed = 2f,
                },
               new Regular_Point(RegularPointTexture)
                {
                       Position = new Vector2(575, 566)
                }
            };
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public /*override*/ void InitializeContent()
        {
            Level = GenerateLevel(Level, 38);
            //SpriteList = GenerateLevelSpriteList();
        }

    }
}
