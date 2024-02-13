using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace FacilitEase.Hubs
{
    [EnableCors("AllowAngularDev")]
    public class NotificationHub : Hub
    {
        public async Task ReceiveNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}