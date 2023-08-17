using GameDevProject_August.Levels;
using GameDevProject_August.Levels.Level2;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Enemy;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
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

        private List<Sprite> _sprites;

        private float _timer;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        private Texture2D _regularPointTexture;

        private FallingCode fallingCode;

        private bool _isFromMainMenu;


        Level level;

        public Level2State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            ScreenWidth = Game1.ScreenWidth;
            ScreenHeight = Game1.ScreenHeight;

            _isFromMainMenu = isFromMainMenu;


            isNextLevelTrigger = false;

            level = new Level2(new Level2BlockFactory());
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
            _sprites = new List<Sprite>()
            {
                new MainCharacter(personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture)
                {
                    Position = new Vector2(10,600),
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
                new RatMage(ratMoveTexture, ratCastTexture, ratIdleTexture, glitchDeathTexture)
                {
                    Position = new Vector2(225, 408),
                    Speed = 2f,
                    Bullet = new EnemyBullet(ratProjectile),
        },
                new Regular_Point(RegularPointTexture)
                {
                    Position = new Vector2(865 ,400)
                }
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

                if (sprite_1 is MainCharacter)
                {
                    var player = sprite_1 as MainCharacter;
                    if (player.HasDied)
                    {
                        _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content));
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

            if (PlayingState.isNextLevelTrigger)
            {
                PlayingState.isNextLevelTrigger = false;
                _game.ChangeState(new Level3State(_game, _graphicsDevice, _content, false));
            }
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
