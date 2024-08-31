using ProductSearch.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace ProductSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductService productService, ILogger<HomeController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchByMultipleCriteria(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName,
     string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {
            //Console.WriteLine($"Search criteria: ProductName={productName}, ProductKeyword={productKeyword}, Description={description}, SupervisorName={supervisorName}, SupervisorLastName={supervisorLastName}, Department={Department}, CoSupervisorName={coSupervisorName}, CoSupervisorLastName={coSupervisorLastName}, CoSupervisorDepartment={coSupervisorDepartment}");

            var filteredProducts = await _productService.GetFilteredProductsAsync(productName, productKeyword, description, supervisorName, supervisorLastName,
                Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
            var productList = filteredProducts.ToList();

            Console.WriteLine($"Filtered products count: {productList.Count}");

            ViewBag.Search = "Search by Multiple Criteria";
            return View("Index", productList);
        }

    }
}