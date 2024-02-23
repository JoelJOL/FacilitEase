using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class L3AdminReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public L3AdminReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReportDataYearTicketStatus(int id)
        {
            return Ok(_reportService.GetReportDataYearTicketStatus(id));
        }

        [HttpGet("chartdata/{id}")]
        public async Task<ActionResult<ChartData>> GetBarChartData(int id)
        {
            return Ok(_reportService.GetBarChartData(id));
        }

        [HttpGet("profiledata/{id}")]
        public async Task<ActionResult<ProfileData>> GetProfileData(int id)
        {
            return Ok(_reportService.GetProfileData(id));
        }

        [HttpGet("reportdata/{id}")]
        public async Task<ActionResult<WeekReport>> GetDailyAndWeeklyData(int id)
        {
            return Ok(_reportService.GetDailyAndWeeklyData(id));
        }

        [HttpGet("categoryReport/{id}")]
        public async Task<ActionResult<CategoryReportData>> GetCategoryReport(int id)
        {
            return Ok(_reportService.GetReportDataByCategory(id));
        }
    }
}