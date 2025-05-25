using System.Diagnostics;
using JewelryStore_Prokofeva.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using JewelryStore_Prokofeva.Services;

namespace JewelryStore_Prokofeva.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly APIService _apiService;

        public HomeController()
        {
            _apiService = new APIService();
        }

        public async Task<IActionResult> Index()
        {
            var products = await _apiService.LoadProductsAsync(); // �������� ������ 
            var productsLast = await _apiService.LoadLastProductsAsync(); // �������� ��������� 10 �������
            var bestSellers = await _apiService.LoadBestSellersAsync(); // �������� ������ ������������


            var viewModel = new ProductsViewModel
            {
                ListProduct = products,
                BestSellers = bestSellers,
                NewArrivals = productsLast
            };

            return View(viewModel);
        }


        public IActionResult Details(int id)
        {
            // ������ ��� ����������� ������� ��������
            return View();
        }
        public async Task<IActionResult> Search(string title)
        {
            var products = await _apiService.SearchProductsAsync(title);
            return PartialView("_ProductNamesList", products); // �������� ���� ������ ���������
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
