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
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string phoneNumber, string address, int productId, string toppingIds)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            List<Topping> toppings = new List<Topping>();
            decimal toppingTotal = 0;

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
                toppingTotal = toppings.Sum(t => t.Price);
            }

            decimal totalAmount = product.Price + toppingTotal;

            // Trừ stock của sản phẩm
            if (product.Stock <= 0)
            {
                return BadRequest("Sản phẩm đã hết hàng");
            }
            product.Stock -= 1;  // Giảm 1 đơn vị sản phẩm

            // Trừ stock của topping
            foreach (var topping in toppings)
            {
                if (topping.Stock <= 0)
                {
                    return BadRequest($"Topping {topping.Name} đã hết hàng");
                }
                topping.Stock -= 1;  // Giảm 1 đơn vị topping
            }

            // Lưu lại thông tin vào DB
            await _productService.UpdateProductAsync(product); // Cập nhật stock sản phẩm
            foreach (var topping in toppings)
            {
                await _toppingService.UpdateToppingAsync(topping); // Cập nhật stock topping
            }

            var order = new Order
            {
                UserId = userId.Value,
                PhoneNumber = phoneNumber,
                Address = address,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDate = DateTime.Now
            };

            var orderDetail = new OrderDetail
            {
                ProductId = productId,
                ToppingIds = string.Join(",", toppings.Select(t => t.ToppingId)),
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

        public async Task<IActionResult> Order()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            var orders = await _orderService.GetOrdersByUserId(userId.Value);
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            var toppingDetailsDict = new Dictionary<string, string>();

            foreach (var detail in orderDetails)
            {
                if (!string.IsNullOrEmpty(detail.ToppingIds))
                {
                    var toppingIdList = detail.ToppingIds.Split(',')
                        .Select(id => int.TryParse(id, out int tid) ? tid : (int?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
                        .ToList();

                    var toppings = await _toppingService.GetToppingByIdsAsync(toppingIdList);
                    var toppingDetails = string.Join(", ", toppings.Select(t => $"{t.Name} ({t.Price:N0} VND)"));

                    toppingDetailsDict[detail.ToppingIds] = toppingDetails;
                }
            }

            ViewBag.ToppingDetails = toppingDetailsDict;

            return View(orderDetails);
        }

    }
}
