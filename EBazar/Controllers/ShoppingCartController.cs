using EBazar_BAL.Models;
using EBazar_DAL;
using EBazar_DAL.Entities;
using EBazar_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : Controller
    {
        public readonly AppDbContext _context;
        public ShoppingCartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getCartByUsername")]
        public async Task<IActionResult> GetCartByUsername(string username)
        {
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var cartReturn = new ShoppingCartModel();
            cartReturn.Amount = cart.Amount;
            cartReturn.Id = cart.Id;
            return Ok(cartReturn);
        }

        [HttpGet("getCartItemsUsername")]
        public async Task<IActionResult> GetCartItemsByUsername(string username)
        {
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var cartItems = await _context.CartItems.Where(x => x.CartId == cart.Id).ToListAsync();
            var cartItemsReturn = new List<CartItemModel>();
            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products.Where(x => x.Id == cartItem.ProductId).FirstOrDefaultAsync();
                var cartItemReturn = new CartItemModel();
                cartItemReturn.Quantity = cartItem.quantity;
                cartItemReturn.Price = cartItem.price;
                cartItemReturn.ProductName = product.Name;
                cartItemReturn.ProductId = product.Id;
                cartItemsReturn.Add(cartItemReturn);

            }
            return Ok(cartItemsReturn);
        }

        [HttpPost("addItems")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var user = await _context.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var shoppingCartItem = await _context.CartItems.Where(x => x.CartId == cart.Id
            && x.ProductId == int.Parse(request.ProductId)).FirstOrDefaultAsync();
            var product = await _context.Products.Where(x => x.Id == int.Parse(request.ProductId)).FirstOrDefaultAsync();
            if (shoppingCartItem == null)
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    price = product.Price,
                    quantity = int.Parse(request.Quantity),
                    CartId = cart.Id
                };

                _context.CartItems.Add(cartItem);
                cart.Amount = cart.Amount + cartItem.quantity * cartItem.price;
            }
            else
            {
                shoppingCartItem.quantity += int.Parse(request.Quantity);
                _context.CartItems.Update(shoppingCartItem);
                cart.Amount = cart.Amount + int.Parse(request.Quantity) * shoppingCartItem.price;
            }
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return Ok("Succes");
        }

        [HttpDelete("deleteItems")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            var user = await _context.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var shoppingCartItem = await _context.CartItems.Where(x => x.CartId == cart.Id
            && x.ProductId == int.Parse(request.ProductId)).FirstOrDefaultAsync();
            var product = await _context.Products.Where(x => x.Id == int.Parse(request.ProductId)).FirstOrDefaultAsync();
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.quantity > 1)
                {
                    shoppingCartItem.quantity--;
                    cart.Amount = cart.Amount - shoppingCartItem.price;
                }
                else
                {
                    cart.Amount = cart.Amount - shoppingCartItem.price;

                }
            }
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return Ok("Succes");
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var user = await _context.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
            {
            "card",
            },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(cart.Amount)*100,
                        Currency = "ron",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Shopping Cart",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = $"https://localhost:5001/api/ShoppingCart/success/{cart.Id}/{request.Address}/{request.City}/{request.Country}/{request.Phone}",
                CancelUrl = "https://localhost:5001/api/ShoppingCart/cancel",
            };
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return Json(new { id = session.Id });
        }

        [HttpPost("checkoutCash")]
        public async Task<IActionResult> CheckoutCash([FromBody] CheckoutRequest request)
        {
            var user = await _context.Users.Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
            var cart = await _context.Carts.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
            var cartItems = _context.CartItems.Where(x => x.CartId == cart.Id).ToList();

            var order = new OrderHistory();
            order.CreatedDate = DateTime.Now;
            order.Amount = cart.Amount;
            order.Username = request.Username;
            order.Address = request.Address;
            order.Country = request.Country;
            order.Phone = request.Phone;
            order.City = request.City;
            await _context.OrdersHistory.AddAsync(order);
            cart.Amount = 0;
            await _context.SaveChangesAsync();

            var orderAdd = await _context.OrdersHistory
                .Where(x => x.Username == user.UserName)
                .OrderByDescending(o => o.CreatedDate)
                .FirstOrDefaultAsync();

            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetails();
                orderDetail.OrderId = orderAdd.Id;
                var product = await _context.Products.Where(x => x.Id == cartItem.ProductId).FirstOrDefaultAsync();
                orderDetail.ProductName = product.Name;
                orderDetail.Quantity = cartItem.quantity;
                await _context.OrderDetails.AddAsync(orderDetail);
                _context.CartItems.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
            return Ok("Success");
        }

        [HttpGet("success/{id}/{address}/{city}/{country}/{phone}")]
        public async Task<IActionResult> Success(int id, string address, string city, string country, string phone)
        {
            var cart = await _context.Carts.Where(x => x.Id == id).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == cart.UserId).FirstOrDefaultAsync();
            var cartItems = await _context.CartItems.Where(x => x.CartId == cart.Id).ToListAsync();

            var order = new OrderHistory();
            order.CreatedDate = DateTime.Now;
            order.Amount = cart.Amount;
            order.Username = user.UserName;
            order.Address = address;
            order.Country = country;
            order.Phone = phone;
            order.City = city;
            await _context.OrdersHistory.AddAsync(order);
            cart.Amount = 0;
            await _context.SaveChangesAsync();

            var orderAdd = await _context.OrdersHistory
                .Where(x => x.Username == user.UserName)
                .OrderByDescending(o => o.CreatedDate)
                .FirstOrDefaultAsync();

            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetails();
                orderDetail.OrderId = orderAdd.Id;
                var product = _context.Products.Where(x => x.Id == cartItem.ProductId).FirstOrDefault();
                orderDetail.ProductName = product.Name;
                orderDetail.Quantity = cartItem.quantity;
                await _context.OrderDetails.AddAsync(orderDetail);
                _context.CartItems.Remove(cartItem);
            }
            await _context.SaveChangesAsync();
            return View();
        }

        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
