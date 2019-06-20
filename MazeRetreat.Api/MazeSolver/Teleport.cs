using System;

namespace MazeRetreat.Api.MazeSolver
{
    public class Teleport : Cell
    {
        #region [ Public Properties ]

        public Byte Address { get; set; }

        #endregion

        #region [ Construction ]

        public Teleport() { }

        public Teleport(Int32 x, Int32 y, Byte address) : base(x, y)
        {
            this.Address = address;
        }

        #endregion
    }
}