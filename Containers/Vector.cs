using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner.Containers
{
    public class Vector
    {
        private const string EAST = "east";
        private const string WEST = "west";
        private const string NORTH = "north";

        public int x { get; set; }

        public int y { get; set; }

        public Vector() { }

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(string direction, int steps)
        {
            switch (direction)
            {
                case EAST:
                    x = steps;
                    break;
                case WEST:
                    x = -steps;
                    break;
                case NORTH:
                    y = steps;
                    break;
                default:
                    y = -steps;
                    break;
            }
        }
    }
}
