
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ABSM.Models;
using ABSM.ViewModels;


namespace ABSM.Services
{
    public class ShoppingCart
    {
        
        private readonly ApplicationDbContext _db;
        private readonly string _cartId;

        public ShoppingCart(HttpContextBase context) 
            : this(context, new ApplicationDbContext())
        {
        }

        public ShoppingCart(HttpContextBase httpContext, ApplicationDbContext storeContext)
        {
            _db = storeContext;
            _cartId = GetCartId(httpContext);
        }

        public async Task AddAsync(int productId)
        {
            var product = await _db.Products
                .SingleOrDefaultAsync(p => p.ProductID == productId);

            if (product == null)
            {
                // TODO: throw exception or do something
                return;
            }

            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);

            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    CartId = _cartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(int productId)
        {
            var cartItem = await _db.CartItems
                .SingleOrDefaultAsync(c => c.ProductId == productId && c.CartId == _cartId);

            var itemCount = 0;

            if (cartItem == null)
            {
                return itemCount;
            }

            if (cartItem.Count > 1)
            {
                cartItem.Count--;
                itemCount = cartItem.Count;
            }
            else
            {
                _db.CartItems.Remove(cartItem);
            }

            await _db.SaveChangesAsync();

            return itemCount;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            return await _db.CartItems.Include("Product")
                .Where(c => c.CartId == _cartId).ToArrayAsync();
        }

        public async Task<decimal> PaymentCheckoutAsync(PaymentCheckoutViewModel model, string userName)
        {  
            var items = await GetCartItemsAsync();
            var order = new Order()
            {
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                Phone = model.Phone,
                Email = userName,
                OrderDate = DateTime.Now,
                PaymentMode=model.PaymentMode
            };

            foreach (var item in items)
            {
                var detail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count

                };
                var itemQty = UpdateQtyAsync(item.ProductId, item.Count);
                order.Total += (item.Product.Price * item.Count);

                order.OrderDetails.Add(detail);
            }

                 model.Total = order.Total;



                 order.TransactionId = model.PaymentMode;
                _db.Orders.Add(order);
                _db.CartItems.RemoveRange(items);
                await _db.SaveChangesAsync();


            return model.Total;
        }
        public async Task<PaymentResult> CheckoutAsync(CheckoutViewModel model,string userName)
        {
            var items = await GetCartItemsAsync();
            var order = new Order()
            {
                FullName=model.FullName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                Phone = model.Phone,
                Email = userName,
                OrderDate = DateTime.Now
            };

            foreach (var item in items)
            {
                var detail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                  
                };
                var itemQty = UpdateQtyAsync(item.ProductId,item.Count);
                order.Total += (item.Product.Price*item.Count);

                order.OrderDetails.Add(detail);
            }

            model.Total = order.Total;

            var gateway = new PaymentGateway();
            var result = gateway.ProcessPayment(model);

            if (result.Succeeded)
            {
                order.TransactionId = result.TransactionId;
                _db.Orders.Add(order);
                _db.CartItems.RemoveRange(items);
                await _db.SaveChangesAsync();
            }
           
            return result;
        }


        public async Task<int> UpdateQtyAsync(int productId,int Count)
        {
            var product = await _db.Products
                .SingleOrDefaultAsync(c => c.ProductID == productId );

            var itemQty = 0;

            if (product == null)
            {
                return itemQty;
            }

            if (product.Quantity > 1)
            {
                product.Quantity= product.Quantity-Count;
                itemQty = product.Quantity;
            }
            else
            {
                _db.Products.Remove(product);
            }

            await _db.SaveChangesAsync();

            return itemQty;
        }
        private string GetCartId(HttpContextBase http)
        {
            
            var cookie = http.Request.Cookies.Get("ShoppingCart");
            var cartId = string.Empty;

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie = new HttpCookie("ShoppingCart");
                cartId = Guid.NewGuid().ToString();

                cookie.Value = cartId;
                cookie.Expires = DateTime.Now.AddDays(7);

                http.Response.Cookies.Add(cookie);
            }
            else
            {
                cartId = cookie.Value;
            }

            return cartId;
        }

    }
}