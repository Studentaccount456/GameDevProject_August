using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.SpotterEnemy.MeleeEnemy
{
    abstract public class MeleeEnemy : SpotterEnemy
    {
        protected Texture2D ShootTexture;
        protected Animation animationShoot;
        protected bool isShootingAnimating = false;
        protected int shootAnimationFrameIndex = 0;

        protected bool canSeeEnemy = true;

        protected bool _enemySpotted;

        public Vector2 EnemyPosition;


        public MeleeEnemy(Texture2D moveTexture, Texture2D shootTexture, Texture2D deathTexture, Vector2 StartPosition, Vector2 offsetPositionSpotter, int widthSpotter, int heightSpotter) : base(moveTexture, deathTexture, StartPosition, offsetPositionSpotter, widthSpotter, heightSpotter)
        {
            ShootTexture = shootTexture;

            animationShoot = new Animation(AnimationType.Attack, shootTexture);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Block> blocks)
        {
            base.Update(gameTime, sprites, blocks);

            MeleeAttackImplementation(gameTime);
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueDrawRules(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }

        protected void MeleeAttackImplementation(GameTime gameTime)
        {
            animationShoot.Update(gameTime);

            UniqueMeleeAttackImplementation(gameTime);

        }

        protected abstract void UniqueMeleeAttackImplementation(GameTime gameTime);
    }
}
