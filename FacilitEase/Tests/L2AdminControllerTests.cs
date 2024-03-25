using FacilitEase.Controllers;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FacilitEase.Tests
{
    public class L2AdminControllerTests
    {
        [Fact]
        public void GetAgents_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor
            // Act
            var result = controller.GetAgents(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetTicketDetails_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.GetTicketDetails(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetUnassignedTickets_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.GetUnassignedTickets(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void AssignTicketToAgent_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.AssignTicketToAgent(new AssignTicket());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAssignedTickets_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.GetAssignedTickets(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetEscalatedTickets_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.GetEscalatedTickets(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAgentsByDepartment_ReturnsOkResult()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var ticketServiceMock = new Mock<ITicketService>();
            var slaServiceMock = new Mock<ISLAService>();
            var controller = new L2AdminController(employeeServiceMock.Object, ticketServiceMock.Object, slaServiceMock.Object); // Provide ISLAService in the constructor

            // Act
            var result = controller.GetAgentsByDepartment(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}