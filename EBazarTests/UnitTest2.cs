using EBazar_BAL.Interfaces;
using EBazar_BAL.Managers;
using EBazar_BAL.Models;
using EBazar_DAL;
using EBazar_DAL.Entities;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using EBazar_DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazarTests
{


    [TestFixture]
    public class ObjectTypeAndProductsTesting
    {

        private AppDbContext _contextMock;
        private ObjectTypeRepository _objectTypeRepository;
        private ProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Data Source=LAPTOP-MO2LLB6V;Initial Catalog=EbazarDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")  // Replace "your_connection_string_here" with your actual connection string
                .Options;

            _contextMock = new AppDbContext(dbContextOptions);
            _objectTypeRepository = new ObjectTypeRepository(_contextMock);
            _productRepository = new ProductRepository(_contextMock);
        }

        [Test, Order(1)]
        public async Task CreateCategory()
        {
            var objectTypeCreate = new ObjectTypeModel
            {
                Type = "TestObjectType"
            };

            var createType = await _objectTypeRepository.CreateObjectType(objectTypeCreate);
            Assert.IsTrue(createType, "ObjectType was not created!");

        }
        [Test, Order(2)]
        public async Task CreateProducts()
        {
            var product1 = new ProductModelCreate
            {
                Name = "TestObjectType",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };
            var product2 = new ProductModelCreate
            {
                Name = "TestObjectTypeFilterByName1",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };
            var product3 = new ProductModelCreate
            {
                Name = "TestObjectTypeFilterByName2",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };

            var createProduct1 = await _productRepository.CreateProduct(product1);
            var createProduct2 = await _productRepository.CreateProduct(product2);
            var createProduct3 = await _productRepository.CreateProduct(product3);

            Assert.IsTrue(createProduct1, "Product1 was not created!");
            Assert.IsTrue(createProduct2, "Product2 was not created!");
            Assert.IsTrue(createProduct3, "Product3 was not created!");
        }

        [Test, Order(3)]
        public async Task GetAllProducts()
        {
            var product1 = new ProductModelCreate
            {
                Name = "TestObjectType",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };
            var product2 = new ProductModelCreate
            {
                Name = "TestObjectTypeFilterByName1",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };
            var product3 = new ProductModelCreate
            {
                Name = "TestObjectTypeFilterByName2",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };

            var allProducts = await _productRepository.GetAllProducts();
            var product1Exists = false;
            var product2Exists = false;
            var product3Exists = false;
            foreach (var product in allProducts)
            {
                var productCreate = _productRepository.ModelViewToModelCreate(product);
                if (productCreate.Name.Equals(product1.Name) &&
                    productCreate.Description.Equals(product1.Description) &&
                    (productCreate.Price == product1.Price) &&
                    productCreate.Type.Equals(product1.Type))
                {
                    product1Exists = true;
                }
                else if (productCreate.Name.Equals(product2.Name) &&
                    productCreate.Description.Equals(product2.Description) &&
                    (productCreate.Price == product2.Price) &&
                    productCreate.Type.Equals(product2.Type))
                {
                    product2Exists = true;
                }
                else if (productCreate.Name.Equals(product3.Name) &&
                    productCreate.Description.Equals(product3.Description) &&
                    (productCreate.Price == product3.Price) &&
                    productCreate.Type.Equals(product3.Type))
                {
                    product3Exists = true;
                }
            }
            Assert.IsTrue(product1Exists, "Product1 was not found!");
            Assert.IsTrue(product2Exists, "Product2 was not found!");
            Assert.IsTrue(product3Exists, "Product3 was not found!");

        }

        [Test, Order(4)]
        public async Task FilterProducts()
        {
            var type = "TestObjectType";
            var productsByType = await _productRepository.GetProductsByType(type);
            var sizeFilterByType = 0;
            foreach(var prod in productsByType)
            {
                sizeFilterByType++;
            }
            Assert.IsTrue(sizeFilterByType == 3, "Get products by type failed!");

            var name = "TestObjectTypeFilterBy";
            var productsByName = await _productRepository.GetProductsByName(name);
            var sizeFilterByName = 0;
            foreach(var prod in productsByName)
            {
                sizeFilterByName++;
            }
            Assert.IsTrue(sizeFilterByName == 2, "Get products by name failed!");
        }

        [Test, Order(5)]
        public async Task UpdateProduct()
        {
            var productOld = new ProductModelCreate
            {
                Name = "TestObjectType",
                Description = "Test Description",
                Price = 4000,
                Type = "TestObjectType"
            };

            var productNew = new ProductModel
            {
                Name = "TestObjectTypeUpdate",
                Description = "Test Description Update",
                Price = 4500,
            };

            var product = await _productRepository.GetProductByName(productOld.Name);
            Assert.IsNotNull(product, "Error at getting product by name!");
            var updateBool = await _productRepository.UpdateProduct(product.Id, productNew);
            Assert.IsTrue(updateBool, "Update product failed!");
            product = await _productRepository.GetProductByName(productNew.Name);
            Assert.IsNotNull(product, "Error at getting product by name after update!");

        }

        [Test, Order(6)]
        public async Task DeleteProductAndObjectType()
        {
            var product = new ProductModel
            {
                Name = "TestObjectTypeUpdate",
                Description = "Test Description Update",
                Price = 4500,
            };
            var deleteBool = await _productRepository.DeleteProduct(product.Name);
            Assert.IsTrue(deleteBool, "Delete product failed!");
            var objectTypeModel = new ObjectTypeModel
            {
                Type = "TestObjectType"
            };
            var deleteCategory = await _objectTypeRepository.DeleteObjectType(objectTypeModel);
            Assert.IsTrue(deleteCategory, "Delete category failed!");
        }

    }
}
