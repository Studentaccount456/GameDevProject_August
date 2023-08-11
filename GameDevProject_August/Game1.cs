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

        public static Texture2D GrassTexture, DirtTexture, StoneTexture;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Random = new Random();

            /*
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            */

            ScreenWidth = 1280;
            ScreenHeight = 720;

            // Set the desired resolution
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            // Apply the changes
            _graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GrassTexture = Content.Load<Texture2D>("Textures\\Tiles\\DirtAboveGround");
            DirtTexture  = Content.Load<Texture2D>("Textures\\Tiles\\DirtUnderGround");
            StoneTexture = Content.Load<Texture2D>("Textures\\Tiles\\Stone");

            Restart();

        }

        private void Restart()
        {
            var personMoveTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_MoveRight");
            var personShootTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_ShootRight"); ;
            var personIdleTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_Idle"); ;
            var personDeathTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_Dies");
            var personStandStillTexture = Content.Load<Texture2D>("Animations\\MainCharacter\\MC_StandStill");

            var ratMoveTexture = Content.Load<Texture2D>("Animations\\Rat\\Rat_MoveRight");
            var ratCastTexture = Content.Load<Texture2D>("Animations\\Rat\\Rat_Cast_One");
            var ratIdleTexture = Content.Load<Texture2D>("Animations\\Rat\\Rat_Idle");
            var ratStandStillTexture = Content.Load<Texture2D>("Animations\\Rat\\Rat_StandStill");
            _score = new Score(Content.Load<SpriteFont>("Fonts\\Font_Score"), ScreenWidth, ScreenHeight);

            var porcupineMoveTexture = Content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_MoveRight");
            var porcupineStandStillTexture = Content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_StandStill");


            var dragonflyMoveTexture = Content.Load<Texture2D>("Animations\\DragonFly\\DragonFly_MoveRight");


            //Yet to insert
            var glitchDeathTexture = Content.Load<Texture2D>("Animations\\Death\\GlitchDeathEffect");

            var minotaurMoveTexture = Content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_RunRight");
            var minotaurCastTexture = Content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Attack");
            var minotaurIdleTexture = Content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Idle");
            var minotaurStandStillTexture = Content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_StandStill");



            _sprites = new List<Sprite>()
            {
                new MainCharacter(personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture)
                {
                    Position = new Vector2(500,20),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.Down,
                        Up = System.Windows.Forms.Keys.Up,
                        Left = System.Windows.Forms.Keys.Left,
                        Right = System.Windows.Forms.Keys.Right,
                        Shoot = System.Windows.Forms.Keys.Space
                    },
                    Speed = 10f,
                    Bullet = new PlayerBullet(Content.Load<Texture2D>("Textures\\GoToeBullet")),
                    Score = _score
                },
                
                  new Minotaur(minotaurMoveTexture, minotaurCastTexture, minotaurIdleTexture, glitchDeathTexture, minotaurStandStillTexture)
                {
                    Position = new Vector2(10,10),
                    Input = new Input()
                    {
                        Down = System.Windows.Forms.Keys.S,
                        Up = System.Windows.Forms.Keys.Z,
                        Left = System.Windows.Forms.Keys.Q,
                        Right = System.Windows.Forms.Keys.D,
                        Shoot = System.Windows.Forms.Keys.Space
                    },
                    Speed = 10f,
        },
                /*
                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture)
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
                }
                
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
                /*
                new RatMage(ratMoveTexture, ratCastTexture, ratIdleTexture, glitchDeathTexture, ratStandStillTexture)
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
                    Speed = 10f,
                    Bullet = new EnemyBullet(Content.Load<Texture2D>("Textures\\RatArrow")),

        },*/
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