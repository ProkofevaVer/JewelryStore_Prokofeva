using JewelryStore_Prokofeva.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JewelryStore_Prokofeva.Services;

namespace JewelryStore_Prokofeva.Controllers
{
    public class PriceRange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public PriceRange(int id, string name, decimal? minPrice, decimal? maxPrice)
        {
            Id = id;
            Name = name;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }




    [Route("api/[controller]")]
    [ApiController]
    [Route("catalog")]
    public class CategoryProductController : Controller
    {
        private readonly APIService _apiService;

        public CategoryProductController()
        {
            _apiService = new APIService();
        }
        // Метод для получения ценовых диапазонов
        private List<PriceRange> GetPriceRanges()
        {
            return new List<PriceRange>
            {
                new PriceRange(1, "От 1000", 1000, null),
                new PriceRange(2, "От 2000", 2000, null),
                new PriceRange(3, "От 2000 до 5000", 2000, 5000),
                new PriceRange(4, "От 5000 до 15000", 5000, 15000),
                new PriceRange(5, "От 15000 и дороже", 15000, null)
            };
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(
         
            [FromQuery] int[]? selectedCategoryIds,
            [FromQuery] int[]? selectedMaterialIds,
            [FromQuery] int[]? selectedForWhomIds,
            [FromQuery] int[]? selectedInsertionIds,
            [FromQuery] int[]? selectedPriceRanges,
            [FromQuery] decimal? customMinPrice, // New parameter for custom min price
            [FromQuery] decimal? customMaxPrice, // New parameter for custom max price
            [FromQuery] int page = 1, // Номер текущей страницы
            [FromQuery] int pageSize = 2) // Количество товаров на странице)
        {
            var categories = await _apiService.GetCategoriesAsync();
            var materials = await _apiService.GetMaterialsAsync();
            var products = await _apiService.GetProductsAsync();
            ViewBag.Products = products ?? new List<Product>(); // Инициализируем пустым списком, если null

            var forWhom = await _apiService.GetForWhoAsync();
            var insertions = await _apiService.GetInsertionsAsync();
            var priceRanges = GetPriceRanges();

            // Calculate min and max prices from products
            var minPrice = products?.Min(p => p.Price) ?? 0;
            var maxPrice = products?.Max(p => p.Price) ?? 0;
            // Set default values for ViewBag
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;


            if (selectedCategoryIds != null && selectedCategoryIds.Length > 0)
            {
                products = products?.Where(p => selectedCategoryIds.Contains(p.Category.Id)).ToList();
            }
            if (selectedMaterialIds != null && selectedMaterialIds.Length > 0)
            {
                products = products?.Where(p => selectedMaterialIds.Contains(p.Material.Id)).ToList();
            }
            if (selectedForWhomIds != null && selectedForWhomIds.Length > 0)
            {
                products = products?.Where(p => selectedForWhomIds.Contains(p.ForWho.Id)).ToList();
            }
            // Обработка выбранных вставок
            if (selectedInsertionIds != null && selectedInsertionIds.Length > 0)
            {
                products = products?.Where(p => p.InsertionsDetails.Any(id => selectedInsertionIds.Contains(id.Insertion.Id))).ToList();
            }
            if (customMinPrice.HasValue || customMaxPrice.HasValue)
            {
                products = products?.Where(p =>
                    (!customMinPrice.HasValue || p.PriceDiscounr >= customMinPrice) &&
                    (!customMaxPrice.HasValue || p.PriceDiscounr <= customMaxPrice)).ToList();
            }

            // Фильтрация по ценовым диапазонам
            if (selectedPriceRanges != null && selectedPriceRanges.Length > 0)
            {
                products = products?.Where(p =>
                    selectedPriceRanges.Any(rangeId =>
                    {
                        var range = priceRanges.FirstOrDefault(r => r.Id == rangeId);
                        return range != null &&
                               (range.MinPrice == null || p.PriceDiscounr >= range.MinPrice) &&
                               (range.MaxPrice == null || p.PriceDiscounr <= range.MaxPrice);
                    })).ToList();
            }

            // Получаем общее количество товаров
            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Получаем товары для текущей страницы
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Сохранение выбранных идентификаторов в ViewBag
            ViewBag.SelectedCategoryIds = selectedCategoryIds?.ToList() ?? new List<int>();
            ViewBag.SelectedMaterialIds = selectedMaterialIds?.ToList() ?? new List<int>();
            ViewBag.SelectedForWhomIds = selectedForWhomIds?.ToList() ?? new List<int>();
            ViewBag.SelectedInsertionIds = selectedInsertionIds?.ToList() ?? new List<int>();
            ViewBag.SelectedPriceRanges = selectedPriceRanges?.ToList() ?? new List<int>();
            ViewBag.CustomMinPrice = customMinPrice;
            ViewBag.CustomMaxPrice = customMaxPrice;

            // Передаем данные в представление
            ViewBag.Products = pagedProducts;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.Categories = categories;
            ViewBag.Materials = materials;
            ViewBag.ForWhom = forWhom;
            ViewBag.Insertions = insertions;
            ViewBag.PriceRanges = priceRanges;

            return View();
        }






        [HttpGet]
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


