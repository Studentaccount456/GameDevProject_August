using GameDevProject_August.AnimationClasses;
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
            DeathAnimationOffset = new Vector2(0, 5);
            BowDownAnimationOffset = new Vector2(0, 4);

            // Standard walks right
            #region MoveAnimation
            animationMove.AddFrame(0, 0, 30, 50);
            animationMove.AddConsistentFramesWithStartCoördinates(128, 0, 128, 0, 28, 50, 3);
            animationMove.AddFrame(512, 0, 30, 50);
            animationMove.AddConsistentFramesWithStartCoördinates(640, 0, 128, 0, 34, 50, 3);
            #endregion

            //Height is 48 for each frame
            #region animationShoot
            animationShoot.fps = 4;
            animationShoot.AddFrame(0, 0, 36, 50);
            animationShoot.AddFrame(126, 0, 44, 50);
            animationShoot.AddFrame(256, 0, 40, 50);
            animationShoot.AddFrame(384, 0, 44, 50);
            #endregion

            #region IdleAnimation
            animationIdle.fps = 8;
            animationIdle.AddConsistentFrames(128, 0, 32, 44, 8);
            #endregion

            //Height is 44 for each frame
            #region DeathAnimation
            animationDeath.fps = 8;
            animationDeath.AddFrame(0, 0, 32, 44);
            animationDeath.AddFrame(128, 0, 42, 44);
            animationDeath.AddFrame(260, 0, 50, 44);
            animationDeath.AddConsistentFramesWithStartCoördinates(388, 0, 128, 0, 50, 44, 2);
            #endregion

            //Height is 44 for each frame
            #region JumpAnimation
            animationJump.AddFrame(0, 0, 30, 44);
            animationJump.AddConsistentFramesWithStartCoördinates(128, 0, 128, 0, 28, 44, 3);
            animationJump.AddFrame(510, 0, 28, 44);
            animationJump.AddConsistentFramesWithStartCoördinates(640, 0, 128, 0, 28, 44, 2);
            animationJump.AddFrame(896, 0, 30, 44);
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

        // LSP
        public Archeologist()
        {

        }
    }
}