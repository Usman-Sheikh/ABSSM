using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ABSM.Services;
using ABSM.Models;
using ABSM.ViewModels;
using Microsoft.AspNet.Identity;

namespace ABSM.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public async Task<ActionResult> Index()
        {
            var cart = new ShoppingCart(HttpContext);
            var items = await cart.GetCartItemsAsync();

            return View(new ShoppingCartViewModel
            {
                Items = items,
                Total = CalcuateCart(items)
            });
        }

        public async Task<ActionResult> AddToCart(int id)
        {
            var cart = new ShoppingCart(HttpContext);

            await cart.AddAsync(id);

            return RedirectToAction("index");
        }

        public async Task<ActionResult> RemoveFromCart(int id)
        {
            var cart = new ShoppingCart(HttpContext);

            await cart.RemoveAsync(id);

            return RedirectToAction("index");
        }



        [Authorize]
        public ActionResult CheckoutEasyPaisa()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutEasyPaisa(PaymentCheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = new ShoppingCart(HttpContext);
            var userName = User.Identity.Name;


            var result = await cart.PaymentCheckoutAsync(model, userName);
          
            if (result>0)
            {
                if(model.PaymentMode== "Easy Paisa")
                {
                TempData["transactionId"] = "Your Payment Mode is Easy Paisa Please send the Total Amout PKR: "+result +" to 0345-8111997. " ;

                }
                else
                {
                    TempData["transactionId"] = "Your Payment Mode is Cash On Delivery Please pay the Total Amout PKR: " + result + "when riders deliver's tha package.You can track your order with your email id";

                }
                return RedirectToAction("Complete");
            }

            ModelState.AddModelError(string.Empty, "Somthing Went Wrong");

            return View(model);
        }


        [Authorize]
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = new ShoppingCart(HttpContext);
            var userName = User.Identity.Name;
            
            
            var result = await cart.CheckoutAsync(model, userName);

            if (result.Succeeded)
            {
                TempData["transactionId"] = "Thanks for your order. Your transaction ID is "+ result.TransactionId;
                return RedirectToAction("Complete");
            }

            ModelState.AddModelError(string.Empty, result.Message);

            return View(model);
        }

        public ActionResult Complete()
        {
            ViewBag.TransactionId = (string) TempData["transactionId"];

            return View();
        }

        private static decimal CalcuateCart(IEnumerable<CartItem> items)
        {
            return items.Sum(item => (item.Product.Price*item.Count));
        }
    }
}