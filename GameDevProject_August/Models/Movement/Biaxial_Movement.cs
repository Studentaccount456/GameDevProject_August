namespace GameDevProject_August.Models.Movement
{
    public enum Direction_X
    {
        Left,
        Right,
    }

    public enum Direction_Y
    {
        Up,
        Down,
        None
    }

    public class Biaxial_Movement
    {
        public Direction_X Direction_X{ get; set; }
        public Direction_Y Direction_Y { get; set; }


        public void flipDirectionLeftAndRight()
        {
            if (Direction_X == Direction_X.Left)
            {
                Direction_X = Direction_X.Right;
            }
            else if (Direction_X == Direction_X.Right)
            {
                Direction_X = Direction_X.Left;
            }
        }

        public void flipDirectionUpAndDown()
        {
            if (Direction_Y == Direction_Y.Up)
            {
                Direction_Y = Direction_Y.Down;
            }
            else if (Direction_Y == Direction_Y.Down)
            {
                Direction_Y = Direction_Y.Up;
            }
        }
    }
}
