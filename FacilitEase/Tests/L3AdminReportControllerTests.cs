using FacilitEase.Controllers;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FacilitEase.UnitTests.Controllers
{
    public class L3AdminReportControllerTests
    {
        [Fact]
        public async Task GetL3AdminReport_ReturnsNotFound()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            var report = new Report { Total = 10, Resolved = 5, Escalated = 5 };
            reportServiceMock.Setup(service => service.GetReportDataYearTicketStatus(2)).Returns(report);
            var controller = new ReportController(reportServiceMock.Object);

            // Act
            var result = await controller.GetReportDataYearTicketStatus(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}