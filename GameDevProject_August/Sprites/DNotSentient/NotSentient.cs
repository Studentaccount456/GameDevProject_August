using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DNotSentient
{
    abstract public class NotSentient : Sprite
    {
        public bool IsDestroyed = false;

        public Texture2D staticTexture;

        public NotSentient(Texture2D texture) : base(texture)
        {
            staticTexture = texture;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(staticTexture, Position, Color.White);

            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
        }

        protected void UpdatePositionAndResetVelocity()
        {
            Position += Velocity;

            Velocity = Vector2.Zero;
        }

        protected virtual void CollisionRules(List<Sprite> sprites)
        {
            UniqueCollisionRules(sprites);
        }

        protected abstract void UniqueCollisionRules(List<Sprite> sprites);
    }
}
