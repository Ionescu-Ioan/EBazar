using EBazar.Controllers;
using EBazar_BAL.Models;
using EBazar_DAL;
using EBazar_DAL.Entities;
using EBazar_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazarTests
{
    [TestFixture]
    public class ShoppingCartTests
    {

        private AppDbContext _contextMock;
        private ShoppingCartController _shoppingCartController;
        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Data Source=LAPTOP-MO2LLB6V;Initial Catalog=EbazarDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")  // Replace "your_connection_string_here" with your actual connection string
                .Options;

            _contextMock = new AppDbContext(dbContextOptions);
            _shoppingCartController = new ShoppingCartController(_contextMock);

        }

        [Test, Order(1)]
        public async Task AddItemsToUserCart()
        {
            var userModel = new UserModel()
            {
                UserName = "rinualex",
                Email = "rinualexandru3@gmail.com",
                FirstName = "rinualex",
                LastName = "rinualex",
                Age = 22,
            };
            var productTest = new ProductModelCreate
            {
                Name = "Samsung Galaxy S24",
                Description = "Telefon Samsung",
                Price = 5000,
                Type = "Telefoane"
            };
            var quantity = 3;
            var user = await _contextMock.Users.Where(x => x.UserName == userModel.UserName).FirstOrDefaultAsync();
            var cart = await _contextMock.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var amount = cart.Amount;
            var expectedAmount = amount + productTest.Price * quantity;
            var cartReturn = new ShoppingCartModel();
            cartReturn.Amount = cart.Amount;

            var shoppingCartItem = await _contextMock.CartItems.Where(x => x.CartId == cart.Id && x.ProductId == 1).FirstOrDefaultAsync();
            var product = await _contextMock.Products.Where(x => x.Id == 1).FirstOrDefaultAsync();
            if (shoppingCartItem == null)
            {
                try
                {
                    var cartItem = new CartItem
                    {
                        ProductId = product.Id,
                        price = product.Price,
                        quantity = quantity,
                        CartId = cart.Id
                    };

                    _contextMock.CartItems.Add(cartItem);
                    cart.Amount = cart.Amount + cartItem.quantity * cartItem.price;
                    if(cart.Amount < 0)
                    {
                        throw new InvalidOperationException("Amount is not right!");
                    }
                }
                catch(Exception e)
                {
                    Assert.Throws<InvalidOperationException>(() => { throw e; }, "Amount is not right!");
                }
            }
            else
            {
                try
                {
                    shoppingCartItem.quantity += quantity;
                    _contextMock.CartItems.Update(shoppingCartItem);
                    cart.Amount = cart.Amount + quantity * shoppingCartItem.price;
                    if (cart.Amount < 0)
                    {
                        throw new InvalidOperationException("Amount is not right!");
                    }
                }
                catch(Exception e)
                {
                    Assert.Throws<InvalidOperationException>(() => { throw e; }, "Amount is not right!");
                }
            }
            _contextMock.Carts.Update(cart);
            await _contextMock.SaveChangesAsync();
            cart = await _contextMock.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            Assert.IsTrue(expectedAmount == cart.Amount, "Amount is not right!");
            quantity = 2;
            expectedAmount = expectedAmount - quantity * product.Price;
            shoppingCartItem = await _contextMock.CartItems.Where(x => x.CartId == cart.Id && x.ProductId == 1).FirstOrDefaultAsync();
            while (quantity > 0)
            {
                if (shoppingCartItem != null)
                {
                    if (shoppingCartItem.quantity > 1)
                    {
                        try
                        {

                            shoppingCartItem.quantity--;
                            cart.Amount = cart.Amount - shoppingCartItem.price;
                            if (cart.Amount < 0)
                            {
                                throw new InvalidOperationException("Amount is not right!");
                            }
                        }
                        catch (Exception e)
                        {
                            Assert.Throws<InvalidOperationException>(() => { throw e; }, "Amount is not right!");

                        }
                    }
                    else
                    {
                        try
                        {
                            cart.Amount = cart.Amount - shoppingCartItem.price;
                            if (cart.Amount < 0)
                            {
                                throw new InvalidOperationException("Amount is not right!");
                            }
                        }
                        catch (Exception e)
                        {
                            Assert.Throws<InvalidOperationException>(() => { throw e; }, "Amount is not right!");
                        }
                    }
                }
                quantity--;
            }
            _contextMock.Carts.Update(cart);
            await _contextMock.SaveChangesAsync();
            cart = await _contextMock.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            Assert.IsTrue(expectedAmount == cart.Amount, "Amount is not right!");
            var cartItems = await _contextMock.CartItems.Where(x => x.CartId == cart.Id).ToListAsync();
            var cartItemsReturn = new List<CartItemModel>();
            foreach (var cartItem in cartItems)
            {
                product = await _contextMock.Products.Where(x => x.Id == cartItem.ProductId).FirstOrDefaultAsync();
                var cartItemReturn = new CartItemModel();
                cartItemReturn.Quantity = cartItem.quantity;
                cartItemReturn.Price = cartItem.price;
                cartItemReturn.ProductName = product.Name;
                cartItemReturn.ProductId = product.Id;
                cartItemsReturn.Add(cartItemReturn);
            }
            Assert.IsNotNull(cartItemsReturn, "Error at getting the car items.");

            user = await _contextMock.Users.Where(x => x.UserName == userModel.UserName).FirstOrDefaultAsync();
            cart = await _contextMock.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            cartItems = _contextMock.CartItems.Where(x => x.CartId == cart.Id).ToList();
            var request = new CheckoutRequest()
            {
                Username = userModel.UserName,
                Address = "TestAddress",
                City = "TestCity",
                Country = "TestCountry",
                Phone = "1234567890"
            };
            
            var order = new OrderHistory();
            order.CreatedDate = DateTime.Now;
            order.Amount = cart.Amount;
            order.Username = request.Username;
            order.Address = request.Address;
            order.Country = request.Country;
            order.Phone = request.Phone;
            order.City = request.City;
            await _contextMock.OrdersHistory.AddAsync(order);
            cart.Amount = 0;
            await _contextMock.SaveChangesAsync();
            cart = await _contextMock.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            Assert.IsTrue(cart.Amount == 0, "Error at removing items from cart after checkout!");

            var orderAdd = await _contextMock.OrdersHistory
                .Where(x => x.Username == user.UserName)
                .OrderByDescending(o => o.CreatedDate)
                .FirstOrDefaultAsync();

            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetails();
                orderDetail.OrderId = orderAdd.Id;
                var productItem = await _contextMock.Products.Where(x => x.Id == cartItem.ProductId).FirstOrDefaultAsync();
                orderDetail.ProductName = productItem.Name;
                orderDetail.Quantity = cartItem.quantity;
                await _contextMock.OrderDetails.AddAsync(orderDetail);
                _contextMock.CartItems.Remove(cartItem);
            }
            await _contextMock.SaveChangesAsync();
            cartItems = _contextMock.CartItems.Where(x => x.CartId == cart.Id).ToList();
            Assert.IsTrue(cartItems.Count == 0, "Error at removing items from Cartitems");
        }
    }
}
