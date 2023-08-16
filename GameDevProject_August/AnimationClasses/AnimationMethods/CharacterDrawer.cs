using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.Sentient.Characters.Main;
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
