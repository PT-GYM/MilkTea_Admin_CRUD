using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace MilkTea_Customer.Controllers
{
    public class MenuController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IToppingService _toppingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService _categoryService;
        private readonly IComboService _comboService;
        private const int PageSize = 3;

        public MenuController(
            IOrderService orderService,
            IProductService productService,
            IToppingService toppingService,
            IOrderDetailService orderDetailService,
            ICategoryService categoryService,
            IComboService comboService)
        {
            _orderService = orderService;
            _productService = productService;
            _toppingService = toppingService;
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
            _comboService = comboService;
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
            var combos = await _comboService.GetAllCombos();
            ViewBag.Combos = combos;

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
            if (toppings == null)
            {
                toppings = new List<Topping>(); 
            }

            ViewBag.Toppings = toppings;

            return View(product);
        }

        public async Task<IActionResult> ConfirmOrder(string phoneNumber, string address, int productId, string toppingIds)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            bool success = await _orderService.ConfirmOrderAsync(phoneNumber, address, productId, toppingIds, userId.Value);

            if (success)
            {
                return RedirectToAction("OrderConfirmation");
            }

            return BadRequest("Có lỗi xảy ra khi xử lý đơn hàng, vui lòng thử lại.");
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

            var toppingDetailsDict = await _orderDetailService.GetToppingDetailsByOrderIdAsync(orderId);

            ViewBag.ToppingDetails = toppingDetailsDict;

            return View(orderDetails);
        }
        public async Task<IActionResult> ShowComboDetail(int comboId)
        {
            var combo = await _comboService.GetComboById(comboId);
            if (combo == null)
            {
                return NotFound();
            }

            return View("ComboDetail", combo);
        }
    }
}
