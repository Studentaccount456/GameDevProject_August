using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient
{
    public class Sentient : Sprite
    {
        // Death
        public bool isDeathAnimating = false;

        public bool IsKilled = false;

        // Move
        public float Speed = 2f;

        public bool HasDied = false;

        public Sentient(Texture2D texture) : base(texture)
        {
        }

 

        /*protected void CollisionRules(GameTime gameTime, List<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is PlayerBullet && sprite is NotSentient notSentient)
                {
                    Game1.PlayerScore.MainScore++;
                    isDeathAnimating = true;
                    notSentient.IsDestroyed = true;
                }
                if (sprite is Dragonfly)
                {
                    continue;
                }

                SpecificCollisionRules(sprite);
                /*if (sprite.RectangleHitbox.Intersects(RectangleHitbox) && sprite is Archeologist && sprite is Sentient sentient)
                {
                    sentient.isDeathAnimating = true;
                }*/
        //GlitchDeathInit(gameTime, sprite, 2);

        //UpdatePositionAndResetVelocity();
        /*}
    }

    protected abstract void SpecificCollisionRules(Sprite sprite);*/
    }
}
