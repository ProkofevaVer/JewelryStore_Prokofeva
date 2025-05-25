using JewelryStore_Prokofeva.Models;
using System.Threading.Tasks;
using JewelryStore_Prokofeva.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api_JewelryStore.Models.DTO_Models;
using JewelryStore_Prokofeva.ModelsDb;
using Azure.Core;

namespace JewelryStore_Prokofeva.Controllers
{

    public class PriceRange1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public PriceRange1(int id, string name, decimal? minPrice, decimal? maxPrice)
        {
            Id = id;
            Name = name;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }

    //public class CartViewModel
    //{
    //    public IEnumerable<dynamic> CartItems { get; set; }
    //    public decimal TotalOriginalPrice { get; set; }
    //    public decimal TotalDiscountPrice { get; set; }
    //}


    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly APIService _apiService;

        public ClientController()
        {
            _apiService = new APIService();
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateCartItemStatus(int id, [FromBody] UpdateStatusRequest updateStatusRequest)
        {
            var response = await _apiService.UpdateCartItemStatusAsync(id, updateStatusRequest);

            if (response.IsSuccessStatusCode)
            {
                return Ok(); // Возвращаем успешный ответ
            }

            return BadRequest("Ошибка при обновлении статуса"); // Возвращаем ошибку
        }



        //[HttpPost]
        //public async Task<IActionResult> UpdateCartItemStatus(int id, string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        return BadRequest("Статус обязателен");
        //    }

        //    var statusUpdate = new UpdateStatusRequest { Status = status };
        //    var response = await _apiService.UpdateCartItemStatusAsync(id, statusUpdate);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return Ok(new { message = "Статус успешно обновлен" });
        //    }
        //    else
        //    {
        //        return NotFound($"CartItem с ID {id} не найден или обновление не удалось.");
        //    }
        //}


        public async Task<IActionResult> ClientBasket(int id)
        {
            ViewData["User Id"] = id; // Сохраняем User ID для использования в представлении
                                      // Получаем все элементы корзины для данного пользователя
            var cartItems = await _apiService.GetCartItemAsync();
            var userCartItems = cartItems.Where(ci => ci.UserId == id).ToList();

            // Получаем все продукты
            var products = await _apiService.GetProductsAsync();

            // Создаем вью-модель для корзины
            var viewModel = userCartItems.Select(ci =>
            {
                var jewelrySize = products.SelectMany(p => p.JewelrySizes)
                                          .FirstOrDefault(js => js.Id == ci.JewelrySizesItemId);
                var product = products.FirstOrDefault(p => p.Id == jewelrySize?.JewelryItemId);

                return new
                {
                    Id = ci.Id,
                    ProductTitle = product?.Title,
                    PhotoUrl = product?.PhotoUrl,
                    Size = jewelrySize?.Size,
                    Quantity = ci.CardQuantity,
                    OriginalPrice = product?.Price, // Старая цена
                    DiscountPrice = product?.PriceDiscounr, // Цена со скидкой
                    Discount = product?.Discount > 0 ? product.Discount : null, // Скидка
                    Status = ci.Status
                };
            }).ToList();

            // Расчет общей суммы
            var totalOriginalPrice = viewModel.Sum(item => item.OriginalPrice ?? 0); // Сумма всех оригинальных цен
            var totalDiscountPrice = viewModel.Sum(item => item.DiscountPrice ?? 0); // Сумма всех цен со скидкой
            var totalDiscount = totalOriginalPrice - totalDiscountPrice;
            // Создаем объект для передачи в представление
            var finalViewModel = new CartViewModel
            {
                CartItems = viewModel,
                TotalOriginalPrice = totalOriginalPrice,
                TotalDiscountPrice = totalDiscountPrice,
                TotalDiscount = totalDiscount
            };

            return View(finalViewModel);
        }


        //public async Task<IActionResult> Search(string title,int userId)
        //{
        //    //var userId = HttpContext.Session.GetString("User Id"); // пример получения ID пользователя из сессии
        //    ViewData["User Id"] = userId;
        //    var products = await _apiService.SearchProductsAsync(title);

        //    return PartialView("Home/_ProductNamesList", products); // Передаем весь список продуктов
        //}


        // Метод для получения ценовых диапазонов
        private List<PriceRange1> GetPriceRanges()
        {
            return new List<PriceRange1>
            {
                new PriceRange1(1, "От 1000", 1000, null),
                new PriceRange1(2, "От 2000", 2000, null),
                new PriceRange1(3, "От 2000 до 5000", 2000, 5000),
                new PriceRange1(4, "От 5000 до 15000", 5000, 15000),
                new PriceRange1(5, "От 15000 и дороже", 15000, null)
            };
        }

        [HttpGet("catalog/{id}")]
        public async Task<IActionResult> ClientCategoryProduct(int id,
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
            ViewData["User Id"] = id; ;
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

        [HttpGet("home/{id}")]
        public async Task<IActionResult> ClientHomePage(int id)
        {
            var products = await _apiService.LoadProductsAsync(); // Load products
            var productsLast = await _apiService.LoadLastProductsAsync(); // Load last products
            var bestSellers = await _apiService.LoadBestSellersAsync(); // Load best sellers

            var viewModel = new ProductsViewModel
            {
                ListProduct = products,
                BestSellers = bestSellers,
                NewArrivals = productsLast
            };

            ViewData["User Id"] = id; // Optionally keep this if you need it
            return View(viewModel);
        }


        [HttpGet("product/{id}/{userId}")]
        public async Task<IActionResult> ClientproductDetelisPage(int id, int userId)
        {
            var products = await _apiService.LoadProductsAsync(); // Load all products
            var product = products?.FirstOrDefault(p => p.Id == id); // Find the product by ID

            if (product == null)
            {
                return NotFound(); // Return 404 if product not found
            }

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Sizes = product.JewelrySizes ?? new List<JewelrySizeDto>()
            };

            ViewData["User Id"] = userId; // Pass the user ID to the view
            return View(viewModel);
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Invalid cart item.");
            }

            var response = await _apiService.AddCartItemAsync(cartItem);
            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Item added to cart successfully." });
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = errorMessage });
        }

        //[HttpPut("update-cart-item-status/{id}")]
        //public async Task<IActionResult> UpdateCartItemStatus(int id, [FromBody] string status)
        //{
        //    var response = await _apiService.UpdateCartItemStatusAsync(id, status);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return Ok(new { message = "Item status updated successfully." });
        //    }

        //    var errorMessage = await response.Content.ReadAsStringAsync();
        //    return BadRequest(new { message = errorMessage });
        //}

    }
}
