using System;

namespace MazeRetreat.Api.MazeSolver
{
    public class Cell
    {
        #region [ Public Properties ]

        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Int32? Step { get; set; }

        #endregion

        #region [ Construction ]

        public Cell() { }

        public Cell(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }
}