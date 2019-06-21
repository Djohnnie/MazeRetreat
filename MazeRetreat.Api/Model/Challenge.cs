using System;

namespace MazeRetreat.Api.Model
{
    public class Challenge
    {
        /// <summary>
        /// The unique identifier for the challenge maze.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// A description for this specific maze.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// A Base64 encoded string containing the maze data.
        /// </summary>
        public String Maze { get; set; }

        /// <summary>
        /// A semi-colon separated list of steps needed to traverse the maze.
        /// </summary>
        public String Solution { get; set; }

        /// <summary>
        /// A link to a rendered image of the maze.
        /// </summary>
        public String RenderedMaze { get; set; }

        /// <summary>
        /// A link to a rendered image of the solution.
        /// </summary>
        public String RenderedSolution { get; set; }
    }
}