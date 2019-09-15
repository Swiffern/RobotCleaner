using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner.Containers
{
    public class Command
    {
        public string direction { get; set; }

        public int steps { get; set; }
    }
}
