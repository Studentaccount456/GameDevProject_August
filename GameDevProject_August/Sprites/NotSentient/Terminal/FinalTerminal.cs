using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.NotSentient.Collectibles;
using GameDevProject_August.Sprites.NotSentient.Projectiles;
using GameDevProject_August.Sprites.Sentient.Characters.Enemy;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.NotSentient.Terminal
{
    public class FinalTerminal : Sprite
    {
        public bool HasDied = false;

        public Texture2D StandStillTexture;

        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, StandStillTexture.Width, StandStillTexture.Height);
            }
        }

        public FinalTerminal(Texture2D texture)
            : base(texture)
        {
            StandStillTexture = texture;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            foreach (var sprite in sprites)
            {
                if (sprite is FinalTerminal)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet)
                {
                    HasDied = true;
                    sprite.IsRemoved = true;
                }                               
            }
            Velocity = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(StandStillTexture, Position, Color.White);

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
        }
    }
}
