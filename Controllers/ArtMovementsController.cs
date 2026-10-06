using Microsoft.AspNetCore.Mvc;
using ArtHistoryMap.Api.DTOs;
using ArtHistoryMap.Api.Services;

namespace ArtHistoryMap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtMovementsController : ControllerBase
    {
        private readonly ArtMovementService _service;

        public ArtMovementsController(ArtMovementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArtMovementSummaryDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("graph")]
        public async Task<ActionResult<GraphDto>> GetGraph()
            => Ok(await _service.GetGraphAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ArtMovementDetailDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
