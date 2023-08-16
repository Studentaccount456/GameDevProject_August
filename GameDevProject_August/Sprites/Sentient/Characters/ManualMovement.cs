using GameDevProject_August.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameDevProject_August.Sprites.Sentient.Characters
{
    public enum DirectionMove
    {
        MovingUp, MovingDown, MovingLeft, MovingRight, None
    }

    public class ManualMovement
    {
        public Input InputSprite = new Input()
        {
            Down = System.Windows.Forms.Keys.Down,
            Up = System.Windows.Forms.Keys.Up,
            Left = System.Windows.Forms.Keys.Left,
            Right = System.Windows.Forms.Keys.Right,
            Shoot = System.Windows.Forms.Keys.Space
        };


        public DirectionMove DirectionOfMovement;

        public ManualMovement()
        {
        }

        public void Move(Vector2 Position, Rectangle rectangle, Vector2 velocity, float speed, Vector2 facingDirection, bool facingDirectionIndicator)
        {

            if (InputSprite == null)
                return;

            if (Keyboard.GetState().IsKeyDown((Keys)InputSprite.Up))
            {
                velocity.Y -= speed;
                DirectionOfMovement = DirectionMove.MovingUp;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)InputSprite.Down))
            {
                velocity.Y += speed;
                DirectionOfMovement = DirectionMove.MovingDown;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)InputSprite.Left))
            {
                velocity.X -= speed;
                facingDirection = -Vector2.UnitX;
                facingDirectionIndicator = false;
                DirectionOfMovement = DirectionMove.MovingLeft;
            }
            if (Keyboard.GetState().IsKeyDown((Keys)InputSprite.Right))
            {
                velocity.X += speed;
                facingDirection = Vector2.UnitX;
                facingDirectionIndicator = true;
                DirectionOfMovement = DirectionMove.MovingRight;

            }
            if (!Keyboard.GetState().IsKeyDown((Keys)InputSprite.Left) || Keyboard.GetState().IsKeyDown((Keys)InputSprite.Right) || Keyboard.GetState().IsKeyDown((Keys)InputSprite.Down) || Keyboard.GetState().IsKeyDown((Keys)InputSprite.Up))
            {
                DirectionOfMovement = DirectionMove.None;
            }

            Position = Vector2.Clamp(Position, new Vector2(0, 0 + rectangle.Height / 4), new Vector2(Game1.ScreenWidth - rectangle.Width, Game1.ScreenHeight - rectangle.Height));
        }
    }
}
