using System;
using System.Threading.Tasks;
using MazeRetreat.Api.Logic;
using MazeRetreat.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace MazeRetreat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MazeController : ControllerBase
    {
        private readonly MazeLogic _mazeLogic;

        public MazeController(MazeLogic mazeLogic)
        {
            _mazeLogic = mazeLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetFirstMaze()
        {
            var challenge = await _mazeLogic.GetFirstMaze();
            return challenge != null ? Ok(challenge) : (IActionResult)NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaze(Guid id)
        {
            var challenge = await _mazeLogic.GetMaze(id);
            return challenge != null ? Ok(challenge) : (IActionResult)NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PostSolution(Guid id, [FromBody] Solution solution)
        {
            var image = await _mazeLogic.CheckSolution(id, solution);
            return image != null ? (IActionResult)File(image.Data, "image/png") : BadRequest();
        }
    }
}