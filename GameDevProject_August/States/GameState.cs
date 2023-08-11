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
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.States
{
    public class GameState : State
    {
        public static Random Random;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        private Texture2D _regularPointTexture;

        private float _timer;

        private bool _hasStarted = false;

        private Score _score;

        private Texture2D GrassTexture, DirtTexture, StoneTexture;

        private Color _backgroundColour = Color.CornflowerBlue;

        private List<Component> _gameComponents;


        private Texture2D RegularPointTexture;

        private Texture2D personMoveTexture;
        private Texture2D personShootTexture;
        private Texture2D personIdleTexture;
        private Texture2D personDeathTexture;
        private Texture2D personStandStillTexture;

        private Texture2D ratMoveTexture;
        private Texture2D ratCastTexture;
        private Texture2D ratIdleTexture;
        private Texture2D ratStandStillTexture;

        private SpriteFont fontOfScoreLoaded;

        private Texture2D porcupineMoveTexture;
        private Texture2D porcupineStandStillTexture;


        private Texture2D dragonflyMoveTexture;


        private Texture2D glitchDeathTexture;

        private Texture2D minotaurMoveTexture;
        private Texture2D minotaurCastTexture;
        private Texture2D minotaurIdleTexture;
        private Texture2D minotaurStandStillTexture;

        private Texture2D playerBullet;







        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            content.RootDirectory = "Content";
            Random = new Random();
            ScreenWidth = Game1.ScreenWidth; 
            ScreenHeight = Game1.ScreenHeight;

            GrassTexture = content.Load<Texture2D>("Textures\\Tiles\\DirtAboveGround");
            DirtTexture = content.Load<Texture2D>("Textures\\Tiles\\DirtUnderGround");
            StoneTexture = content.Load<Texture2D>("Textures\\Tiles\\Stone");
            RegularPointTexture = content.Load<Texture2D>("Textures\\Point_1");

            personMoveTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_MoveRight");
            personShootTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_ShootRight"); ;
            personIdleTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_Idle"); ;
            personDeathTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_Dies");
            personStandStillTexture = content.Load<Texture2D>("Animations\\MainCharacter\\MC_StandStill");

            ratMoveTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_MoveRight");
            ratCastTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_Cast_One");
            ratIdleTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_Idle");
            ratStandStillTexture = content.Load<Texture2D>("Animations\\Rat\\Rat_StandStill");
            fontOfScoreLoaded = content.Load<SpriteFont>("Fonts\\Font_Score");

            porcupineMoveTexture = content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_MoveRight");
            porcupineStandStillTexture = content.Load<Texture2D>("Animations\\Porcupine\\Porcupine_StandStill");


            dragonflyMoveTexture = content.Load<Texture2D>("Animations\\DragonFly\\DragonFly_MoveRight");


            //Yet to insert
            glitchDeathTexture = content.Load<Texture2D>("Animations\\Death\\GlitchDeathEffect");

            minotaurMoveTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_RunRight");
            minotaurCastTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Attack");
            minotaurIdleTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_Idle");
            minotaurStandStillTexture = content.Load<Texture2D>("Animations\\Minotaur\\Minotaur_StandStill");

            playerBullet = content.Load<Texture2D>("Textures\\GoToeBullet");

            Restart();

        }

        private void Restart()
        {
            _score = new Score(fontOfScoreLoaded, ScreenWidth, ScreenHeight);

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
                    Bullet = new PlayerBullet(playerBullet),
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
            _regularPointTexture = RegularPointTexture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

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

            _score.Draw(spriteBatch);

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
                        Restart();
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
                sprite.Update(gameTime, _sprites);
            }

            //SpawnFallingCode();

            SpawnRegularPoint();

            PostUpdate(gameTime);
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
    }
}
