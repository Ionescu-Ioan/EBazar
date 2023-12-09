using EBazar_DAL.Entities;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProductModelView> ToProductModelView(Product product)
        {
            var type = await _context.ObjectTypes.Where(x => x.Id == product.ObjectTypeId).FirstOrDefaultAsync();
            var productModel = new ProductModelView
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = type.Type
            };
            return productModel;
        }

        public ProductModelCreate ModelViewToModelCreate(ProductModelView product)
        {
            var productModel = new ProductModelCreate
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type
            };
            return productModel;
        }


        public async Task<bool> CreateProduct(ProductModelCreate productModel)
        {
            var type = await _context.ObjectTypes.Where(x => x.Type == productModel.Type).FirstOrDefaultAsync();
            if (type == null)
                return false;
            var product = new Product
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Price = productModel.Price,
                ObjectTypeId = type.Id
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(string name)
        {
            var product = await _context.Products.Where(x => x.Name == name).FirstOrDefaultAsync();
            var carts = await _context.Carts.ToListAsync();
            foreach (var cart in carts)
            {
                var user = await _context.Users.Where(x => x.Id == cart.UserId).FirstOrDefaultAsync();
                var cartProduct = await _context.CartItems.Where(x => x.ProductId == product.Id && x.CartId == cart.Id).FirstOrDefaultAsync();
                if (cartProduct != null)
                {
                    cart.Amount = cart.Amount - cartProduct.price * cartProduct.quantity;
                    _context.Carts.Update(cart);
                }
            }
            if (product != null)
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ProductModelView> GetProductById(int id)
        {
            var product = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null)
                return null;
            var productModel = await ToProductModelView(product);
            return productModel;
        }

        public async Task<ICollection<ProductModelView>> GetProductsByName(string name)
        {
            var products = await _context.Products.Where(x => x.Name.Contains(name)).ToListAsync();
            var collection = new List<ProductModelView>();
            foreach (var product in products)
            {
                var productModel = await ToProductModelView(product);
                collection.Add(productModel);
            }
            return collection;
        }

        public async Task<ProductModelView> GetProductByName(string name)
        {
            var product = await _context.Products.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
            var returnProduct = await ToProductModelView(product);
            return returnProduct;
        }

        public async Task<ICollection<ProductModelView>> GetProductsByType(string type)
        {
            var typeProd = await _context.ObjectTypes.Where(x => x.Type == type).FirstOrDefaultAsync();

            if (typeProd == null) return null;

            var products = await _context.Products.Where(x => x.ObjectTypeId == typeProd.Id).ToListAsync();
            var collection = new List<ProductModelView>();
            foreach (var product in products)
            {
                var productModel = await ToProductModelView(product);
                collection.Add(productModel);
            }
            return collection;
        }

        public async Task<bool> UpdateProduct(int id, ProductModel productModel)
        {
            var product = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null)
                return false;
            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Price = productModel.Price;
            _context.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<ProductModelView>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            var collection = new List<ProductModelView>();
            foreach (var product in products)
            {
                var productModel = await ToProductModelView(product);
                collection.Add(productModel);
            }
            return collection;
        }
    }
}
