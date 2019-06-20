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

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var image = await _imageLogic.LoadImage(id);
            return image != null ? (IActionResult)File(image.Data, "image/png") : NotFound();
        }

        //[HttpPost]
        //public async Task<IActionResult> Render([FromBody]String mazeData)
        //{
        //    var image = await _renderingLogic.RenderMaze(mazeData);
        //    var image = await _imageLogic.StoreImage()
        //}
    }
}