﻿using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace FacilitEase.Controllers
{
    [Authorize(Roles="L2Admin")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("api/{id}")]
        public async Task<ActionResult<Report>> GetReportDataYearTicketStatus(int id)
        {
            return Ok(_reportService.GetReportDataYearTicketStatus(id));
        }

        [HttpGet("api/chartdata/{id}")]
        public async Task<ActionResult<ChartData>> GetBarChartData(int id)
        {
            return Ok(_reportService.GetBarChartData(id));
        }

        [HttpGet("api/profiledata/{id}")]
        public async Task<ActionResult<ProfileData>> GetProfileData(int id)
        {
            return Ok(_reportService.GetProfileData(id));
        }

        [HttpGet("api/reportdata/{id}")]
        public async Task<ActionResult<WeekReport>> GetDailyAndWeeklyData(int id)
        {
            return Ok(_reportService.GetDailyAndWeeklyData(id));
        }

        [HttpGet("api/categoryReport/{id}")]
        public async Task<ActionResult<CategoryReportData>> GetCategoryReport(int id)
        {
            return Ok(_reportService.GetReportDataByCategory(id));
        }
        [HttpGet("api/tickets/admin/{id}")]
        public IActionResult GetTicketsByAdmin(int Id, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            return Ok(_reportService.GetTicketsByAdmin(Id, sortField, sortOrder, pageIndex, pageSize, searchQuery));
        }
        [HttpGet("api/exportdata")]
        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            ManagerTicketResponse<AdminReportTickets> dataResponse=_reportService.GetTicketsByAdmin(2,"SubmittedDate", "desc", 0, 10, "");

            if (dataResponse != null && dataResponse.Data != null)
            {
                var data = dataResponse.Data;

                // Add headers
                workSheet.Cells[1, 1].Value = "Id";
                workSheet.Cells[1, 2].Value = "Category";
                workSheet.Cells[1, 3].Value = "Raised By";
                workSheet.Cells[1, 4].Value = "Raised Date";
                workSheet.Cells[1, 5].Value = "Closed Date";
                workSheet.Cells[1, 6].Value = "Priority";
                workSheet.Cells[1, 7].Value = "Status";
                workSheet.Cells[1, 8].Value = "Department";
                workSheet.Cells[1, 9].Value = "Location";

                // Populate data under each heading
                int row = 2; // Start from the second row (after headers)
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1].Value = item.Id;
                    workSheet.Cells[row, 2].Value = item.TicketName;
                    workSheet.Cells[row, 3].Value = item.EmployeeName;
                    
                    workSheet.Cells[row, 5].Value = item.AssignedTo;
                    workSheet.Cells[row, 6].Value = item.SubmittedDate;
                    workSheet.Cells[row, 7].Value = item.Priority;
                    workSheet.Cells[row, 8].Value = item.Status;
                    workSheet.Cells[row, 9].Value = item.Location;
                    // Add other values similarly
                    row++;
                }

                // Convert ExcelPackage to a byte array
                byte[] fileBytes = excel.GetAsByteArray();

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
            }
            else
            {
                // Handle case where there is no data or an error occurred
                return NotFound(); // Or return an appropriate response
            }

        }
    }
}