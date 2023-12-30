using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Levels;
using GameDevProject_August.Models.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy
{
    public class AttackingEnemy : Enemy
    {
        public Texture2D AttackTexture;
        protected Animation animationAttack;
        protected Vector2 offsetAnimationAttack = new Vector2(0, 0);

        protected bool isAttackingAnimating = false;


        public AttackingEnemy(Texture2D moveTexture, Texture2D attackTexture, Texture2D deathTexture, Vector2 StartPosition) : base(moveTexture, deathTexture, StartPosition)
        {
            AttackTexture = attackTexture;
            animationAttack = new Animation(AnimationType.Attack, attackTexture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAttackingAnimating && !isDeathAnimating)
            {
                animationHandlerEnemy.DrawAnimation(spriteBatch, animationAttack, Position + offsetAnimationAttack, Movement.Direction);
            }
            else
            {
                base.Draw(spriteBatch);
            }
        }

        protected override void HitBoxTracker()
        {
            throw new NotImplementedException();
        }

        protected override void UniqueCollisionRules(Sprite sprite, Rectangle hitbox, bool isHardSpot)
        {
            throw new NotImplementedException();
        }

        protected override void UniqueMovingRules(GameTime gameTime, List<Block> blocks)
        {
            throw new NotImplementedException();
        }
    }
}
