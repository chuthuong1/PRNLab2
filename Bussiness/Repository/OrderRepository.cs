using WebApplication1.Bussiness.IRepository;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Repository
{
    public class OrderRepository : IOrderRepository
    {
        NorthWindContext context;

        public OrderRepository()
        {
        }

        public OrderRepository(NorthWindContext context)
        {
            this.context = context;
        }

        public void CreateOrder(Order order)
        {
            context.Orders.Add(order);
            //  context.SaveChanges();
        }

    }
}
