using GameDevProject_August.Levels;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Terminal
{
    public class FinalTerminal : NotSentient
    {
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, staticTexture.Width, staticTexture.Height);
            }
        }

        public FinalTerminal(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            CollisionRules(sprites);
        }

        protected override void UniqueCollisionRules(List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is FinalTerminal)
                {
                    continue;
                }

                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                {
                    IsDestroyed = true;
                    notSentient.IsDestroyed = true;
                }
            }
        }
    }
}
