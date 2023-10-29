using GameDevProject_August.Levels.Level2;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.LevelStates
{
    public class Level2State : PlayingState
    {

        // Might use later
        private List<Component> _gameComponents;

        public Level2State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            levelNumber = 2;
            nextLevelIndicator = levelNumber + 1;

            Level = new Level2(new Level2BlockFactory());
            InitializeContent();
            InitializeScore(levelNumber, isFromMainMenu);

            GenerateLevelSprites();

        }


        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(10,600), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore, true),


                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture, new Vector2(250, 595))
                {
                    Position = new Vector2(250, 595),
                    Speed = 2f,
                },
                new Dragonfly(dragonflyMoveTexture, glitchDeathTexture,new Vector2(650,570))
                {
                    Position = new Vector2(650,570),
                    Speed = 2f,
                },
                new RatMage(ratMoveTexture, ratCastTexture, ratIdleTexture, glitchDeathTexture, new Vector2(305, 408), new Vector2(300,101), 600, 150)
                {
                    Speed = 2f,
                    Bullet = new EnemyBullet(ratProjectile),
        },
                new Regular_Point(RegularPointTexture)
                {
                    Position = new Vector2(865 ,400)
                }
            };
        }
    }
}
