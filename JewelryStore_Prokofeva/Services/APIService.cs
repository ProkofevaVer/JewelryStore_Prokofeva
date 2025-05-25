using System.Text;
using System.Text.Json;
using Api_JewelryStore.Models.DTO_Models;
using JewelryStore_Prokofeva.Models;
using JewelryStore_Prokofeva.ModelsDb;
using Newtonsoft.Json;

namespace JewelryStore_Prokofeva.Services
{
    public class APIService
    {
        private HttpClient _httpClient;

        public APIService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7202/api/");
        }


        private async Task<List<Product>?> GetProductsFromApiAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Product>>();
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при обращении к API: {e.Message}");
                return null;
            }
        }

        private async Task<List<T>?> GetDataFromApiAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<T>>();
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при обращении к API: {e.Message}");
                return null;
            }
        }
        public async Task<List<UserDto>?> GetLoginAsync()
        {
            return await GetDataFromApiAsync<UserDto>("Login");
        }

        public async Task<HttpResponseMessage> RegisterUser(RegisterDto registerDto)
        {
            var json = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("Login/register", content);
        }
        public async Task<List<CartItem>?> GetCartItemAsync()
        {
            return await GetDataFromApiAsync<CartItem>("CartItem");
        }
        public async Task<HttpResponseMessage> AddCartItemAsync(CartItem cartItem)
        {
            var json = JsonConvert.SerializeObject(cartItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("CartItem/addCard", content);
        }

        public async Task<HttpResponseMessage> UpdateCartItemStatusAsync(int id, UpdateStatusRequest updateStatusRequest)
        {
            var json = JsonConvert.SerializeObject(updateStatusRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"CartItem/{id}/status", content);
            return response;
        }


        private async Task<List<PurchaseDto>?> GetDataFromApiPurchasesAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<PurchaseDto>>();
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка при обращении к API: {e.Message}");
                return null;
            }
        }

        public async Task<List<PurchaseDto>?> GetPurchasesAsync()
        {
            return await GetDataFromApiPurchasesAsync("Purchases");
        }


        public async Task<HttpResponseMessage> AddJewelryItemAsync(AddJewelryItemDto addJewelryItemDto)
        {
            var json = JsonConvert.SerializeObject(addJewelryItemDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("AddProduct", content);
        }

        //// Method to add a new jewelry item
        //public async Task<bool> AddJewelryItemAsync(AddJewelryItemDto newItem)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync("AddProduct", newItem);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        Console.WriteLine($"Error while calling API: {e.Message}");
        //        return false;
        //    }
        //}







        //public async Task<CartItem?> UpdateCartItemStatusAsync(int cartItemId, string newStatus)
        //{
        //    var requestBody = new { status = newStatus };

        //    var response = await _httpClient.PostAsJsonAsync($"CartItem/{cartItemId}/status", requestBody);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var updatedCartItem = await response.Content.ReadFromJsonAsync<CartItem>();
        //        return updatedCartItem;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Ошибка обновления статуса: {response.ReasonPhrase}");
        //        return null;
        //    }
        //}



        //public async Task<bool> UpdateCartItemStatusAsync(int id, string newStatus)
        //{
        //    var content = new StringContent($"\"{newStatus}\"", System.Text.Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PutAsync($"CartItem/updateStatus/{id}", content);
        //    return response.IsSuccessStatusCode;
        //}


        public async Task<List<CategoryDto>?> GetCategoriesAsync()
        {
            return await GetDataFromApiAsync<CategoryDto>("Category");
        }

        public async Task<List<MaterialDto>?> GetMaterialsAsync()
        {
            return await GetDataFromApiAsync<MaterialDto>("Material");
        }
        public async Task<List<ForWhoDto>?> GetForWhoAsync()
        {
            return await GetDataFromApiAsync<ForWhoDto>("ForWhom");
        }

        public async Task<List<InsertionDto>?> GetInsertionsAsync()
        {
            return await GetDataFromApiAsync<InsertionDto>("Insertion");
        }
        public async Task<List<Product>?> GetProductsAsync()
        {
            return await GetDataFromApiAsync<Product>("Product");
        }

        // Загрузка всех продуктов
        public async Task<List<Product>?> LoadProductsAsync()
        {
            return await GetProductsFromApiAsync("Product");
        }
        public async Task<List<Product>?> SearchProductsAsync(string title)
        {
            var products = await GetDataFromApiAsync<Product>($"Product/search?title={Uri.EscapeDataString(title)}");
            return products?.Take(5).ToList(); // Ограничиваем до 5 товаров
        }






        public async Task<List<JewelrySizeDto>?> LoadSizesForProductAsync(int productId)
        {
            try
            {
                // Отправка GET-запроса к API для получения размеров продукта
                var response = await _httpClient.GetAsync($"Product/{productId}/Sizes");

                // Проверка успешности ответа
                if (response.IsSuccessStatusCode)
                {
                    // Чтение и десериализация содержимого ответа в список JewelrySizeDto
                    return await response.Content.ReadFromJsonAsync<List<JewelrySizeDto>>();
                }
                else
                {
                    // Логирование ошибки, если ответ не успешен
                    Console.WriteLine($"Ошибка: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                // Логирование исключения при возникновении ошибки запроса
                Console.WriteLine($"Ошибка при обращении к API: {e.Message}");
                return null;
            }
        }

        // Загрузка последних 10 продуктов
        public async Task<List<Product>?> LoadLastProductsAsync()
        {
            var products = await GetProductsFromApiAsync("Product");
            return products?.OrderByDescending(p => p.Id).Take(10).ToList();
        }

        // Загрузка бестселлеров (товары со скидкой)
        public async Task<List<Product>?> LoadBestSellersAsync()
        {
            var products = await GetProductsFromApiAsync("Product");
            return products?
                .Where(p => p.Discount > 0)
                .OrderByDescending(p => p.Discount)
                .Take(10)
                .ToList();
        }



    }
}