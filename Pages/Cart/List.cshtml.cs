using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bussiness.DTO;
using WebApplication1.DataAccess.Models;
using WebApplication1.Helper;
using WebApplication1.Bussiness.IRepository;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
using WebApplication1.Hubs;
using WebApplication1.Bussiness.Repository;

namespace WebApplication1.Pages.Cart
{
    public class ListModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<QuantityHub> _hubContext;
        public IOrderRepository _orderRepository;

        public List<CartItem> CartItems { get; set; }

        public ListModel(IProductRepository productRepository, IHubContext<QuantityHub> hubContext)
        {
            _productRepository = productRepository;
            _hubContext = hubContext;
        }

        public void OnGet()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        }

        public void OnPostUpdate(int? productId, int? quantity)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (productId != null)
            {
                var c = CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
                c.Quantity = quantity.Value;
                ViewData["mess"] = "Update Successful!";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", CartItems);
        }

        public void OnPostDelete(int? productId)
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (productId != null)
            {
                var c = CartItems.FirstOrDefault(c => c.Product.ProductId == productId);
                CartItems.Remove(c);
                ViewData["mess"] = "Delete Successful!";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", CartItems);
        }

        public async Task<IActionResult> OnPostCheckout()
        {
            CartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (CartItems == null || CartItems.Count == 0)
            {
                ViewData["mess"] = "Giỏ hàng đang trống!";
                return Page();
            }
            else
            {
                foreach (var cartItem in CartItems)
                {
                    var product = _productRepository.GetProductById(cartItem.Product.ProductId);
                    if (product != null)
                    {
                        product.UnitsInStock -= cartItem.Quantity;
                        product.UnitsOnOrder -= cartItem.Quantity;
                        _productRepository.UpdateProduct(product);
                    }
                }
                // Tạo đối tượng Order mới


                HttpContext.Session.Remove("cart");
                ViewData["mess"] = "Checkout Successful!";

                // Lấy danh sách các ProductId và Quantity từ giỏ hàng
                var productIds = CartItems.Select(c => c.Product.ProductId).ToList();
                var quantities = CartItems.Select(c => c.Quantity).ToList();

                // Gửi thông điệp SignalR để cập nhật số lượng UnitsInStock
                await _hubContext.Clients.All.SendAsync("UpdateUnitsInStock", productIds, quantities);

                return RedirectToPage("/Products/List");

            }
        }
    }
}