﻿using GameDevProject_August.Models;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player.Characters;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public Player(Texture2D texture) : base(texture)
        {

        }

        public Player()
        {

        }

        public Sprite makePlayer(Input input, TypePlayer typePlayer, Texture2D moveTexture, Texture2D shootTexture, Texture2D idleTexture, Texture2D deathTexture, Texture2D standStillTexture, Texture2D jumpTexture, Texture2D bowDownTexture,
                                Vector2 startPosition, float speed, PlayerBullet bullet, Score score, bool facingDirectionIndicator)
        {
            switch (typePlayer)
            {
                case TypePlayer.Archeologist:
                    return new Archeologist(moveTexture, shootTexture, idleTexture, deathTexture, standStillTexture, jumpTexture, bowDownTexture) 
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
