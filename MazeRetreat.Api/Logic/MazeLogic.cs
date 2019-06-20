using System;
using System.Linq;
using System.Threading.Tasks;
using MazeRetreat.Api.DataAccess;
using MazeRetreat.Api.Extensions;
using MazeRetreat.Api.Helpers;
using MazeRetreat.Api.MazeSolver;
using MazeRetreat.Api.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Challenge> GetFirstMaze()
        {
            var mazeId = await _dbContext.Mazes.OrderBy(x => x.SysId).Select(x => (Guid?)x.Id).FirstOrDefaultAsync();
            return mazeId.HasValue ? await GetMaze(mazeId.Value) : null;
        }

        public async Task<Challenge> GetMaze(Guid id)
        {
            var maze = await _dbContext.Mazes.SingleOrDefaultAsync(x => x.Id == id);

            if (maze != null)
            {
                var mazeData = maze.Data.Base64Decode();
                byte[] renderingData = _renderingLogic.RenderMaze(mazeData, null);
                var image = await _imageLogic.StoreImage(renderingData);
                var solution = maze.SysId == 1 ? Solve(mazeData) : null;

                return new Challenge
                {
                    Id = maze.Id,
                    Maze = maze.Data,
                    Solution = solution,
                    RenderedMaze = $"http://{_mazeRetreatContext.HostUri}/api/rendering/{image.Id}"
                };
            }

            return null;
        }

        public async Task<Image> CheckSolution(Guid id, Solution solution)
        {
            var maze = await _dbContext.Mazes.SingleOrDefaultAsync(x => x.Id == id);

            if (maze != null)
            {
                var mazeData = maze.Data.Base64Decode();
                string calculatedSolutionData = Solve(mazeData);

                if (solution.Data == calculatedSolutionData)
                {
                    byte[] renderingData = _renderingLogic.RenderMaze(mazeData, calculatedSolutionData);
                    return await _imageLogic.StoreImage(renderingData);
                }
            }

            return null;
        }

        private String Solve(String mazeData)
        {
            Solver solver = new Solver();
            return solver.Solve(mazeData);
        }
    }
}