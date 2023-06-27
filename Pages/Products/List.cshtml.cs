using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Helper;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DataAccess.Models;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.ChatHubSignarl;
using Unipluss.Sign.ExternalContract.Entities;

namespace WebApplication1.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IHubContext<CartHub> _hubContext;
        public IProductRepository productRepository { get; set; }
        public List<ProductDTO> Products { get; set; } // Danh sách sản phẩm
        public List<CategoryDTO> Categories { get; set; } // Danh sách danh mục

        public ListModel(IProductRepository productRepository, IHubContext<CartHub> hubContext)
        {
            this.productRepository = productRepository;
            _hubContext = hubContext;
        }

        public void OnGet(int? categoryId)
        {

            // Lấy danh sách danh mục từ repository
            Categories = productRepository.GetCategories();

            // Lấy sản phẩm theo danh mục (nếu categoryId được chọn)
            if (categoryId != null)
            {
                Products = productRepository.GetProductsByCategory(categoryId.Value);
            }
            else
            {
                Products = productRepository.GetProducts();
            }


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
            // Gửi thông điệp realtime đến tất cả các kết nối khách hàng
            _hubContext.Clients.All.SendAsync("ReceiveCartQuantity", cart.Count);
        }

        private void GetData()
        {

            // Lấy danh sách danh mục từ repository
            Categories = productRepository.GetCategories();

            // Lấy tất cả sản phẩm
            Products = productRepository.GetProducts();

        }




    }
}
