using GameDevProject_August.Levels;
using GameDevProject_August.Levels.Level1;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Enemy;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.States
{
    public class GameState : PlayingState
    {
        public static Random Random;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        private float _timer;

        private bool _hasStarted = false;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;

        private Texture2D _regularPointTexture;

        
        private Texture2D RegularPointTexture;

        private Texture2D backgroundTexture;

        private FallingCode fallingCode;

        public static bool isNextLevelTrigger;


        Level level;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            Random = new Random();
            ScreenWidth = Game1.ScreenWidth; 
            ScreenHeight = Game1.ScreenHeight;

            RegularPointTexture = content.Load<Texture2D>("Textures\\Point_1");
            backgroundTexture = content.Load<Texture2D>("BackGrounds\\BackGround_Standard");

            level = new Level1(new Level1BlockFactory());
            LoadContent(content);
            InitializeContent();
            fallingCode = new FallingCode(playerBullet);

            Restart();

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

        private void Restart()
        {
            //PlayerScore = new Score(fontOfScoreLoaded, ScreenWidth, ScreenHeight);

            _sprites = new List<Sprite>()
            {
                new MainCharacter(personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture)
                {
                    Position = new Vector2(20,30),

                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.Down,
                        Up = System.Windows.Forms.Keys.Up,
                        Left = System.Windows.Forms.Keys.Left,
                        Right = System.Windows.Forms.Keys.Right,
                        Shoot = System.Windows.Forms.Keys.Space
                    },

                    Speed = 10f,
                    Bullet = new PlayerBullet(playerBullet),
                    Score = PlayerScore
                },
                /*
                  new Minotaur(minotaurMoveTexture, minotaurCastTexture, minotaurIdleTexture, glitchDeathTexture, minotaurStandStillTexture)
                {
                    Position = new Vector2(500,10),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D,
                        Shoot = System.Windows.Forms.Keys.M
                    },
                    Speed = 10f,
        },
                
                /*
                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture, dragonflyStandStillTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) - (ratMoveTexture.Width / 2) + 5, ScreenHeight - ratMoveTexture.Height + 6),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D,
                        Shoot = System.Windows.Forms.Keys.Space
                    },
                    Speed = 2f,
                },
                */
                /*
                new Porcupine(porcupineMoveTexture, glitchDeathTexture,porcupineStandStillTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) - (ratMoveTexture.Width / 2) + 5, ScreenHeight - ratMoveTexture.Height + 6),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D,
                        Shoot = System.Windows.Forms.Keys.Space
                    },
                    Speed = 2f,
                },
                */
                
                /*
                new RatMage(ratMoveTexture, ratCastTexture, ratIdleTexture, glitchDeathTexture, ratStandStillTexture)
                {
                    Position = new Vector2(100,100),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D,
                        Shoot = System.Windows.Forms.Keys.Space
                    },
                    Speed = 10f,
                    Bullet = new EnemyBullet(ratProjectile),

        },*/
            };

            _hasStarted = false;
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

            PlayerScore.Draw(spriteBatch);

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
                        //Restart();
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

            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                _hasStarted = true;
            }

            if (!_hasStarted)
            {
                return;
            }

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

            //SpawnFallingCode();

            SpawnRegularPoint();

            PostUpdate(gameTime);

            if(isNextLevelTrigger)
            {
                _game.ChangeState(new Level2State(_game, _graphicsDevice, _content));
            }
        }

        private void SpawnFallingCode()
        {
            if (_timer > 0.25f)
            {
                _timer = 0;
                _sprites.Add(new FallingCode(playerBullet));
            }
        }

        private void SpawnRegularPoint()
        {
            if (_timer > 1)
            {
                _timer = 0;

                var xPos = Random.Next(0, ScreenWidth - _regularPointTexture.Width);
                var yPos = Random.Next(0, ScreenHeight - _regularPointTexture.Height);

                _sprites.Add(new Regular_Point(_regularPointTexture)
                {
                    Position = new Vector2(xPos, yPos)

                });
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
