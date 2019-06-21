using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MazeRetreat.Api.Logic;
using MazeRetreat.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// This endpoint provides you with a list of all maze challenges.
        /// </summary>
        /// <returns>A list of challenges, containing all maze challenges for this CodeRetreat.</returns>
        /// <remarks>A challenge will contain a unique identifier, a short description, a Base64 encoded layout pattern and a link to a rendered image.</remarks>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<Challenge>))]
        public async Task<IActionResult> GetAllMazes()
        {
            var challenges = await _mazeLogic.GetAllMazes();
            return Ok(challenges);
        }

        /// <summary>
        /// This endpoint provides you with a single maze challenge based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the maze challenge you want to retrieve.</param>
        /// <returns>The challenge you have requested.</returns>
        /// <remarks>A challenge will contain a unique identifier, a short description, a Base64 encoded layout pattern and a link to a rendered image.</remarks>
        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(Challenge))]
        public async Task<IActionResult> GetMaze(Guid id)
        {
            var challenge = await _mazeLogic.GetMaze(id);
            return challenge != null ? Ok(challenge) : (IActionResult)NotFound();
        }

        /// <summary>
        /// This endpoint should be used, to send your solution. You will receive some feedback.
        /// </summary>
        /// <param name="id">The unique identifier of the maze challenge you want send your solution for.</param>
        /// <param name="solution">An object with data property containing your proposed solution.</param>
        /// <returns>Some feedback about your proposed solution.</returns>
        [HttpPut("{id}")]
        [SwaggerResponse(200, Type = typeof(Feedback))]
        public async Task<IActionResult> PutSolution(Guid id, [FromBody] Solution solution)
        {
            var feedback = await _mazeLogic.CheckSolution(id, solution);
            return feedback != null ? Ok(feedback) : (IActionResult)NotFound();
        }
    }
}