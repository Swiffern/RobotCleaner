using RobotCleaner.Containers;
using RobotCleaner.Data;
using RobotCleaner.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner
{
    public class Cleaner : ICleaner
    {
        private const string EAST = "east";
        private const string WEST = "west";
        private const string NORTH = "north";

        private CleanerContext _context;

        public Cleaner(CleanerContext context)
        {
            _context = context;
        }

        public CleaningResult Clean(Instructions instructions)
        {
            var startTime = DateTime.Now;

            var grid = new Grid(instructions.start);

            foreach(var command in instructions.commands)
                ProcessCommand(grid, command);

            var endTime = DateTime.Now;

            return StoreResult(instructions.commands.Count, grid.UniquePlacesCleaned, (endTime - startTime).TotalSeconds);
        }

        #region Privates

        private CleaningResult StoreResult(int numCommands, long uniquePlaces, double runtime)
        {
            var result = new CleaningResult
            {
                commands = numCommands,
                result = uniquePlaces,
                timestamp = DateTime.Now,
                duration = runtime
            };

            _context.CleaningResult.Add(result);
            _context.SaveChanges();

            return result;
        }

        private static void ProcessCommand(Grid grid, Command command)
        {
            var direction = GetDirection(command.direction);

            for (int i = 0; i < command.steps; i++)
            {
                grid.Move(direction.x, direction.y);
            }
        }

        private static (int x, int y) GetDirection(string direction)
        {
            switch (direction)
            {
                case EAST:
                    return (1, 0);
                case WEST:
                    return (-1, 0);
                case NORTH:
                    return (0, 1);
                default:
                    return (0, -1);
            }
        }

        #endregion
    }
}
