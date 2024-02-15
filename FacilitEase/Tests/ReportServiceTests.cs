using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace FacilitEase.Tests
{
    [TestFixture]
    public class ReportServiceTests
    {
        [Test]
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
            new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "resolved" } // Changing this to 'resolved' instead of 'escalated'
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
            Assert.Equals(2, report.Resolved); // Fail if the number of resolved tickets is not 2
            Assert.Equals(1, report.Escalated); // Pass if there is one escalated ticket
            Assert.Equals(5, report.Total); // Pass if the total is 5
        }
    }
}