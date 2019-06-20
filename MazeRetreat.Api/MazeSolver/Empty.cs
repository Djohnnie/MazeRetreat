using System;

namespace MazeRetreat.Api.MazeSolver
{
    public class Empty : Cell
    {
        #region [ Construction ]

        public Empty() { }

        public Empty(Int32 x, Int32 y) : base(x, y)
        {
        }

        #endregion
    }
}