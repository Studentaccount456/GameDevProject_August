using GameDevProject_August.AnimationClasses;
using GameDevProject_August.AnimationClasses.AnimationMethods;
using GameDevProject_August.Models;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.TypeOfPlayer.ShootingPlayer.Characters;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameDevProject_August.Sprites.DSentient.TypeSentient.Player
{
    public enum TypePlayer
    {
        Archeologist
    }
    public class Player : Sentient
    {
        public Input Input;

        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Score Score;

        protected Animation animationMove;
        protected Animation animationDeath;
        protected Animation animationIdle;
        protected Animation animationShoot;
        protected Animation animationJump;

        public Dictionary<string, Animation> animationDictionary;

        protected Texture2D AttackTexture;
        protected Texture2D IdleTexture;
        protected Texture2D DeathTexture;
        protected Texture2D StandStillTexture;
        protected Texture2D JumpTexture;
        protected Texture2D BowDownTexture;

        public AnimationHandler AnimationHandler_Player;

        protected bool isIdling = false;
        protected const float IdleTimeoutDuration = 3.0f;
        protected float idleTimer = 0f;
        protected bool standStillNoIdle = false;

        protected bool hasJumped;

        protected const float GravityAcceleration = 125;

        protected bool keyPressed;

        public Player(Texture2D texture) : base(texture)
        {

        }

        public Player()
        {

        }

        public Sprite makePlayer(Input input, TypePlayer typePlayer, Texture2D moveTexture, Texture2D attackTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture,
                                Vector2 startPosition, float speed, PlayerBullet bullet, Score score, bool facingDirectionIndicator)
        {
            switch (typePlayer)
            {
                case TypePlayer.Archeologist:
                    return new Archeologist(moveTexture, attackTexture, idleTexture, deathTexture, standStillTexture, jumpTexture, bowDownTexture) 
                    {
                        Input = input,
                        Position = startPosition,
                        Speed = speed,
                        Bullet = bullet,
                        Score = score,
                        facingDirectionIndicator = facingDirectionIndicator
                    }
                    ;
                default:
                    return null;
            }
        }
    }
}
