using Microsoft.AspNetCore.SignalR;
using WebApplication1.DataAccess.Models;
//using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Hubs
{
    public class QuantityHub : Hub // Hub is a class in the SignalR library
    {
        private readonly IHubContext<QuantityHub> _hubContext;
    }
}
