using GameDevProject_August.Levels;
using GameDevProject_August.Levels.Level2;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
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
    public class Level2State : PlayingState
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        private Texture2D _regularPointTexture;

        private bool _isFromMainMenu;

        public Level2State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;
            nextLevelIndicator = 3;

            _isFromMainMenu = isFromMainMenu;


            isNextLevelTrigger = false;

            Level = new Level2(new Level2BlockFactory());
            LoadContent(content);
            InitializeContent();
            InitializeScore(2, isFromMainMenu);
            fallingCode = new FallingCode(playerBullet);

            GenerateLevelSprites();

        }

        public Level GenerateLevel(Level level, int tileSize)
        {
            Level newlevel = null;

            if (level is Level2)
            {
                newlevel = new Level2(new Level2BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }

            return newlevel;
        }

        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(10,600), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore, true),


                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture)
                {
                    Position = new Vector2(250, 595),
                    Speed = 2f,
                },
                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture)
                {
                    Position = new Vector2(650,570),
                    Speed = 2f,
                },
                new RatMage(ratMoveTexture, ratCastTexture, ratIdleTexture, glitchDeathTexture, new Vector2(225, 408), new Vector2(300,101), 600, 150)
                {
                    Speed = 2f,
                    Bullet = new EnemyBullet(ratProjectile),
        },
                new Regular_Point(RegularPointTexture)
                {
                    Position = new Vector2(865 ,400)
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
