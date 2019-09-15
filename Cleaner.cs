using RobotCleaner.Containers;
using RobotCleaner.Data;
using RobotCleaner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotCleaner
{
    public class Cleaner : ICleaner
    {
        private Vector _currentPosition;
        private HashSet<Vector> _places = new HashSet<Vector>();
        private CleanerContext _context;

        public Cleaner(CleanerContext context)
        {
            _context = context;
        }

        public CleaningResult Clean(Instructions instructions)
        {
            var startTime = DateTime.Now;
            var places = new HashSet<Vector>();

            _currentPosition = instructions.start;
            places.Add(instructions.start);

            foreach (var command in instructions.commands)
                ProcessCommand(command);

            var endTime = DateTime.Now;

            return StoreResult(instructions.commands.Count, _places.Count, (endTime - startTime).TotalSeconds);
        }

        private CleaningResult StoreResult(int numCommands, int uniquePlaces, double runtime)
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

        #region Privates

        private void ProcessCommand(Command command)
        {
            var direction = GetDirection(command.direction);

            for (int i = 0; i < command.steps; i++)
                Move(direction);
        }

        private void Move(Vector direction)
        {
            _currentPosition.x += direction.x;
            _currentPosition.y += direction.y;

            _places.Add(new Vector(_currentPosition.x, _currentPosition.y));
        }

        private Vector GetDirection(string direction)
        {
            switch (direction)
            {
                case "east":
                    return new Vector(1, 0);
                case "west":
                    return new Vector(-1, 0);
                case "north":
                    return new Vector(0, 1);
                default:
                    return new Vector(0, -1);
            }
        }

        #endregion
    }
}
