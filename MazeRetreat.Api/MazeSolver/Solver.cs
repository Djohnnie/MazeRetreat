using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRetreat.Api.MazeSolver
{
    public class Solver
    {
        public String Solve(String mazeData)
        {
            Maze maze = BuildMaze(mazeData);

            maze.Cells.ForEach(x => x.Step = null);
            var startCell = maze.Cells.SingleOrDefault(x => x is Start);
            if (startCell != null)
            {
                Step(maze, startCell, step: 1);
            }
            var finishCell = maze.Cells.SingleOrDefault(x => x is Finish);
            if (finishCell != null)
            {
                Backtrack(maze, finishCell);
            }

            return BuildSolution(maze);
        }

        private Maze BuildMaze(String mazeData)
        {
            var mazeRows = mazeData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var maze = new Maze(mazeRows[0].Length, mazeRows.Length);

            for (int x = 0; x < maze.Width; x++)
            {
                for (int y = 0; y < maze.Height; y++)
                {
                    switch (mazeRows[y][x])
                    {
                        case '.':
                            maze.Cells.Add(new Empty(x, y));
                            break;
                        case '#':
                            maze.Cells.Add(new Wall(x, y));
                            break;
                        case 'S':
                            maze.Cells.Add(new Start(x, y));
                            break;
                        case 'F':
                            maze.Cells.Add(new Finish(x, y));
                            break;
                        case '1':
                            maze.Cells.Add(new Teleport(x, y, 1));
                            break;
                        case '2':
                            maze.Cells.Add(new Teleport(x, y, 2));
                            break;
                        case '3':
                            maze.Cells.Add(new Teleport(x, y, 3));
                            break;
                        case '4':
                            maze.Cells.Add(new Teleport(x, y, 4));
                            break;
                    }
                }
            }

            return maze;
        }

        private String BuildSolution(Maze maze)
        {
            StringBuilder solutionBuilder = new StringBuilder();

            var cells = maze.Cells.Where(x => x.Step.HasValue).OrderBy(x => x.Step.Value).ToList();

            int step = 0;
            foreach (var cell in cells)
            {
                step++;
                solutionBuilder.Append($"{step}:{cell.X},{cell.Y};");
            }

            return solutionBuilder.ToString();
        }

        private void Step(Maze maze, Cell startCell, Int32 step)
        {
            Step(maze, new List<Cell> { startCell }, step);
        }

        private void Step(Maze maze, IEnumerable<Cell> cells, Int32 step)
        {
            var finishFound = false;
            List<Cell> nextCells = new List<Cell>();
            foreach (var cell in cells)
            {
                var neighbourCells = FindNeighbours(maze, cell);
                var found = neighbourCells.Any(x => x is Finish);
                if (found)
                {
                    neighbourCells.Single(x => x is Finish).Step = step;
                }
                else
                {
                    foreach (var nextCell in neighbourCells)
                    {
                        if (nextCell.Step == null || nextCell.Step > step)
                        {
                            nextCell.Step = step;
                            nextCells.Add(nextCell);
                            var teleportedCell = FindCorrespondingTeleportCell(maze, nextCell);
                            if (teleportedCell != null) { teleportedCell.Step = step; }
                        }
                    }
                }
                finishFound = finishFound || found;
            }

            if (!finishFound && nextCells.Count > 0)
            {
                Step(maze, nextCells, step + 1);
            }
        }

        private List<Cell> FindNeighbours(Maze maze, Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();
            var currentCell = FindCorrespondingTeleportCell(maze, cell) ?? cell;
            AddNeighbourCell(maze, neighbours, currentCell.X - 1, currentCell.Y);
            AddNeighbourCell(maze, neighbours, currentCell.X, currentCell.Y - 1);
            AddNeighbourCell(maze, neighbours, currentCell.X + 1, currentCell.Y);
            AddNeighbourCell(maze, neighbours, currentCell.X, currentCell.Y + 1);
            return neighbours;
        }

        private void AddNeighbourCell(Maze maze, List<Cell> neighbours, Int32 x, Int32 y)
        {
            if (x >= 0 && x < maze.Width && y >= 0 && y < maze.Height)
            {
                var neighbourCell = maze[x, y];
                if (neighbourCell != null && (neighbourCell is Empty || neighbourCell is Teleport || neighbourCell is Finish))
                {
                    neighbours.Add(neighbourCell);
                }
            }
        }

        private void Backtrack(Maze maze, Cell cell)
        {
            List<Cell> shortestPathCells = new List<Cell>();
            Backtrack(maze, cell, shortestPathCells);
            maze.Cells.ForEach(c => { if (!shortestPathCells.Contains(c)) { c.Step = null; } });
        }

        private void Backtrack(Maze maze, Cell cell, List<Cell> history)
        {
            if (!(cell is Start))
            {
                history.Add(cell);
                var previousCellInPath = FindPreviousCellInPath(maze, cell);
                if (previousCellInPath != null)
                {
                    Backtrack(maze, previousCellInPath, history);
                }
            }
        }

        private Cell FindPreviousCellInPath(Maze maze, Cell cell)
        {
            var neighbourCells = FindNeighbours(maze, cell);
            var previousCells = neighbourCells.Where(x => x.Step.HasValue && x.Step == cell.Step - 1);
            var previousCell = previousCells.FirstOrDefault(x => !(x is Teleport)) ?? previousCells.FirstOrDefault();
            return previousCell;
        }

        private Cell FindCorrespondingTeleportCell(Maze maze, Cell cell)
        {
            var teleportCell = cell as Teleport;
            if (teleportCell != null)
            {
                return maze.Cells
                    .Where(x => x is Teleport && x != teleportCell)
                    .Cast<Teleport>()
                    .SingleOrDefault(x => x.Address == teleportCell.Address);
            }
            return null;
        }
    }
}