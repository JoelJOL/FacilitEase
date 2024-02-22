using FacilitEase.Hubs;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class NotificationServiceTests
{
    [Fact]
    public async Task MonitorTicketChanges_ShouldSendNotifications()
    {
        // Arrange
        var hubContextMock = new Mock<IHubContext<NotificationHub>>();
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var ticketRepositoryMock = new Mock<IRepository<TBL_TICKET>>();
        var userRoleMappingRepositoryMock = new Mock<IUserRoleMappingRepository>();
        var userRepositoryMock = new Mock<IRepository<TBL_USER>>();
        var employeeRepositoryMock = new Mock<IRepository<TBL_EMPLOYEE>>();
        var notificationRepositoryMock = new Mock<IRepository<TBL_NOTIFICATION>>();

        ticketRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<TBL_TICKET>());
        userRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<TBL_USER>());
        employeeRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<TBL_EMPLOYEE>());
        userRoleMappingRepositoryMock.Setup(repo => repo.GetUserIdsByRoleId(It.IsAny<int>())).Returns(new List<int>());

        unitOfWorkMock.SetupGet(uow => uow.Ticket).Returns(Mock.Of<ITicketRepository>());
        unitOfWorkMock.SetupGet(uow => uow.User).Returns(Mock.Of<IUserRepository>());
        unitOfWorkMock.SetupGet(uow => uow.Employee).Returns(Mock.Of<IEmployeeRepository>());
        unitOfWorkMock.SetupGet(uow => uow.UserRoleMapping).Returns(Mock.Of<IUserRoleMappingRepository>());
        unitOfWorkMock.SetupGet(uow => uow.Notification).Returns(Mock.Of<INotificationRepository>());

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(provider => provider.GetService(typeof(IUnitOfWork))).Returns(unitOfWorkMock.Object);

        var serviceScopeMock = new Mock<IServiceScope>();
        serviceScopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProviderMock.Object);

        var scopeMock = new Mock<IServiceScopeFactory>();
        scopeMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);

        hubContextMock.SetupGet(hub => hub.Clients).Returns(Mock.Of<IHubClients>());

        var service = new NotificationService(hubContextMock.Object, scopeMock.Object);

        // Act
        await service.MonitorTicketChanges(CancellationToken.None);

        // Assert
        // Add your assertions here to verify the expected behavior, for example, checking if notifications were sent.
        hubContextMock.Verify(hub => hub.Clients.All.SendAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
