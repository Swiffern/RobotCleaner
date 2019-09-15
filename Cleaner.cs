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
        private const int OFFSET = 100000;
        private const int GRID_SIZE = 200000;

        private CleanerContext _context;

        private Vector _currentPosition;
        private bool[][] _places;
        private long _uniquePlacesCleaned = 0;

        public Cleaner(CleanerContext context)
        {
            _context = context;
            InitGrid();
        }

        public CleaningResult Clean(Instructions instructions)
        {
            var startTime = DateTime.Now;

            _currentPosition = instructions.start;
            _places[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET] = true;
            _uniquePlacesCleaned++;

            foreach (var command in instructions.commands)
                ProcessCommand(command);

            var endTime = DateTime.Now;

            return StoreResult(instructions.commands.Count, _uniquePlacesCleaned, (endTime - startTime).TotalSeconds);
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

        private void ProcessCommand(Command command)
        {
            for (int i = 0; i < command.steps; i++)
                Move(command.direction);
        }

        private void Move(string direction)
        {
            switch (direction)
            {
                case "east":
                    _currentPosition.x++;
                    break;
                case "west":
                    _currentPosition.x--;
                    break;
                case "north":
                    _currentPosition.y++;
                    break;
                default:
                    _currentPosition.y--;
                    break;
            }

            if (!_places[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET])
            {
                _places[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET] = true;
                _uniquePlacesCleaned++;
            }
        }

        private void InitGrid()
        {
            _places = new bool[GRID_SIZE][];

            for (int i = 0; i < _places.Length; i++)
                _places[i] = new bool[GRID_SIZE];
        }

        #endregion
    }
}
