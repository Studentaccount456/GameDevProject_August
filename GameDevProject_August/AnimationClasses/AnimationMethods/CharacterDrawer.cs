using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameDevProject_August.AnimationClasses.AnimationMethods
{

    public enum CharacterType
    {
        Main_Character,
        Enemy_FullMoveset,
        Enemy_OnlyMove
    }
    public class CharacterDrawer
    {
        private CharacterType _characterType;

        //private bool isDeathAnimating = false;

        public AnimationHandler _animationHandler;



        public CharacterDrawer(AnimationHandler animationHandler/*, CharacterType characterType*/) 
        { 
            //_characterType = characterType;
            _animationHandler = animationHandler;
        }

        /*public void Draw(SpriteBatch spriteBatch)
        {
            switch (_characterType)
            {
                case CharacterType.Main_Character:
                    DrawMainCharacter(spriteBatch); 
                    break;
                case CharacterType.Enemy_FullMoveset:
                    DrawEnemyFullMoveset(spriteBatch);
                    break;
                case CharacterType.Enemy_OnlyMove:
                    DrawEnemyOnlyMove(spriteBatch);
                    break;
            }
        }*/

        public void DrawMainCharacter(SpriteBatch spriteBatch, AnimationHandler animationHandler,
                                       Dictionary<string, Animation> animations,
                                       Texture2D standStillTexture, Vector2 Position, bool isDeathAnimating,
                                       bool isShootingAnimating, bool facingDirectionIndicator, bool isIdling,
                                       bool standStillNoIdle, bool isMovingleft, bool isMovingRight, bool isMovingUp, bool isMovingDown)
        {
            // TODO MainCharacter facing side deathAnimation
            if (isDeathAnimating)
            {
                animationHandler.DrawAnimation(spriteBatch, animations["DeathAnimation"], Position, true);
                if (animations["DeathAnimation"].IsAnimationComplete)
                {
                    isDeathAnimating = false; // Stop death animation
                }
            }
            else if (isShootingAnimating)
            {
                if (facingDirectionIndicator == true)
                {
                    animationHandler.DrawAnimation(spriteBatch, animations["AttackAnimation"], Position, true);
                }
                else if (facingDirectionIndicator == false)
                {
                    animationHandler.DrawAnimation(spriteBatch, animations["AttackAnimation"], Position, false);
                }
                if (animations["AttackAnimation"].IsAnimationComplete)
                {
                    isShootingAnimating = false;
                }
            }
            else if (isMovingleft)
            {
                animationHandler.DrawAnimation(spriteBatch, animations["MoveAnimation"], Position, false);
            }
            else if (isMovingRight)
            {
                animationHandler.DrawAnimation(spriteBatch, animations["MoveAnimation"], Position, true);
            }
            else if (isIdling)
            {
                if (facingDirectionIndicator == true)
                {
                    animationHandler.DrawAnimation(spriteBatch, animations["IdleAnimation"], Position, true);
                }
                else if (facingDirectionIndicator == false)
                {
                    animationHandler.DrawAnimation(spriteBatch, animations["IdleAnimation"], Position, false);
                }
            }
            else if (facingDirectionIndicator == true && standStillNoIdle == true && !isShootingAnimating || (facingDirectionIndicator == true && !isShootingAnimating && isMovingDown) || (facingDirectionIndicator == true && !isShootingAnimating && isMovingUp))
            {
                animationHandler.DrawOneFrameAnimation(spriteBatch, standStillTexture, Position, true);
            }
            else if (facingDirectionIndicator == false && standStillNoIdle == true && !isShootingAnimating || (facingDirectionIndicator == false && !isShootingAnimating && isMovingUp) || (facingDirectionIndicator == false && !isShootingAnimating && isMovingDown))
            {
                animationHandler.DrawOneFrameAnimation(spriteBatch, standStillTexture, Position, false);
            }
        }

    private void DrawEnemyFullMoveset(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        private void DrawEnemyOnlyMove(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
    
}
