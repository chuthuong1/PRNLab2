using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.IRepository
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}
