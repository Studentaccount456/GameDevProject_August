using GameDevProject_August.Levels.Level1;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Collectibles;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles;
using GameDevProject_August.Sprites.DSentient;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Enemy;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.LevelStates
{
    public class Level1State : PlayingState
    {

        // Might use later
        private List<Component> _gameComponents;

        public Level1State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            nextLevelIndicator = 2;

            Level = new Level1(new Level1BlockFactory());
            InitializeContent();
            InitializeScore(1, isFromMainMenu);

            GenerateLevelSprites();


        }

        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(10,564), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore, true),

                new Minotaur(minotaurMoveTexture, minotaurCastTexture, minotaurIdleTexture, glitchDeathTexture, new Vector2(1085,566), new Vector2(285,71), 354, 115)
                {
                    Speed = 11f,
                },

                new Porcupine(porcupineMoveTexture, glitchDeathTexture)
                {
                    Position = new Vector2(210,600),
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
