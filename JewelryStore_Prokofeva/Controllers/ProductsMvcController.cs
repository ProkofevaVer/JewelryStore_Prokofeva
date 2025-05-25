using Api_JewelryStore.Models.DTO_Models;
using JewelryStore_Prokofeva.Models;
using JewelryStore_Prokofeva.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JewelryStore_Prokofeva.Controllers
{
    public class ProductsMvcController : Controller
    {
        private readonly APIService _apiService;

        public ProductsMvcController()
        {
            _apiService = new APIService();
        }

        public async Task<IActionResult> NewArrivals()
        {
            var productsLast = await _apiService.LoadLastProductsAsync(); // Используем новый метод для получения последних 10 продуктов
            return View(productsLast);
        }

        public async Task<IActionResult> Details(int id)
        {
            var products = await _apiService.LoadProductsAsync(); // Загрузите все продукты
            var product = products?.FirstOrDefault(p => p.Id == id); // Найдите нужный продукт по ID
            if (product == null)
            {
                return NotFound(); // Если продукт не найден, возвращаем 404
            }
            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Sizes = product.JewelrySizes ?? new List<JewelrySizeDto>() 
            };
            return View(viewModel);
        }

        //[HttpGet("Search")]
        //public async Task<IActionResult> Search(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
        //    {
        //        return BadRequest("Запрос должен содержать как минимум 3 символа.");
        //    }

        //    var products = await _apiService.SearchProductsAsync(query);
        //    return Ok(products);
        //}
    }
}
