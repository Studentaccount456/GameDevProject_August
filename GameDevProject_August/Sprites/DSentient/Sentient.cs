using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject_August.Sprites.DSentient
{
    public class Sentient : Sprite
    {
        protected Texture2D MoveTexture;
        protected Vector2 offsetMoveAnimation = new Vector2(0, 0);

        public bool isDeathAnimating = false;

        public bool IsKilled = false;

        public float Speed = 2f;

        public bool HasDied = false;

        public Sentient(Texture2D moveTexture) : base()
        {
            MoveTexture = moveTexture;
        }

        public Sentient()
        {

        }
    }
}
