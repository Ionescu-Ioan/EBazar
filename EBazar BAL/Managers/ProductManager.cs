using EBazar_BAL.Interfaces;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> CreateProduct(ProductModelCreate productModel)
        {
            return await _productRepository.CreateProduct(productModel);
        }

        public async Task<bool> DeleteProduct(string name)
        {
            return await _productRepository.DeleteProduct(name);
        }

        public async Task<ICollection<ProductModelView>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<ProductModelView> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<ICollection<ProductModelView>> GetProductsByName(string name)
        {
            return await _productRepository.GetProductsByName(name);
        }

        public async Task<ICollection<ProductModelView>> GetProductsByType(string type)
        {
            return await _productRepository.GetProductsByType(type);
        }

        public async Task<bool> UpdateProduct(int id, ProductModel productModel)
        {
            return await _productRepository.UpdateProduct(id, productModel);
        }
    }
}
