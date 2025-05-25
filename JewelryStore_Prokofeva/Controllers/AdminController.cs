using JewelryStore_Prokofeva.Models;
using JewelryStore_Prokofeva.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JewelryStore_Prokofeva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly APIService _apiService;

        public AdminController()
        {
            _apiService = new APIService();
        }

        [HttpGet("adminpage/{id}")]
        public async Task<IActionResult> AdminPage(int id)
        {
            ViewData["AdminId"] = id;

            // Fetch products from the API
            var products = await _apiService.GetProductsAsync();          
            return View(products);
        }

        [HttpGet("addjewelryitem/{id}")]
        public async Task<IActionResult> AddJewelryItem(int id)
        {
            ViewData["AdminId"] = id;

            // Получаем данные для выпадающих списков
            var categories = await _apiService.GetCategoriesAsync();
            var materials = await _apiService.GetMaterialsAsync();
            var forWho = await _apiService.GetForWhoAsync();

            ViewBag.Categories = categories;
            ViewBag.Materials = materials;
            ViewBag.ForWho = forWho;

            return View(); // Убедитесь, что это представление существует
        }


        //[HttpPost("addjewelryitem")]
        //public async Task<IActionResult> AddJewelryItem([FromBody] AddJewelryItemDto newItem, int adminId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Вызываем метод API для добавления нового ювелирного предмета
        //        var result = await _apiService.AddJewelryItemAsync(newItem);
        //        if (result)
        //        {
        //            // Успешное добавление, можно перенаправить на страницу с товарами или другую страницу
        //            return RedirectToAction("AdminPage", new { id = adminId });
        //        }
        //        else
        //        {
        //            // Обработка ошибок, если добавление не удалось
        //            ModelState.AddModelError("", "Не удалось добавить товар. Попробуйте еще раз.");
        //        }
        //    }

        //    // Если модель не валидна или добавление не удалось, возвращаемся к представлению
        //    ViewData["AdminId"] = adminId;
        //    var categories = await _apiService.GetCategoriesAsync();
        //    var materials = await _apiService.GetMaterialsAsync();
        //    var forWho = await _apiService.GetForWhoAsync();

        //    ViewBag.Categories = categories;
        //    ViewBag.Materials = materials;
        //    ViewBag.ForWho = forWho;

        //    return View(newItem);
        //}




        [HttpPost("addjewelryitem_")]
        public async Task<IActionResult> AddToJewelryItem(AddJewelryItemDto newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Ошибка валидации данных.", errors = ModelState }); // Возвращаем ошибки валидации
            }

            // Log the received item for debugging
            Console.WriteLine($"Received item: {JsonConvert.SerializeObject(newItem)}");

            var response = await _apiService.AddJewelryItemAsync(newItem);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Item added to cart successfully." });
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = errorMessage });
        }


        //[HttpPost("addjewelryitem")]
        //public async Task<IActionResult> AddJewelryItem(AddJewelryItemDto newItem, int adminId)
        //{
        //    var success = await _apiService.AddJewelryItemAsync(newItem);
        //    if (success)
        //    {
        //        return RedirectToAction("AdminPage", new { id = adminId }); // Используйте переданный ID администратора
        //    }
        //    else
        //    {
        //        ViewData["ErrorMessage"] = "Не удалось добавить ювелирный предмет.";
        //        return View(newItem);
        //    }
        //}




        [HttpGet("reportsale/{id}")]
        public async Task<IActionResult> ReportSale(int id)
        {
            ViewData["AdminId"] = id;
            var purchases = await _apiService.GetPurchasesAsync();
            return View(purchases);
        }
    }
}
