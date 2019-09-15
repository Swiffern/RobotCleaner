using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner.Containers
{
    public class Instructions
    {
        public Vector start { get; set; }

        public IList<Command> commands { get; set; }
    }
}
