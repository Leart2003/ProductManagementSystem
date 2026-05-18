using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProduktController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;

        public ProduktController(IProductRepository productRepository, UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            List<Product> products = await _productRepository.GetAllAsync();

            if (User.IsInRole("Admin"))
                return Ok(products);

            var userId = _userManager.GetUserId(User);
            var userProducts = products.Where(p => p.UserId == userId).ToList();

            return Ok(userProducts);
        }
    }
}
