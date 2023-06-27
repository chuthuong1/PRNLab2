using WebApplication1.Bussiness.DTO;

namespace WebApplication1.Bussiness.IRepository
{
    public interface IProductRepository
    {
        public List<ProductDTO> GetProducts();
        public ProductDTO GetProduct(int id);
        public ProductDTO GetProductById(int id);
        void UpdateProduct(ProductDTO product);
    }
}
