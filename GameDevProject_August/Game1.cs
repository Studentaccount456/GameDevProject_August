using GameDevProject_August.Models;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.Characters.Enemy;
using GameDevProject_August.Sprites.Characters.Main;
using GameDevProject_August.Sprites.Collectibles;
using GameDevProject_August.Sprites.Projectiles;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GameDevProject_August
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random Random;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        private Texture2D _regularPointTexture;

        private float _timer;

        private bool _hasStarted = false;

        private Score _score;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Random = new Random();

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Restart();

        }

        private void Restart()
        {
            var personTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_MoveRight");
            var ratEnemyTexture = Content.Load<Texture2D>("Textures\\Enemy_Rat");
            _score = new Score(Content.Load<SpriteFont>("Fonts\\Font_Score"), ScreenWidth, ScreenHeight);



            _sprites = new List<Sprite>()
            {
                new MainCharacter(personTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) /*- (personTexture.Width / 2)*/, ScreenHeight /*- personTexture.Height*/),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.Down,
                        Up = System.Windows.Forms.Keys.Up,
                        Left = System.Windows.Forms.Keys.Left,
                        Right = System.Windows.Forms.Keys.Right
                    },
                    Speed = 10f,
                    Bullet = new Bullet(Content.Load<Texture2D>("Textures\\GoToeBullet")),
                    Score = _score
                },
                new RatEnemy(ratEnemyTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) - (ratEnemyTexture.Width / 2) + 5, ScreenHeight - ratEnemyTexture.Height + 6),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D
                    },
                    Speed = 10f,
        },
            };

            _hasStarted = false;
            _regularPointTexture = Content.Load<Texture2D>("Textures\\Point_1");
        }

        protected override void Update(GameTime gameTime)
        {
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
                sprite.Update(gameTime, _sprites);
            }

            //SpawnFallingCode();

            SpawnRegularPoint();

            PostUpdate();

            base.Update(gameTime);
        }

        private void SpawnFallingCode()
        {
            if (_timer > 0.25f)
            {
                _timer = 0;
                _sprites.Add(new FallingCode(Content.Load<Texture2D>("Textures\\GoToeBullet")));
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

        private void PostUpdate()
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
                        Restart();
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }


            _score.Draw(_spriteBatch);

            /*
            // Scores multiple players
            var fontY = 10;
            var i = 0;
            foreach (var sprite in _sprites)
            {
                if(sprite is MainCharacter)
                {
                    _spriteBatch.DrawString(_font, String.Format("Player {0} : {1}", ++i, ((MainCharacter)sprite).Score), new Vector2(10, fontY += 20), Color.Red);
                }
            }
            */

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}