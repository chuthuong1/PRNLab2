using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApplication1.ChatHubSignarl
{
    public class CartHub : Hub
    {
        public async Task UpdateCartQuantity(int quantity)
        {
            // Gửi thông điệp realtime đến tất cả các kết nối khách hàng
            await Clients.All.SendAsync("ReceiveCartQuantity", quantity);
        }
        public async Task UpdateCartItemQuantity(int productId, int quantity)
        {
            // Gửi thông điệp realtime đến tất cả các kết nối khách hàng
            await Clients.All.SendAsync("ReceiveCartItemQuantity", productId, quantity);
        }
    }
}
