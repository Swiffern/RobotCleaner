using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner.Containers
{
    public struct Vector
    {
        public int x { get; set; }

        public int y { get; set; }

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
