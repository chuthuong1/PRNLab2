using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unipluss.Sign.ExternalContract.Entities;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Mapping;

using WebApplication1.DataAccess.Manager;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Repository
{
    public class ProductRepository : IProductRepository
    {
        NorthWindContext context;
        Mapper mapper;
        public ProductRepository(NorthWindContext context)
        {
            this.context = context;
            this.mapper = MapperConfig.InitializeAutomapper();
        }

        public ProductDTO GetProduct(int id)
        {
            ProductManager m = new ProductManager(context);
            ProductDTO product = mapper.Map<ProductDTO>(m.GetProduct(id));
            return product;
        }

        public ProductDTO GetProductById(int id)
        {
            // Kết nối với cơ sở dữ liệu và truy vấn sản phẩm dựa trên ID
            ProductManager m = new ProductManager(context);
            // Sau đó, chuyển đổi dữ liệu sản phẩm từ định dạng cơ sở dữ liệu thành đối tượng ProductDTO
            // Trả về đối tượng ProductDTO chứa thông tin sản phẩm tương ứng với ID đã cho

            var productEntity = context.Products.FirstOrDefault(p => p.ProductId == id);
            if (productEntity != null)
            {
                var productDTO = ConvertToProductDTO(productEntity);
                return productDTO;
            }

            return null;
        }
        // Phương thức chuyển đổi từ entity sang DTO
        private ProductDTO ConvertToProductDTO(Product productEntity)
        {
            var productDTO = new ProductDTO
            {
                ProductId = productEntity.ProductId,
                ProductName = productEntity.ProductName,
                SupplierId = productEntity.SupplierId,
                CategoryId = productEntity.CategoryId,
                QuantityPerUnit = productEntity.QuantityPerUnit,
                UnitPrice = productEntity.UnitPrice,
                UnitsInStock = productEntity.UnitsInStock,
                UnitsOnOrder = productEntity.UnitsOnOrder,
                ReorderLevel = productEntity.ReorderLevel,
                Discontinued = productEntity.Discontinued,

            };

            // Chuyển đổi thông tin về danh mục sản phẩm
            if (productEntity.Category != null)
            {
                productDTO.Category = new CategoryDTO
                {
                    CategoryId = productEntity.Category.CategoryId,
                    CategoryName = productEntity.Category.CategoryName,
                    Description = productEntity.Category.Description,
                    Picture = productEntity.Category.Picture
                };
            }

            return productDTO;
        }

        public List<ProductDTO> GetProducts()
        {
            ProductManager m = new ProductManager(context);
            List<ProductDTO> products = mapper.Map<List<ProductDTO>>(m.GetProducts());
            return products;
        }



        public void UpdateProduct(ProductDTO product)
        {
            var productEntity = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (productEntity != null)
            {
                // Cập nhật thông tin số lượng sản phẩm
                productEntity.UnitsInStock = (short?)product.UnitsInStock;

                // Lưu các thay đổi vào cơ sở dữ liệu
                context.SaveChanges();
            }
        }
        public List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            // Lấy danh sách sản phẩm theo danh mục (category) từ cơ sở dữ liệu
            var products = context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToList();

            // Chuyển đổi danh sách sản phẩm thành DTO và trả về
            return mapper.Map<List<ProductDTO>>(products);
        }

        public List<CategoryDTO> GetCategories()
        {
            var categories = context.Categories.ToList();
            var categoryDTOs = mapper.Map<List<CategoryDTO>>(categories);
            return categoryDTOs;
        }

        public List<ProductDTO> GetProductsByCategory(int? categoryId)
        {
            try
            {

                    // Trường hợp categoryId không phải null, load sản phẩm theo categoryId
                    var products = context.Products
                        .Include(p => p.Category)
                        .Where(p => p.CategoryId == categoryId)
                        .ToList();

                    return mapper.Map<List<ProductDTO>>(products);
                
            }
            catch (Exception ex)
            {
                throw new FormatException();
            }
        }

    }
}

