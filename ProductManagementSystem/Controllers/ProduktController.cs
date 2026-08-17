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
        /// <summary>
        /// Get all products 
        /// </summary>
        /// <returns>If user is admin returns all products</returns>
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
        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="product">Create a product for the given fields</param>
        /// <returns>Returns a positive message if product is created</returns>
        /// <response code="200">Product added succesfully"</response>

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {

            var userId = _userManager.GetUserId(User);

            product.Id = Guid.NewGuid();
            product.UserId = userId;

            await _productRepository.AddAsync(product);

            return Ok("Product added succesfully");

        }
        /// <summary>
        /// Updates a product if user is Admin
        /// </summary>
        /// <param name="id">The id of the product to be updated</param>
        /// <param name="updatedProduct">The product object containing the updated product information.</param>
        /// <returns>Returns a message if product is updated</returns>
        /// <response code="404">Product not found</response>
        /// <response code="200">Product updated succesfully</response>

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
        /// <summary>
        /// Deleted a product if user is admin role
        /// </summary>
        /// <param name="id">The id of the product to be deleted</param>
        /// <returns>
        /// Returns a message if product is deleted
        /// </returns>
        /// <response code="404">Product not found</response>
        /// <response code="201">Product is deleted succefully</response>

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
