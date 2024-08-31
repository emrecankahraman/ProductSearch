using ProductSearch.Models;
using ProductSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductSearch.Controllers
{
    public class DeleteController : Controller
    {
        private readonly IProductService _productService;

        private readonly ILogger<DeleteController> _logger;
        public DeleteController(IProductService productService, ILogger<DeleteController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Delete()
        {
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> SearchByMultipleCriteria(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName,
           string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {

            var filteredProducts = await _productService.GetFilteredProductsAsync(productName, productKeyword, description, supervisorName, supervisorLastName, Department,
                coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
            var productList = filteredProducts.ToList();
            ViewBag.Search = "Search by Multiple Criteria";
            return View("Delete", productList);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
