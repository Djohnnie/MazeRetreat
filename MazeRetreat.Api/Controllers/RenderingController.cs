using System;
using System.Threading.Tasks;
using MazeRetreat.Api.Logic;
using Microsoft.AspNetCore.Mvc;

namespace MazeRetreat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenderingController : ControllerBase
    {
        private readonly ImageLogic _imageLogic;
        private readonly RenderingLogic _renderingLogic;

        public RenderingController(
            ImageLogic imageLogic,
            RenderingLogic renderingLogic)
        {
            _imageLogic = imageLogic;
            _renderingLogic = renderingLogic;
        }

        /// <summary>
        /// This endpoint will provide you with a PNG image, based on the unique identifier of the rendering you are requesting.
        /// </summary>
        /// <param name="id">The unique identifier of the rendering you are requesting.</param>
        /// <returns>A PNG image.</returns>
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var image = await _imageLogic.LoadImage(id);
            return image != null ? (IActionResult)File(image.Data, "image/png") : NotFound();
        }
    }
}