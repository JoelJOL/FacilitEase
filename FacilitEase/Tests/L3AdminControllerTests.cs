using FacilitEase.Controllers;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;




namespace FacilitEase.Tests
{
    public class L3AdminControllerTests
    {
        [Fact]
        public void GetCommentTextByTicketId_ReturnsCommentText()
        {
            // Arrange
           /* var ticketId = 123;
            var mockService = new Mock<L3AdminService>(); // Replace AdminService with your actual service class
            var expectedCommentText = "Mocked comment text";
            mockService.Setup(s => s.GetCommentTextByTicketId(ticketId)).Returns(expectedCommentText);

            var logger = Mock.Of<ILogger<L3AdminController>>(); // Mock the logger
            var controller = new L3AdminController(mockService.Object, logger);

            // Act
            var result = controller.GetCommentTextByTicketId(ticketId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCommentText, okResult.Value);*/
        }
    }
}
