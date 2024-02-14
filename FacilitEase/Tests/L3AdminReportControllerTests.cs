using Xunit;
using Moq;
using FacilitEase.Controllers;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FacilitEase.UnitTests.Controllers
{
    public class L3AdminReportControllerTests
    {
        private L3AdminReportController _controller;
        private Mock<IReportService> _mockReportService;

        public L3AdminReportControllerTests()
        {
            _mockReportService = new Mock<IReportService>();
            _controller = new L3AdminReportController(_mockReportService.Object);
        }

        [Fact]
        public async Task GetL3AdminReport_ReturnsNotFound()
        {
            // Arrange
            var reportServiceMock = new Mock<IReportService>();
            reportServiceMock.Setup(service => service.GetReportData(It.IsAny<int>())).Returns<Report>(null);
            var controller = new L3AdminReportController(reportServiceMock.Object);

            // Act
            var result = await controller.GetReportData(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}