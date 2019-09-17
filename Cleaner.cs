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

            var (verticalLines, horizontalLines) = CreateLines(instructions);

            MergeLines(verticalLines, horizontalLines);
            CheckOverlapping(verticalLines, horizontalLines);

            var uniquePoints = verticalLines.Sum(a => a.UniquePoints) + horizontalLines.Sum(a => a.CountUniquePoints());

            var endTime = DateTime.Now;

            return StoreResult(instructions.commands.Count, uniquePoints, (endTime - startTime).TotalSeconds);
        }

        #region Privates

        private static void CheckOverlapping(IList<Line> verticalLines, IList<Line> horizontalLines)
        {
            Parallel.ForEach(verticalLines, (vertical) =>
            {
                CheckLines(vertical, horizontalLines);
            });
        }

        private static void CheckLines(Line vertical, IList<Line> horizontalLines)
        {
            vertical.UniquePoints = vertical.CountUniquePoints();

            foreach(var horizontal in horizontalLines)
            {
                if (vertical.Left >= horizontal.Left && vertical.Left <= horizontal.Right)
                    vertical.UniquePoints--;
            }
        }

        private static (IList<Line> verticalLines, IList<Line> horizontalLines) MergeLines(IList<Line> verticalLines, IList<Line> horizontalLines)
        {
            Parallel.Invoke(
                () =>
                {
                    for (int i = verticalLines.Count - 1; i > 0; i--)
                    {
                        var line1 = verticalLines[i];

                        for (int j = 0; j < i; j++)
                        {
                            var line2 = verticalLines[j];

                            if (line1.Position.x == line2.Position.x)
                            {
                                if (VerticalOverlaps(line1, line2))
                                {
                                    line2.Position.y = Math.Max(line1.Top, line2.Top);
                                    line2.Direction.x = Math.Min(line1.Bottom, line2.Bottom);
                                    verticalLines.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                    }

                },
                () =>
                {
                    for (int i = horizontalLines.Count - 1; i > 0; i--)
                    {
                        var line1 = horizontalLines[i];

                        for (int j = 0; j < i; j++)
                        {
                            var line2 = horizontalLines[j];

                            if (line1.Position.y == line2.Position.y)
                            {
                                if (HorizontalOverlaps(line1, line2))
                                {
                                    line2.Position.y = Math.Max(line1.Left, line2.Left);
                                    line2.Direction.x = Math.Min(line1.Right, line2.Right);
                                    horizontalLines.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                    }
                }
            );

            return (verticalLines, horizontalLines);
        }

        private static bool HorizontalOverlaps(Line line1, Line line2)
        {
            return line2.Left < line1.Right && line1.Left < line2.Right;
        }

        private static bool VerticalOverlaps(Line line1, Line line2)
        {
            return line2.Bottom < line1.Top && line1.Bottom < line2.Top;
        }

        private static (IList<Line> verticalLines, IList<Line> horizontalLines) CreateLines(Instructions instructions)
        {
            var verticalLines = new List<Line>();
            var horizontalLines = new List<Line>();

            var firstCommand = instructions.commands.First();
            firstCommand.posX = instructions.start.x;
            firstCommand.posY = instructions.start.y;

            var firstLine = new Line(firstCommand.posX, firstCommand.posY, firstCommand.direction, firstCommand.steps);

            if (firstLine.Direction.x == 0)
                verticalLines.Add(firstLine);
            else
                horizontalLines.Add(firstLine);

            ProcessCommands(instructions, verticalLines, horizontalLines, firstCommand);

            return (verticalLines, horizontalLines);
        }

        private static void ProcessCommands(Instructions instructions, List<Line> verticalLines, List<Line> horizontalLines, Command firstCommand)
        {
            var previousCommand = firstCommand;

            foreach (var command in instructions.commands.Skip(1))
            {
                command.posX = previousCommand.posX;
                command.posY = previousCommand.posY;

                switch (previousCommand.direction)
                {
                    case EAST:
                        command.posX += previousCommand.steps;
                        break;
                    case WEST:
                        command.posX -= previousCommand.steps;
                        break;
                    case NORTH:
                        command.posY += previousCommand.steps;
                        break;
                    default:
                        command.posY -= previousCommand.steps;
                        break;
                }

                var line = new Line(command.posX, command.posY, command.direction, command.steps);

                if (line.Direction.x == 0)
                    verticalLines.Add(line);
                else
                    horizontalLines.Add(line);

                previousCommand = command;
            }
        }

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

        #endregion
    }
}
