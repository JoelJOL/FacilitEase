using FacilitEase.Models.EntityModels;
using FacilitEase.Services;
using Moq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FacilitEase.Data;
using FacilitEase.UnitOfWork;

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

            var testData = new List<TBL_TICKET_ASSIGNMENT>
            {
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "resolved" },
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "resolved" },
                new TBL_TICKET_ASSIGNMENT { EmployeeId = 1, EmployeeStatus = "escalated" }
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
            Assert.Equals(2, report.Resolved); // Two resolved tickets for the user with id 1
            Assert.Equals(1, report.Escalated); // One escalated ticket for the user with id 1
            Assert.Equals(3, report.Total); // Total should be sum of resolved and escalated tickets
        }
    }
}
