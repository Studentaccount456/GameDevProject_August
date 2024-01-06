using GameDevProject_August.Levels.Level1;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.AttackingEnemy.SpotterEnemy.MeleeEnemy.Enemies;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy.PassiveEnemy;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using GameDevProject_August.States.StateTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.LevelStates
{
    public class Level1State : PlayingState
    {
        public Level1State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            levelNumber = 1;
            nextLevelIndicator = levelNumber + 1;

            Level = new Level1(new Level1BlockFactory());
            InitializeContent();
            InitializeScore(levelNumber, isFromMainMenu);

            GenerateLevelSprites();


        }

        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(10,564), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore),

                new Minotaur(minotaurMoveTexture, minotaurCastTexture, minotaurIdleTexture, glitchDeathTexture, new Vector2(1085,566), new Vector2(285,71), 354, 115)
                {
                    Speed = 11f,
                },
                new Porcupine(porcupineMoveTexture, glitchDeathTexture, new Vector2(210,600))
                {
                    Speed = 2f,
                },
               new Regular_Point(RegularPointTexture)
                {
                    Position = new Vector2(575, 566)
                }
            };
        }

    }
}
