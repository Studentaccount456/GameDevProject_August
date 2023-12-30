using GameDevProject_August.Levels.Level3;
using GameDevProject_August.Sprites;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Projectiles.Bullets.Types;
using GameDevProject_August.Sprites.DNotSentient.TypeNotSentient.Terminal;
using GameDevProject_August.Sprites.DSentient.TypeSentient.Player;
using GameDevProject_August.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameDevProject_August.States.LevelStates
{
    public class Level3State : PlayingState
    {

        // Might use later
        private List<Component> _gameComponents;

        public Level3State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isFromMainMenu) : base(game, graphicsDevice, content)
        {
            levelNumber = 3;
            nextLevelIndicator = levelNumber + 1;

            Level = new Level3(new Level3BlockFactory());
            InitializeContent();
            InitializeScore(levelNumber, isFromMainMenu);

            GenerateLevelSprites();

        }

        private void GenerateLevelSprites()
        {
            SpriteList = new List<Sprite>()
            {
                player_1.makePlayer(arrowInput(),TypePlayer.Archeologist,personMoveTexture, personShootTexture, personIdleTexture, personDeathTexture, personStandStillTexture, personJumpTexture, personBowDownTexture, new Vector2(1200,600), 7f, new PlayerBullet(playerBullet), Game1.PlayerScore, false),

                new FinalTerminal(finalTerminalTexture)
                {
                    Position = new Vector2(500, 465)
                }
            };
        }
    }
}
