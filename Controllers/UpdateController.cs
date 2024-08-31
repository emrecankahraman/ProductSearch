using ProductSearch.Models;
using ProductSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ProductSearch.Controllers
{
    public class UpdateController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly ILogger<UpdateController> _logger;

        public UpdateController(IProductService productService,IAccountService accountService, ILogger<UpdateController> logger)
        {
            _productService = productService;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task<IActionResult> Update()
        {
            var products = await _productService.GetAllProductsAsync();
            if (!products.Any())
            {
                _logger.LogWarning("No products found in the database.");
                return View(new List<Product>());
            }
            return View(products);
        }

        [HttpPost]
        public async Task<ActionResult> SearchByMultipleCriteria(string productName, string productKeyword, string description, string supervisorName, string supervisorLastName,
            string Department, string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment)
        {
            //var initialResults = _productService.GetAllProductsAsync().AsQueryable();
            //var filteredResults = _productService.GetFilteredProductsAsync(initialResults, productName, productKeyword, description, supervisorName, supervisorLastName, Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
            //var filteredProducts = _productService.GetFilteredProductsAsync(productName, productKeyword, description, supervisorName, supervisorLastName, Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);

            //ViewBag.Search = "Search by Multiple Criteria";
            //return View("Update", filteredProducts);

            var filteredProducts = await _productService.GetFilteredProductsAsync(productName, productKeyword, description, supervisorName, supervisorLastName, Department, coSupervisorName, coSupervisorLastName, coSupervisorDepartment);
            var productList = filteredProducts.ToList();
            ViewBag.Search = "Search by Multiple Criteria";
            return View("Update", productList);
        }


        [HttpGet]
        public async Task<IActionResult> GetKeywords(int id)
        {
            var keywords = await _productService.GetKeywordsAsync(id);
            return Json(keywords);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, string selectedValue, string newValue)
        {
            int currentUserId = _accountService.GetCurrentUserId(User); 
            var (success, updatedKeywords) = await _productService.UpdateProductAsync(id, selectedValue, newValue, currentUserId);

            if (success)
            {
                return Json(new { success = true, updatedKeywords });
            }
            else
            {
                return Json(new { success = false, message = "Update failed" });
            }
        }
    }
}
