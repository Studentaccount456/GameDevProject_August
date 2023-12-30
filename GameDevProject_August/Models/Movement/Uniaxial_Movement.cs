namespace GameDevProject_August.Models.Movement
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    public class Uniaxial_Movement
    {
        public Direction Direction { get; set; }

        
        public void flipDirectionLeftAndRight()
        {
            if (Direction == Direction.Left) 
            {
                Direction = Direction.Right;
            } else if (Direction == Direction.Right)
            {
                Direction = Direction.Left;
            }
        }

        public void flipDirectionUpAndDown()
        {
            if (Direction == Direction.Up)
            {
                Direction = Direction.Down;
            }
            else if (Direction == Direction.Down)
            {
                Direction = Direction.Up;
            }
        }
    }
}
