using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace FacilitEase.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public void GetReportData_ReturnsCorrectReport()
        {
            // Arrange
            int userId = 1;
            var mockContext = new Mock<AppDbContext>();
            var mockDbSet = new Mock<DbSet<TBL_TICKET_ASSIGNMENT>>();

            // Modify test data to intentionally make the test fail
            var testData = new List<TBL_TICKET_ASSIGNMENT>
            {
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "resolved" },
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "resolved" },
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "escalated" } // Corrected to 'escalated' instead of 'resolved'
            }.AsQueryable();

            mockDbSet.As<IQueryable<TBL_TICKET_ASSIGNMENT>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<TBL_TICKET_ASSIGNMENT>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<TBL_TICKET_ASSIGNMENT>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<TBL_TICKET_ASSIGNMENT>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            mockContext.Setup(c => c.TBL_TICKET_ASSIGNMENT).Returns(mockDbSet.Object);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var reportService = new ReportService(mockUnitOfWork.Object, mockContext.Object);

            // Act
            var report = reportService.GetReportData(userId);

            // Assert
            Assert.Equal(2, report.Resolved); // Pass if the number of resolved tickets is 2
            Assert.Equal(1, report.Escalated); // Pass if there is one escalated ticket
            Assert.Equal(3, report.Total); // Pass if the total is 3
        }
    }
}
