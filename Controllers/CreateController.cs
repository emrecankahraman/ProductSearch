using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductSearch.Models;
using Microsoft.IdentityModel.Tokens;
using ProductSearch.Controllers;
using ProductSearch.Services;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

public class CreateController : Controller
{
    private readonly IProductService _productService;
    private readonly IAccountService _accountService;

    private readonly ILogger<CreateController> _logger;
    public CreateController(IAccountService accountService, IProductService productService,ILogger<CreateController> logger)
    {
        _accountService = accountService;
        _productService = productService;
        _logger = logger;

    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string Name, string Keywords, string Description, string Department,
                                        string supervisorName, string supervisorLastName, string supervisorDepartment,
                                        string coSupervisorName, string coSupervisorLastName, string coSupervisorDepartment,
                                        int Quantity)
    {
        var currentUserId = _accountService.GetCurrentUserId(User);

        var product = new Product
        {
            Name = Name,
            Description = Description,
            Department = Department,
            Quantity = Quantity,
            CreateDate = DateTime.Now,
            CreatedById = currentUserId,
            // SupervisorId and CoSupervisorId will be set by the service method
        };

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid.");
            return View(product);
        }

        try
        {
            var createdProduct = await _productService.CreateProductAsync(Name, Keywords, Description, Department,
                                                                          supervisorName, supervisorLastName, supervisorDepartment,
                                                                          coSupervisorName, coSupervisorLastName, coSupervisorDepartment,
                                                                          currentUserId, Quantity, product);
            
            ViewBag.Message = "Kayıt başarılı.";
            return View(createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            ViewBag.Message = "Bir hata oluştu: " + ex.Message;
            return View(product);
        }
    }

}