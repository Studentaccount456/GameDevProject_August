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

        private float _timer;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        private FallingCode fallingCode;

        public static bool isNextLevelTrigger;

        private bool _isFromMainMenu;

        public Level1State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;

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
            /*
            else if (level is Level2)
            {
                newlevel = new Level2(new Level2BlockFactory());
                newlevel.Generate(newlevel.Map, tileSize);
            }
            */


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

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in SpriteList.ToArray())
            {
                sprite.Update(gameTime, SpriteList, Level.TileList);

                switch (sprite.PieceOfCodeToFall)
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
            }

            PostUpdate(gameTime, SpriteList);

            if (PlayingState.isNextLevelTrigger)
            {
                PlayingState.isNextLevelTrigger = false;
                _game.ChangeState(new Level2State(_game, _graphicsDevice, _content, false));
            }
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
