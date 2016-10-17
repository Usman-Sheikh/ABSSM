using ABSM.Models;
using ABSM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ABSM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Shop")]
    public class OwnerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            int shopid = Convert.ToInt32(Session["Shop"].ToString());
            var complains = db.Complains.Include("Shop").Where(c => c.ShopID == shopid);
            return View(complains);
        }

        
        #region Products
        // GET: Admin/Products
        public ActionResult ListProducts()
        {
            int shopid = Convert.ToInt32(Session["Shop"].ToString());
            var products = db.Products.Include("Shop").Include("Category").Where(p=>p.ShopID==shopid);
            return View(products.ToList());
        }


       

        // GET: Admin/Products/Details/5
        public ActionResult PDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }



        // GET: Admin/Products/Create
        public ActionResult PCreate()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PCreate(Product product, HttpPostedFileBase doc)
        {
            string path;
            if (doc != null)
            {

                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);
                    product.ImageUrl = "~/Content/Images/" + filename;
                }
                else
                {
                    ModelState.AddModelError("", "Document size must be less then 5MB");
                    return View(product);
                }
                if (ModelState.IsValid)
                {
                    product.ShopID= Convert.ToInt32(Session["Shop"].ToString());
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("ListProducts");
                }


            }
            ModelState.AddModelError("", "Please upload image");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult PEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include("Category").Include("Shop").Where(x=>x.ProductID==id).SingleOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PEdit(Product product, string ImageValue, HttpPostedFileBase doc)
        {
            string path;
            if (doc != null && ModelState.IsValid)
            {

                var filename = Path.GetFileName(doc.FileName);
                var extension = Path.GetExtension(filename).ToLower();
                if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                {
                    path = HostingEnvironment.MapPath(Path.Combine("~/Content/Images/", filename));
                    doc.SaveAs(path);
                    product.ImageUrl = "~/Content/Images/" + filename;
                    product.ShopID = Convert.ToInt32(Session["Shop"].ToString());
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListProducts");
                }
                else
                {
                    ModelState.AddModelError("", "Only Image Allowed");
                    return View(product);
                }
               
            }
            else if (ModelState.IsValid)
            {
                product.ImageUrl = ImageValue;
                db.Entry(product).State = EntityState.Modified;
                product.ShopID = Convert.ToInt32(Session["Shop"].ToString());
                db.SaveChanges();
                return RedirectToAction("ListProducts");

            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult PDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include("Category").Include("Shop").Where(x => x.ProductID == id).SingleOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PDeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("ListProducts");
        }
        #endregion

        #region Categories
        public ActionResult ListCategories()
        {
            int shopid = Convert.ToInt32(Session["Shop"].ToString());
            var products = db.Categories.Include("Shop").Where(p => p.ShopID == shopid);
            return View(products.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            if (ModelState.IsValid)
            {   category.ShopID= Convert.ToInt32(Session["Shop"].ToString());
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }



        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "Name", category.ShopID);
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.ShopID= Convert.ToInt32(Session["Shop"].ToString());
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShopID = new SelectList(db.Shops, "ShopID", "Name", category.ShopID);
            return View(category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion

        #region Orders
        public ActionResult ListOrders()
        {
            int shopid = Convert.ToInt32(Session["Shop"].ToString());
            var orderDetails = db.OrderDetails.Where(p => p.Product.ShopID == shopid).Include("Order").ToList();
            return View(orderDetails);
        }


        public ActionResult Order(int? id)
        {

            var order = db.Orders.Where(o => o.OrderId == id).SingleOrDefault();
            return View(order);
        }


        public ActionResult Income()
        {
            int shopid = Convert.ToInt32(Session["Shop"].ToString());
            var orderDetails = db.OrderDetails.Where(p => p.Product.ShopID == shopid).ToList();
            var result = new IncomeViewModel
            {
                Items = orderDetails,
                Total = Calcuate(orderDetails),
            };
            return View(result);
        }
        [HttpPost]
        public ActionResult Income(int? Month)
        {
            if (Month > 0)
            {
                int shopid = Convert.ToInt32(Session["Shop"].ToString());
                var orderDetails = db.OrderDetails.Where(p => p.Product.ShopID == shopid && p.Order.OrderDate.Month == Month).ToList();
                if (orderDetails != null)
                {
                    var result = new IncomeViewModel
                    {
                        Items = orderDetails,
                        Total = Calcuate(orderDetails),
                    };
                    return View(result);
                }

                ViewBag.Message = "No Record Found";

                return View();

            }
            else
            {
                ViewBag.Message = "Select Month";
                return View();
            }
            
        }

        private static decimal Calcuate(IEnumerable<OrderDetail> items)
        {
            return items.Sum(item => (item.Product.Price * item.Quantity));
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}