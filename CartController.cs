using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebApplication1.ChatHubSignarl;
using WebApplication1.Bussiness.DTO;
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Helper;

namespace WebApplication1
{
    // Lớp CartController xử lý các hành động liên quan đến giỏ hàng
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<CartHub> _hubContext;
        public IProductRepository productRepository { get; set; }
        public List<ProductDTO> Products { get; set; } // Danh sách sản phẩm
        public List<CategoryDTO> Categories { get; set; } // Danh sách danh mục

        public CartController(IHttpContextAccessor httpContextAccessor, IHubContext<CartHub> hubContext, IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        private List<CartItem> _cart
        {
            get
            {
                var session = _httpContextAccessor.HttpContext.Session;
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(session, "Cart");

                if (cart == null)
                {
                    cart = new List<CartItem>();
                    SessionHelper.SetObjectAsJson(session, "Cart", cart);
                }

                return cart;
            }
        }

        // Phương thức thêm sản phẩm vào giỏ hàng
        public IActionResult AddProductToCart(int productId, int quantity)
        {
            // TODO: Lấy thông tin sản phẩm từ cơ sở dữ liệu hoặc nơi lưu trữ khác
            var product = productRepository.GetProduct(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Kiểm tra số lượng sản phẩm có đủ để mua hay không
            if (product.UnitsInStock < quantity)
            {
                return BadRequest("Số lượng sản phẩm không đủ");
            }

            // Cập nhật số lượng UnitsInStock sau khi mua
            product.UpdatedUnitsInStock = (int)(product.UnitsInStock - quantity);

            // Thêm sản phẩm đã được cập nhật vào giỏ hàng
            _cart.Add(new CartItem { Product = product, Quantity = quantity });

            // Gửi thông điệp realtime bằng SignalR
            _hubContext.Clients.All.SendAsync("UpdateCart", _cart);

            return Ok();
        }

        // Phương thức hiển thị giỏ hàng
        public IActionResult ViewCart()
        {
            var cart = _cart;
            return View(cart);
        }

        // TODO: Các phương thức khác (xóa sản phẩm, cập nhật số lượng, thanh toán, v.v.)
    }
}
