using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Helper;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Pages.Products
{
    public class ListModel : PageModel
    {
        public List<ProductDTO> products { get; set; }
        public IProductRepository productRepository { get; set; }
        public int CurrentPage { get; set; }
        public int ProductsPerPage { get; set; } = 15;
        public int TotalProducts { get; set; }

        public ListModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public void OnGet(int page = 1)
        {
            CurrentPage = page;
            GetData();
        }

        public IActionResult OnPostAddToCart(int? productId)
        {
            if (productId != null)
            {
                AddProductToCart(productRepository.GetProduct(productId.Value));
                ViewData["mess"] = "Add To Cart Successful!";
            }
            GetData();
            return RedirectToPage("/Cart/List");
        }

        private void AddProductToCart(ProductDTO p)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
                cart.Add(new CartItem()
                {
                    Product = p,
                    Quantity = 1
                });
            }
            else
            {
                CartItem c = cart.FirstOrDefault(c => c.Product.ProductId == p.ProductId);
                if (c == null)
                {
                    cart.Add(new CartItem()
                    {
                        Product = p,
                        Quantity = 1
                    });
                }
                else
                {
                    c.Quantity += 1;
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        }

        private void GetData()
        {
            products = productRepository.GetProducts();
            TotalProducts = products.Count;
        }

        public List<ProductDTO> GetProductsForPage(int currentPage, int productsPerPage)
        {
            int skip = (currentPage - 1) * productsPerPage;
            return products.Skip(skip).Take(productsPerPage).ToList();
        }
    }
}
