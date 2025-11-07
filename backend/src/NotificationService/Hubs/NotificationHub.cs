using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendLikeNotification(string videoId, long likeCount)
        {
            await Clients.All.SendAsync("ReceiveLikeNotification", videoId, likeCount);
        }

        public async Task SendCommentNotification(string videoId, string comment)
        {
            await Clients.All.SendAsync("ReceiveCommentNotification", videoId, comment);
        }
    }
}
