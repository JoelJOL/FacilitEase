using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class L3AdminReprtController : ControllerBase
    {
        private readonly IReportService _reportService;

        public L3AdminReprtController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReportData(int id)
        {
            return Ok(_reportService.GetReportData(id));
        }
        [HttpGet("chartdata/{id}")]
        public async Task<ActionResult<ChartData>> GetChartData(int id)
        {
            return Ok(_reportService.GetChartData(id));
        }
        [HttpGet("profiledata/{id}")]
        public async Task<ActionResult<ProfileData>> GetProfileData(int id)
        {
            return Ok(_reportService.GetProfileData(id));
        }
    }
}
