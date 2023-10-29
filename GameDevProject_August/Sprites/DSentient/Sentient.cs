using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DSentient
{
    public class Sentient : Sprite
    {
        // Death
        public bool isDeathAnimating = false;

        public bool IsKilled = false;

        // Move
        public float Speed = 2f;

        public bool HasDied = false;

        public Sentient(Texture2D texture) : base(texture)
        {
        }
    }
}
