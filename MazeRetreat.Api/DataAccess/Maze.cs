using System;

namespace MazeRetreat.Api.DataAccess
{
    public class Maze
    {
        public Guid Id { get; set; }
        public Int32 SysId { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
    }
}