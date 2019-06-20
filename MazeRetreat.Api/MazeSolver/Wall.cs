using System;

namespace MazeRetreat.Api.MazeSolver
{
    public class Wall : Cell
    {
        #region [ Construction ]

        public Wall() { }

        public Wall(Int32 x, Int32 y) : base(x, y)
        {
        }

        #endregion
    }
}