using GameDevProject_August.AnimationClasses;
using GameDevProject_August.Models.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters
{
    public class Archeologist : ShootingPlayer
    {
        // Consistent Hitbox
        public override Rectangle RectangleHitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 34, 44);
            }
        }


        public Archeologist(Texture2D moveTexture, Texture2D attackTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture)
            : base(moveTexture, attackTexture, idleTexture, deathTexture, standStillTexture, jumpTexture, bowDownTexture)
        {

            // Standard walks right
            #region MoveAnimation
            animationMove.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(512, 0, 30, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(640, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(768, 0, 34, 50)));
            animationMove.AddFrame(new AnimationFrame(new Rectangle(896, 0, 34, 50)));
            #endregion

            //Height is 48 for each frame
            #region animationShoot
            animationShoot.fps = 4;
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(0, 0, 36, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(126, 0, 44, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(256, 0, 40, 50)));
            animationShoot.AddFrame(new AnimationFrame(new Rectangle(384, 0, 44, 50)));
            #endregion

            //Height is 44 for each frame
            #region Idle
            animationIdle.fps = 8;
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(128, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(256, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(384, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(512, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(640, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(768, 0, 32, 44)));
            animationIdle.AddFrame(new AnimationFrame(new Rectangle(896, 0, 32, 44)));
            #endregion

            //Height is 44 for each frame
            #region DeathAnimation
            animationDeath.fps = 8;
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(0, 0, 32, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(128, 0, 42, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(260, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(388, 0, 50, 44)));
            animationDeath.AddFrame(new AnimationFrame(new Rectangle(516, 0, 50, 44)));
            #endregion

            #region JumpAnimation
            animationJump.AddFrame(new AnimationFrame(new Rectangle(0, 0, 30, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(128, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(256, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(384, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(510, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(640, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(768, 0, 28, 44)));
            animationJump.AddFrame(new AnimationFrame(new Rectangle(896, 0, 30, 44)));
            #endregion

            animationDictionary = new Dictionary<string, Animation>
{
                                { "MoveAnimation", animationMove },
                                { "AttackAnimation", animationShoot },
                                { "DeathAnimation", animationDeath },
                                { "IdleAnimation", animationIdle },
                                { "JumpAnimation", animationJump }
                                };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isDeathAnimating)
            {
                if (Movement.Direction_X == Direction_X.Left)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, Direction.Left);
                }
                else if (Movement.Direction_X == Direction_X.Right)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["DeathAnimation"], Position, Direction.Right);
                }
                if (animationDictionary["DeathAnimation"].IsAnimationComplete)
                {
                    HasDied = true;
                    IsKilled = true;
                }
            }
            else if (isAttackingAnimating)
            {
                if (Movement.Direction_X == Direction_X.Right)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, Direction.Right);
                }
                else if (Movement.Direction_X == Direction_X.Left)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["AttackAnimation"], Position, Direction.Left);
                }
                if (animationDictionary["AttackAnimation"].IsAnimationComplete)
                {
                    isAttackingAnimating = false;
                }
            }
            else if (hasJumped && Movement.Direction_X == Direction_X.Right && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, Direction.Right);
            }
            else if (hasJumped && Movement.Direction_X == Direction_X.Left && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["JumpAnimation"], Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Left && isCharacterMoving == true)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Right && isCharacterMoving == true)
            {
                AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["MoveAnimation"], Position, Direction.Right);
            }
            else if (isIdling)
            {
                if (Movement.Direction_X == Direction_X.Right && isCharacterMoving == false)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, Direction.Right);
                }
                else if (Movement.Direction_X == Direction_X.Left && isCharacterMoving == false)
                {
                    AnimationHandler_Player.DrawAnimation(spriteBatch, animationDictionary["IdleAnimation"], Position, Direction.Left);
                }
            }

            else if (Movement.Direction_X == Direction_X.Right && standStillNoIdle == true && !isAttackingAnimating || Movement.Direction_X == Direction_X.Right && !isAttackingAnimating && Movement.Direction_Y == Direction_Y.None || Movement.Direction_X == Direction_X.Right && !isAttackingAnimating && Movement.Direction_Y == Direction_Y.Up)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, Direction.Right);
            }
            else if (Movement.Direction_X == Direction_X.Left && standStillNoIdle == true && !isAttackingAnimating || Movement.Direction_X == Direction_X.Left && !isAttackingAnimating && Movement.Direction_Y == Direction_Y.Up || Movement.Direction_X == Direction_X.Left && !isAttackingAnimating && Movement.Direction_Y == Direction_Y.None)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, StandStillTexture, Position, Direction.Left);
            }
            else if (Movement.Direction_X == Direction_X.Right && Movement.Direction_Y == Direction_Y.Down && !hasJumped)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), Direction.Right);
            }
            else if (Movement.Direction_X == Direction_X.Left && Movement.Direction_Y == Direction_Y.Down && !hasJumped)
            {
                AnimationHandler_Player.DrawOneFrameAnimation(spriteBatch, BowDownTexture, Position + new Vector2(0, 2), Direction.Left);
            }
            spriteBatch.DrawRectangle(RectangleHitbox, Color.Blue);
        }
    }
}