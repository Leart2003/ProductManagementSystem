using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProduct _productRepository;

        public ProductService(IProduct product)
        {
            _productRepository = product;
        }

        public async Task<IEnumerable<Product>> GetAllBooksAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product> GetBookById(Guid Id)
        {
            return await _productRepository.GetProductByIdAsync(Id);

        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _productRepository.AddAsync(product);
            return product;
        }
        //public async Task<Product> DeleteProduct(Guid id)
        //{
        //    await _productRepository.DeleteAsync(id);
        //}


    }
}
