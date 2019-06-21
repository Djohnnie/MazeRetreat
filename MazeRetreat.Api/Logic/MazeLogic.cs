using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MazeRetreat.Api.DataAccess;
using MazeRetreat.Api.Extensions;
using MazeRetreat.Api.Helpers;
using MazeRetreat.Api.MazeSolver;
using MazeRetreat.Api.Model;
using Microsoft.EntityFrameworkCore;
using Maze = MazeRetreat.Api.DataAccess.Maze;

namespace MazeRetreat.Api.Logic
{
    public class MazeLogic
    {
        private readonly DatabaseContext _dbContext;
        private readonly RenderingLogic _renderingLogic;
        private readonly ImageLogic _imageLogic;
        private readonly MazeRetreatContext _mazeRetreatContext;

        public MazeLogic(
            DatabaseContext dbContext,
            RenderingLogic renderingLogic,
            ImageLogic imageLogic,
            MazeRetreatContext mazeRetreatContext)
        {
            _dbContext = dbContext;
            _renderingLogic = renderingLogic;
            _imageLogic = imageLogic;
            _mazeRetreatContext = mazeRetreatContext;
        }

        public async Task<List<Challenge>> GetAllMazes()
        {
            var challenges = new List<Challenge>();
            var mazes = await _dbContext.Mazes.ToListAsync();
            foreach (var maze in mazes)
            {
                challenges.Add(await MapChallenge(maze));
            }

            return challenges;
        }

        private async Task<Challenge> MapChallenge(Maze maze)
        {
            String solution = null;
            String renderedSolution = null;

            if (maze.SysId == 1)
            {
                var mazeData = maze.Data.Base64Decode();
                solution = Solve(mazeData);

                var solutionImageId = await _imageLogic.GetImageByChecksum(solution);
                if (!solutionImageId.HasValue)
                {
                    byte[] renderingData = _renderingLogic.RenderMaze(mazeData, solution);
                    solutionImageId = await _imageLogic.StoreImage(renderingData, solution);
                }

                renderedSolution = $"http://{_mazeRetreatContext.HostUri}/api/rendering/{solutionImageId}";
            }

            var imageId = await _imageLogic.GetImageByChecksum(maze.Data);
            if (!imageId.HasValue)
            {
                var mazeData = maze.Data.Base64Decode();
                byte[] renderingData = _renderingLogic.RenderMaze(mazeData, null);
                imageId = await _imageLogic.StoreImage(renderingData, maze.Data);
            }

            return new Challenge
            {
                Id = maze.Id,
                Description = maze.Description,
                Maze = maze.Data,
                Solution = solution,
                RenderedMaze = $"http://{_mazeRetreatContext.HostUri}/api/rendering/{imageId}",
                RenderedSolution = renderedSolution
            };
        }

        public async Task<Challenge> GetMaze(Guid id)
        {
            var maze = await _dbContext.Mazes.SingleOrDefaultAsync(x => x.Id == id);

            if (maze != null)
            {
                var mazeData = maze.Data.Base64Decode();
                var imageId = await _imageLogic.GetImageByChecksum(maze.Data);
                if (!imageId.HasValue)
                {
                    byte[] renderingData = _renderingLogic.RenderMaze(mazeData, null);
                    imageId = await _imageLogic.StoreImage(renderingData, maze.Data);
                }

                return new Challenge
                {
                    Id = maze.Id,
                    Description = maze.Description,
                    Maze = maze.Data,
                    RenderedMaze = $"http://{_mazeRetreatContext.HostUri}/api/rendering/{imageId}"
                };
            }

            return null;
        }

        public async Task<Feedback> CheckSolution(Guid id, Solution solution)
        {
            var maze = await _dbContext.Mazes.SingleOrDefaultAsync(x => x.Id == id);

            if (maze != null)
            {
                string renderedMaze = await GetRenderedMaze(maze);

                try
                {
                    var mazeData = maze.Data.Base64Decode();
                    string calculatedSolutionData = Solve(mazeData);

                    string renderedSolution = await GetRenderedMaze(maze, solution.Data);
                    string myRenderedSolution = await GetRenderedMaze(maze, calculatedSolutionData);

                    if (solution.Data == calculatedSolutionData)
                    {
                        return new Feedback
                        {
                            Description = "Your solution looks very nice :)",
                            YourSolution = solution.Data,
                            RenderedMaze = renderedMaze,
                            YourRenderedSolution = renderedSolution
                        };
                    }

                    return new Feedback
                    {
                        Description = "My solution is different than yours, and I am the allmighty know-it-all :p",
                        YourSolution = solution.Data,
                        RenderedMaze = renderedMaze,
                        YourRenderedSolution = renderedSolution,
                        MyRenderedSolution = myRenderedSolution
                    };
                }
                catch
                {
                    return new Feedback
                    {
                        Description = "Your solution made me very sad! I can't even decode it properly :(",
                        YourSolution = solution.Data,
                        RenderedMaze = renderedMaze
                    };
                }
            }

            return null;
        }

        private String Solve(String mazeData)
        {
            Solver solver = new Solver();
            return solver.Solve(mazeData);
        }

        private async Task<String> GetRenderedMaze(Maze maze, String solutionData = null)
        {
            if (maze != null)
            {
                if (solutionData != null) { solutionData = $"{maze.Data}-{solutionData}"; }
                var mazeData = maze.Data.Base64Decode();
                var imageId = await _imageLogic.GetImageByChecksum(solutionData ?? maze.Data);
                if (!imageId.HasValue)
                {
                    byte[] renderingData = _renderingLogic.RenderMaze(mazeData, solutionData);
                    imageId = await _imageLogic.StoreImage(renderingData, solutionData ?? maze.Data);
                }

                return $"http://{_mazeRetreatContext.HostUri}/api/rendering/{imageId}";
            }

            return null;
        }
    }
}