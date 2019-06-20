using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRetreat.Api.MazeSolver
{
    public class Maze
    {
        #region [ Private Members ]

        private readonly List<Cell> _cells = new List<Cell>();

        #endregion

        #region [ Public Properties ]

        public Int32 Width { get; set; }

        public Int32 Height { get; set; }

        public Cell this[Int32 x, Int32 y]
        {
            get { return _cells.SingleOrDefault(c => c.X == x && c.Y == y); }
            set
            {
                var cell = _cells.SingleOrDefault(c => c.X == x && c.Y == y);
                if (cell == null)
                {
                    _cells.Add(value);
                }
            }
        }

        public List<Cell> Cells { get { return _cells; } }

        #endregion

        #region [ Construction ]

        public Maze() { }

        public Maze(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
        }

        #endregion
    }
}