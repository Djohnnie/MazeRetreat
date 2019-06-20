using System;

namespace MazeRetreat.Api.Model
{
    public class Challenge
    {
        public Guid Id { get; set; }
        public String Maze { get; set; }
        public String Solution { get; set; }
        public String RenderedMaze { get; set; }
    }
}