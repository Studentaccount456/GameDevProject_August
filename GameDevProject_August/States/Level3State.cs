using GameDevProject_August.Levels.Level2;
using GameDevProject_August.Levels;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDevProject_August.Sprites;
using Microsoft.Xna.Framework.Input;
using GameDevProject_August.Levels.Level3;
using GameDevProject_August.Sprites.NotSentient.Terminal;
using GameDevProject_August.UI;

namespace GameDevProject_August.States
{
    public class Level3State : PlayingState
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        private float _timer;

        private List<Component> _gameComponents;

        private Texture2D _regularPointTexture;

        private Texture2D backgroundTexture;

        private FallingCode fallingCode;

        public static bool isNextLevelTrigger;

        Level level;

        private bool _isFromMainMenu;

        public Level3State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;

            _isFromMainMenu = isFromMainMenu;


            backgroundTexture = content.Load<Texture2D>("BackGrounds\\BackGround_Standard");

            level = new Level3(new Level3BlockFactory());
            LoadContent(content);
            InitializeContent();
            InitializeScore(3, isFromMainMenu);
            fallingCode = new FallingCode(playerBullet);

            Restart();

        }

        public Level GenerateLevel(Level level, int tileSize)
        {
            Level newlevel = null;

            if (level is Level3)
            {
                newlevel = new Level3(new Level3BlockFactory());
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

        private void Restart()
        {
            //PlayerScore = new Score(fontOfScoreLoaded, ScreenWidth, ScreenHeight);

            _sprites = new List<Sprite>()
            {
                new MainCharacter(personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture)
                {
                    Position = new Vector2(1200,600),
                    facingDirectionIndicator = false,
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.Down,
                        Up = System.Windows.Forms.Keys.Up,
                        Left = System.Windows.Forms.Keys.Left,
                        Right = System.Windows.Forms.Keys.Right,
                        Shoot = System.Windows.Forms.Keys.Space
                    },

                    Speed = 7f,
                    Bullet = new PlayerBullet(playerBullet),
                    Score = Game1.PlayerScore
                },
                new FinalTerminal(finalTerminalTexture)
                {
                    Position = new Vector2(500, 465)
                }
            };
            _regularPointTexture = RegularPointTexture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            DrawBackground(backgroundTexture, spriteBatch);

            level.Draw(spriteBatch);

            /* Empty atm
            foreach (var component in _gameComponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
            */

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }

            Game1.PlayerScore.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite_1 = _sprites[i];

                if (sprite_1.IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }

                if (sprite_1 is FinalTerminal)
                {
                    var finalTerminal = sprite_1 as FinalTerminal;
                    if (finalTerminal.HasDied)
                    {
                        _game.ChangeState(new VictoryState(_game, _graphicsDevice, _content));
                    }
                }
            }
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

            foreach (var sprite in _sprites.ToArray())
            {
                sprite.Update(gameTime, _sprites, level.TileList);

                switch (sprite.PieceOfCodeToFall)
                {
                    case 1:
                        fallingCode.LetCodeFall(_sprites, FallingCodeMinotaur);
                        break;
                    case 2:
                        fallingCode.LetCodeFall(_sprites, FallingCodeDragonFly);
                        break;
                    case 3:
                        fallingCode.LetCodeFall(_sprites, FallingCodePorcupine);
                        break;
                    case 4:
                        fallingCode.LetCodeFall(_sprites, FallingCodeRatMage);
                        break;
                    case 5:
                        fallingCode.LetCodeFall(_sprites, FallingCodePoint);
                        break;
                    default:
                        break;
                }
            }

            PostUpdate(gameTime);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public /*override*/ void InitializeContent()
        {
            level = GenerateLevel(level, 38);
            //SpriteList = GenerateLevelSpriteList();
        }
    }
}
