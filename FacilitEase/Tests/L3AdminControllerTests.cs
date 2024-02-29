using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FacilitEase.Controllers;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FacilitEase.Tests.Controllers
{
    public class L3AdminControllerTests
    {
        [Fact]
        public void GetTicketDetails_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.GetTicketDetails(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CloseRaisedTicketStatus_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.CloseRaisedTicketStatus(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

       

        
        [Fact]
        public void ForwardRaisedTicketStatus_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.ForwardRaisedTicketStatus(1, 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ForwardRaisedTicketToDept_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.ForwardRaisedTicketToDept(1, 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTicketsByAgent_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.GetTicketsByAgent(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetResolvedTicketsByAgent_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.GetResolvedTicketsByAgent(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetOnHoldTicketsByAgent_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.GetOnHoldTicketsByAgent(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void AcceptTicketCancellation_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.AcceptTicketCancellation(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DenyTicketCancellation_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.DenyTicketCancellation(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteComment_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            mockAdminService.Setup(service => service.DeleteCommentAsync(It.IsAny<int>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteComment(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTimeSinceLastUpdate_ReturnsOkResult()
        {
            // Arrange
            var mockAdminService = new Mock<IL3AdminService>();
            mockAdminService.Setup(service => service.GetTimeSinceLastUpdate(It.IsAny<int>())).Returns("1 hour ago");
            var mockLogger = new Mock<ILogger<L3AdminController>>();
            var controller = new L3AdminController(mockAdminService.Object, mockLogger.Object);

            // Act
            var result = controller.GetTimeSinceLastUpdate(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
