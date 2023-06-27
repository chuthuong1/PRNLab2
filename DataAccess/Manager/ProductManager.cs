﻿using WebApplication1.DataAccess.Models;
namespace WebApplication1.DataAccess.Manager
{
    public class ProductManager
    {
        NorthWindContext context;
        public ProductManager(NorthWindContext context)
        {
            this.context = context;
        }
        public List<Product> GetProducts()
        {
            return context.Products.ToList();
        }
        public Product GetProduct(int id)
        {
            return context.Products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}
