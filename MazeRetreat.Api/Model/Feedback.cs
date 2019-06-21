using System;

namespace MazeRetreat.Api.Model
{
    public class Feedback
    {
        /// <summary>
        /// A descriptive feedback on your solution.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// The solution you have provided.
        /// </summary>
        public String YourSolution { get; set; }

        /// <summary>
        /// A link to a rendered image of your solution.
        /// </summary>
        public String YourRenderedSolution { get; set; }

        /// <summary>
        /// A link to a rendered image of my solution, if you want to cheat.
        /// </summary>
        public String MyRenderedSolution { get; set; }

        /// <summary>
        /// A link to a rendered image of the maze without solution.
        /// </summary>
        public String RenderedMaze { get; set; }
    }
}