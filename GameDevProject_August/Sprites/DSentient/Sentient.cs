using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DSentient
{
    public class Sentient : Sprite
    {
        protected Texture2D MoveTexture;

        // Death
        public bool isDeathAnimating = false;

        public bool IsKilled = false;

        // Move
        public float Speed = 2f;

        public bool HasDied = false;

        public Sentient(Texture2D texture) : base(texture)
        {
            MoveTexture = texture;
        }

        public Sentient()
        {
        }
    }
}
