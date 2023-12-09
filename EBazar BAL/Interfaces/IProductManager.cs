using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Interfaces
{
    public interface IProductManager
    {
        Task<bool> CreateProduct(ProductModelCreate productModel);
        Task<bool> UpdateProduct(int id, ProductModel productModel);
        Task<ProductModelView> GetProductById(int id);
        Task<ICollection<ProductModelView>> GetProductsByName(string name);
        Task<ICollection<ProductModelView>> GetProductsByType(string type);
        Task<ICollection<ProductModelView>> GetAllProducts();
        Task<bool> DeleteProduct(string name);
    }
}
