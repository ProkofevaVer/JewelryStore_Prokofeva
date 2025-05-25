using JewelryStore_Prokofeva.Models;
using JewelryStore_Prokofeva.ModelsDb;
using JewelryStore_Prokofeva.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace JewelryStore_Prokofeva.Controllers
{
    public class AccountController : Controller
    {
        private readonly APIService _apiService;
        private readonly PasswordHasher<RegisterDto> _passwordHasher;

        public AccountController()
        {
            _apiService = new APIService();
            _passwordHasher = new PasswordHasher<RegisterDto>();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var users = await _apiService.GetLoginAsync();
            var user = users?.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Удалено хеширование пароля
                if (user.Password == password) // Сравнение паролей в открытом виде
                {
                    if (user.IsAdmin)
                    {
                        return RedirectToAction("AdminPage", "Admin", new { id = user.Id });
                    }
                    else
                    {
                        return RedirectToAction("ClientHomePage", "Client", new { id = user.Id });
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Ошибка валидации данных.", errors = ModelState }); // Возвращаем ошибки валидации
            }

            // Удалено хеширование пароля
            // registerDto.Password = _passwordHasher.HashPassword(registerDto, registerDto.Password);

            // Вызов API для регистрации пользователя
            var response = await _apiService.RegisterUser(registerDto);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Регистрация прошла успешно! Вы можете войти в систему." });
            }
            // Обработка ошибки от API
            var errorMessage = await response.Content.ReadAsStringAsync();
            return BadRequest(new { message = errorMessage }); // Возвращаем сообщение об ошибке
        }
    }
}


