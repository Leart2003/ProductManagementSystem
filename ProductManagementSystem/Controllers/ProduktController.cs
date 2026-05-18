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

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {

            var userId = _userManager.GetUserId(User);

            product.Id = Guid.NewGuid();
            product.UserId = userId;

            await _productRepository.AddAsync(product);

            return Ok("Product added succesfully");

        }

        [HttpPut("{id}")]


        public async Task<IActionResult> UpdateProduct(Guid id, Product updatedProduct)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (product.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Category = updatedProduct.Category;

            await _productRepository.UpdateAsync(product);
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);

            if (product.UserId != userId && !User.IsInRole("Admin"))


            {
                return Forbid();
            }

            return Ok("Product was deleted succesfully");

        }
    }
}
