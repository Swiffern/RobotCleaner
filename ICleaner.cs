using RobotCleaner.Containers;
using RobotCleaner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner
{
    public interface ICleaner
    {
        CleaningResult Clean(Instructions instructions);
    }
}
