using Microsoft.AspNetCore.SignalR;
using Payment.BusinessLayer.Abstract;

namespace Payment.WebApi.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string title, string message, bool isRead,string url, DateTime createdAt)
        {
            await Clients.All.SendAsync("ReceiveNotification", title, message, isRead,url, createdAt);
        }
    }
}
