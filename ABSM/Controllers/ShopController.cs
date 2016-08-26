using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABSM.Models;
namespace ABSM.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Shop
        public ActionResult Index(int id)
        {
            var shop = db.Shops.Where(s => s.ShopID == id).SingleOrDefault();
            return View(shop);
        }

        public ActionResult Complain(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public ActionResult Complain(Complain model)
        {
            if (ModelState.IsValid)
            {
                db.Complains.Add(model);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Store");
        }



        public ActionResult Products(int id)
        {
            var products = db.Products.Where(p => p.ShopID == id).OrderByDescending(o=>o.Price).Take(4).ToList();
            return PartialView("_RelatedProducts",products);
        }

    }
}