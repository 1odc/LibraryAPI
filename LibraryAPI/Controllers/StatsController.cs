using LibraryAPI.Dtos;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/stats")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("books")]
        public async Task<ActionResult<BookStatsDto>> GetBookStats()
            => Ok(await _statsService.GetBookStatsAsync());
    }
}