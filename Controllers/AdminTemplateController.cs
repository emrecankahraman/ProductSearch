using ProductSearch.Models;
using ProductSearch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductSearch.Controllers
{
    [Authorize(Policy = "AdminRole")]

    public class AdminTemplateController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminTemplateController> _logger;

        public AdminTemplateController(IAdminService adminService, ILogger<AdminTemplateController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {

            var products = await _adminService.GetDashboardDataAsync();

            if (products == null)
            {
                products = new List<Product>();
            }

            

            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAdmin(Admin admin)
        {
            admin.Role = "Admin"; 
            if (ModelState.IsValid)
            {
                admin.Role = "Admin"; 

                _adminService.AddAdmin(admin);
                return RedirectToAction("Add");
            }
            return View("Add", admin);
        }
        [HttpPost]
        public IActionResult AddUser(Admin user)
        {
            user.Role = "User"; 
            if (ModelState.IsValid)
            {
                _adminService.AddUser(user);
                return RedirectToAction("Add");
            }
            return View("Add", user);
        }
        public async Task<IActionResult> Tables()
        {
            var persons = await _adminService.GetAllPersonsAsync();
            if (persons == null)
            {
                _logger.LogWarning("GetAllPersonsAsync returned null");
                persons = new List<Person>();
            }
            else
            {
                _logger.LogInformation($"Retrieved {persons.Count} persons");
            }
            return View(persons);
        }

        public IActionResult Forms()
        {
            return View();
        }
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
