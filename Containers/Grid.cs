using System;
using System.Collections;
using System.Collections.Generic;

namespace RobotCleaner.Containers
{
    public class Grid
    {
        private const int OFFSET = 100000;
        private const int GRID_SIZE = 200000;
        private BitArray[] _structure;
        private Vector _currentPosition;

        public long UniquePlacesCleaned { get; set; }

        public Grid(Vector startPosition)
        {
            _structure = InitGrid();
            _currentPosition = startPosition;
            _structure[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET] = true;
        }

        public void Move(int x, int y)
        {
            _currentPosition.x += x;
            _currentPosition.y += y;

            if (!_structure[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET])
            {
                _structure[_currentPosition.x + OFFSET][_currentPosition.y + OFFSET] = true;
                UniquePlacesCleaned++;
            }
        }

        private static BitArray[] InitGrid()
        {
            var grid = new BitArray[GRID_SIZE];

            for (int i = 0; i < grid.Length; i++)
                grid[i] = new BitArray(GRID_SIZE);

            return grid;
        }
    }
}
