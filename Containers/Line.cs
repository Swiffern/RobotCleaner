using System;

namespace RobotCleaner.Containers
{
    public class Line
    {
        public Vector Position { get; set; }

        public Vector Direction { get; set; }

        public int Left
        {
            get { return Math.Min(Position.x, Position.x + Direction.x); }
        }
        
        public int Right
        {
            get { return Math.Max(Position.x, Position.x + Direction.x); }
        }

        public int Top
        {
            get { return Math.Max(Position.y, Position.y + Direction.y); }
        }

        public int Bottom
        {
            get { return Math.Min(Position.y, Position.y + Direction.y); }
        }

        public int UniquePoints { get; set; }

        public Line() { }

        public Line(int x, int y, string direction, int steps)
        {
            Position = new Vector(x, y);
            Direction = new Vector(direction, steps);
        }

        public int CountUniquePoints()
        {
            return Math.Abs(Direction.x + Direction.y) + 1;
        }
    }
}
