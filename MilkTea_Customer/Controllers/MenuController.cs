
using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTea_Customer.Controllers
{
    public class MenuController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IToppingService _toppingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService _categoryService;
        private const int PageSize = 3;

        public MenuController(
            IOrderService orderService,
            IProductService productService,
            IToppingService toppingService,
            IOrderDetailService orderDetailService,
            ICategoryService categoryService)
        {
            _orderService = orderService;
            _productService = productService;
            _toppingService = toppingService;
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Show(string searchKeyword, int? categoryId, int pageNumber = 1)
        {
            var allProducts = (await _productService.GetAllProductsAsync()).ToList();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                allProducts = allProducts.Where(p => p.Name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue)
            {
                allProducts = allProducts.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            int totalRecords = allProducts.Count();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            var products = allProducts.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.SelectedCategory = categoryId;

            var categories = await _categoryService.GetCategories();
            ViewBag.Categories = categories;

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchKeyword)
        {
            return RedirectToAction("Show", new { searchKeyword });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var toppings = await _toppingService.GetAllToppingsAsync();

            ViewBag.Toppings = toppings;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string phoneNumber, string address, int productId, string toppingIds)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy UserId từ session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(); // Hoặc xử lý khi UserId không tồn tại trong session
            }

            List<Topping> toppings = new List<Topping>();
            decimal toppingTotal = 0;

            // Nếu có toppingIds, phân tách và lấy thông tin các topping
            if (!string.IsNullOrEmpty(toppingIds))
            {
                var toppingIdList = toppingIds.Split(',')
                                              .Select(id =>
                                              {
                                                  return int.TryParse(id, out int result) ? result : (int?)null;
                                              })
                                              .Where(id => id.HasValue)
                                              .Select(id => id.Value)
                                              .ToList();

                toppings = await _toppingService.GetToppingByIdsAsync(toppingIdList);
                toppingTotal = toppings.Sum(t => t.Price); // Tính tổng giá trị topping
            }

            decimal totalAmount = product.Price + toppingTotal; // Tổng tiền cho sản phẩm + topping

            // Tạo đơn hàng
            var order = new Order
            {
                UserId = userId.Value, // Gán UserId từ session
                PhoneNumber = phoneNumber,
                Address = address,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDate = DateTime.Now
            };

            // Tạo OrderDetail cho sản phẩm và tất cả topping
            var orderDetail = new OrderDetail
            {
                ProductId = productId,
                ToppingIds = string.Join(",", toppings.Select(t => t.ToppingId)), // Lưu tất cả topping dưới dạng chuỗi
                Quantity = 1,
                SubTotal = product.Price + toppingTotal
            };

            await _orderService.CreateOrder(order, new List<OrderDetail> { orderDetail });

            return RedirectToAction("OrderConfirmation");
        }


        public async Task<IActionResult> OrderConfirmation()
        {
            return View();
        }
    }
}
